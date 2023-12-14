namespace Orc.FilterBuilder;

using System;
using System.Collections;
using System.Threading.Tasks;
using Catel.IoC;
using MethodTimer;

public class FilterService : IFilterService
{
    private readonly IReflectionService _reflectionService;
    private FilterScheme? _selectedFilter;

    public FilterService(IFilterSchemeManager filterSchemeManager)
    {
        ArgumentNullException.ThrowIfNull(filterSchemeManager);

        var scope = filterSchemeManager.Scope;
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
        _reflectionService = this.GetServiceLocator().ResolveRequiredType<IReflectionService>(scope);
#pragma warning restore IDISP004 // Don't ignore created IDisposable.

        filterSchemeManager.Updated += OnFilterSchemeManagerUpdated;
    }

    public FilterScheme? SelectedFilter
    {
        get { return _selectedFilter; }
        set
        {
            // ORCOMP-257: don't check for equality

            _selectedFilter = value;

            SelectedFilterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Occurs when any of the filters has been updated.
    /// </summary>
    public event EventHandler<EventArgs>? FiltersUpdated;
    /// <summary>
    /// Occurs when the currently selected filter has changed.
    /// </summary>
    public event EventHandler<EventArgs>? SelectedFilterChanged;

    public Task FilterCollectionAsync(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection)
    {
        ArgumentNullException.ThrowIfNull(filter);

        FilterCollection(filter, rawCollection, filteredCollection);

        return Task.CompletedTask;
    }

    [Time]
    public void FilterCollection(FilterScheme filter, IEnumerable rawCollection, IList filteredCollection)
    {
        ArgumentNullException.ThrowIfNull(filter);

        filter.EnsureIntegrity(_reflectionService);

        filter.Apply(rawCollection, filteredCollection);
    }

    private void OnFilterSchemeManagerUpdated(object? sender, EventArgs e)
    {
        FiltersUpdated?.Invoke(this, EventArgs.Empty);
    }
}
