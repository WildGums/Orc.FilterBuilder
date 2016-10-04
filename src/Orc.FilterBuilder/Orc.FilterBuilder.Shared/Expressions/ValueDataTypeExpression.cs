// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueDataTypeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class ValueDataTypeExpression<TValue> : NullableDataTypeExpression
        where TValue : struct, IComparable, IFormattable, IConvertible, IComparable<TValue>, IEquatable<TValue>
    {
        #region Fields
        private readonly Comparer<TValue> _comparer;
        #endregion

        protected ValueDataTypeExpression()
            : base()
        {
            _comparer = Comparer<TValue>.Default;

            SelectedCondition = Condition.EqualTo;
            Value = default(TValue);
        }

        #region Properties
        public TValue Value { get; set; }
        #endregion

        #region Methods
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
                        return entityValue != null && _comparer.Compare(entityValue.Value, Value) > 0;

                    case Condition.LessThan:
                        return entityValue != null && _comparer.Compare(entityValue.Value, Value) < 0;

                    case Condition.GreaterThanOrEqualTo:
                        return entityValue != null && _comparer.Compare(entityValue.Value, Value) >= 0;

                    case Condition.LessThanOrEqualTo:
                        return entityValue != null && _comparer.Compare(entityValue.Value, Value) <= 0;

                    case Condition.IsNull:
                        return entityValue == null;

                    case Condition.NotIsNull:
                        return entityValue != null;

                    default:
                        throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
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
                        throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}
