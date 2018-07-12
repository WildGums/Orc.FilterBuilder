// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using Catel;
    using Catel.Data;
    using Catel.Runtime.Serialization;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    [Serializable]
    public class TimeSpanExpression : NullableDataTypeExpression
    {
        #region Constructors
        protected TimeSpanExpression(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public TimeSpanExpression()
            : this(true)
        {
            
        }

        public TimeSpanExpression(bool isNullable)
        {
            IsNullable = isNullable;
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

            const int averageDaysInYear = 365;
            const int averageDaysInMonth = 30;

            if (!e.HasPropertyChanged(() => SelectedSpanType) && !e.HasPropertyChanged(() => Amount))
            {
                return;
            }

            switch (SelectedSpanType)
            {
                case TimeSpanType.Years:
                    Value = TimeSpan.FromDays(Amount * averageDaysInYear);
                    break;

                case TimeSpanType.Months:
                    Value = TimeSpan.FromDays(Amount * averageDaysInMonth);
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
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_TypeIsNotSupported_Pattern"), SelectedSpanType));
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
                    throw new NotSupportedException(string.Format(LanguageHelper.GetString("FilterBuilder_Exception_Message_ConditionIsNotSupported_Pattern"), SelectedCondition));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}
