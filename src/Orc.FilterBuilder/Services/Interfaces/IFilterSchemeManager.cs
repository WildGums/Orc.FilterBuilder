namespace Orc.FilterBuilder;

using System;
using System.Threading.Tasks;

public interface IFilterSchemeManager
{
    bool AutoSave { get; set; }
    FilterSchemes FilterSchemes { get; }
        
    event EventHandler<EventArgs>? Updated;
    event EventHandler<EventArgs>? Loaded;
    event EventHandler<EventArgs>? Saved;

    Task UpdateFiltersAsync();
    Task<bool> LoadAsync(string? fileName = null);
    Task SaveAsync(string? fileName = null);
}
