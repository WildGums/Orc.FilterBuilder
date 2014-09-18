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
    using Models;

    public class FilterService : IFilterService
    {
        #region Fields
        private FilterScheme _selectedFilter;
        #endregion

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

        public event EventHandler<EventArgs> SelectedFilterChanged;

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
    }
}