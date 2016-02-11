// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerExpression.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using System.Diagnostics;
    using Orc.FilterBuilder.Models;

    [DebuggerDisplay("{ValueControlType} {SelectedCondition} {Value}")]
    public class IntegerExpression : NullableDataTypeExpression
    {
        #region Constructors
        public IntegerExpression()
            : this(false)
        {
        }

        public IntegerExpression(bool isNullable)
        {
            IsNullable = isNullable;
            SelectedCondition = Condition.EqualTo;
            Value = 0;
            ValueControlType = ValueControlType.Integer;
        }
        #endregion

        #region Properties
        public int Value { get; set; }
        #endregion

        #region Methods
        public override bool CalculateResult(IPropertyMetadata propertyMetadata, object entity)
        {
            if (IsNullable)
            {
                var entityValue = propertyMetadata.GetValue<int?>(entity);
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
                var entityValue = propertyMetadata.GetValue<int>(entity);
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

        public override string ToString()
        {
            return string.Format("{0} '{1}'", SelectedCondition.Humanize(), Value);
        }
        #endregion
    }
}