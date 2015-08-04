// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterServiceExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System.Collections.Generic;
    using Models;

    public static class IFilterServiceExtensions
    {
        #region Methods
        public static IEnumerable<TItem> FilterCollection<TItem>(this IFilterService filterService, FilterScheme filter, IEnumerable<TItem> rawCollection)
        {
            var filteredCollection = new List<TItem>();

            if (rawCollection == null)
            {
                return filteredCollection;
            }

            if (filter == null)
            {
                filteredCollection.AddRange(rawCollection);
                return filteredCollection;
            }

            filterService.FilterCollection(filter, rawCollection, filteredCollection);

            return filteredCollection;
        }

        public static IEnumerable<TItem> FilterCollectionWithCurrentFilter<TItem>(this IFilterService filterService, IEnumerable<TItem> rawCollection)
        {
            return filterService.FilterCollection(filterService.SelectedFilter, rawCollection);
        }
        #endregion
    }
}