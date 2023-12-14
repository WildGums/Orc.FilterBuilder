namespace Orc.FilterBuilder;

using System;
using Catel;
using Properties;

public static class ConditionExtensions
{
    public static string Humanize(this Condition condition)
    {
        return condition switch
        {
            Condition.Contains => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_Contains)),
            Condition.DoesNotContain => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_DoesNotContain)),
            Condition.StartsWith => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_StartsWith)),
            Condition.DoesNotStartWith => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_DoesNotStartWith)),
            Condition.EndsWith => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_EndsWith)),
            Condition.DoesNotEndWith => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_DoesNotEndWith)),
            Condition.EqualTo => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_EqualTo)),
            Condition.NotEqualTo => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_NotEqualTo)),
            Condition.GreaterThan => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_GreaterThan)),
            Condition.LessThan => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_LessThan)),
            Condition.GreaterThanOrEqualTo => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_GreaterThanOrEqualTo)),
            Condition.LessThanOrEqualTo => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_LessThanOrEqualTo)),
            Condition.IsEmpty => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_IsEmpty)),
            Condition.NotIsEmpty => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_NotIsEmpty)),
            Condition.IsNull => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_IsNull)),
            Condition.NotIsNull => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_NotIsNull)),
            Condition.Matches => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_Matches)),
            Condition.DoesNotMatch => LanguageHelper.GetRequiredString(nameof(Resources.FilterBuilder_DoesNotMatch)),
            _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
        };
    }
}
