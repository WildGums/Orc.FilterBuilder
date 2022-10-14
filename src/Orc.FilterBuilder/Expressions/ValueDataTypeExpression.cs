namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using Catel;

    public abstract class ValueDataTypeExpression<TValue> : NullableDataTypeExpression
        where TValue : struct, IComparable, IFormattable, IComparable<TValue>, IEquatable<TValue>
    {
        private readonly Comparer<TValue> _comparer;

        protected ValueDataTypeExpression()
            : base()
        {
            _comparer = Comparer<TValue>.Default;

            SelectedCondition = Condition.EqualTo;
            Value = default(TValue);
        }

        public TValue Value { get; set; }

        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            if (IsNullable)
            {
                var entityValue = propertyMetadata.GetValue<TValue?>(entity);

                switch (SelectedCondition)
                {
                    case Condition.EqualTo:
                        return object.Equals(entityValue, Value);

                    case Condition.NotEqualTo:
                        return !object.Equals(entityValue, Value);

                    case Condition.GreaterThan:
                        return entityValue is not null && _comparer.Compare(entityValue.Value, Value) > 0;

                    case Condition.LessThan:
                        return entityValue is not null && _comparer.Compare(entityValue.Value, Value) < 0;

                    case Condition.GreaterThanOrEqualTo:
                        return entityValue is not null && _comparer.Compare(entityValue.Value, Value) >= 0;

                    case Condition.LessThanOrEqualTo:
                        return entityValue is not null && _comparer.Compare(entityValue.Value, Value) <= 0;

                    case Condition.IsNull:
                        return entityValue is null;

                    case Condition.NotIsNull:
                        return entityValue is not null;

                    default:
                        throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
                }
            }
            else
            {
                var entityValue = propertyMetadata.GetValue<TValue>(entity);
                switch (SelectedCondition)
                {
                    case Condition.EqualTo:
                        return object.Equals(entityValue, Value);

                    case Condition.NotEqualTo:
                        return !object.Equals(entityValue, Value);

                    case Condition.GreaterThan:
                        return _comparer.Compare(entityValue, Value) > 0;

                    case Condition.LessThan:
                        return _comparer.Compare(entityValue, Value) < 0;

                    case Condition.GreaterThanOrEqualTo:
                        return _comparer.Compare(entityValue, Value) >= 0;

                    case Condition.LessThanOrEqualTo:
                        return _comparer.Compare(entityValue, Value) <= 0;

                    default:
                        throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
    }
}
