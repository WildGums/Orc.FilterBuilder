// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using Orc.FilterBuilder.Models;

    public class DateTimeExpression : DataTypeExpression
    {
        #region Constructors
        public DateTimeExpression()
            : this(false)
        {
        }

        public DateTimeExpression(bool isNullable)
        {
            IsNullable = isNullable;
            SelectedCondition = Condition.EqualTo;
            Value = DateTime.Now;
            ValueControlType = ValueControlType.DateTime;
        }
        #endregion

        #region Properties
        public bool IsNullable { get; set; }

        public DateTime Value { get; set; }
        #endregion

        #region Methods
        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            if (IsNullable)
            {
                var entityValue = propertyMetadata.GetValue<DateTime?>(entity);
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
                var entityValue = propertyMetadata.GetValue<DateTime>(entity);
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