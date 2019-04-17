// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBuilderViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Services;
    using Views;
    using CollectionHelper = FilterBuilder.CollectionHelper;

    [Serializable]
    public class FilterBuilderViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly IServiceLocator _serviceLocator;

        private readonly FilterScheme _noFilterFilter = new FilterScheme(typeof(object), "Default")
        {
            CanEdit = false,
            CanDelete = false
        };

        private IFilterSchemeManager _filterSchemeManager;
        private IFilterService _filterService;
        private IReflectionService _reflectionService;
        private readonly ILanguageService _languageService;
        private Type _targetType;
        private FilterSchemes _filterSchemes;
        private bool _applyingFilter;
        #endregion

        #region Constructors
        public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager,
            IFilterService filterService, IMessageService messageService, IServiceLocator serviceLocator, IReflectionService reflectionService, ILanguageService languageService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => filterSchemeManager);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => reflectionService);
            Argument.IsNotNull(() => languageService);

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
        #endregion

        #region Properties
        public List<FilterGroup> FilterGroups { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        public bool AllowLivePreview { get; set; }
        public bool EnableAutoCompletion { get; set; }
        public bool AutoApplyFilter { get; set; }
        public bool AllowReset { get; set; }
        public bool AllowDelete { get; set; }

        public IEnumerable RawCollection { get; set; }
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

        public object Scope { get; set; }
        #endregion

        #region Commands
        public TaskCommand NewSchemeCommand { get; private set; }

        private async Task OnNewSchemeExecuteAsync()
        {
            if (_targetType is null)
            {
                Log.Warning("Target type is unknown, cannot get any type information to create filters");
                return;
            }

            var filterScheme = new FilterScheme(_targetType);
            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);

            if (await _uiVisualizerService.ShowDialogAsync<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                _filterSchemes.Schemes.Add(filterScheme);

                UpdateFilterGroups();

                ApplyFilterScheme(filterScheme, true);

                await _filterSchemeManager.UpdateFiltersAsync();
            }
        }

        public TaskCommand<FilterScheme> EditSchemeCommand { get; private set; }

        private bool OnEditSchemeCanExecute(FilterScheme filterScheme)
        {
            if (filterScheme is null)
            {
                return false;
            }

            if (!filterScheme.CanEdit)
            {
                return false;
            }

            return true;
        }

        private async Task OnEditSchemeExecuteAsync(FilterScheme filterScheme)
        {
            try
            {
                filterScheme.EnsureIntegrity(_reflectionService);

                var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);

                if (await _uiVisualizerService.ShowDialogAsync<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
                {
                    await _filterSchemeManager.UpdateFiltersAsync();

                    if (ReferenceEquals(filterScheme, _filterService.SelectedFilter))
                    {
                        Log.Debug("Current filter has been edited, re-applying filter");

                        _filterService.SelectedFilter = filterScheme;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to edit filter scheme '{filterScheme?.Title}'");
                throw;
            }
        }

        public TaskCommand ApplySchemeCommand { get; private set; }

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
            Log.Debug("Applying filter scheme '{0}'", SelectedFilterScheme);

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

            if (_noFilterFilter is null)
            {
                return false;
            }

            var selectedFilterScheme = SelectedFilterScheme;
            if (selectedFilterScheme is null)
            {
                return false;
            }

            if (ReferenceEquals(selectedFilterScheme, _noFilterFilter))
            {
                return false;
            }

            return true;
        }

        private void OnResetSchemeExecute()
        {
            SelectedFilterScheme = _noFilterFilter;
        }

        public TaskCommand<FilterScheme> DeleteSchemeCommand { get; private set; }

        private bool OnDeleteSchemeCanExecute(FilterScheme filterScheme)
        {
            if (!AllowDelete)
            {
                return false;
            }

            var selectedFilterScheme = SelectedFilterScheme;
            if (selectedFilterScheme is null)
            {
                return false;
            }

            if (!selectedFilterScheme.CanDelete)
            {
                return false;
            }

            return true;
        }

        private async Task OnDeleteSchemeExecuteAsync(FilterScheme filterScheme)
        {
            if (await _messageService.ShowAsync(string.Format(_languageService.GetString("FilterBuilder_ShowAsync_Message_AreYouSureYouWantToDeleteFilterQuestion_Pattern"), filterScheme.Title), _languageService.GetString("FilterBuilder_ShowAsync_DeleteFilterQuestion_Caption"), MessageButton.YesNo) == MessageResult.Yes)
            {
                _filterSchemeManager.FilterSchemes.Schemes.Remove(filterScheme);

                SelectedFilterScheme = _noFilterFilter;

                await _filterSchemeManager.UpdateFiltersAsync();
            }
        }
        #endregion

        #region Methods
        private void OnScopeChanged()
        {
            if (_filterSchemeManager != null)
            {
                _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;
                _filterService.SelectedFilterChanged -= OnFilterServiceSelectedFilterChanged;
            }

            var scope = Scope;
            _filterSchemeManager = _serviceLocator.ResolveType<IFilterSchemeManager>(scope);
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;

            _filterService = _serviceLocator.ResolveType<IFilterService>(scope);
            _filterService.SelectedFilterChanged += OnFilterServiceSelectedFilterChanged;

            _reflectionService = _serviceLocator.ResolveType<IReflectionService>(scope);

            UpdateFilters();
        }

        private void ApplyFilterScheme(FilterScheme filterScheme, bool force = false)
        {
            if (filterScheme is null || _applyingFilter)
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

            if (_filterSchemes != null)
            {
                _filterSchemes.Schemes.CollectionChanged -= OnFilterSchemesCollectionChanged;
            }

            _filterSchemes = _filterSchemeManager.FilterSchemes;

            if (_filterSchemes != null)
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

                if (_targetType != null && _filterSchemes != null)
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
            var newFilterScheme = _filterService.SelectedFilter ?? _noFilterFilter;
            ApplyFilterScheme(newFilterScheme);
        }
        #endregion
    }
}
