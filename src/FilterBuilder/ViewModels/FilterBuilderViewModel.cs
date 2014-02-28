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

        private bool _appliedSelectedScheme;
        private Type _targetType;
        private readonly FilterSchemes _filterSchemes;

        #region Constructors
        public FilterBuilderViewModel(IUIVisualizerService uiVisualizerService, IFilterSchemeManager filterSchemeManager)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => filterSchemeManager);

            _uiVisualizerService = uiVisualizerService;
            _filterSchemeManager = filterSchemeManager;

            _filterSchemes = filterSchemeManager.FilterSchemes;

            NewSchemeCommand = new Command(OnNewSchemeExecute);
            EditSchemeCommand = new Command(OnEditSchemeExecute, OnEditSchemeCanExecute);
            ApplySchemeCommand = new Command(OnApplySchemeExecute, OnApplySchemeCanExecute);
        }
        #endregion

        #region Properties
        public List<FilterScheme> AvailableSchemes { get; private set; }
        public FilterScheme SelectedFilterScheme { get; set; }

        public IEnumerable RawCollection { get; set; }
        public IList FilteredCollection { get; set; }

        public Command NewSchemeCommand { get; private set; }
        public Command EditSchemeCommand { get; private set; }
        public Command ApplySchemeCommand { get; private set; }
        #endregion

        #region Methods
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
            if (RawCollection == null)
            {
                _targetType = null;
                AvailableSchemes = new List<FilterScheme>();
            }
            else
            {
                _targetType = CollectionHelper.GetTargetType(RawCollection);
                AvailableSchemes = (from scheme in _filterSchemes.Schemes
                                    where _targetType.IsAssignableFromEx(scheme.TargetType)
                                    select scheme).ToList();
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

            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(filterScheme) ?? false)
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
            if (_uiVisualizerService.ShowDialog<EditFilterViewModel>(SelectedFilterScheme) ?? false)
            {
                _filterSchemeManager.UpdateFilters();
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
            IDisposable suspendToken = null;
            var filteredCollectionType = FilteredCollection.GetType();
            if (filteredCollectionType.IsGenericTypeEx() && filteredCollectionType.GetGenericTypeDefinitionEx() == typeof(FastObservableCollection<>))
            {
                suspendToken = (IDisposable)filteredCollectionType.GetMethodEx("SuspendChangeNotifications").Invoke(FilteredCollection, null);
            }

            FilteredCollection.Clear();

            foreach (object item in RawCollection)
            {
                if (SelectedFilterScheme.CalculateResult(item))
                {
                    FilteredCollection.Add(item);
                }
            }

            if (suspendToken != null)
            {
                suspendToken.Dispose();
            }
        }

        protected override void Initialize()
        {
            _filterSchemeManager.Loaded += OnFilterSchemeManagerLoaded;

            if (AvailableSchemes != null)
            {
                SelectedFilterScheme = AvailableSchemes.FirstOrDefault();
            }
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