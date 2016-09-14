// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests.Shared.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using FilterBuilder.Models;
    using NUnit.Framework;
    using Tests.Models;

    public class FilterSchemeFacts
    {
        [TestFixture]
        public class TheToStringMethod
        {
            [TestCase]
            public void WorksCorrectlyOnEmptyFilterScheme()
            {
                var filterScheme = new FilterScheme
                {
                    Title = "Default"
                };

                var actual = filterScheme.ToString();
                var expected = @"Default";

                Assert.AreEqual(expected, actual);
            }

            [TestCase]
            public void WorksCorrectlyOnLargeFilterScheme()
            {
                var filterScheme = FilterSchemeHelper.GenerateFilterScheme();

                var actual = filterScheme.ToString();
                var expected = @"Test filter
(StringProperty contains '123' and BoolProperty is equal to 'True' and IntProperty is greater than or equal to '42') and (StringProperty contains '123' and BoolProperty is equal to 'True' and IntProperty is greater than or equal to '42')";

                Assert.AreEqual(expected, actual);
            }

            [TestCase]
            public void CalculateResultEqualsLinqResultOnFilterScheme()
            {
                var filterScheme = FilterSchemeHelper.GenerateFilterScheme();
                var testCollection = FilterSchemeHelper.GenerateMatchingAndNonMatchingCollection();

                var linqLambdaExpression = filterScheme.ToLinqExpression<TestFilterModel>();

                var filterSchemeCollection = testCollection.Where(item => filterScheme.CalculateResult(item));
                var linqCollection = testCollection.Where(linqLambdaExpression.Compile());

                CollectionAssert.AreEqual(filterSchemeCollection, linqCollection);
            }
        }
    }
}