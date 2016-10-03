// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueDataTypeExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Catel;

    using Orc.FilterBuilder.Models;

    public abstract class ValueDataTypeExpression<TValue> : NullableDataTypeExpression
        where TValue : struct, IComparable, IFormattable, IConvertible, IComparable<TValue>, IEquatable<TValue>
    {
        #region Fields
        private readonly Comparer<TValue> _comparer;
        #endregion



        #region Constructors
        protected ValueDataTypeExpression() : base()
        {
            _comparer = Comparer<TValue>.Default;

            SelectedCondition = Condition.EqualTo;
            Value = default(TValue);
        }
        #endregion



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

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="propertyExpr">LINQ <see cref="MemberExpression"/>.</param>
        /// <returns>LINQ Expression.</returns>
        public override Expression ToLinqExpression(Expression propertyExpr)
        {
            Argument.IsNotNull(() => propertyExpr);

            Expression valueExpr;

            // Nullable type
            if (IsNullable)
            {
                // Null-Check first
                switch (SelectedCondition)
                {
                    case Condition.IsNull:
                        return Expression.Equal(propertyExpr, Expression.Constant(null, typeof(TValue?)));

                    case Condition.NotIsNull:
                        return Expression.NotEqual(propertyExpr, Expression.Constant(null, typeof(TValue?)));
                }

                // Not null-check
                valueExpr = Expression.Constant(Value, typeof(TValue?));

                // return CreateNullableOperatorExpression(propertyExpr, valueExpr);
                return CreateOperatorExpression(propertyExpr, valueExpr);
            }
            else
            {
                // Not nullable, get provided constant value
                valueExpr = Expression.Constant(Value, typeof(TValue));

                return CreateOperatorExpression(propertyExpr, valueExpr);
            }
        }

        private Expression CreateNullableOperatorExpression(Expression propExpr, Expression valueExpr)
        {
            Expression propHasValueExpr = Expression.Property(propExpr, "HasValue");
            Expression propValueExpr = Expression.Property(propExpr, "Value");

            Expression operatorExpr = CreateOperatorExpression(propValueExpr, valueExpr);

            return Expression.AndAlso(propHasValueExpr, operatorExpr);
        }

        private Expression CreateOperatorExpression(Expression propertyExpr, Expression valueExpr)
        {
            // Operators
            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return Expression.Equal(propertyExpr, valueExpr);

                case Condition.NotEqualTo:
                    return Expression.NotEqual(propertyExpr, valueExpr);

                case Condition.GreaterThan:
                    return Expression.GreaterThan(propertyExpr, valueExpr);

                case Condition.LessThan:
                    return Expression.LessThan(propertyExpr, valueExpr);

                case Condition.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(propertyExpr, valueExpr);

                case Condition.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(propertyExpr, valueExpr);

                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}