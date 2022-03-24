namespace Orc.FilterBuilder.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using FilterBuilderControl = Views.FilterBuilderControl;

    [Explicit]
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
        
        protected override void InitializeTarget(string id)
        {
            base.InitializeTarget(id);

            var target = Target;

            target.Execute<InitViewModelMethodRun>();
        }

        [TestCase(FilterBuilderControlTestData.TestScopeWith0CustomRecords)]
        [TestCase(FilterBuilderControlTestData.TestScopeWith3CustomRecords)]
        [TestCase(FilterBuilderControlTestData.TestScopeWith5CustomRecords)]
        public void CorrectlyInitializeScope(string scope)
        {
            //Init raw collection
            var target = Target;
            var current = target.Current;
            current.RawCollection = TestCollection;

            Wait.UntilResponsive();

            //After setting the scope, control should be initialized with corresponding
            //items
            current.Scope = scope;

            Wait.UntilResponsive();

            var expectedFilterSchemes = FilterBuilderControlTestData.GetSchemes(scope);

            //After scope changed items should be initialized as expected
            var actualFilterSchemeItems = target.Items;

            Assert.That(actualFilterSchemeItems[0].Title, Is.EqualTo("Default"));

            //Notice skip first - it should be default
            Assert.That(actualFilterSchemeItems.Skip(1), Is.EquivalentTo(expectedFilterSchemes?.Schemes)
                .Using<FilterBuilderControlListItem, FilterScheme>((x, y) => Equals(x.Title, y.Title)));
        }

        [Test]
        public void CorrectlyStartEditFilterItem()
        {
            //Init raw collection
            var target = Target;
            var current = target.Current;
            current.RawCollection = TestCollection;

            Wait.UntilResponsive();

            //After setting the scope, control should be initialized with corresponding
            //items
            current.Scope = FilterBuilderControlTestData.TestScopeWith5CustomRecords;

            Wait.UntilResponsive();

            var items = target.Items;

            var defaultItem = items[0];

            //Shouldn't be any edit window for Default filter
            Assert.That(defaultItem.CanEdit(), Is.False);

            foreach (var testingItem in items.Skip(1) /*skip default*/)
            {
                var filterEdit = testingItem.Edit();

                //It should be edit window for any item in a filter list
                Assert.That(filterEdit, Is.Not.Null);
                //Title of editing item should match title in Edit window
                Assert.That(testingItem.Title, Is.EqualTo(filterEdit.EditFilterView.Title));

                //Close window by accepting it
                filterEdit.Accept();

                Wait.UntilResponsive(200);
            }
        }

        [Test]
        public void CorrectlyDeleteItem()
        {
            //Init raw collection
            var target = Target;
            var current = target.Current;
            current.RawCollection = TestCollection;

            Wait.UntilResponsive();

            //After setting the scope, control should be initialized with corresponding
            //items
            const string scope = FilterBuilderControlTestData.TestScopeWith5CustomRecords;
            current.Scope = scope;
            var expectedItems = FilterBuilderControlTestData.GetSchemes(scope);

            Wait.UntilResponsive();

            var items = target.Items;

            var defaultItem = items[0];

            //Shouldn't be able to delete default item
            Assert.That(defaultItem.CanEdit(), Is.False);

            var itemToDelete = items[1];
            itemToDelete.Delete();

            items = target.Items;
            
            //Item shouldn't be here in collection
            Assert.That(items, Does.Not.Contains(itemToDelete)
                .Using<FilterBuilderControlListItem, FilterBuilderControlListItem>((x, y) => Equals(x.Title, y.Title)));

            //Count should be equal because expected collection doesn't have default item
            Assert.That(target.Items.Count, Is.EqualTo(expectedItems.Schemes.Count));
        }
    }
}
