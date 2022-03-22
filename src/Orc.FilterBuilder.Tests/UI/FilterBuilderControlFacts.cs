namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Orc.Automation;
    using Views;

    [TestFixture]
    public class FilterBuilderControlFacts : StyledControlTestFacts<FilterBuilderControl>
    {
        [Target]
        public Automation.FilterBuilderControl Target { get; set; }

        public static IReadOnlyList<TestEntity> TestCollection =>
            new List<TestEntity>
            {
                new() { Age = 1, FirstName = "Record 1", DateOfBirth = DateTime.Today},
                new() { Age = 2, FirstName = "Record 2", DateOfBirth = new DateTime(1989, 7, 4)},
                new() { Age = 3, FirstName = "Record 3", DateOfBirth = new DateTime(1989, 4, 17)},
            };

        [Test]
        public void Correctly()
        {
            var target = Target;
            var current = target.Current;
            current.RawCollection = TestCollection;

            current.Scope = "1";
        }
    }
}
