// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeToConditionsConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using Catel.Reflection;
    using Catel.Windows.Data.Converters;

    public class DataTypeExpressionToConditionsConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var dataTypeExpression = value as DataTypeExpression;
            if (dataTypeExpression != null)
            {
                bool isNullable = false;

                if (PropertyHelper.IsPropertyAvailable(dataTypeExpression, "IsNullable"))
                {
                    isNullable = PropertyHelper.GetPropertyValue<bool>(dataTypeExpression, "IsNullable");
                }

                object conditions = isNullable ? ConditionHelper.GetNullableValueConditions() : ConditionHelper.GetValueConditions();

                switch (dataTypeExpression.ValueControlType)
                {
                    case ValueControlType.Text:
                        conditions = ConditionHelper.GetStringConditions();
                        break;

                    case ValueControlType.DateTime:
                        // No custom conditions
                        break;

                    case ValueControlType.Boolean:
                        conditions = ConditionHelper.GetBooleanConditions();
                        break;

                    case ValueControlType.TimeSpan:
                        // No custom conditions
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return conditions;
            }

            return null;
        }
        #endregion
    }
}