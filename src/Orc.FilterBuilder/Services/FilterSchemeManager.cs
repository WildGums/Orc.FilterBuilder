namespace Orc.FilterBuilder;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.Logging;

public class FilterSchemeManager : IFilterSchemeManager
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(FilterSchemeManager));

    private readonly IFilterSerializationService _filterSerializationService;
    private readonly IAppDataService _appDataService;

    private string? _lastFileName;

    public FilterSchemeManager(IFilterSerializationService filterSerializationService, IAppDataService appDataService)
    {
        _filterSerializationService = filterSerializationService;
        _appDataService = appDataService;

        AutoSave = true;
        FilterSchemes = new FilterSchemes();
    }

    public bool AutoSave { get; set; }
    public FilterSchemes FilterSchemes { get; private set; }

    public event EventHandler<EventArgs>? Updated;
    public event EventHandler<EventArgs>? Loaded;
    public event EventHandler<EventArgs>? Saved;

    public virtual async Task UpdateFiltersAsync()
    {
        try
        {
            Updated?.Invoke(this, EventArgs.Empty);

            if (AutoSave)
            {
                await SaveAsync();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update filters");
            throw;
        }
    }

    public virtual async Task<bool> LoadAsync(string? fileName = null)
    {
        fileName = GetFileName(fileName);

        _lastFileName = fileName;

        var filterSchemes = await _filterSerializationService.LoadFiltersAsync(fileName);

        FilterSchemes = filterSchemes;

        Loaded?.Invoke(this, EventArgs.Empty);
        Updated?.Invoke(this, EventArgs.Empty);

        return true;
    }

    public virtual async Task SaveAsync(string? fileName = null)
    {
        fileName = GetFileName(fileName);

        await _filterSerializationService.SaveFiltersAsync(fileName, FilterSchemes);

        Saved?.Invoke(this, EventArgs.Empty);
    }

    protected virtual string GetDefaultFileName()
    {
        var defaultFileName = Path.Combine(_appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "FilterSchemes.json");
        return defaultFileName;
    }

    private string GetFileName(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = _lastFileName ?? GetDefaultFileName();
        }

        _lastFileName = fileName;

        return fileName;
    }
}
