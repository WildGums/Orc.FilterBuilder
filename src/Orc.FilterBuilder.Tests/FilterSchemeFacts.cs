namespace Orc.FilterBuilder.Tests.Models;

using NUnit.Framework;

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
    }
}