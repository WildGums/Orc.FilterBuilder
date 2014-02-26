// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using Fasterflect;

    public class DecimalExpression : DataTypeExpression
    {
        #region Constructors
        public DecimalExpression()
            : this(false)
        {
        }

        public DecimalExpression(bool isNullable)
        {
            IsNullable = isNullable;
            SelectedCondition = Condition.EqualTo;
            Value = 0;
            ValueControlType = ValueControlType.Text;
        }
        #endregion

        #region Properties
        public bool IsNullable { get; set; }

        public decimal Value { get; set; }
        #endregion

        #region Methods
        private void OnIsNullableChanged()
        {
            Conditions = IsNullable ? GetNullableValueConditions() : GetValueConditions();
        }

        public override bool CalculateResult(string propertyName, object entity)
        {
            if (IsNullable)
            {
                var entityValue = (decimal?) entity.GetPropertyValue(propertyName);
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
                var entityValue = (decimal) entity.GetPropertyValue(propertyName);
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
        }
        #endregion
    }
}