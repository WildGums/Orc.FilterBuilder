// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class NumericExpression : DataTypeExpression
    {
        private double _value;

        public NumericExpression()
        {
            SelectedCondition = Condition.EqualTo;
            Value = 0d;
            ValueControlType = ValueControlType.Text;
        }

        public NumericExpression(Type type)
            : this()
        {
            IsNullable = type.IsNullable();
        }

        #region Properties
        public bool IsNullable { get; set; }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        #region Methods
        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            if (IsNullable)
            {
                var entityValue = propertyMetadata.GetValue(entity);
                double doubleValue = 0d;
                if (entityValue != null)
                {
                    doubleValue = Convert.ToDouble(entityValue);
                }
                switch (SelectedCondition)
                {
                    case Condition.EqualTo:
                        return entityValue != null && doubleValue == Value;

                    case Condition.NotEqualTo:
                        return entityValue != null && doubleValue != Value;

                    case Condition.GreaterThan:
                        return entityValue != null && doubleValue > Value;

                    case Condition.LessThan:
                        return entityValue != null && doubleValue < Value;

                    case Condition.GreaterThanOrEqualTo:
                        return entityValue != null && doubleValue >= Value;

                    case Condition.LessThanOrEqualTo:
                        return entityValue != null && doubleValue <= Value;

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
                var entityValue = propertyMetadata.GetValue(entity);
                var doubleValue = Convert.ToDouble(entityValue);
                switch (SelectedCondition)
                {
                    case Condition.EqualTo:
                        return doubleValue == Value;

                    case Condition.NotEqualTo:
                        return doubleValue != Value;

                    case Condition.GreaterThan:
                        return doubleValue > Value;

                    case Condition.LessThan:
                        return doubleValue < Value;

                    case Condition.GreaterThanOrEqualTo:
                        return doubleValue >= Value;

                    case Condition.LessThanOrEqualTo:
                        return doubleValue <= Value;

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