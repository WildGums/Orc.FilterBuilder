namespace Orc.FilterBuilder;

using System.Collections.Generic;

public static class ConditionHelper
{
    public static List<Condition> GetValueConditions()
    {
        return new List<Condition>
        {
            Condition.EqualTo,
            Condition.NotEqualTo,
            Condition.LessThan,
            Condition.LessThanOrEqualTo,
            Condition.GreaterThan,
            Condition.GreaterThanOrEqualTo
        };
    }

    public static List<Condition> GetNullableValueConditions()
    {
        var conditions = GetValueConditions();
        conditions.Add(Condition.IsNull);
        conditions.Add(Condition.NotIsNull);
        return conditions;
    }

    public static List<Condition> GetBooleanConditions()
    {
        return new List<Condition>
        {
            Condition.EqualTo
        };
    }
        
    public static List<Condition> GetStringConditions()
    {
        return new List<Condition>
        {
            Condition.Contains,
            Condition.DoesNotContain,
            Condition.StartsWith,
            Condition.DoesNotStartWith,
            Condition.EndsWith,
            Condition.DoesNotEndWith,
            Condition.EqualTo,
            Condition.NotEqualTo,
            Condition.Matches,
            Condition.DoesNotMatch,
            Condition.IsEmpty,
            Condition.NotIsEmpty,
            Condition.IsNull,
            Condition.NotIsNull
        };
    }

    public static bool GetIsValueRequired(Condition condition)
    {
        return
            condition == Condition.EqualTo ||
            condition == Condition.NotEqualTo ||
            condition == Condition.GreaterThan ||
            condition == Condition.LessThan ||
            condition == Condition.GreaterThanOrEqualTo ||
            condition == Condition.LessThanOrEqualTo ||
            condition == Condition.Contains ||
            condition == Condition.DoesNotContain ||
            condition == Condition.EndsWith ||
            condition == Condition.DoesNotEndWith ||
            condition == Condition.StartsWith ||
            condition == Condition.DoesNotStartWith ||
            condition == Condition.Matches ||
            condition == Condition.DoesNotMatch;
    }
}
