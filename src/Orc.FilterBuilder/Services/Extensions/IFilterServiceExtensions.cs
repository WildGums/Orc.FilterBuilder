// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public static class IFilterServiceExtensions
    {
        #region Methods
        public static async Task<IEnumerable<TItem>> FilterCollectionAsync<TItem>(this IFilterService filterService, FilterScheme filter, IEnumerable<TItem> rawCollection)
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

            await filterService.FilterCollectionAsync(filter, rawCollection, filteredCollection);

            return filteredCollection;
        }

        public static Task<IEnumerable<TItem>> FilterCollectionWithCurrentFilterAsync<TItem>(this IFilterService filterService, IEnumerable<TItem> rawCollection)
        {
            return filterService.FilterCollectionAsync(filterService.SelectedFilter, rawCollection);
        }
        #endregion
    }
}