namespace Orc.FilterBuilder.Example.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;
using Catel.Reflection;
using Catel.Services;
using FilterBuilder.ViewModels;
using global::FilterBuilder.Example.Models;
using global::FilterBuilder.Example.Services;
    
public class RibbonViewModel : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly ITestDataService _testDataService;
    private readonly IFilterSchemeManager _filterSchemeManager;
    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly IFilterService _filterService;

    private Type _targetType;
    private FilterSchemes _filterSchemes;

    public RibbonViewModel(ITestDataService testDataService, IFilterSchemeManager filterSchemeManager,
        IFilterService filterService, IUIVisualizerService uiVisualizerService)
    {
        _testDataService = testDataService;
        _filterSchemeManager = filterSchemeManager;
        _filterService = filterService;
        _uiVisualizerService = uiVisualizerService;
        RawItems = _testDataService.GetTestItems();

        NewSchemeCommand = new TaskCommand(OnNewSchemeExecuteAsync);
    }

    public ObservableCollection<TestEntity> RawItems { get; private set; }

    public List<FilterScheme> AvailableSchemes { get; private set; }

    public FilterScheme SelectedFilterScheme { get; set; }

    private readonly FilterScheme _noFilterFilter = new FilterScheme(typeof(object), "Default")
    {
        CanDelete = false,
        CanEdit = false
    };

    public TaskCommand NewSchemeCommand { get; private set; }

    private async Task OnNewSchemeExecuteAsync()
    {
        if (_targetType is null)
        {
            Log.Warning("Target type is unknown, cannot get any type information to create filters");
            return;
        }

        var filterScheme = new FilterScheme(_targetType);
        var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawItems, true, true);

        var result = await _uiVisualizerService.ShowDialogAsync<EditFilterViewModel>(filterSchemeEditInfo);
        if (result.DialogResult ?? false)
        {
            AvailableSchemes.Add(filterScheme);

            _filterSchemes.Schemes.Add(filterScheme);
            _filterService.SelectedFilter = filterScheme;

            await _filterSchemeManager.UpdateFiltersAsync();
        }
    }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;
        _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;

        UpdateFilters();
    }

    protected override async Task CloseAsync()
    {
        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
        }

        _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
        _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;

        await base.CloseAsync();
    }

    private void UpdateFilters()
    {
        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
        }

        _filterSchemes = _filterSchemeManager.FilterSchemes;

        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged += OnFilterSchemesCollectionChanged;
        }

        var newSchemes = new List<FilterScheme>();

        if (RawItems is null)
        {
            _targetType = null;
        }
        else
        {
            _targetType = CollectionHelper.GetTargetType(RawItems);
            newSchemes.AddRange(from scheme in _filterSchemes.Schemes
                where _targetType != null && _targetType.IsAssignableFromEx(scheme.TargetType)
                select scheme);
        }

        newSchemes.Insert(0, _noFilterFilter);

        if (AvailableSchemes is null || !Catel.Collections.CollectionHelper.IsEqualTo(AvailableSchemes, newSchemes))
        {
            AvailableSchemes = newSchemes;
            SelectedFilterScheme = newSchemes.FirstOrDefault();
        }
    }

    private void OnFilterSchemesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateFilters();
    }

    private void OnFilterSchemeManagerLoaded(object sender, EventArgs eventArgs)
    {
        UpdateFilters();
    }

    private void OnFilterServiceSelectedFilterChanged(object sender, EventArgs e)
    {
        if (_filterService.SelectedFilter is null)
        {
            SelectedFilterScheme = AvailableSchemes.First();
        }
        else
        {
            SelectedFilterScheme = _filterService.SelectedFilter;
        }
    }
}