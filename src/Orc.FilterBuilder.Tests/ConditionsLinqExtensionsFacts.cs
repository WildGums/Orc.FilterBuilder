namespace Orc.FilterBuilder.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Orc.FilterBuilder;

[TestFixture]
public class ConditionsLinqExtensionsFacts
{
    private static readonly IComparer<Human> HumanComparer = Comparer<Human>.Create((h1, h2) =>
        string.Compare(h1.STRING, h2.STRING, StringComparison.InvariantCulture));

    [TestFixture]
    public class When_The_SelectedCondition_Is_EqualTo
    {
        [Test]
        public void Filters_Correctly_With_Bool_Property()
        {
            var predicate = BuildPredicate(human => human.BOOL, new BooleanExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = true
            });

            Assert.That(People.Where(t => t.BOOL), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }


        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE == new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT == 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Bool_Property()
        {
            var predicate = BuildPredicate(human => human.NullBOOL, new BooleanExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = true
            });

            Assert.That(People.Where(t => t.NullBOOL.HasValue && t.NullBOOL.Value), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.NullDATE == new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.NullINT == 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.EqualTo,
                Value = "Ann"
            });

            Assert.That(People.Where(t => t.STRING is not null && t.STRING == "Ann"), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }


    [TestFixture]
    public class When_The_SelectedCondition_Is_Contains
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.Contains,
                Value = "nn"
            });

            Assert.That(People.Where(t => t.STRING is not null && t.STRING.Contains("nn")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }


    [TestFixture]
    public class When_The_SelectedCondition_Is_DoesNotContain
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.DoesNotContain,
                Value = "nn"
            });

            Assert.That(People.Where(t => t.STRING is null || !t.STRING.Contains("nn")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_StartsWith
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.StartsWith,
                Value = "A"
            });

            Assert.That(People.Where(t => t.STRING is not null && t.STRING.StartsWith("A")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_DoesNotStartWith
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.DoesNotStartWith,
                Value = "A"
            });

            Assert.That(People.Where(t => t.STRING is null || !t.STRING.StartsWith("A")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_EndsWith
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.EndsWith,
                Value = "io"
            });

            Assert.That(People.Where(t => t.STRING is not null && t.STRING.EndsWith("io")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_DoesNotEndWith
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.DoesNotEndWith,
                Value = "io"
            });

            Assert.That(People.Where(t => t.STRING is null || !t.STRING.EndsWith("io")), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_NotEqualTo
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE != new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT != 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.NullDATE != new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = 5
            });
            Assert.That(People.Where(t => t.NullINT != 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.NotEqualTo,
                Value = "Sergio"
            });

            Assert.That(People.Where(t => t.STRING is null || t.STRING != "Sergio"), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_GreaterThan
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.GreaterThan,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE > new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.GreaterThan,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT > 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.GreaterThan,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.NullDATE > new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.GreaterThan,
                Value = 5
            });

            Assert.That(People.Where(t => t.NullINT > 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.GreaterThan,
                Value = "Ann"
            });

            Assert.That(
                People.Where(t => string.Compare(t.STRING, "Ann", StringComparison.InvariantCultureIgnoreCase) > 0), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_GreaterThanOrEqualTo
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = new DateTime(1990, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE >= new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT >= 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = new DateTime(1990, 1, 1)
            });
            Assert.That(People.Where(t => t.NullDATE >= new DateTime(1990, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.NullINT >= 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.GreaterThanOrEqualTo,
                Value = "Ann"
            });

            Assert.That(
                People.Where(t =>
                    string.Compare(t.STRING, "Ann", StringComparison.InvariantCultureIgnoreCase) >= 0), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_LessThan
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.LessThan,
                Value = new DateTime(2000, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE < new DateTime(2000, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.LessThan,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT < 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.LessThan,
                Value = new DateTime(2000, 1, 1)
            });

            Assert.That(People.Where(t => t.NullDATE < new DateTime(2000, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.LessThan,
                Value = 5
            });

            Assert.That(People.Where(t => t.NullINT < 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.LessThan,
                Value = "Sergio"
            });

            Assert.That(
                People.Where(t =>
                    string.Compare(t.STRING, "Sergio", StringComparison.InvariantCultureIgnoreCase) < 0), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_LessThanOrEqualTo
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = new DateTime(2000, 1, 1)
            });

            Assert.That(People.Where(t => t.DATE <= new DateTime(2000, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.INT <= 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = new DateTime(2000, 1, 1)
            });

            Assert.That(People.Where(t => t.NullDATE <= new DateTime(2000, 1, 1)), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = 5
            });

            Assert.That(People.Where(t => t.NullINT <= 5), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.LessThanOrEqualTo,
                Value = "Sergio"
            });

            Assert.That(
                People.Where(t =>
                    string.Compare(t.STRING, "Sergio", StringComparison.InvariantCultureIgnoreCase) <= 0), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_IsNull
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.IsNull
            });

            Assert.That(People.Where(x => x.NullDATE is null).ToList(), Is.EqualTo(People.Where(predicate).ToList()).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.IsNull
            });

            Assert.That(new List<Human>(), Is.EqualTo(People.Where(predicate)).AsCollection);
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.IsNull
            });

            Assert.That(People.Where(t => t.NullDATE is null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.IsNull
            });

            Assert.That(People.Where(t => t.NullINT is null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.IsNull
            });

            Assert.That(
                People.Where(t => t.STRING is null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_NotIsNull
    {
        [Test]
        public void Filters_Correctly_With_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.DATE, new DateTimeExpression
            {
                SelectedCondition = Condition.NotIsNull
            });

            Assert.That(People, Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Int_Property()
        {
            var predicate = BuildPredicate(human => human.INT, new IntegerExpression
            {
                SelectedCondition = Condition.NotIsNull
            });

            Assert.That(People, Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_DateTime_Property()
        {
            var predicate = BuildPredicate(human => human.NullDATE, new DateTimeExpression
            {
                SelectedCondition = Condition.NotIsNull
            });

            Assert.That(People.Where(t => t.NullDATE is not null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_Nullable_Int_Property()
        {
            var predicate = BuildPredicate(human => human.NullINT, new IntegerExpression
            {
                SelectedCondition = Condition.NotIsNull
            });

            Assert.That(People.Where(t => t.NullINT is not null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }

        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.NotIsNull
            });

            Assert.That(
                People.Where(t => t.STRING is not null), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_IsEmpty
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.IsEmpty
            });

            Assert.That(
                People.Where(t => t.STRING == string.Empty), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    [TestFixture]
    public class When_The_SelectedCondition_Is_NotIsEmpty
    {
        [Test]
        public void Filters_Correctly_With_String_Property()
        {
            var predicate = BuildPredicate(human => human.STRING, new StringExpression
            {
                SelectedCondition = Condition.NotIsEmpty
            });

            Assert.That(
                People.Where(t => t.STRING != string.Empty), Is.EqualTo(People.Where(predicate)).Using(HumanComparer));
        }
    }

    private static string GetPropertyName(Expression expression)
    {
        switch (expression)
        {
            case UnaryExpression _:
                return GetPropertyName(((UnaryExpression)expression).Operand);
            case MemberExpression _:
                return ((MemberExpression)expression).Member.Name;
            default:
                return null;
        }
    }

    private static Func<Human, bool> BuildPredicate(Expression<Func<Human, object>> propertySelectionExpression,
        DataTypeExpression dataTypeExpression)
    {
        var propertyName = GetPropertyName(propertySelectionExpression.Body);
        var conditionGroup = new ConditionGroup
        {
            Type = ConditionGroupType.And
        };

        var type = typeof(Human);

        var propertyExpression = new PropertyExpression
        {
            Property = new PropertyMetadata(type, type.GetProperty(propertyName)),
            DataTypeExpression = dataTypeExpression
        };

        conditionGroup.Items.Add(propertyExpression);

        return conditionGroup.BuildLambda<Human>().Compile();
    }

    public static List<Human> People => new List<Human>
    {
        new Human("Ann", false, true, 0, 0, new DateTime(1990, 1, 1), new DateTime(1990, 1, 1)),
        new Human("Sergio", true, false, 5, 5, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1)),
        new Human(string.Empty, true, true, 5, 5, new DateTime(2010, 1, 1), null),
        new Human(null, true, null, 10, null, new DateTime(2020, 1, 1), null)
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
