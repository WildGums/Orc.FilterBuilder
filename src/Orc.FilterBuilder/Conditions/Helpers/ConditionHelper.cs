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
            condition is Condition.EqualTo 
                or Condition.NotEqualTo
                or Condition.GreaterThan 
                or Condition.LessThan 
                or Condition.GreaterThanOrEqualTo 
                or Condition.LessThanOrEqualTo 
                or Condition.Contains 
                or Condition.DoesNotContain 
                or Condition.EndsWith 
                or Condition.DoesNotEndWith 
                or Condition.StartsWith 
                or Condition.DoesNotStartWith 
                or Condition.Matches 
                or Condition.DoesNotMatch;
    }
}
