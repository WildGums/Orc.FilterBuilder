namespace Orc.FilterBuilder
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Runtime.Serialization.Xml;
    using Orc.FileSystem;

    public class FilterSerializationService : IFilterSerializationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IXmlSerializer _xmlSerializer;

        public FilterSerializationService(IDirectoryService directoryService, IFileService fileService,
            IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => xmlSerializer);

            _directoryService = directoryService;
            _fileService = fileService;
            _xmlSerializer = xmlSerializer;
        }

        public async virtual Task<FilterSchemes> LoadFiltersAsync(string fileName)
        {
            Argument.IsNotNullOrWhitespace(() => fileName);

            Log.Info($"Loading filter schemes from '{fileName}'");

            var filterSchemes = new FilterSchemes();

            try
            {
                if (_fileService.Exists(fileName))
                {
                    using (var stream = _fileService.OpenRead(fileName))
                    {
                        _xmlSerializer.Deserialize(filterSchemes, stream, null);
                    }
                }

                Log.Debug("Loaded filter schemes from '{0}'", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load filter schemes");
            }

            return filterSchemes;
        }

        public async virtual Task SaveFiltersAsync(string fileName, FilterSchemes filterSchemes)
        {
            Argument.IsNotNullOrWhitespace(() => fileName);
            Argument.IsNotNull(() => filterSchemes);

            Log.Info($"Saving filter schemes to '{fileName}'");

            try
            {
                using (var stream = _fileService.OpenWrite(fileName))
                {
                    _xmlSerializer.Serialize(filterSchemes, stream, null);
                }

                Log.Debug("Saved filter schemes to '{0}'", fileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save filter schemes");
            }
        }
    }
}
