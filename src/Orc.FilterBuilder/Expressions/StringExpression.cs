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
    private static readonly CacheStorage<string, Regex> _regexCache = new CacheStorage<string, Regex>(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);
    private static readonly CacheStorage<string, bool> _regexIsValidCache = new CacheStorage<string, bool>(() => ExpirationPolicy.Sliding(TimeSpan.FromMinutes(1)), false, EqualityComparer<string>.Default);

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
        if (entityValue is null && propertyMetadata.Type.IsEnumEx())
        {
            var entityValueAsObject = propertyMetadata.GetValue(entity);
            if (entityValueAsObject is not null)
            {
                entityValue = entityValueAsObject.ToString();
            }
        }

        switch (SelectedCondition)
        {
            case Condition.Contains:
                return entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) != -1;

            case Condition.DoesNotContain:
                return entityValue is not null && entityValue.IndexOf(Value, StringComparison.CurrentCultureIgnoreCase) == -1;

            case Condition.EndsWith:
                return entityValue is not null && entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase);

            case Condition.DoesNotEndWith:
                return entityValue is not null && !entityValue.EndsWith(Value, StringComparison.CurrentCultureIgnoreCase);

            case Condition.EqualTo:
                return entityValue == Value;

            case Condition.GreaterThan:
                return string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) > 0;

            case Condition.GreaterThanOrEqualTo:
                return string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) >= 0;

            case Condition.IsEmpty:
                return entityValue == string.Empty;

            case Condition.IsNull:
                return entityValue is null;

            case Condition.LessThan:
                return string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) < 0;

            case Condition.LessThanOrEqualTo:
                return string.Compare(entityValue, Value, StringComparison.OrdinalIgnoreCase) <= 0;

            case Condition.NotEqualTo:
                return entityValue != Value;

            case Condition.NotIsEmpty:
                return entityValue != string.Empty;

            case Condition.NotIsNull:
                return entityValue is not null;

            case Condition.StartsWith:
                return entityValue is not null && entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase);

            case Condition.DoesNotStartWith:
                return entityValue is not null && !entityValue.StartsWith(Value, StringComparison.CurrentCultureIgnoreCase);

            case Condition.Matches:
                return entityValue is not null && _regexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value))
                                               && _regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue);

            case Condition.DoesNotMatch:
                return entityValue is not null && _regexIsValidCache.GetFromCacheOrFetch(Value, () => RegexHelper.IsValid(Value))
                                               && !_regexCache.GetFromCacheOrFetch(Value, () => new Regex(Value, RegexOptions.Compiled, TimeSpan.FromSeconds(1))).IsMatch(entityValue);

            default:
                throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
        }
    }

    public override string ToString()
    {
        return $"{SelectedCondition.Humanize()} '{Value}'";
    }
}