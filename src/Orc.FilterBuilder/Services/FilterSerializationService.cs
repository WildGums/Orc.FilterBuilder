namespace Orc.FilterBuilder;

using System;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;
using FileSystem;
using Microsoft.Extensions.Logging;
using Orc.Serialization.Json;

public class FilterSerializationService : IFilterSerializationService
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(FilterSerializationService));

    private readonly IFileService _fileService;
    private readonly IJsonSerializerFactory _jsonSerializerFactory;

    public FilterSerializationService(IFileService fileService,
        IJsonSerializerFactory jsonSerializerFactory)
    {
        _fileService = fileService;
        _jsonSerializerFactory = jsonSerializerFactory;
    }

    public virtual async Task<FilterSchemes> LoadFiltersAsync(string fileName)
    {
        Argument.IsNotNullOrWhitespace(() => fileName);

        Logger.LogInformation("Loading filter schemes from '{FileName}'", fileName);

        var filterSchemes = new FilterSchemes();

        try
        {
            if (_fileService.Exists(fileName))
            {
                var serializer = _jsonSerializerFactory.CreateSerializer(CreateSettings());

                await using var stream = _fileService.OpenRead(fileName);

                filterSchemes = serializer.Deserialize<FilterSchemes>(stream);
            }

            Logger.LogDebug("Loaded filter schemes from '{FileName}'", fileName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load filter schemes");
        }

        return filterSchemes ?? new FilterSchemes();
    }

    public virtual async Task SaveFiltersAsync(string fileName, FilterSchemes filterSchemes)
    {
        Argument.IsNotNullOrWhitespace(() => fileName);
        ArgumentNullException.ThrowIfNull(filterSchemes);

        Logger.LogInformation("Saving filter schemes to '{FileName}'", fileName);

        try
        {
            var serializer = _jsonSerializerFactory.CreateSerializer(CreateSettings());

            await using (var stream = _fileService.OpenWrite(fileName))
            {
                serializer.Serialize(stream, filterSchemes);
            }

            Logger.LogDebug("Saved filter schemes to '{FileName}'", fileName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save filter schemes");
        }
    }

    private static JsonSerializerSettings CreateSettings()
    {
        return new JsonSerializerSettings
        {
            UseTypeInfoConverter = true
        };
    }
}
