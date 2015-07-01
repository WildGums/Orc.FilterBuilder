// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterSchemeFacts.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Tests.Shared.Models
{
    using NUnit.Framework;
    using Tests.Models;

    public class FilterSchemeFacts
    {
        [TestFixture]
        public class TheToStringMethod
        {
            [TestCase]
            public void WorksCorrectly()
            {
                var filterScheme = FilterSchemeHelper.GenerateFilterScheme();

                var actual = filterScheme.ToString();
                var expected = @"Test filter
(StringProperty contains '123' and BoolProperty is equal to 'True' and IntProperty is greater than or equal to '42') and (StringProperty contains '123' and BoolProperty is equal to 'True' and IntProperty is greater than or equal to '42')";

                Assert.AreEqual(expected, actual);
            }
        }
    }
}