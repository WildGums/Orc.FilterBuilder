namespace Orc.FilterBuilder.AlternativeExample.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using FilterBuilder.Services;
    using FilterBuilder.ViewModels;
    using Models;
    using Services;
    using System.Threading.Tasks;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFilterSchemeManager _filterSchemeManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFilterService _filterService;

        private IDataProvider _dataProvider;
        private FilterSchemes _filterSchemes;

        private readonly FilterScheme NoFilterFilter = new FilterScheme(new DictionaryMetadataProvider(), "Default");

        public RibbonViewModel(IDataProvider dataProvider, IFilterSchemeManager filterSchemeManager, IFilterService filterService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => dataProvider);
            Argument.IsNotNull(() => filterSchemeManager);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => uiVisualizerService);

            _dataProvider = dataProvider;
            _filterService = filterService;
            _filterSchemeManager = filterSchemeManager;
            _uiVisualizerService = uiVisualizerService;

            RawItems = dataProvider.GetData();
            MetadataProvider = dataProvider.GetMetadata();

            NewSchemeCommand = new Command(OnNewSchemeExecute);
        }

        public IEnumerable RawItems { get; private set; }
        public IMetadataProvider MetadataProvider { get; private set; }

        public Command NewSchemeCommand { get; private set; }

        public ObservableCollection<FilterScheme> AvailableSchemes { get; private set; }

        public FilterScheme SelectedFilterScheme { get; set; }

        private async void OnNewSchemeExecute()
        {
            var filterScheme = new FilterScheme(_dataProvider.GetMetadata());
            // TODO Unfortunately autocompletion would not work until catel:AutoCompletion is ready to work with metadata separated from the data
            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, _dataProvider.GetData(), false, false);

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
            var metadataProvider = _dataProvider.GetMetadata();

            newSchemes.AddRange((from scheme in _filterSchemes.Schemes
                                 where metadataProvider != null && metadataProvider.IsAssignableFromEx(scheme.TargetDataDescriptor)
                                 select scheme));

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
