// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    using Catel.Data;
    using Catel.Runtime.Serialization;

    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class TimeSpanExpression : NullableDataTypeExpression
    {
        #region Constants
        private const int AverageDaysInYear = 365;
        private const int AverageDaysInMonth = 30;
        #endregion



        #region Constructors
        public TimeSpanExpression() : this(true) { }

        public TimeSpanExpression(bool isNullable)
        {
            IsNullable = isNullable;
            SelectedCondition = Condition.EqualTo;
            Value = TimeSpan.FromHours(1);
            ValueControlType = ValueControlType.TimeSpan;
            SpanTypes = Enum.GetValues(typeof(TimeSpanType)).Cast<TimeSpanType>().ToList();
            SelectedSpanType = TimeSpanType.Hours;
        }
        #endregion



        #region Properties
        public TimeSpan Value { get; set; }

        [ExcludeFromSerialization]
        public List<TimeSpanType> SpanTypes { get; set; }

        public TimeSpanType SelectedSpanType { get; set; }

        public float Amount { get; set; }
        #endregion



        #region Methods
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.HasPropertyChanged(() => SelectedSpanType) || e.HasPropertyChanged(() => Amount))
            {
                switch (SelectedSpanType)
                {
                    case TimeSpanType.Years:
                        Value = TimeSpan.FromDays(Amount * AverageDaysInYear);
                        break;

                    case TimeSpanType.Months:
                        Value = TimeSpan.FromDays(Amount * AverageDaysInMonth);
                        break;

                    case TimeSpanType.Days:
                        Value = TimeSpan.FromDays(Amount);
                        break;

                    case TimeSpanType.Hours:
                        Value = TimeSpan.FromHours(Amount);
                        break;

                    case TimeSpanType.Minutes:
                        Value = TimeSpan.FromMinutes(Amount);
                        break;

                    case TimeSpanType.Seconds:
                        Value = TimeSpan.FromSeconds(Amount);
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Type '{0}' is not supported.", SelectedSpanType));
                }
            }
        }

        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            var entityValue = propertyMetadata.GetValue<TimeSpan>(entity);
            switch (SelectedCondition)
            {
                case Condition.EqualTo:
                    return entityValue == Value;

                case Condition.NotEqualTo:
                    return entityValue != Value;

                case Condition.GreaterThan:
                    return entityValue > Value;

                case Condition.LessThan:
                    return entityValue < Value;

                case Condition.GreaterThanOrEqualTo:
                    return entityValue >= Value;

                case Condition.LessThanOrEqualTo:
                    return entityValue <= Value;

                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }
        }

        /// <summary>
        ///   Converts <see cref="ConditionTreeItem"/> to a LINQ <see cref="Expression"/>
        /// </summary>
        /// <param name="propertyExpr">LINQ <see cref="MemberExpression"/>.</param>
        /// <returns>LINQ Expression.</returns>
        public override Expression ToLinqExpression(Expression propertyExpr)
        {
            var valueExpr = Expression.Constant(Value, typeof(TimeSpan));

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