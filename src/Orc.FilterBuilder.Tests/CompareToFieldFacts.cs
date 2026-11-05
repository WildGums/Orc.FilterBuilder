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
    private static readonly IPropertyCollection OrderProperties = new InstanceProperties(typeof(FieldComparisonOrder));

    private static List<FieldComparisonOrder> CreateOrders()
    {
        return new List<FieldComparisonOrder>
        {
            new()
            {
                Id = 1,
                PromiseDate = new DateTime(2026, 11, 17), NewPromiseDate = new DateTime(2026, 11, 17), PreviousPromiseDate = new DateTime(2026, 11, 17),
                Quantity = 5, DeliveredQuantity = 5,
                Route = "PAINT", PreviousRoute = "PAINT",
                IsPlanned = true, WasPlanned = true,
                CurrentStep = FieldComparisonStep.Process, PreviousStep = FieldComparisonStep.Process
            },
            new()
            {
                Id = 2,
                PromiseDate = new DateTime(2026, 11, 4), NewPromiseDate = new DateTime(2026, 11, 16), PreviousPromiseDate = null,
                Quantity = 5, DeliveredQuantity = 3,
                Route = "PREGAL", PreviousRoute = "PAINT",
                IsPlanned = true, WasPlanned = false,
                CurrentStep = FieldComparisonStep.Paint, PreviousStep = FieldComparisonStep.Load
            },
            new()
            {
                Id = 3,
                PromiseDate = new DateTime(2026, 11, 20), NewPromiseDate = new DateTime(2026, 11, 10), PreviousPromiseDate = new DateTime(2026, 11, 10),
                Quantity = 2, DeliveredQuantity = 7,
                Route = "PAINT", PreviousRoute = "PAINTED",
                IsPlanned = false, WasPlanned = false,
                CurrentStep = FieldComparisonStep.Load, PreviousStep = FieldComparisonStep.Paint
            },
            new()
            {
                Id = 4,
                PromiseDate = new DateTime(2026, 11, 1), NewPromiseDate = null, PreviousPromiseDate = null,
                Quantity = 0, DeliveredQuantity = null,
                Route = null, PreviousRoute = "PAINT",
                IsPlanned = false, WasPlanned = false,
                CurrentStep = FieldComparisonStep.Load, PreviousStep = null
            }
        };
    }

    private static PropertyExpression CreatePropertyExpression(string propertyName, Condition condition, string valuePropertyName)
    {
        var propertyExpression = new PropertyExpression
        {
            Property = OrderProperties.GetProperty(propertyName)
        };

        var dataTypeExpression = propertyExpression.DataTypeExpression;
        Assert.That(dataTypeExpression, Is.Not.Null, $"No expression created for '{propertyName}'");

        dataTypeExpression.SelectedCondition = condition;
        dataTypeExpression.ValueProperty = OrderProperties.GetProperty(valuePropertyName);
        Assert.That(dataTypeExpression.ValueProperty, Is.Not.Null, $"Unknown property '{valuePropertyName}'");

        return propertyExpression;
    }

    private static int[] GetMatchingIds(ConditionTreeItem conditionTreeItem)
    {
        return CreateOrders().Where(conditionTreeItem.CalculateResult).Select(x => x.Id).ToArray();
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
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), condition, nameof(FieldComparisonOrder.PromiseDate));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1, 3, 4 })]
        [TestCase(Condition.NotEqualTo, new[] { 2 })]
        [TestCase(Condition.GreaterThan, new int[0])]
        [TestCase(Condition.GreaterThanOrEqualTo, new[] { 1, 3 })]
        public void Treats_Two_Nulls_As_Equal_And_Never_Orders_Nulls(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), condition, nameof(FieldComparisonOrder.PreviousPromiseDate));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1 })]
        [TestCase(Condition.NotEqualTo, new[] { 2, 3, 4 })]
        [TestCase(Condition.GreaterThan, new[] { 2 })]
        [TestCase(Condition.LessThan, new[] { 3 })]
        public void Compares_Int_To_Nullable_Int_Field(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.Quantity), condition, nameof(FieldComparisonOrder.DeliveredQuantity));

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(nameof(FieldComparisonOrder.Route), Condition.EqualTo, nameof(FieldComparisonOrder.PreviousRoute), new[] { 1 })]
        [TestCase(nameof(FieldComparisonOrder.Route), Condition.NotEqualTo, nameof(FieldComparisonOrder.PreviousRoute), new[] { 2, 3, 4 })]
        [TestCase(nameof(FieldComparisonOrder.Route), Condition.Contains, nameof(FieldComparisonOrder.PreviousRoute), new[] { 1 })]
        [TestCase(nameof(FieldComparisonOrder.PreviousRoute), Condition.StartsWith, nameof(FieldComparisonOrder.Route), new[] { 1, 3 })]
        [TestCase(nameof(FieldComparisonOrder.PreviousRoute), Condition.DoesNotStartWith, nameof(FieldComparisonOrder.Route), new[] { 2 })]
        [TestCase(nameof(FieldComparisonOrder.PreviousRoute), Condition.Matches, nameof(FieldComparisonOrder.Route), new[] { 1, 3 })]
        public void Compares_String_To_String_Field_And_Never_Matches_Text_Against_Null(string propertyName, Condition condition, string valuePropertyName, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(propertyName, condition, valuePropertyName);

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [TestCase(Condition.EqualTo, new[] { 1, 3, 4 })]
        [TestCase(Condition.NotEqualTo, new[] { 2 })]
        public void Compares_Bool_To_Bool_Field(Condition condition, int[] expectedIds)
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.IsPlanned), condition, nameof(FieldComparisonOrder.WasPlanned));

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
                Property = OrderProperties.GetProperty(nameof(FieldComparisonOrder.CurrentStep)),
                DataTypeExpression = new EnumExpression<FieldComparisonStep>(false)
                {
                    SelectedCondition = condition,
                    ValueProperty = OrderProperties.GetProperty(nameof(FieldComparisonOrder.PreviousStep))
                }
            };

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(expectedIds));
        }

        [Test]
        public void Treats_A_Field_Of_Another_Type_As_Null_Instead_Of_Throwing()
        {
            var equalTo = CreatePropertyExpression(nameof(FieldComparisonOrder.Quantity), Condition.EqualTo, nameof(FieldComparisonOrder.LongQuantity));
            var notEqualTo = CreatePropertyExpression(nameof(FieldComparisonOrder.Quantity), Condition.NotEqualTo, nameof(FieldComparisonOrder.LongQuantity));

            Assert.That(GetMatchingIds(equalTo), Is.Empty);
            Assert.That(GetMatchingIds(notEqualTo), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void Uses_The_Constant_Value_Again_When_The_Field_Is_Cleared()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), Condition.EqualTo, nameof(FieldComparisonOrder.PromiseDate));
            var dataTypeExpression = (DateTimeExpression)propertyExpression.DataTypeExpression;

            dataTypeExpression.Value = new DateTime(2026, 11, 16);
            dataTypeExpression.ValueProperty = null;

            Assert.That(GetMatchingIds(propertyExpression), Is.EqualTo(new[] { 2 }));
        }
    }

    [TestFixture]
    public class TheBuildLambdaMethod
    {
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.EqualTo, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.NotEqualTo, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.GreaterThan, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.GreaterThanOrEqualTo, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.LessThan, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.LessThanOrEqualTo, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.IsNull, nameof(FieldComparisonOrder.PromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.EqualTo, nameof(FieldComparisonOrder.PreviousPromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.NotEqualTo, nameof(FieldComparisonOrder.PreviousPromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), Condition.GreaterThan, nameof(FieldComparisonOrder.PreviousPromiseDate))]
        [TestCase(nameof(FieldComparisonOrder.Quantity), Condition.EqualTo, nameof(FieldComparisonOrder.DeliveredQuantity))]
        [TestCase(nameof(FieldComparisonOrder.Quantity), Condition.NotEqualTo, nameof(FieldComparisonOrder.DeliveredQuantity))]
        [TestCase(nameof(FieldComparisonOrder.Quantity), Condition.GreaterThan, nameof(FieldComparisonOrder.DeliveredQuantity))]
        [TestCase(nameof(FieldComparisonOrder.Quantity), Condition.LessThan, nameof(FieldComparisonOrder.DeliveredQuantity))]
        [TestCase(nameof(FieldComparisonOrder.Route), Condition.EqualTo, nameof(FieldComparisonOrder.PreviousRoute))]
        [TestCase(nameof(FieldComparisonOrder.Route), Condition.NotEqualTo, nameof(FieldComparisonOrder.PreviousRoute))]
        [TestCase(nameof(FieldComparisonOrder.Route), Condition.Contains, nameof(FieldComparisonOrder.PreviousRoute))]
        [TestCase(nameof(FieldComparisonOrder.PreviousRoute), Condition.StartsWith, nameof(FieldComparisonOrder.Route))]
        [TestCase(nameof(FieldComparisonOrder.PreviousRoute), Condition.EndsWith, nameof(FieldComparisonOrder.Route))]
        [TestCase(nameof(FieldComparisonOrder.IsPlanned), Condition.EqualTo, nameof(FieldComparisonOrder.WasPlanned))]
        [TestCase(nameof(FieldComparisonOrder.IsPlanned), Condition.NotEqualTo, nameof(FieldComparisonOrder.WasPlanned))]
        public void Matches_The_In_Memory_Result(string propertyName, Condition condition, string valuePropertyName)
        {
            var propertyExpression = CreatePropertyExpression(propertyName, condition, valuePropertyName);
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(propertyExpression);

            var predicate = conditionGroup.BuildLambda<FieldComparisonOrder>().Compile();
            var actualIds = CreateOrders().Where(predicate).Select(x => x.Id).ToArray();

            Assert.That(actualIds, Is.EqualTo(GetMatchingIds(propertyExpression)));
        }

        [Test]
        public void Returns_Changed_Promise_Dates()
        {
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), Condition.NotEqualTo, nameof(FieldComparisonOrder.PromiseDate)));

            var predicate = conditionGroup.BuildLambda<FieldComparisonOrder>().Compile();

            Assert.That(CreateOrders().Where(predicate).Select(x => x.Id), Is.EqualTo(new[] { 2, 3, 4 }));
        }

        [Test]
        public void Throws_When_The_Field_Has_Another_Type()
        {
            var conditionGroup = new ConditionGroup
            {
                Type = ConditionGroupType.And
            };
            conditionGroup.Items.Add(CreatePropertyExpression(nameof(FieldComparisonOrder.Quantity), Condition.EqualTo, nameof(FieldComparisonOrder.LongQuantity)));

            Assert.Throws<InvalidOperationException>(() => conditionGroup.BuildLambda<FieldComparisonOrder>());
        }
    }

    [TestFixture]
    public class TheToStringMethod
    {
        [Test]
        public void Shows_The_Field_Name_Instead_Of_The_Value()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), Condition.NotEqualTo, nameof(FieldComparisonOrder.PromiseDate));

            Assert.That(propertyExpression.ToString(), Is.EqualTo("NewPromiseDate is not equal to [PromiseDate]"));
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

            var filterScheme = new FilterScheme(typeof(FieldComparisonOrder), "Changed promise dates");
            filterScheme.Root.Items.Add(CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), Condition.NotEqualTo, nameof(FieldComparisonOrder.PromiseDate)));

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

            var liveProperties = reflectionService.GetInstanceProperties(typeof(FieldComparisonOrder));

            // Both sides must be the live instances, a deserialized one cannot read values
            Assert.That(loadedPropertyExpression.Property, Is.SameAs(liveProperties.GetProperty(nameof(FieldComparisonOrder.NewPromiseDate))));
            Assert.That(loadedValueProperty, Is.Not.Null);
            Assert.That(loadedValueProperty.Name, Is.EqualTo(nameof(FieldComparisonOrder.PromiseDate)));
            Assert.That(loadedValueProperty, Is.SameAs(liveProperties.GetProperty(nameof(FieldComparisonOrder.PromiseDate))));
            Assert.That(CreateOrders().Where(loadedFilterScheme.CalculateResult).Select(x => x.Id), Is.EqualTo(new[] { 2, 3, 4 }));
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
                Property = OrderProperties.GetProperty(nameof(FieldComparisonOrder.Quantity))
            };
            var integerExpression = (IntegerExpression)propertyExpression.DataTypeExpression;
            integerExpression.SelectedCondition = Condition.EqualTo;
            integerExpression.Value = 5;

            var filterScheme = new FilterScheme(typeof(FieldComparisonOrder), "Quantity of five");
            filterScheme.Root.Items.Add(propertyExpression);

            var filterSchemes = new FilterSchemes();
            filterSchemes.Schemes.Add(filterScheme);

            using var temporaryFilesContext = new TemporaryFilesContext("compare-to-field-constant");
            var fileName = temporaryFilesContext.GetFile("filters.json", true);

            await filterSerializationService.SaveFiltersAsync(fileName, filterSchemes);
            var loadedFilterScheme = (await filterSerializationService.LoadFiltersAsync(fileName)).Schemes.Single();
            loadedFilterScheme.EnsureIntegrity(reflectionService);

            Assert.That(CreateOrders().Where(loadedFilterScheme.CalculateResult).Select(x => x.Id), Is.EqualTo(new[] { 1, 2 }));
        }

        [Test]
        public void Keeps_A_Field_That_No_Longer_Exists_Instead_Of_Using_The_Constant_Value()
        {
            var propertyExpression = CreatePropertyExpression(nameof(FieldComparisonOrder.NewPromiseDate), Condition.EqualTo, nameof(FieldComparisonOrder.PromiseDate));
            var dataTypeExpression = (DateTimeExpression)propertyExpression.DataTypeExpression;
            dataTypeExpression.Value = new DateTime(2026, 11, 16);

            var storedValueProperty = new PropertyMetadata
            {
                Name = "RemovedPromiseDate",
                OwnerType = typeof(FieldComparisonOrder),
                Type = typeof(DateTime)
            };
            dataTypeExpression.ValueProperty = storedValueProperty;

            var filterScheme = new FilterScheme(typeof(FieldComparisonOrder));
            filterScheme.Root.Items.Add(propertyExpression);

            var reflectionServiceMock = new Mock<IReflectionService>();
            reflectionServiceMock.Setup(x => x.GetInstanceProperties(typeof(FieldComparisonOrder))).Returns(OrderProperties);

            filterScheme.EnsureIntegrity(reflectionServiceMock.Object);

            Assert.That(dataTypeExpression.ValueProperty, Is.SameAs(storedValueProperty));

            // The stored field has no value, so only rows where NewPromiseDate is null are equal to it
            Assert.That(GetMatchingIds(filterScheme.Root), Is.EqualTo(new[] { 4 }));
        }
    }

    [TestFixture]
    public class TheCanCompareToMethod
    {
        [TestCase(nameof(FieldComparisonOrder.NewPromiseDate), nameof(FieldComparisonOrder.PromiseDate), true)]
        [TestCase(nameof(FieldComparisonOrder.PromiseDate), nameof(FieldComparisonOrder.NewPromiseDate), true)]
        [TestCase(nameof(FieldComparisonOrder.Route), nameof(FieldComparisonOrder.PreviousRoute), true)]
        [TestCase(nameof(FieldComparisonOrder.CurrentStep), nameof(FieldComparisonOrder.PreviousStep), true)]
        [TestCase(nameof(FieldComparisonOrder.Quantity), nameof(FieldComparisonOrder.LongQuantity), false)]
        [TestCase(nameof(FieldComparisonOrder.Quantity), nameof(FieldComparisonOrder.Route), false)]
        [TestCase(nameof(FieldComparisonOrder.PromiseDate), nameof(FieldComparisonOrder.PromiseDate), false)]
        public void Returns_Whether_The_Fields_Have_The_Same_Type(string propertyName, string otherPropertyName, bool expected)
        {
            var property = OrderProperties.GetProperty(propertyName);
            var otherProperty = OrderProperties.GetProperty(otherPropertyName);

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
            var property = OrderProperties.GetProperty(nameof(FieldComparisonOrder.NewPromiseDate));

            var result = (IEnumerable<IPropertyMetadata>)converter.Convert(new object[] { property, OrderProperties.Properties }, typeof(object), null, null);

            Assert.That(result.Select(x => x.Name), Is.EqualTo(new[] { nameof(FieldComparisonOrder.PreviousPromiseDate), nameof(FieldComparisonOrder.PromiseDate) }));
        }

        [Test]
        public void Returns_Empty_List_When_No_Property_Is_Selected()
        {
            var converter = new ComparablePropertiesMultiValueConverter();

            var result = (IEnumerable<IPropertyMetadata>)converter.Convert(new object[] { null, OrderProperties.Properties }, typeof(object), null, null);

            Assert.That(result, Is.Empty);
        }
    }
}

public enum FieldComparisonStep
{
    Load,
    Process,
    Paint
}

public class FieldComparisonOrder
{
    public int Id { get; set; }

    public DateTime PromiseDate { get; set; }

    public DateTime? NewPromiseDate { get; set; }

    public DateTime? PreviousPromiseDate { get; set; }

    public int Quantity { get; set; }

    public int? DeliveredQuantity { get; set; }

    public long LongQuantity { get; set; }

    public string Route { get; set; }

    public string PreviousRoute { get; set; }

    public bool IsPlanned { get; set; }

    public bool WasPlanned { get; set; }

    public FieldComparisonStep CurrentStep { get; set; }

    public FieldComparisonStep? PreviousStep { get; set; }
}
