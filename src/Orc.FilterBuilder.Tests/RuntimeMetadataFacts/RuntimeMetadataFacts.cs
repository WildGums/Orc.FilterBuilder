// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuntimeMetadataFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests
{
    using System.Collections.Generic;
    using Catel.IoC;
    using FilterBuilder.Services;
    using NUnit.Framework;

    [TestFixture]
    public class RuntimeMetadataFacts
    {
        [Test]
        public void CorrectlyFilterEntitiesWithRuntimeMetadata()
        {
            var serviceLocator = ServiceLocator.Default;

            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            
            var filterScheme = FilterSchemeInitializationHelper.GetTestFilterScheme();
            var initialCollection = TestDataProvider.GetInitialCollection();
            var resultList = new List<TestFilterRuntimeModel>();

            var filterService = typeFactory.CreateInstanceWithParametersAndAutoCompletion<FilterService>();
            filterService.FilterCollection(filterScheme, initialCollection, resultList);

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(resultList[0].Attributes[AttributeTypeNames.StringAttribute].Value, "one");
        }
    }
}
