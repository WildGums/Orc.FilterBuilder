using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Orc.FilterBuilder.Models;
using Orc.FilterBuilder.Conditions;
using System.Linq.Expressions;

namespace Orc.FilterBuilder.Tests
{
    [TestFixture]
    public class LinqExpressionFacts
    {
        [TestCase]
        public void Bool_ExpressionTest()
        {
            var items = CalcHumansFor("HasChailds", new BooleanExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = true 
            });
            Assert.True(ListEquals(items, People.Where((t) => t.HasChailds ).ToList()));

             items = CalcHumansFor("Married", new BooleanExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = true
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Married.HasValue && t.Married.Value).ToList()));


        }

        [TestCase]
        public void String_ExpressionTest()
        {
            var items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.Contains,
                Value = "nn"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name != null && t.Name.Contains("nn")).ToList()));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.DoesNotContain,
                Value = "nn"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name == null || !t.Name.Contains("nn")).ToList()));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.StartsWith,
                Value = "A"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name != null && t.Name.StartsWith("A")).ToList()));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.DoesNotStartWith,
                Value = "A"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name == null || !t.Name.StartsWith("A")).ToList()));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.EndsWith,
                Value = "io"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name != null && t.Name.EndsWith("io")).ToList()));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.DoesNotEndWith,
                Value = "io"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name == null || !t.Name.EndsWith("io")).ToList()));


            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = "Ann"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name != null && t.Name == "Ann").ToList()));



            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = "Sergio"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.Name == null || t.Name != "Sergio").ToList()));



            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = "Ann"
            });

            Assert.True(ListEquals(
                items,
                People.Where((t) => string.Compare(t.Name, "Ann", StringComparison.InvariantCultureIgnoreCase) > 0).ToList()
                ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = "Ann"
            });
            Assert.True(ListEquals(
               items,
               People.Where((t) => string.Compare(t.Name, "Ann", StringComparison.InvariantCultureIgnoreCase) >= 0).ToList()
               ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = "Sergio"
            });
            Assert.True(ListEquals(
     items,
     People.Where((t) => string.Compare(t.Name, "Sergio", StringComparison.InvariantCultureIgnoreCase) < 0).ToList()
     ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = "Sergio"
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => string.Compare(t.Name, "Sergio", StringComparison.InvariantCultureIgnoreCase) <= 0).ToList()
            ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => t.Name==null).ToList()
            ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => t.Name != null).ToList()
            ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.IsEmpty,
            });
            Assert.True(ListEquals(
                    items,
                  People.Where((t) => t.Name == string.Empty ).ToList()
            ));

            items = CalcHumansFor("Name", new StringExpression()
            {
                SelectedCondition = Condition.NotIsEmpty,
            });
            Assert.True(ListEquals(
                    items,
                   People.Where((t) => t.Name != string.Empty).ToList()
            ));
        }

        bool ListEquals(IList<Human> m1, IList<Human> m2)
        {
            if (m1 == null || m2 == null) return false;
            if (m1.Count != m2.Count) return false;
            var c = m1.Count;
            for (int i = 0; i < c; i++)
            {
                if (m1[i].Name != m2[i].Name)
                {
                    return false;
                }
            }
            return true;
        }

        List<Human> CalcHumansFor(string propertyName, DataTypeExpression dataTypeExpression)

        {
            var conditionGroup = new ConditionGroup()
            {
                Type = ConditionGroupType.And
            };

            var type = typeof(Human);

            var propertyExpression = new PropertyExpression()
            {
                Property = new PropertyMetadata(type, type.GetProperty(propertyName)),
                DataTypeExpression = dataTypeExpression
            };

            conditionGroup.Items.Add(propertyExpression);

            var expression = conditionGroup.MakeFunction<Human>();
            var result = People.AsQueryable().Where(expression).ToList();
            return result;
        }
        
        public List<Human> People => new List<Human>()
        {
            new Human("Ann",5,false),
            new Human("Sergio", 35, true ),
            new Human("", 35, true ),
            new Human(null, 35, true )

        };

        public class Human
        {
            public Human(string name, int age, bool hasChailds)
            {
                Name = name;
                Age = age;
                HasChailds = hasChailds;
            }
            public string Name { get; set; }
            public int Age { get; set; }
            public bool HasChailds { get; set; }
            public bool? Married { get; set; }
        }

    }
}