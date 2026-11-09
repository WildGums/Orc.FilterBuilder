namespace Orc.FilterBuilder.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Orc.FilterBuilder.Converters;

public class CompareToFieldFacts
{
    private static readonly IPropertyCollection EntityProperties = new InstanceProperties(typeof(FieldComparisonEntity));

    private static List<FieldComparisonEntity> CreateEntities()
    {
        return new List<FieldComparisonEntity>
        {
            new()
            {
                Id = 1,
                StartDate = new DateTime(2024, 1, 17), EndDate = new DateTime(2024, 1, 17), PreviousEndDate = new DateTime(2024, 1, 17),
                Count = 5, OtherCount = 5,
                Name = "Red", OtherName = "Red",
                IsEnabled = true, WasEnabled = true,
                Status = FieldComparisonStatus.Active, PreviousStatus = FieldComparisonStatus.Active
            },
            new()
            {
                Id = 2,
                StartDate = new DateTime(2024, 1, 4), EndDate = new DateTime(2024, 1, 16), PreviousEndDate = null,
                Count = 5, OtherCount = 3,
                Name = "Blue", OtherName = "Red",
                IsEnabled = true, WasEnabled = false,
                Status = FieldComparisonStatus.Closed, PreviousStatus = FieldComparisonStatus.Draft
            },
            new()
            {
                Id = 3,
                StartDate = new DateTime(2024, 1, 20), EndDate = new DateTime(2024, 1, 10), PreviousEndDate = new DateTime(2024, 1, 10),
                Count = 2, OtherCount = 7,
                Name = "Red", OtherName = "Redwood",
                IsEnabled = false, WasEnabled = false,
                Status = FieldComparisonStatus.Draft, PreviousStatus = FieldComparisonStatus.Closed
            },
            new()
            {
                Id = 4,
                StartDate = new DateTime(2024, 1, 1), EndDate = null, PreviousEndDate = null,
                Count = 0, OtherCount = null,
                Name = null, OtherName = "Red",
                IsEnabled = false, WasEnabled = false,
                Status = FieldComparisonStatus.Draft, PreviousStatus = null
            }
        };
    }

    private static PropertyExpression CreatePropertyExpression(string propertyName, Condition condition, string valuePropertyName)
    {
        var propertyExpression = new PropertyExpression
        {
            Property = EntityProperties.GetProperty(propertyName)
        };

        var dataTypeExpression = propertyExpression.DataTypeExpression;
        Assert.That(dataTypeExpression, Is.Not.Null, $"No expression created for '{propertyName}'");

        dataTypeExpression.SelectedCondition = condition;
        dataTypeExpression.ValueProperty = EntityProperties.GetProperty(valuePropertyName);
        Assert.That(dataTypeExpression.ValueProperty, Is.Not.Null, $"Unknown property '{valuePropertyName}'");

        return propertyExpression;
    }

    private static int[] GetMatchingIds(ConditionTreeItem conditionTreeItem)
    {
        return CreateEntities().Where(conditionTreeItem.CalculateResult).Select(x => x.Id).ToArray();
    }

