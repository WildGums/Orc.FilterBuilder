namespace Orc.FilterBuilder.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.IoC;
    using FilterBuilder.Services;
    using NUnit.Framework;

    [TestFixture]
    public class RuntimeMetadataFacts
    {
        [Test]
        public async Task CorrectlyFilterEntitiesWithRuntimeMetadataAsync()
        {
            var serviceLocator = ServiceLocator.Default;

            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            
            var filterScheme = await FilterSchemeInitializationHelper.GetTestFilterSchemeAsync();
            var initialCollection = TestDataProvider.GetInitialCollection();
            var resultList = new List<TestFilterRuntimeModel>();

            var filterService = typeFactory.CreateInstance<FilterService>();
            filterService.FilterCollection(filterScheme, initialCollection, resultList);

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(resultList[0].Attributes[AttributeTypeNames.StringAttribute].Value, "one");
        }
    }
}
