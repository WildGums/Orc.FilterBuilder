namespace Orc.FilterBuilder.Tests;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class FilterSchemeInitializationHelper
{
    public static async Task<FilterScheme> GetTestFilterSchemeAsync()
    {
        return new FilterScheme();
//        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

//        using var serviceProvider = serviceCollection.BuildServiceProvider();

//        var 

//        var serviceLocator = ServiceLocator.Default;

//#pragma warning disable IDISP001 // Dispose created
//        var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
//#pragma warning restore IDISP001 // Dispose created
//        var reflectionService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TestFilterRuntimeModelReflectionService>(TestAttributeTypeProvider.AttributeTypes.Values.ToList());
//        serviceLocator.RegisterInstance<IReflectionService>(reflectionService);

//        using var tempFileContext = new TemporaryFilesContext("filters");
//        var tempFile = tempFileContext.GetFile($"testFilters.json", true);
//        var sourceFile = Path.Combine(AssemblyDirectoryHelper.GetCurrentDirectory(), $"Resources\\Files\\filters.json");
//        File.Copy(sourceFile, tempFile, true);

//        var filterManager = serviceLocator.ResolveType<IFilterSchemeManager>();
//        await filterManager.LoadAsync(tempFile);

//        return filterManager.FilterSchemes.Schemes.FirstOrDefault(x => x.Title == "Test");
    }
}
