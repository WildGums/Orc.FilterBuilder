// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderControlModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Services;
    using Views;
    using CollectionHelper = Orc.FilterBuilder.CollectionHelper;

    public class FilterBuilderViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFilterSchemeManager _filterSchemeManager;
        private readonly IFilterService _filterService;
        private readonly IMessageService _messageService;

        private readonly FilterScheme NoFilterFilter = new FilterScheme(new TypeMetadataProvider(typeof(object)), "Default");
        private FilterSchemes _filterSchemes;

        #region Constructors
        public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager,
            IFilterService filterService, IMessageService messageService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => filterSchemeManager);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => messageService);

            _uiVisualizerService = uiVisualizerService;
            _filterSchemeManager = filterSchemeManager;
            _filterService = filterService;
            _messageService = messageService;

            NewSchemeCommand = new Command(OnNewSchemeExecute);
            EditSchemeCommand = new Command<FilterScheme>(OnEditSchemeExecute, OnEditSchemeCanExecute);
            ApplySchemeCommand = new Command(OnApplySchemeExecute, OnApplySchemeCanExecute);
            ResetSchemeCommand = new Command(OnResetSchemeExecute, OnResetSchemeCanExecute);
            DeleteSchemeCommand = new Command<FilterScheme>(OnDeleteSchemeExecute, OnDeleteSchemeCanExecute);
        }
        #endregion

        #region Properties
        public ObservableCollection<FilterScheme> AvailableSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        public bool AllowLivePreview { get; set; }
        public bool EnableAutoCompletion { get; set; }
        public bool AutoApplyFilter { get; set; }
        public bool AllowReset { get; set; }
        public bool AllowDelete { get; set; }

        public IEnumerable RawCollection { get; set; }
        public IMetadataProvider MetadataProvider { get; set; }
        public IList FilteredCollection { get; set; }

        /// <summary>
        /// Current <see cref="FilterBuilderControl"/> mode
        /// </summary>
        public FilterBuilderMode Mode { get; set; }

        /// <summary>
        /// Filtering function if <see cref="FilterBuilderControl"/> mode is 
        /// <see cref="FilterBuilderMode.FilteringFunction"/>
        /// </summary>
        public Func<object, bool> FilteringFunc { get; set; }
        #endregion

        #region Commands
        public Command NewSchemeCommand { get; private set; }

        private void OnNewSchemeExecute()
        {
            if (MetadataProvider == null)
            {
                Log.Warning("Target data structure is unknown, cannot get any structure information to create filters");
                return;
            }

            var filterScheme = new FilterScheme(MetadataProvider);
            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);

            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                AvailableSchemes.Add(filterScheme);
                _filterSchemes.Schemes.Add(filterScheme);
                SelectedFilterScheme = filterScheme;

                _filterSchemeManager.UpdateFilters();
            }
        }

        public Command<FilterScheme> EditSchemeCommand { get; private set; }

        private bool OnEditSchemeCanExecute(FilterScheme filterScheme)
        {
            if (filterScheme == null)
            {
                return false;
            }

            if (AvailableSchemes.Count == 0)
            {
                return false;
            }

            if (ReferenceEquals(AvailableSchemes[0], filterScheme))
            {
                return false;
            }

            return true;
        }

        private void OnEditSchemeExecute(FilterScheme filterScheme)
        {
            filterScheme.EnsureIntegrity();

            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);

            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                _filterSchemeManager.UpdateFilters();

                ApplyFilter();
            }
        }

        public Command ApplySchemeCommand { get; private set; }

        private bool OnApplySchemeCanExecute()
        {
            if (SelectedFilterScheme == null)
            {
                return false;
            }

            if (MetadataProvider == null)
            {
                return false;
            }

            if (RawCollection == null)
            {
                return false;
            }

            if (FilteredCollection == null && Mode == FilterBuilderMode.Collection)
            {
                return false;
            }

            return true;
        }

        private async void OnApplySchemeExecute()
        {
            //build filtered collection only if current mode is Collection
            if (Mode == FilterBuilderMode.Collection)
            {
                FilteringFunc = null;
                await _filterService.FilterCollectionAsync(SelectedFilterScheme, RawCollection, FilteredCollection);
            }
            else
            {
                FilteringFunc = SelectedFilterScheme.CalculateResult;
            }
        }

        public Command ResetSchemeCommand { get; private set; }

        private bool OnResetSchemeCanExecute()
        {
            if (!AllowReset)
            {
                return false;
            }

            if (AvailableSchemes == null)
            {
                return false;
            }

            if (AvailableSchemes.Count == 0)
            {
                return false;
            }

            if (ReferenceEquals(SelectedFilterScheme, AvailableSchemes[0]))
            {
                return false;
            }

            return true;
        }

        private void OnResetSchemeExecute()
        {
            if (AvailableSchemes.Count > 0)
            {
                SelectedFilterScheme = AvailableSchemes[0];
            }
        }

        public Command<FilterScheme> DeleteSchemeCommand { get; private set; }

        private bool OnDeleteSchemeCanExecute(FilterScheme filterScheme)
        {
            if (filterScheme == null)
            {
                return false;
            }

            if (!AllowDelete)
            {
                return false;
            }

            if (AvailableSchemes == null)
            {
                return false;
            }

            if (AvailableSchemes.Count == 0)
            {
                return false;
            }

            if (ReferenceEquals(filterScheme, AvailableSchemes[0]))
            {
                return false;
            }

            return true;
        }

        private async void OnDeleteSchemeExecute(FilterScheme filterScheme)
        {
            if (await _messageService.Show(string.Format("Are you sure you want to delete filter '{0}'?", filterScheme.Title), "Delete filter?", MessageButton.YesNo) == MessageResult.Yes)
            {
                _filterSchemeManager.FilterSchemes.Schemes.Remove(filterScheme);

                SelectedFilterScheme = AvailableSchemes[0];

                _filterSchemeManager.UpdateFilters();
            }
        }
        #endregion

        #region Methods
        private async void OnSelectedFilterSchemeChanged()
        {
            if (SelectedFilterScheme == null || ReferenceEquals(SelectedFilterScheme, _filterService.SelectedFilter))
            {
                return;
            }

            _filterService.SelectedFilter = SelectedFilterScheme;

            if (AutoApplyFilter)
            {
                ApplyFilter();
            }
        }

        private void OnRawCollectionChanged()
        {
            UpdateFilters();

            ApplyFilter();
        }

        private void OnMetadataProviderChanged()
        {
            UpdateFilters();

            ApplyFilter();
        }

        private void OnFilteredCollectionChanged()
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            ApplySchemeCommand.Execute();
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

            if (RawCollection != null && MetadataProvider != null)
            {
                newSchemes.AddRange((from scheme in _filterSchemes.Schemes
                                     where MetadataProvider.IsAssignableFromEx(scheme.TargetDataDescriptor)
                                     select scheme));
            }

            newSchemes.Insert(0, NoFilterFilter);

            if (AvailableSchemes == null || !Catel.Collections.CollectionHelper.IsEqualTo(AvailableSchemes, newSchemes))
            {
                AvailableSchemes = newSchemes;

                var selectedFilter = _filterService.SelectedFilter ?? NoFilterFilter;
                SelectedFilterScheme = selectedFilter;
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
        #endregion
    }
}