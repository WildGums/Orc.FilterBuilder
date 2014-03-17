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
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Orc.FilterBuilder.Models;
    using Orc.FilterBuilder.Services;
    using CollectionHelper = Orc.FilterBuilder.CollectionHelper;

    public class FilterBuilderViewModel : ViewModelBase
    {
        private readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFilterSchemeManager _filterSchemeManager;

        private Type _targetType;
        private FilterSchemes _filterSchemes;

        #region Constructors
        public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => filterSchemeManager);

            _uiVisualizerService = uiVisualizerService;
            _filterSchemeManager = filterSchemeManager;

            NewSchemeCommand = new Command(OnNewSchemeExecute);
            EditSchemeCommand = new Command(OnEditSchemeExecute, OnEditSchemeCanExecute);
            ApplySchemeCommand = new Command(OnApplySchemeExecute, OnApplySchemeCanExecute);
            ResetSchemeCommand = new Command(OnResetSchemeExecute, OnResetSchemeCanExecute);
        }
        #endregion

        #region Properties
        public ObservableCollection<FilterScheme> AvailableSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        public bool AllowLivePreview { get; set; }
        public bool EnableAutoCompletion { get; set; }
        public bool AutoApplyFilter { get; set; }
        public bool AllowReset { get; set; }

        public IEnumerable RawCollection { get; set; }
        public IList FilteredCollection { get; set; }

        public Command NewSchemeCommand { get; private set; }
        public Command EditSchemeCommand { get; private set; }
        public Command ApplySchemeCommand { get; private set; }
        public Command ResetSchemeCommand { get; private set; }
        #endregion

        #region Methods
        private void OnSelectedFilterSchemeChanged()
        {
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
            _filterSchemes = _filterSchemeManager.FilterSchemes;

            if (RawCollection == null)
            {
                _targetType = null;
                AvailableSchemes = new ObservableCollection<FilterScheme>();
            }
            else
            {
                _targetType = CollectionHelper.GetTargetType(RawCollection);
                AvailableSchemes = new ObservableCollection<FilterScheme>((from scheme in _filterSchemes.Schemes
                                                                           where _targetType.IsAssignableFromEx(scheme.TargetType)
                                                                           select scheme));
            }

            AvailableSchemes.Insert(0, new FilterScheme(typeof(object), "No filter"));

            SelectedFilterScheme = AvailableSchemes.FirstOrDefault();
        }

        private void OnNewSchemeExecute()
        {
            if (_targetType == null)
            {
                Log.Warning("Target type is unknown, cannot get any type information to create filters");
                return;
            }

            var filterScheme = new FilterScheme(_targetType);
            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);
            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                AvailableSchemes.Add(filterScheme);
                _filterSchemes.Schemes.Add(filterScheme);
                SelectedFilterScheme = filterScheme;

                _filterSchemeManager.UpdateFilters();
            }
        }

        private bool OnEditSchemeCanExecute()
        {
            if (SelectedFilterScheme == null)
            {
                return false;
            }

            if (AvailableSchemes.Count == 0)
            {
                return false;
            }

            if (ReferenceEquals(AvailableSchemes[0], SelectedFilterScheme))
            {
                return false;
            }

            return true;
        }

        private void OnEditSchemeExecute()
        {
            var filterSchemeEditInfo = new FilterSchemeEditInfo(SelectedFilterScheme, RawCollection, AllowLivePreview, EnableAutoCompletion);
            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterSchemeEditInfo) ?? false)
            {
                _filterSchemeManager.UpdateFilters();

                ApplyFilter();
            }
        }

        private bool OnApplySchemeCanExecute()
        {
            if (SelectedFilterScheme == null)
            {
                return false;
            }

            if (RawCollection == null)
            {
                return false;
            }

            if (FilteredCollection == null)
            {
                return false;
            }

            return true;
        }

        private void OnApplySchemeExecute()
        {
            SelectedFilterScheme.Apply(RawCollection, FilteredCollection);
        }

        private bool OnResetSchemeCanExecute()
        {
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

        protected override void Initialize()
        {
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;

            UpdateFilters();
        }

        protected override void Close()
        {
            _filterSchemeManager.Loaded -= OnFilterSchemeManagerLoaded;

            base.Close();
        }

        private void OnFilterSchemeManagerLoaded(object sender, EventArgs eventArgs)
        {
            UpdateFilters();
        }
        #endregion
    }
}