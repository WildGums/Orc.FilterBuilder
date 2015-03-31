// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System;
    using System.Collections;
    using Catel;
    using MethodTimer;
    using Models;

    public class FilterService : IFilterService
    {
        private readonly IFilterSchemeManager _filterSchemeManager;

        #region Fields
        private FilterScheme _selectedFilter;
        #endregion

        public FilterService(IFilterSchemeManager filterSchemeManager)
        {
            Argument.IsNotNull(() => filterSchemeManager);

            _filterSchemeManager = filterSchemeManager;
            _filterSchemeManager.Updated += OnFilterSchemeManagerUpdated;
        }

        #region Properties
        public FilterScheme SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (ReferenceEquals(_selectedFilter, value))
                {
                    return;
                }

                _selectedFilter = value;

                SelectedFilterChanged.SafeInvoke(this);
            }
        }
        #endregion

        /// <summary>
        /// Occurs when any of the filters has been updated.
        /// </summary>
        public event EventHandler<EventArgs> FiltersUpdated;

        /// <summary>
        /// Occurs when the currently selected filter has changed.
        /// </summary>
        public event EventHandler<EventArgs> SelectedFilterChanged;

        [Time]
        public void FilterCollection(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filter);

            filter.EnsureIntegrity();

            if (filteredCollection == null)
            {
                return;
            }

            filter.Apply(rawCollection, filteredCollection);
        }

        private void OnFilterSchemeManagerUpdated(object sender, EventArgs e)
        {
            FiltersUpdated.SafeInvoke(this);
        }
    }
}