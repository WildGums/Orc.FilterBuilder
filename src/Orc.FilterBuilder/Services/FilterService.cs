﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Threading;
    using MethodTimer;

    public class FilterService : IFilterService
    {
        #region Fields
        private readonly IReflectionService _reflectionService;
        private FilterScheme _selectedFilter;
        #endregion

        #region Constructors
        public FilterService(IFilterSchemeManager filterSchemeManager)
        {
            Argument.IsNotNull(() => filterSchemeManager);

            var scope = filterSchemeManager.Scope;
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            _reflectionService = this.GetServiceLocator().ResolveType<IReflectionService>(scope);
#pragma warning restore IDISP004 // Don't ignore created IDisposable.

            filterSchemeManager.Updated += OnFilterSchemeManagerUpdated;
        }
        #endregion

        #region Properties
        public FilterScheme SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                // ORCOMP-257: don't check for equality

                _selectedFilter = value;

                SelectedFilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region IFilterService Members
        /// <summary>
        /// Occurs when any of the filters has been updated.
        /// </summary>
        public event EventHandler<EventArgs> FiltersUpdated;

        /// <summary>
        /// Occurs when the currently selected filter has changed.
        /// </summary>
        public event EventHandler<EventArgs> SelectedFilterChanged;

        public Task FilterCollectionAsync(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filter);

            FilterCollection(filter, rawCollection, filteredCollection);

            return TaskHelper.Completed;
        }

        [Time]
        public void FilterCollection(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection)
        {
            Argument.IsNotNull(() => filter);

            filter.EnsureIntegrity(_reflectionService);

            if (filteredCollection is null)
            {
                return;
            }

            filter.Apply(rawCollection, filteredCollection);
        }
        #endregion

        #region Methods
        private void OnFilterSchemeManagerUpdated(object sender, EventArgs e)
        {
            FiltersUpdated?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
