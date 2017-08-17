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
        public void LinqExpressions_BoolTest()
        {
            var items = CalcHumansFor("BOOL", new BooleanExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = true
            });
            Assert.True(ListEquals(items, People.Where((t) => t.BOOL).ToList()));

            items = CalcHumansFor("NullBOOL", new BooleanExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = true
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullBOOL.HasValue && t.NullBOOL.Value).ToList()));


        }

        [TestCase]
        public void String_ExpressionTest()
        {
            string propertyName = "STRING";
            var items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.Contains,
                Value = "nn"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING != null && t.STRING.Contains("nn")).ToList()));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.DoesNotContain,
                Value = "nn"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING == null || !t.STRING.Contains("nn")).ToList()));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.StartsWith,
                Value = "A"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING != null && t.STRING.StartsWith("A")).ToList()));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.DoesNotStartWith,
                Value = "A"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING == null || !t.STRING.StartsWith("A")).ToList()));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.EndsWith,
                Value = "io"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING != null && t.STRING.EndsWith("io")).ToList()));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.DoesNotEndWith,
                Value = "io"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING == null || !t.STRING.EndsWith("io")).ToList()));


            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = "Ann"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING != null && t.STRING == "Ann").ToList()));



            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = "Sergio"
            });
            Assert.True(ListEquals(items, People.Where((t) => t.STRING == null || t.STRING != "Sergio").ToList()));



            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = "Ann"
            });

            Assert.True(ListEquals(
                items,
                People.Where((t) => string.Compare(t.STRING, "Ann", StringComparison.InvariantCultureIgnoreCase) > 0).ToList()
                ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = "Ann"
            });
            Assert.True(ListEquals(
               items,
               People.Where((t) => string.Compare(t.STRING, "Ann", StringComparison.InvariantCultureIgnoreCase) >= 0).ToList()
               ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = "Sergio"
            });
            Assert.True(ListEquals(
     items,
     People.Where((t) => string.Compare(t.STRING, "Sergio", StringComparison.InvariantCultureIgnoreCase) < 0).ToList()
     ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = "Sergio"
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => string.Compare(t.STRING, "Sergio", StringComparison.InvariantCultureIgnoreCase) <= 0).ToList()
            ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => t.STRING == null).ToList()
            ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(
                    items,
                    People.Where((t) => t.STRING != null).ToList()
            ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.IsEmpty,
            });
            Assert.True(ListEquals(
                    items,
                  People.Where((t) => t.STRING == string.Empty).ToList()
            ));

            items = CalcHumansFor(propertyName, new StringExpression()
            {
                SelectedCondition = Condition.NotIsEmpty,
            });
            Assert.True(ListEquals(
                    items,
                   People.Where((t) => t.STRING != string.Empty).ToList()
            ));
        }
        [TestCase]
        public void LinqExpressions_IntTest()
        {
            var items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT == 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT == 5).ToList()));

            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT != 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT != 5).ToList()));

            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT > 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT > 5).ToList()));

            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT >= 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT >= 5).ToList()));


            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT < 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT < 5).ToList()));

            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.INT <= 5).ToList()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = 5
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT <= 5).ToList()));

            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(items, new List<Human>()));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT == null).ToList()));


            items = CalcHumansFor("INT", new IntegerExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(items, People));

            items = CalcHumansFor("NullINT", new IntegerExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullINT != null).ToList()));
        }
        [TestCase]
        public void LinqExpressions_DateTimeTest()
        {
            var items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE == new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.EqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE == new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE != new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE != new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE > new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.GreaterThan,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE > new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE >= new DateTime(1990, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE >= new DateTime(1990, 1, 1)).ToList()));


            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = new DateTime(2000, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE < new DateTime(2000, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.LessThan,
                Value = new DateTime(2000, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE < new DateTime(2000, 1, 1)).ToList()));

            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = new DateTime(2000, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE <= new DateTime(2000, 1, 1)).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = new DateTime(2000, 1, 1)
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE <= new DateTime(2000, 1, 1)).ToList()));

            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE == null).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.IsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE == null).ToList()));


            items = CalcHumansFor("DATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.DATE != null).ToList()));

            items = CalcHumansFor("NullDATE", new DateTimeExpression()
            {
                SelectedCondition = Condition.NotIsNull,
            });
            Assert.True(ListEquals(items, People.Where((t) => t.NullDATE != null).ToList()));
        }
        bool ListEquals(IList<Human> m1, IList<Human> m2)
        {
            if (m1 == null || m2 == null) return false;
            if (m1.Count != m2.Count) return false;
            var c = m1.Count;
            for (int i = 0; i < c; i++)
            {
                if (m1[i].STRING != m2[i].STRING)
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
            new Human("Ann",false, true, 0,0, new DateTime(1990,1,1), new DateTime(1990,1,1)),
            new Human("Sergio",  true , false, 5,5, new DateTime(2000,1,1), new DateTime(2000,1,1)),
            new Human("",  true ,true, 5,5, new DateTime(2010,1,1), null),
            new Human(null, true, null, 10, null, new DateTime(2020,1,1), null  )
        };

        public class Human
        {
            public Human(string @string,
                bool @bool, bool? nullBOOL,
                int @int, int? nullINT,
                DateTime date,
                DateTime? nullDATE)
            {
                STRING = @string;

                BOOL = @bool;
                NullBOOL = nullBOOL;

                INT = @int;
                NullINT = nullINT;

                DATE = date;
                NullDATE = nullDATE;
            }
            public string STRING { get; set; }
            public int INT { get; set; }
            public int? NullINT { get; set; }
            public bool BOOL { get; set; }
            public bool? NullBOOL { get; set; }
            public DateTime DATE { get; set; }
            public DateTime? NullDATE { get; set; }
        }

    }
}