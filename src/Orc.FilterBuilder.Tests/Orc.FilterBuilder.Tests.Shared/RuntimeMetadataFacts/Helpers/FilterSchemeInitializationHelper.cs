using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.FilterBuilder.Tests
{
    using System.IO;
    using System.Linq;
    using Catel.IoC;
    using FilterBuilder.Models;
    using FilterBuilder.Services;
    using Services;

    public static class FilterSchemeInitializationHelper
    {
        public static FilterScheme GetTestFilterScheme()
        {
            var serviceLocator = ServiceLocator.Default;

            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            var reflectionService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TestFilterRuntimeModelReflectionService>(TestAttributeTypeProvider.AttributeTypes.Values.ToList());
            serviceLocator.RegisterInstance<IReflectionService>(reflectionService);

            var tempFileContext = new TemporaryFilesContext("filters");
            var tempFile = tempFileContext.GetFile($"testFilters.xml", true);
            var sourceFile = $"Resources\\Files\\filters.xml";
            File.Copy(sourceFile, tempFile, true);

            var filterManager = serviceLocator.ResolveType<IFilterSchemeManager>();
            filterManager.Load(tempFile);

            return filterManager.FilterSchemes.Schemes.FirstOrDefault(x => x.Title == "Test");
        }
    }
}
