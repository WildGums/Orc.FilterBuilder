namespace Orc.FilterBuilder;

using Catel.Reflection;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Catel.Caching;
using Catel.Caching.Policies;
using System.Collections.Generic;
using Catel;

[DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
public class StringExpression : DataTypeExpression
{
    private static readonly CacheStorage<string, Regex> RegexCache = new(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);
    private static readonly CacheStorage<string, bool> RegexIsValidCache = new(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);

    public StringExpression()
    {
        SelectedCondition = Condition.Contains;
        Value = string.Empty;
        ValueControlType = ValueControlType.Text;
    }

    public string Value { get; set; }

    public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
    {
        var entityValue = propertyMetadata.GetValue<string>(entity);
        if (entityValue is not null || !propertyMetadata.Type.IsEnumEx())
        {
            return SelectedCondition switch
            {
                Condition.Contains => entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) != -1,
                Condition.DoesNotContain => entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) == -1,
                Condition.EndsWith => entityValue is not null && entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase),
                Condition.DoesNotEndWith => entityValue is not null && !entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase),
                Condition.EqualTo => entityValue == Value,
                Condition.GreaterThan => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) > 0,
                Condition.GreaterThanOrEqualTo => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) >= 0,
                Condition.IsEmpty => entityValue == string.Empty,
                Condition.IsNull => entityValue is null,
                Condition.LessThan => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) < 0,
                Condition.LessThanOrEqualTo => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) <= 0,
                Condition.NotEqualTo => entityValue != Value,
                Condition.NotIsEmpty => entityValue != string.Empty,
                Condition.NotIsNull => entityValue is not null,
                Condition.StartsWith => entityValue is not null && entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase),
                Condition.DoesNotStartWith => entityValue is not null && !entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase),
                Condition.Matches => entityValue is not null && RegexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && RegexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
                Condition.DoesNotMatch => entityValue is not null && RegexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && !RegexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
                _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
            };
        }

        var entityValueAsObject = propertyMetadata.GetValue(entity);
        if (entityValueAsObject is not null)
        {
            entityValue = entityValueAsObject.ToString();
        }

        return SelectedCondition switch
        {
            Condition.Contains => entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) != -1,
            Condition.DoesNotContain => entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) == -1,
            Condition.EndsWith => entityValue is not null && entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase),
            Condition.DoesNotEndWith => entityValue is not null && !entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase),
            Condition.EqualTo => entityValue == Value,
            Condition.GreaterThan => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) > 0,
            Condition.GreaterThanOrEqualTo => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) >= 0,
            Condition.IsEmpty => entityValue == string.Empty,
            Condition.IsNull => entityValue is null,
            Condition.LessThan => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) < 0,
            Condition.LessThanOrEqualTo => string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) <= 0,
            Condition.NotEqualTo => entityValue != Value,
            Condition.NotIsEmpty => entityValue != string.Empty,
            Condition.NotIsNull => entityValue is not null,
            Condition.StartsWith => entityValue is not null && entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase),
            Condition.DoesNotStartWith => entityValue is not null && !entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase),
            Condition.Matches => entityValue is not null && RegexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && RegexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
            Condition.DoesNotMatch => entityValue is not null && RegexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value)) && !RegexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
        };
    }

    public override string ToString()
    {
        return $"{SelectedCondition.Humanize()} '{Value}'";
    }
}
