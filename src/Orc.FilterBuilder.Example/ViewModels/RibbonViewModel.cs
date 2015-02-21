// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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

        private IMetadataProvider _metadataProvider;
        private FilterSchemes _filterSchemes;

        public RibbonViewModel(ITestDataService testDataService, IFilterSchemeManager filterSchemeManager,
             IFilterService filterService, IUIVisualizerService uiVisualizerService)
        {
            _testDataService = testDataService;
            _filterSchemeManager = filterSchemeManager;
            _filterService = filterService;
            _uiVisualizerService = uiVisualizerService;
            RawItems = _testDataService.GetTestItems();
            MetadataProvider = new TypeMetadataProvider(typeof(TestEntity));

            NewSchemeCommand = new Command(OnNewSchemeExecute);
        }

        public ObservableCollection<TestEntity> RawItems { get; private set; }
        public IMetadataProvider MetadataProvider { get; private set; }
        public ObservableCollection<FilterScheme> AvailableSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        private readonly FilterScheme NoFilterFilter = new FilterScheme(new TypeMetadataProvider(typeof(object)), "Default");

        public Command NewSchemeCommand { get; private set; }

        private async void OnNewSchemeExecute()
        {
            if (_metadataProvider == null)
            {
                Log.Warning("Target data structure is unknown, cannot get any structure information to create filters");
                return;
            }

            var filterScheme = new FilterScheme(_metadataProvider);
            var filterSchemeEditInfo = new FilterSchemeEditInfo (filterScheme, RawItems, true, true);

            if (await _uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                AvailableSchemes.Add(filterScheme);
                _filterSchemes.Schemes.Add(filterScheme);
                SelectedFilterScheme = filterScheme;

                _filterSchemeManager.UpdateFilters();
            }
        }

        protected override async Task Initialize()
        {
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;
            _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;

            UpdateFilters();
        }

        protected override async Task Close()
        {
            if (_filterSchemes != null)
            {
                _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
            }

            _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
            _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;

            await base.Close();
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
                _metadataProvider = null;
            }
            else
            {
                // TODO refactor
                var type = CollectionHelper.GetTargetType(RawItems);
                _metadataProvider = new TypeMetadataProvider(type);
                newSchemes.AddRange((from scheme in _filterSchemes.Schemes
                                     where _metadataProvider != null && _metadataProvider.IsAssignableFromEx(scheme.TargetDataDescriptor)
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