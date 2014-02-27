// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Catel.Data;
    using Catel.Runtime.Serialization;
    using Fasterflect;

    public class TimeSpanExpression : DataTypeExpression
    {
        #region Constants
        private const int AverageDaysInYear = 365;
        private const int AverageDaysInMonth = 30;
        #endregion

        #region Constructors
        public TimeSpanExpression()
        {
            SelectedCondition = Condition.EqualTo;
            Value = TimeSpan.FromHours(1);
            ValueControlType = ValueControlType.TimeSpan;
            SpanTypes = Enum.GetValues(typeof (TimeSpanType)).Cast<TimeSpanType>().ToList();
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

        public override bool CalculateResult(string propertyName, object entity)
        {
            var entityValue = (TimeSpan) entity.GetPropertyValue(propertyName);
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
        #endregion
    }
}