namespace Orc.FilterBuilder.Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Catel.IoC;
using NUnit.Framework;

[TestFixture]
public class RuntimeMetadataFacts
{
    [Test]
    public async Task CorrectlyFilterEntitiesWithRuntimeMetadataAsync()
    {
        var serviceLocator = ServiceLocator.Default;

#pragma warning disable IDISP001 // Dispose created
        var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
#pragma warning restore IDISP001 // Dispose created

        var filterScheme = await FilterSchemeInitializationHelper.GetTestFilterSchemeAsync();
        var initialCollection = TestDataProvider.GetInitialCollection();
        var resultList = new List<TestFilterRuntimeModel>();

        var filterService = typeFactory.CreateInstance<FilterService>();
        await filterService.FilterCollectionAsync(filterScheme, initialCollection, resultList);

        Assert.AreEqual(1, resultList.Count);
        Assert.AreEqual("one", resultList[0].Attributes[AttributeTypeNames.StringAttribute].Value);
    }
}