namespace Orc.FilterBuilder.Example.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.Runtime.Serialization;
    using Catel.Runtime.Serialization.Xml;
    using global::FilterBuilder.Example.Models;
    using Orc.FileSystem;
    using Orc.FilterBuilder;

    public class ExampleFilterSerializationService : FilterSerializationService
    {
        public ExampleFilterSerializationService(IDirectoryService directoryService, IFileService fileService, IXmlSerializer xmlSerializer) 
            : base(directoryService, fileService, xmlSerializer)
        {
        }

        public override async Task<FilterSchemes> LoadFiltersAsync(string path)
        {
            var filterSchemes = await base.LoadFiltersAsync(path);

            filterSchemes.Schemes.Add(new FilterScheme(typeof(TestEntity))
            {
                Title = "Demo filter scheme",
                FilterGroup = "Group name",
                CanEdit = false,
                CanDelete = false
            });

            return filterSchemes;
        }

        public override async Task SaveFiltersAsync(string path, FilterSchemes filterSchemes)
        {
            // Create clone with filters we want to serialize
            var finalFilterSchemes = new FilterSchemes();
            finalFilterSchemes.Schemes.AddRange(from x in filterSchemes.Schemes
                                                where x.FilterGroup is null
                                                select x);

            await base.SaveFiltersAsync(path, finalFilterSchemes);
        }
    }
}
