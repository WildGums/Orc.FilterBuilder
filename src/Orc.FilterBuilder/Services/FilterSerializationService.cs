namespace Orc.FilterBuilder;

using System;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;
using Catel.Runtime.Serialization.Xml;
using FileSystem;

public class FilterSerializationService : IFilterSerializationService
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly IFileService _fileService;
    private readonly IXmlSerializer _xmlSerializer;

    public FilterSerializationService(IFileService fileService,
        IXmlSerializer xmlSerializer)
    {
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(xmlSerializer);

        _fileService = fileService;
        _xmlSerializer = xmlSerializer;
    }

    public virtual async Task<FilterSchemes> LoadFiltersAsync(string fileName)
    {
        Argument.IsNotNullOrWhitespace(() => fileName);

        Log.Info($"Loading filter schemes from '{fileName}'");

        var filterSchemes = new FilterSchemes();

        try
        {
            if (_fileService.Exists(fileName))
            {
                await using var stream = _fileService.OpenRead(fileName);
                _xmlSerializer.Deserialize(filterSchemes, stream);
            }

            Log.Debug("Loaded filter schemes from '{0}'", fileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to load filter schemes");
        }

        return filterSchemes;
    }

    public virtual async Task SaveFiltersAsync(string fileName, FilterSchemes filterSchemes)
    {
        Argument.IsNotNullOrWhitespace(() => fileName);
        ArgumentNullException.ThrowIfNull(filterSchemes);

        Log.Info($"Saving filter schemes to '{fileName}'");

        try
        {
            await using (var stream = _fileService.OpenWrite(fileName))
            {
                _xmlSerializer.Serialize(filterSchemes, stream);
            }

            Log.Debug("Saved filter schemes to '{0}'", fileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to save filter schemes");
        }
    }
}
