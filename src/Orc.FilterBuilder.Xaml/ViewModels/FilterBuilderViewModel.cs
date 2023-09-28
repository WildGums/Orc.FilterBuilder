namespace Orc.FilterBuilder.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Catel.Collections;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Reflection;
using Catel.Services;
using Views;
using CollectionHelper = CollectionHelper;

public class FilterBuilderViewModel : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly IMessageService _messageService;
    private readonly IServiceLocator _serviceLocator;

    private readonly FilterScheme _noFilterFilter = new(typeof(object), "Default")
    {
        CanEdit = false,
        CanDelete = false
    };

    private readonly ILanguageService _languageService;
    private IFilterSchemeManager? _filterSchemeManager;
    private IFilterService? _filterService;
    private IReflectionService? _reflectionService;
    private Type? _targetType;
    private FilterSchemes? _filterSchemes;
    private bool _applyingFilter;

    public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager,
        IFilterService filterService, IMessageService messageService, IServiceLocator serviceLocator, IReflectionService reflectionService, ILanguageService languageService)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);
        ArgumentNullException.ThrowIfNull(filterSchemeManager);
        ArgumentNullException.ThrowIfNull(filterService);
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(serviceLocator);
        ArgumentNullException.ThrowIfNull(reflectionService);
        ArgumentNullException.ThrowIfNull(languageService);

        _uiVisualizerService = uiVisualizerService;
        _filterSchemeManager = filterSchemeManager;
        _filterService = filterService;
        _messageService = messageService;
        _serviceLocator = serviceLocator;
        _reflectionService = reflectionService;
        _languageService = languageService;

        FilterGroups = new List<FilterGroup>();

        NewSchemeCommand = new TaskCommand(OnNewSchemeExecuteAsync);
        EditSchemeCommand = new TaskCommand<FilterScheme>(OnEditSchemeExecuteAsync, OnEditSchemeCanExecute);
        ApplySchemeCommand = new TaskCommand(OnApplySchemeExecuteAsync, OnApplySchemeCanExecute);
        ResetSchemeCommand = new Command(OnResetSchemeExecute, OnResetSchemeCanExecute);
        DeleteSchemeCommand = new TaskCommand<FilterScheme>(OnDeleteSchemeExecuteAsync, OnDeleteSchemeCanExecute);
    }

    public List<FilterGroup> FilterGroups { get; private set; }
    public FilterScheme? SelectedFilterScheme { get; set; }

    public bool AllowLivePreview { get; set; }
    public bool EnableAutoCompletion { get; set; }
    public bool AutoApplyFilter { get; set; }
    public bool AllowReset { get; set; }
    public bool AllowDelete { get; set; }

    public IEnumerable? RawCollection { get; set; }
    public IList? FilteredCollection { get; set; }

    /// <summary>
    /// Current <see cref="FilterBuilderControl"/> mode
    /// </summary>
    public FilterBuilderMode Mode { get; set; }

    /// <summary>
    /// Filtering function if <see cref="FilterBuilderControl"/> mode is 
    /// <see cref="FilterBuilderMode.FilteringFunction"/>
    /// </summary>
    public Func<object, bool>? FilteringFunc { get; set; }

    public object? Scope { get; set; }

    public TaskCommand NewSchemeCommand { get; private set; }

    private async Task OnNewSchemeExecuteAsync()
    {
        if (_targetType is null)
        {
            Log.Warning("Target type is unknown, cannot get any type information to create filters");
            return;
        }

        var rawCollection = RawCollection;
        if (rawCollection is null)
        {
            return;
        }

        var filterScheme = new FilterScheme(_targetType);
        var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, rawCollection, AllowLivePreview, EnableAutoCompletion);

        var result = await _uiVisualizerService.ShowDialogAsync<EditFilterViewModel>(filterSchemeEditInfo);
        if (!(result.DialogResult ?? false))
        {
            return;
        }

        if (_filterSchemes is null || _filterSchemeManager is null)
        {
            return;
        }

        _filterSchemes.Schemes.Add(filterScheme);

        UpdateFilterGroups();

        ApplyFilterScheme(filterScheme, true);

        await _filterSchemeManager.UpdateFiltersAsync();
    }

    public TaskCommand<FilterScheme> EditSchemeCommand { get; private set; }

    private bool OnEditSchemeCanExecute(FilterScheme? filterScheme)
    {
        return filterScheme?.CanEdit ?? false;
    }

    private async Task OnEditSchemeExecuteAsync(FilterScheme? filterScheme)
    {
        if (filterScheme is null ||
            _reflectionService is null)
        {
            return;
        }

        var rawCollection = RawCollection;
        if (rawCollection is null)
        {
            return;
        }

        try
        {
            if (_filterService is not null)
            {
                filterScheme.EnsureIntegrity(_reflectionService);
            }

            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, rawCollection, AllowLivePreview, EnableAutoCompletion);

            var result = await _uiVisualizerService.ShowDialogAsync<EditFilterViewModel>(filterSchemeEditInfo);
            if (!(result.DialogResult ?? false))
            {
                return;
            }

            if (_filterSchemeManager is null || _filterService is null)
            {
                return;
            }

            await _filterSchemeManager.UpdateFiltersAsync();

            if (ReferenceEquals(filterScheme, _filterService.SelectedFilter))
            {
                Log.Debug("Current filter has been edited, re-applying filter");

                _filterService.SelectedFilter = filterScheme;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to edit filter scheme '{filterScheme.Title}'");
            throw;
        }
    }

    public TaskCommand ApplySchemeCommand { get; }

    private bool OnApplySchemeCanExecute()
    {
        if (SelectedFilterScheme is null)
        {
            return false;
        }

        if (RawCollection is null)
        {
            return false;
        }

        if (FilteredCollection is null && Mode == FilterBuilderMode.Collection)
        {
            return false;
        }

        return true;
    }

    private async Task OnApplySchemeExecuteAsync()
    {
        var selectedFilterScheme = SelectedFilterScheme;
        if (selectedFilterScheme is null)
        {
            return;
        }

        var rawCollection = RawCollection;
        if (rawCollection is null)
        {
            return;
        }

        var filteredCollection = FilteredCollection;
        if (filteredCollection is null)
        {
            return;
        }

        Log.Debug("Applying filter scheme '{0}'", selectedFilterScheme);

        //build filtered collection only if current mode is Collection
        if (Mode != FilterBuilderMode.Collection)
        {
            FilteringFunc = selectedFilterScheme.CalculateResult;
            return;
        }
            
        if(_filterService is not null)
        {
            FilteringFunc = null;
            await _filterService.FilterCollectionAsync(selectedFilterScheme, rawCollection, filteredCollection);
        }
    }

    public Command ResetSchemeCommand { get; private set; }

    private bool OnResetSchemeCanExecute()
    {
        if (!AllowReset)
        {
            return false;
        }

        var selectedFilterScheme = SelectedFilterScheme;
        if (selectedFilterScheme is null)
        {
            return false;
        }

        return !ReferenceEquals(selectedFilterScheme, _noFilterFilter);
    }

    private void OnResetSchemeExecute()
    {
        SelectedFilterScheme = _noFilterFilter;
    }

    public TaskCommand<FilterScheme> DeleteSchemeCommand { get; private set; }

    private bool OnDeleteSchemeCanExecute(FilterScheme? filterScheme)
    {
        if (!AllowDelete)
        {
            return false;
        }

        return filterScheme?.CanDelete ?? false;
    }

    private async Task OnDeleteSchemeExecuteAsync(FilterScheme? filterScheme)
    {
        if (filterScheme is null)
        {
            return;
        }

        if (await _messageService.ShowAsync(string.Format(_languageService.GetRequiredString("FilterBuilder_ShowAsync_Message_AreYouSureYouWantToDeleteFilterQuestion_Pattern"), filterScheme.Title), 
                _languageService.GetRequiredString("FilterBuilder_ShowAsync_DeleteFilterQuestion_Caption"), MessageButton.YesNo) != MessageResult.Yes)
        {
            return;
        }

        _filterSchemeManager?.FilterSchemes.Schemes.Remove(filterScheme);

        SelectedFilterScheme = _noFilterFilter;

        if (_filterSchemeManager is not null)
        {
            await _filterSchemeManager.UpdateFiltersAsync();
        }
    }

    private void OnScopeChanged()
    {
        if (_filterSchemeManager is not null)
        {
            _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
            _filterSchemeManager = null;
        }

        if (_filterService is not null)
        {
            _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;
            _filterService = null;
        }

        _reflectionService = null;

        var scope = Scope;
        if (_serviceLocator.IsTypeRegistered<IFilterSchemeManager>(scope))
        {
            _filterSchemeManager = _serviceLocator.ResolveRequiredType<IFilterSchemeManager>(scope);
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;
        }

        if (_serviceLocator.IsTypeRegistered<IFilterService>(scope))
        {
            _filterService = _serviceLocator.ResolveRequiredType<IFilterService>(scope);
            _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;
        }

        if (_serviceLocator.IsTypeRegistered<IReflectionService>(scope))
        {
            _reflectionService = _serviceLocator.ResolveRequiredType<IReflectionService>(scope);
        }

        UpdateFilters();
    }

    private void ApplyFilterScheme(FilterScheme filterScheme, bool force = false)
    {
        if (_applyingFilter)
        {
            return;
        }

        if (_filterService is null)
        {
            return;
        }

        _applyingFilter = true;

        var selectedFilterIsDifferent = !ReferenceEquals(SelectedFilterScheme, filterScheme);
        var filterServiceSelectedFilterIsDifferent = !ReferenceEquals(filterScheme, _filterService.SelectedFilter);

        if (selectedFilterIsDifferent)
        {
            SelectedFilterScheme = filterScheme;
        }

        if (filterServiceSelectedFilterIsDifferent)
        {
            _filterService.SelectedFilter = filterScheme;
        }

        if (AutoApplyFilter && (force || selectedFilterIsDifferent || filterServiceSelectedFilterIsDifferent))
        {
            ApplyFilter();
        }

        _applyingFilter = false;
    }

    private void OnSelectedFilterSchemeChanged()
    {
        if (SelectedFilterScheme is null)
        {
            return;
        }

        ApplyFilterScheme(SelectedFilterScheme);
    }

    private void OnRawCollectionChanged()
    {
        Log.Debug("Raw collection changed");

        UpdateFilters();

        ApplyFilter();
    }

    private void OnFilteredCollectionChanged()
    {
        Log.Debug("Filtered collection changed");

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        ApplySchemeCommand.Execute();
    }

    private void UpdateFilters()
    {
        Log.Debug("Updating filters");

        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
        }

        _filterSchemes = _filterSchemeManager?.FilterSchemes;

        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged += OnFilterSchemesCollectionChanged;
        }

        UpdateFilterGroups();
    }

    private void UpdateFilterGroups()
    {
        var applicableFilterSchemes = new List<FilterScheme>();

        if (RawCollection is null)
        {
            _targetType = null;
        }
        else
        {
            _targetType = CollectionHelper.GetTargetType(RawCollection);

            if (_targetType is not null && _filterSchemes is not null)
            {
                ((ICollection<FilterScheme>)applicableFilterSchemes).AddRange((from scheme in _filterSchemes.Schemes
                    where scheme.TargetType != null && _targetType.IsAssignableFromEx(scheme.TargetType)
                    select scheme));
            }
        }

        applicableFilterSchemes.Insert(0, _noFilterFilter);

        var filterGroups = (from filterScheme in applicableFilterSchemes
            //where filterScheme.IsVisible
            orderby filterScheme.FilterGroup, filterScheme.Title, filterScheme.CanDelete
            group filterScheme by filterScheme.FilterGroup into g
            select new FilterGroup(g.Key, g)).ToList();

        // Ensure no filter is at the top
        var noFilterGroup = filterGroups.First(x => x.Title == _noFilterFilter.FilterGroup);
        noFilterGroup.FilterSchemes.Remove(_noFilterFilter);
        noFilterGroup.FilterSchemes.Insert(0, _noFilterFilter);

        FilterGroups = filterGroups;
    }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        if (_filterSchemeManager is not null)
        {
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;
        }

        if (_filterService is not null)
        {
            _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;
        }

        UpdateFilters();
    }

    protected override async Task CloseAsync()
    {
        if (_filterSchemes is not null)
        {
            _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
        }

        if (_filterSchemeManager is not null) 
        {
            _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
        }

        if (_filterService is not null)
        {
            _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;
        }

        await base.CloseAsync();
    }

    private void OnFilterSchemesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateFilters();
    }

    private void OnFilterSchemeManagerLoaded(object? sender, EventArgs eventArgs)
    {
        UpdateFilters();
    }

    private void OnFilterServiceSelectedFilterChanged(object? sender, EventArgs e)
    {
        var newFilterScheme = _filterService?.SelectedFilter ?? _noFilterFilter;
        ApplyFilterScheme(newFilterScheme);
    }
}
