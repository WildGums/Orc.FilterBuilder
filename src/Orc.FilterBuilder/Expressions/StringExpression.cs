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
        var value = ValueProperty is { } valueProperty
            ? valueProperty.GetValue<string>(entity)
            : Value;

        var entityValue = propertyMetadata.GetValue<string>(entity);
        if (entityValue is null && propertyMetadata.Type.IsEnumEx())
        {
            var entityValueAsObject = propertyMetadata.GetValue(entity);
            if (entityValueAsObject is not null)
            {
                entityValue = entityValueAsObject.ToString();
            }
        }

        return CalculateResult(entityValue, value);
    }

    private bool CalculateResult(string? entityValue, string? value)
    {
        // Text conditions never match when either side is null
        if (entityValue is null || value is null)
        {
            return SelectedCondition switch
            {
                Condition.Contains or Condition.DoesNotContain
                    or Condition.StartsWith or Condition.DoesNotStartWith
                    or Condition.EndsWith or Condition.DoesNotEndWith
                    or Condition.Matches or Condition.DoesNotMatch => false,
                _ => CalculateNullSafeResult(entityValue, value)
            };
        }

        return SelectedCondition switch
        {
            Condition.Contains => entityValue.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1,
            Condition.DoesNotContain => entityValue.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) == -1,
            Condition.EndsWith => entityValue.EndsWith(value, StringComparison.CurrentCultureIgnoreCase),
            Condition.DoesNotEndWith => !entityValue.EndsWith(value, StringComparison.CurrentCultureIgnoreCase),
            Condition.StartsWith => entityValue.StartsWith(value, StringComparison.CurrentCultureIgnoreCase),
            Condition.DoesNotStartWith => !entityValue.StartsWith(value, StringComparison.CurrentCultureIgnoreCase),
            Condition.Matches => RegexIsValidCache.GetFromCacheOrFetch(value, () => RegexHelper.IsValid(value)) && RegexCache.GetFromCacheOrFetch(value, () => new Regex(value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
            Condition.DoesNotMatch => RegexIsValidCache.GetFromCacheOrFetch(value, () => RegexHelper.IsValid(value)) && !RegexCache.GetFromCacheOrFetch(value, () => new Regex(value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue),
            _ => CalculateNullSafeResult(entityValue, value)
        };
    }

    private bool CalculateNullSafeResult(string? entityValue, string? value)
    {
        return SelectedCondition switch
        {
            Condition.EqualTo => entityValue == value,
            Condition.NotEqualTo => entityValue != value,
            Condition.GreaterThan => string.Compare(entityValue, value, StringComparison.OrdinalIgnoreCase) > 0,
            Condition.GreaterThanOrEqualTo => string.Compare(entityValue, value, StringComparison.OrdinalIgnoreCase) >= 0,
            Condition.LessThan => string.Compare(entityValue, value, StringComparison.OrdinalIgnoreCase) < 0,
            Condition.LessThanOrEqualTo => string.Compare(entityValue, value, StringComparison.OrdinalIgnoreCase) <= 0,
            Condition.IsEmpty => entityValue == string.Empty,
            Condition.NotIsEmpty => entityValue != string.Empty,
            Condition.IsNull => entityValue is null,
            Condition.NotIsNull => entityValue is not null,
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
        };
    }

    public override string ToString()
    {
        return ToDisplayString(Value);
    }
}
