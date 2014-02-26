// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExpression.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System;
    using Fasterflect;

    public class StringExpression : DataTypeExpression
    {
        #region Constructors
        public StringExpression()
        {
            Conditions = GetStringConditions();
            SelectedCondition = Condition.Contains;
            Value = string.Empty;
            ValueControlType = ValueControlType.Text;
        }
        #endregion

        #region Properties
        public string Value { get; set; }
        #endregion

        #region Methods
        public override bool CalculateResult(string propertyName, object entity)
        {
            var entityValue = entity.GetPropertyValue(propertyName) as string;

            switch (SelectedCondition)
            {
                case Condition.Contains:
                    return entityValue != null && entityValue.Contains(Value);
                case Condition.EndsWith:
                    return entityValue != null && entityValue.EndsWith(Value);
                case Condition.EqualTo:
                    return entityValue == Value;
                case Condition.GreaterThan:
                    return string.Compare(entityValue, Value) > 0;
                case Condition.GreaterThanOrEqualTo:
                    return string.Compare(entityValue, Value) >= 0;
                case Condition.IsEmpty:
                    return entityValue == string.Empty;
                case Condition.IsNull:
                    return entityValue == null;
                case Condition.LessThan:
                    return string.Compare(entityValue, Value) < 0;
                case Condition.LessThanOrEqualTo:
                    return string.Compare(entityValue, Value) <= 0;
                case Condition.NotEqualTo:
                    return entityValue != Value;
                case Condition.NotIsEmpty:
                    return entityValue != string.Empty;
                case Condition.NotIsNull:
                    return entityValue != null;
                case Condition.StartsWith:
                    return entityValue != null && entityValue.StartsWith(Value);
                default:
                    throw new NotSupportedException(string.Format("Condition '{0}' is not supported.", SelectedCondition));
            }
        }
        #endregion
    }
}