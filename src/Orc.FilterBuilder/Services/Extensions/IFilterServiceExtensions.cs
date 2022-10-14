namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class IFilterServiceExtensions
    {
        public static async Task<IEnumerable<TItem>> FilterCollectionAsync<TItem>(this IFilterService filterService, FilterScheme? filter, IEnumerable<TItem>? rawCollection)
        {
            ArgumentNullException.ThrowIfNull(filterService);

            var filteredCollection = new List<TItem>();

            if (rawCollection is null)
            {
                return filteredCollection;
            }

            if (filter is null)
            {
                filteredCollection.AddRange(rawCollection);
                return filteredCollection;
            }

            await filterService.FilterCollectionAsync(filter, rawCollection, filteredCollection);

            return filteredCollection;
        }

        public static Task<IEnumerable<TItem>> FilterCollectionWithCurrentFilterAsync<TItem>(this IFilterService filterService, IEnumerable<TItem>? rawCollection)
        {
            ArgumentNullException.ThrowIfNull(filterService);

            return filterService.FilterCollectionAsync(filterService.SelectedFilter, rawCollection);
        }
    }
}
