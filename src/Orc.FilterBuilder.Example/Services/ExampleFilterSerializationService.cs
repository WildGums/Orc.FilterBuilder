namespace Orc.FilterBuilder.Example.Services;

using System.Linq;
using System.Threading.Tasks;
using Catel.Collections;
using global::FilterBuilder.Example.Models;
using FileSystem;
using FilterBuilder;
using Orc.Serialization.Json;

public class ExampleFilterSerializationService : FilterSerializationService
{
    public ExampleFilterSerializationService(IFileService fileService, IJsonSerializerFactory jsonSerializerFactory)
        : base(fileService, jsonSerializerFactory)
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
