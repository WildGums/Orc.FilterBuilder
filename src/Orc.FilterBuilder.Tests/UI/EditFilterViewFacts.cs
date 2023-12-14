namespace Orc.FilterBuilder.Tests;

using System;
using System.Collections.Generic;
using Automation;
using NUnit.Framework;
using Orc.Automation;
using EditFilterView = Views.EditFilterView;

[Explicit]
[TestFixture]
public class EditFilterViewFacts : StyledControlTestFacts<EditFilterView>
{
    [Target]
    public Automation.EditFilterView Target { get; set; }

    public static IReadOnlyList<TestEntity> TestCollection =>
        new List<TestEntity>
        {
            new() { Age = 1, FirstName = "Record 1", DateOfBirth = DateTime.Today},
            new() { Age = 2, FirstName = "Record 2", DateOfBirth = new DateTime(1989, 7, 4)},
            new() { Age = 3, FirstName = "Record 3", DateOfBirth = new DateTime(1989, 4, 17)},
        };

    [Test]
    public void VerifyApi()
    {
        var target = Target;
        var model = target.Current;

        var filterScheme = FilterSchemeBuilder.Start<TestEntity>()
            .Or()
            .And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 1)
            .Property(nameof(TestEntity.FirstName), Condition.Contains, "1")
            .FinishConditionGroup()

            .And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 2)
            .Property(nameof(TestEntity.FirstName), Condition.Contains, "2")
            .Property(nameof(TestEntity.DateOfBirth), Condition.EqualTo, new DateTime(1989, 4, 7))
            .FinishConditionGroup()

            .ToFilterScheme();

        model.FilterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, TestCollection, true, true);

        EditFilterViewAssert.Match(target, filterScheme);
    }

    [Test]
    public void CorrectlyInitializeTree()
    {
        var target = Target;
        var model = target.Current;

        InitializeTestTree();

        var scheme = model.FilterSchemeEditInfo.FilterScheme;

        EditFilterViewAssert.Match(target, scheme);
    }

    [Test]
    public void CorrectlyFilterPreview()
    {
        var target = Target;
        var model = target.Current;
        var testCollection = TestCollection;

        //Init filter
        var filterScheme = FilterSchemeBuilder.Start<TestEntity>()
            .Or()
            .And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 1)
            .Property(nameof(TestEntity.FirstName), Condition.Contains, "1")
            .FinishConditionGroup()

            .And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 2)
            .Property(nameof(TestEntity.FirstName), Condition.Contains, "2")
            .FinishConditionGroup()

            .ToFilterScheme();
        model.FilterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, testCollection, true, true);

        var expectedPreviewCollection = new List<TestEntity>();
        filterScheme.Apply(testCollection, expectedPreviewCollection);

        target.IsLivePreviewEnabled = true;
        target.IsPreviewCollectionVisible = true;

        Wait.UntilResponsive();

        var previewCollection = target.PreviewCollection;

        Assert.That(expectedPreviewCollection, Is.EquivalentTo(previewCollection));
    }

    private void InitializeTestTree()
    {
        var target = Target;
        var testCollection = TestCollection;

        target.Initialize(testCollection);
        var root = target.Root as EditFilterConditionGroupTreeItem;

        root.And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 1)
            .Property(nameof(TestEntity.DateOfBirth), Condition.GreaterThan, DateTime.Today)
            .Property(nameof(TestEntity.FirstName), Condition.Contains, "1")
            .Property(nameof(TestEntity.IsActive), Condition.EqualTo, true)
            .Or()
            .Property(nameof(TestEntity.IsActive), Condition.EqualTo, false)
            .Property(nameof(TestEntity.Duration), Condition.NotEqualTo, TimeSpan.FromMilliseconds(1000))
            .Property(nameof(TestEntity.FirstName), Condition.IsEmpty)
            .FinishCondition()
            .FinishCondition()

            .And()
            .Property(nameof(TestEntity.Age), Condition.EqualTo, 2)
            .Property(nameof(TestEntity.DateOfBirth), Condition.GreaterThan, DateTime.Today)
            .Property(nameof(TestEntity.Duration), Condition.LessThan, TimeSpan.FromMilliseconds(200000))
            .FinishCondition();
    }
}