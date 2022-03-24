namespace Orc.FilterBuilder.Tests
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Services;

    public static class FilterSchemeInitializationHelper
    {
        public static async Task<FilterScheme> GetTestFilterSchemeAsync()
        {
            var serviceLocator = ServiceLocator.Default;

            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            var reflectionService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TestFilterRuntimeModelReflectionService>(TestAttributeTypeProvider.AttributeTypes.Values.ToList());
            serviceLocator.RegisterInstance<IReflectionService>(reflectionService);

            using var tempFileContext = new TemporaryFilesContext("filters");
            var tempFile = tempFileContext.GetFile($"testFilters.xml", true);
            var sourceFile = Path.Combine(AssemblyDirectoryHelper.GetCurrentDirectory(), $"Resources\\Files\\filters.xml");
            File.Copy(sourceFile, tempFile, true);

            var filterManager = serviceLocator.ResolveType<IFilterSchemeManager>();
            await filterManager.LoadAsync(tempFile);

            return filterManager.FilterSchemes.Schemes.FirstOrDefault(x => x.Title == "Test");
        }
    }
}