    [TestFixture]
    public class TheCalculateResultMethod
    {
        [TestCase(Condition.EqualTo, new[] { 1 })]
        [TestCase(Condition.NotEqualTo, new[] { 2, 3, 4 })]
        [TestCase(Condition.GreaterThan, new[] { 2 })]
        [TestCase(Condition.GreaterThanOrEqualTo, new[] { 1, 2 })]
        [TestCase(Condition.LessThan, new[] { 3 })]
        [TestCase(Condition.LessThanOrEqualTo, new[] { 1, 3 })]
        [TestCase(Condition.IsNull, new[] { 4 })]
        [TestCase(Condition.NotIsNull, new[] { 1, 2, 3 })]
        public void Compares_Nullable_DateTime_To_DateTime_Field(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), condition, nameof(FieldComparisonEntity.StartDate));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1, 3, 4 })]
        [TestCase(Condition.NotEqualTo, new[] { 2 })]
        [TestCase(Condition.GreaterThan, new int[0])]
        [TestCase(Condition.GreaterThanOrEqualTo, new[] { 1, 3 })]
        public void Treats_Two_Nulls_As_Equal_And_Never_Orders_Nulls(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), condition, nameof(FieldComparisonEntity.PreviousEndDate));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1 })]
        [TestCase(Condition.NotEqualTo, new[] { 2, 3, 4 })]
        [TestCase(Condition.GreaterThan, new[] { 2 })]
        [TestCase(Condition.LessThan, new[] { 3 })]
        public void Compares_Int_To_Nullable_Int_Field(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.Count), condition, nameof(FieldComparisonEntity.OtherCount));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(nameof(FieldComparisonEntity.Name), Condition.EqualTo, nameof(FieldComparisonEntity.OtherName), new[] { 1 })]
        [TestCase(nameof(FieldComparisonEntity.Name), Condition.NotEqualTo, nameof(FieldComparisonEntity.OtherName), new[] { 2, 3, 4 })]
        [TestCase(nameof(FieldComparisonEntity.Name), Condition.Contains, nameof(FieldComparisonEntity.OtherName), new[] { 1 })]
        [TestCase(nameof(FieldComparisonEntity.OtherName), Condition.StartsWith, nameof(FieldComparisonEntity.Name), new[] { 1, 3 })]
        [TestCase(nameof(FieldComparisonEntity.OtherName), Condition.DoesNotStartWith, nameof(FieldComparisonEntity.Name), new[] { 2 })]
        [TestCase(nameof(FieldComparisonEntity.OtherName), Condition.Matches, nameof(FieldComparisonEntity.Name), new[] { 1, 3 })]
        public void Compares_String_To_String_Field_And_Never_Matches_Text_Against_Null(string propertyName, Condition condition, string valuePropertyName, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(propertyName, condition, valuePropertyName);

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1, 3, 4 })]
        [TestCase(Condition.NotEqualTo, new[] { 2 })]
        public void Compares_Bool_To_Bool_Field(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.IsEnabled), condition, nameof(FieldComparisonEntity.WasEnabled));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1 })]
        [TestCase(Condition.NotEqualTo, new[] { 2, 3, 4 })]
        [TestCase(Condition.GreaterThan, new[] { 2 })]
        [TestCase(Condition.LessThan, new[] { 3 })]
        public void Compares_Enum_To_Nullable_Enum_Field(Condition condition, int[] expectedIds)
        {
            // Enum expressions are not created automatically for a new property expression
            var propertyExpression = new PropertyExpression
            {
                Property = EntityProperties.GetProperty(nameof(FieldComparisonEntity.Status)),
                DataTypeExpression = new EnumExpression<FieldComparisonStatus>(false)
                {
                    SelectedCondition = condition,
                    ValueProperty = EntityProperties.GetProperty(nameof(FieldComparisonEntity.PreviousStatus))
                }
            };

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [Test]
        public void Treats_A_Field_Of_Another_Type_As_Null_Instead_Of_Throwing()
        {
            var equalTo = CreatePropertyExpression(nameof(FieldComparisonEntity.Count), Condition.EqualTo, nameof(FieldComparisonEntity.LongCount));
            var notEqualTo = CreatePropertyExpression(nameof(FieldComparisonEntity.Count), Condition.NotEqualTo, nameof(FieldComparisonEntity.LongCount));

            Assert.That(GetMatchingIds(equalTo), Is.Empty);
            Assert.That(GetMatchingIds(notEqualTo), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void Uses_The_Constant_Value_Again_When_The_Field_Is_Cleared()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), Condition.EqualTo, nameof(FieldComparisonEntity.StartDate));
            var dataTypeExpression = (DateTimeExpression)propertyExpression.DataTypeExpression;

            dataTypeExpression.Value = new DateTime(2024, 1, 16);
            dataTypeExpression.ValueProperty = null;

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(new[] { 2 }));
        }
    }

    [TestFixture]
    public class TheBuildLambdaMethod
    {
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.EqualTo, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.NotEqualTo, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.GreaterThan, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.GreaterThanOrEqualTo, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.LessThan, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.LessThanOrEqualTo, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.IsNull, nameof(FieldComparisonEntity.StartDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.EqualTo, nameof(FieldComparisonEntity.PreviousEndDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.NotEqualTo, nameof(FieldComparisonEntity.PreviousEndDate))]
        [TestCase(nameof(FieldComparisonEntity.EndDate), Condition.GreaterThan, nameof(FieldComparisonEntity.PreviousEndDate))]
        [TestCase(nameof(FieldComparisonEntity.Count), Condition.EqualTo, nameof(FieldComparisonEntity.OtherCount))]
        [TestCase(nameof(FieldComparisonEntity.Count), Condition.NotEqualTo, nameof(FieldComparisonEntity.OtherCount))]
        [TestCase(nameof(FieldComparisonEntity.Count), Condition.GreaterThan, nameof(FieldComparisonEntity.OtherCount))]
        [TestCase(nameof(FieldComparisonEntity.Count), Condition.LessThan, nameof(FieldComparisonEntity.OtherCount))]
        [TestCase(nameof(FieldComparisonEntity.Name), Condition.EqualTo, nameof(FieldComparisonEntity.OtherName))]
        [TestCase(nameof(FieldComparisonEntity.Name), Condition.NotEqualTo, nameof(FieldComparisonEntity.OtherName))]
        [TestCase(nameof(FieldComparisonEntity.Name), Condition.Contains, nameof(FieldComparisonEntity.OtherName))]
        [TestCase(nameof(FieldComparisonEntity.OtherName), Condition.StartsWith, nameof(FieldComparisonEntity.Name))]
        [TestCase(nameof(FieldComparisonEntity.OtherName), Condition.EndsWith, nameof(FieldComparisonEntity.Name))]
        [TestCase(nameof(FieldComparisonEntity.IsEnabled), Condition.EqualTo, nameof(FieldComparisonEntity.WasEnabled))]
        [TestCase(nameof(FieldComparisonEntity.IsEnabled), Condition.NotEqualTo, nameof(FieldComparisonEntity.WasEnabled))]
        public void Matches_The_In_Memory_Result(string propertyName, Condition condition, string valuePropertyName)
        {
            var propertyExpression = CreatePropertyExpression(propertyName, condition, valuePropertyName);
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(propertyExpression);

            var predicate = conditionGroup.BuildLambda<FieldComparisonEntity>().Compile();
            var actualIds = CreateEntities().Where(predicate).Select(x => x.Id).ToArray();

            Assert.That(actualIds, Is.EqualTo(GetMatchingIds(propertyExpression)));
        }

        [Test]
        public void Returns_Rows_Where_The_Dates_Differ()
        {
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), Condition.NotEqualTo, nameof(FieldComparisonEntity.StartDate)));

            var predicate = conditionGroup.BuildLambda<FieldComparisonEntity>().Compile();

            Assert.That(CreateEntities().Where(predicate).Select(x => x.Id), Is.EqualTo(new[] { 2, 3, 4 }));
        }

        [Test]
        public void Throws_When_The_Field_Has_Another_Type()
        {
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(CreatePropertyExpression(nameof(FieldComparisonEntity.Count), Condition.EqualTo, nameof(FieldComparisonEntity.LongCount)));

            Assert.Throws<InvalidOperationException>(() => conditionGroup.BuildLambda<FieldComparisonEntity>());
        }
    }

    [TestFixture]
    public class TheToStringMethod
    {
        [Test]
        public void Shows_The_Field_Name_Instead_Of_The_Value()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), Condition.NotEqualTo, nameof(FieldComparisonEntity.StartDate));

            Assert.That(propertyExpression.ToString(), Is.EqualTo("EndDate is not equal to [StartDate]"));
        }
    }

    [TestFixture]
    public class TheEnsureIntegrityMethod
    {
        [Test]
        public async Task Restores_The_Field_After_Save_And_Load_Async()
        {
            var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var filterSerializationService = serviceProvider.GetRequiredService<IFilterSerializationService>();
            var reflectionService = serviceProvider.GetRequiredService<IReflectionService>();

            var filterScheme = new FilterScheme(typeof(FieldComparisonEntity), "Dates differ");
            filterScheme.Root.Items.Add(CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), Condition.NotEqualTo, nameof(FieldComparisonEntity.StartDate)));

            var filterSchemes = new FilterSchemes();
            filterSchemes.Schemes.Add(filterScheme);

            using var temporaryFilesContext = new TemporaryFilesContext("compare-to-field");
            var fileName = temporaryFilesContext.GetFile("filters.json", true);

            await filterSerializationService.SaveFiltersAsync(fileName, filterSchemes);
            var loadedFilterSchemes = await filterSerializationService.LoadFiltersAsync(fileName);

            var loadedFilterScheme = loadedFilterSchemes.Schemes.Single();
            loadedFilterScheme.EnsureIntegrity(reflectionService);

            var loadedPropertyExpression = (PropertyExpression)loadedFilterScheme.Root.Items.Single();
            var loadedValueProperty = loadedPropertyExpression.DataTypeExpression?.ValueProperty;

            var liveProperties = reflectionService.GetInstanceProperties(typeof(FieldComparisonEntity));

            // Both sides must be the live instances, a deserialized one cannot read values
            Assert.That(loadedPropertyExpression.Property, Is.SameAs(liveProperties.GetProperty(nameof(FieldComparisonEntity.EndDate))));
            Assert.That(loadedValueProperty, Is.Not.Null);
            Assert.That(loadedValueProperty.Name, Is.EqualTo(nameof(FieldComparisonEntity.StartDate)));
            Assert.That(loadedValueProperty, Is.SameAs(liveProperties.GetProperty(nameof(FieldComparisonEntity.StartDate))));
            Assert.That(CreateEntities().Where(loadedFilterScheme.CalculateResult).Select(x => x.Id), Is.EqualTo(new[] { 2, 3, 4 }));
        }

        [Test]
        public async Task Restores_The_Property_Of_A_Plain_Class_For_A_Constant_Value_After_Save_And_Load_Async()
        {
            var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var filterSerializationService = serviceProvider.GetRequiredService<IFilterSerializationService>();
            var reflectionService = serviceProvider.GetRequiredService<IReflectionService>();

            var propertyExpression = new PropertyExpression
            {
                Property = EntityProperties.GetProperty(nameof(FieldComparisonEntity.Count))
            };
            var integerExpression = (IntegerExpression)propertyExpression.DataTypeExpression;
            integerExpression.SelectedCondition = Condition.EqualTo;
            integerExpression.Value = 5;

            var filterScheme = new FilterScheme(typeof(FieldComparisonEntity), "Count of five");
            filterScheme.Root.Items.Add(propertyExpression);

            var filterSchemes = new FilterSchemes();
            filterSchemes.Schemes.Add(filterScheme);

            using var temporaryFilesContext = new TemporaryFilesContext("compare-to-field-constant");
            var fileName = temporaryFilesContext.GetFile("filters.json", true);

            await filterSerializationService.SaveFiltersAsync(fileName, filterSchemes);
            var loadedFilterScheme = (await filterSerializationService.LoadFiltersAsync(fileName)).Schemes.Single();
            loadedFilterScheme.EnsureIntegrity(reflectionService);

            Assert.That(CreateEntities().Where(loadedFilterScheme.CalculateResult).Select(x => x.Id), Is.EqualTo(new[] { 1, 2 }));
        }

        [Test]
        public void Keeps_A_Field_That_No_Longer_Exists_Instead_Of_Using_The_Constant_Value()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonEntity.EndDate), Condition.EqualTo, nameof(FieldComparisonEntity.StartDate));
            var dataTypeExpression = (DateTimeExpression)propertyExpression.DataTypeExpression;
            dataTypeExpression.Value = new DateTime(2024, 1, 16);

            var storedValueProperty = new PropertyMetadata
            {
                Name = "RemovedDate",
                OwnerType = typeof(FieldComparisonEntity),
                Type = typeof(DateTime)
            };
            dataTypeExpression.ValueProperty = storedValueProperty;

            var filterScheme = new FilterScheme(typeof(FieldComparisonEntity));
            filterScheme.Root.Items.Add(propertyExpression);

            var reflectionServiceMock = new Mock<IReflectionService>();
            reflectionServiceMock.Setup(x => x.GetInstanceProperties(typeof(FieldComparisonEntity))).Returns(EntityProperties);

            filterScheme.EnsureIntegrity(reflectionServiceMock.Object);

            Assert.That(dataTypeExpression.ValueProperty, Is.SameAs(storedValueProperty));

            // The stored field has no value, so only rows where EndDate is null are equal to it
            Assert.That(GetMatchingIds(filterScheme.Root), Is.EqualTo(new[] { 4 }));
        }
    }

    [TestFixture]
    public class TheCanCompareToMethod
    {
        [TestCase(nameof(FieldComparisonEntity.EndDate), nameof(FieldComparisonEntity.StartDate), true)]
        [TestCase(nameof(FieldComparisonEntity.StartDate), nameof(FieldComparisonEntity.EndDate), true)]
        [TestCase(nameof(FieldComparisonEntity.Name), nameof(FieldComparisonEntity.OtherName), true)]
        [TestCase(nameof(FieldComparisonEntity.Status), nameof(FieldComparisonEntity.PreviousStatus), true)]
        [TestCase(nameof(FieldComparisonEntity.Count), nameof(FieldComparisonEntity.LongCount), false)]
        [TestCase(nameof(FieldComparisonEntity.Count), nameof(FieldComparisonEntity.Name), false)]
        [TestCase(nameof(FieldComparisonEntity.StartDate), nameof(FieldComparisonEntity.StartDate), false)]
        public void Returns_Whether_The_Fields_Have_The_Same_Type(string propertyName, string otherPropertyName, bool expected)
        {
            var property = EntityProperties.GetProperty(propertyName);
            var otherProperty = EntityProperties.GetProperty(otherPropertyName);

            Assert.That(property.CanCompareTo(otherProperty), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class TheComparablePropertiesMultiValueConverter
    {
        [Test]
        public void Returns_Other_Fields_Of_The_Same_Type()
        {
            var converter = new ComparablePropertiesMultiValueConverter();
            var property = EntityProperties.GetProperty(nameof(FieldComparisonEntity.EndDate));

            var result = (IEnumerable<IPropertyMetadata>)converter.Convert(new object[] { property, EntityProperties.Properties }, typeof(object), null, null);

            Assert.That(result.Select(x => x.Name), Is.EqualTo(new[] { nameof(FieldComparisonEntity.PreviousEndDate), nameof(FieldComparisonEntity.StartDate) }));
        }

        [Test]
        public void Reuses_The_Result_For_The_Same_Field()
        {
            var converter = new ComparablePropertiesMultiValueConverter();
            var property = EntityProperties.GetProperty(nameof(FieldComparisonEntity.EndDate));

            var first = converter.Convert(new object[] { property, EntityProperties.Properties }, typeof(object), null, null);
            var second = converter.Convert(new object[] { property, EntityProperties.Properties }, typeof(object), null, null);

            Assert.That(second, Is.SameAs(first));
        }

        [Test]
        public void Returns_Empty_List_When_No_Property_Is_Selected()
        {
            var converter = new ComparablePropertiesMultiValueConverter();

            var result = (IEnumerable<IPropertyMetadata>)converter.Convert(new object[] { null, EntityProperties.Properties }, typeof(object), null, null);

            Assert.That(result, Is.Empty);
        }
    }
}

public enum FieldComparisonStatus
{
    Draft,
    Active,
    Closed
}

public class FieldComparisonEntity
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? PreviousEndDate { get; set; }

    public int Count { get; set; }

    public int? OtherCount { get; set; }

    public long LongCount { get; set; }

    public string Name { get; set; }

    public string OtherName { get; set; }

    public bool IsEnabled { get; set; }

    public bool WasEnabled { get; set; }

    public FieldComparisonStatus Status { get; set; }

    public FieldComparisonStatus? PreviousStatus { get; set; }
}
