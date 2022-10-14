namespace Orc.FilterBuilder
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;

    public interface IFilterService
    {
        FilterScheme? SelectedFilter { get; set; }

        Task FilterCollectionAsync(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection);

        void FilterCollection(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection);

        /// <summary>
        /// Occurs when any of the filters has been updated.
        /// </summary>
        event EventHandler<EventArgs>? FiltersUpdated;

        /// <summary>
        /// Occurs when the currently selected filter has changed.
        /// </summary>
        event EventHandler<EventArgs>? SelectedFilterChanged;
    }
}
