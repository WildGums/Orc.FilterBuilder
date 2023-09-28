namespace Orc.FilterBuilder;

using System;

public static class ConditionExtensions
{
    public static string Humanize(this Condition condition)
    {
        switch (condition)
        {
            case Condition.Contains:
                return "contains";

            case Condition.DoesNotContain:
                return "does not contain";

            case Condition.StartsWith:
                return "starts with";

            case Condition.DoesNotStartWith:
                return "does not start with";

            case Condition.EndsWith:
                return "ends with";

            case Condition.DoesNotEndWith:
                return "does not end with";

            case Condition.EqualTo:
                return "is equal to";

            case Condition.NotEqualTo:
                return "is not equal to";

            case Condition.GreaterThan:
                return "is greater than";

            case Condition.LessThan:
                return "is less than";

            case Condition.GreaterThanOrEqualTo:
                return "is greater than or equal to";

            case Condition.LessThanOrEqualTo:
                return "is less than or equal to";

            case Condition.IsEmpty:
                return "is empty";

            case Condition.NotIsEmpty:
                return "is not empty";

            case Condition.IsNull:
                return "is null";

            case Condition.NotIsNull:
                return "is not null";

            case Condition.Matches:
                return "matches";

            case Condition.DoesNotMatch:
                return "does not match";

            default:
                throw new ArgumentOutOfRangeException("condition", condition, null);
        }
    }
}