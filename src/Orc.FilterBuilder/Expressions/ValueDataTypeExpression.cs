namespace Orc.FilterBuilder;

using System;
using System.Collections.Generic;
using Catel;

public abstract class ValueDataTypeExpression<TValue> : NullableDataTypeExpression
    where TValue : struct, IComparable, IFormattable, IComparable<TValue>, IEquatable<TValue>
{
    private readonly Comparer<TValue> _comparer;

    protected ValueDataTypeExpression()
    {
        _comparer = Comparer<TValue>.Default;

        SelectedCondition = Condition.EqualTo;
        Value = default;
    }

    public TValue Value { get; set; }

    public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
    {
        if (IsNullable)
        {
            var entityValue = propertyMetadata.GetValue<TValue?>(entity);

            return SelectedCondition switch
            {
                Condition.EqualTo => Equals(entityValue, Value),
                Condition.NotEqualTo => !Equals(entityValue, Value),
                Condition.GreaterThan => entityValue is not null && _comparer.Compare(entityValue.Value, Value) > 0,
                Condition.LessThan => entityValue is not null && _comparer.Compare(entityValue.Value, Value) < 0,
                Condition.GreaterThanOrEqualTo => entityValue is not null && _comparer.Compare(entityValue.Value, Value) >= 0,
                Condition.LessThanOrEqualTo => entityValue is not null && _comparer.Compare(entityValue.Value, Value) <= 0,
                Condition.IsNull => entityValue is null,
                Condition.NotIsNull => entityValue is not null,
                _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
            };
        }
        else
        {
            var entityValue = propertyMetadata.GetValue<TValue>(entity);
            return SelectedCondition switch
            {
                Condition.EqualTo => Equals(entityValue, Value),
                Condition.NotEqualTo => !Equals(entityValue, Value),
                Condition.GreaterThan => _comparer.Compare(entityValue, Value) > 0,
                Condition.LessThan => _comparer.Compare(entityValue, Value) < 0,
                Condition.GreaterThanOrEqualTo => _comparer.Compare(entityValue, Value) >= 0,
                Condition.LessThanOrEqualTo => _comparer.Compare(entityValue, Value) <= 0,
                _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition))
            };
        }
    }

    public override string ToString()
    {
        return $"{SelectedCondition.Humanize()} '{Value}'";
    }
}
