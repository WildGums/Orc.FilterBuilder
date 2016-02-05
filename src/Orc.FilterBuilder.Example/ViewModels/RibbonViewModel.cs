// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Example.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using FilterBuilder.Services;
    using FilterBuilder.ViewModels;
    using global::FilterBuilder.Example.Models;
    using global::FilterBuilder.Example.Services;
    using Models;

    using CollectionHelper = Orc.FilterBuilder.CollectionHelper;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

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

            NewSchemeCommand = new Command(OnNewSchemeExecute);
        }

        public ObservableCollection<TestEntity> RawItems { get; private set; }
        public ObservableCollection<FilterScheme> AvailableSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        private readonly FilterScheme NoFilterFilter = new FilterScheme(typeof(object), "Default");

        public Command NewSchemeCommand { get; private set; }

        private void OnNewSchemeExecute()
        {
            if (_targetType == null)
            {
                Log.Warning("Target type is unknown, cannot get any type information to create filters");
                return;
            }

            var filterScheme = new FilterScheme(_targetType);
            var filterSchemeEditInfo = new FilterSchemeEditInfo (filterScheme, RawItems, true, true);

            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                AvailableSchemes.Add(filterScheme);

                _filterSchemes.Schemes.Add(filterScheme);
                _filterService.SelectedFilter = filterScheme;

                _filterSchemeManager.UpdateFilters();
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
            if (_filterSchemes != null)
            {
                _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
            }

            _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
            _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;

            await base.CloseAsync();
        }

        private void UpdateFilters()
        {
            if (_filterSchemes != null)
            {
                _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
            }

            _filterSchemes = _filterSchemeManager.FilterSchemes;

            if (_filterSchemes != null)
            {
                _filterSchemes.Schemes.CollectionChanged += OnFilterSchemesCollectionChanged;
            }

            var newSchemes = new ObservableCollection<FilterScheme>();

            if (RawItems == null)
            {
                _targetType = null;
            }
            else
            {
                _targetType = CollectionHelper.GetTargetType(RawItems);
                newSchemes.AddRange((from scheme in _filterSchemes.Schemes
                                     where _targetType != null && _targetType.IsAssignableFromEx(scheme.TargetType)
                                     select scheme));
            }

            newSchemes.Insert(0, NoFilterFilter);

            if (AvailableSchemes == null || !Catel.Collections.CollectionHelper.IsEqualTo(AvailableSchemes, newSchemes))
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
            if (_filterService.SelectedFilter == null)
            {
                SelectedFilterScheme = AvailableSchemes.First();
            }
            else
            {
                SelectedFilterScheme = _filterService.SelectedFilter;
            }
        }
    }
}