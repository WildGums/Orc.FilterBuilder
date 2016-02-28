// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTypeToConditionsConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using Catel.MVVM.Converters;
    using Catel.Reflection;

    public class DataTypeExpressionToConditionsConverter : ValueConverterBase
    {
        public DataTypeExpressionToConditionsConverter()
        {
            
        }

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var dataTypeExpression = value as DataTypeExpression;
            if (dataTypeExpression != null)
            {
                bool isNullable = false;

                if (PropertyHelper.IsPropertyAvailable(dataTypeExpression, "IsNullable", false))
                {
                    isNullable = PropertyHelper.GetPropertyValue<bool>(dataTypeExpression, "IsNullable", false);
                }

                object conditions = isNullable ? ConditionHelper.GetNullableValueConditions() : ConditionHelper.GetValueConditions();

                switch (dataTypeExpression.ValueControlType)
                {
                    case ValueControlType.Boolean:
                        conditions = ConditionHelper.GetBooleanConditions();
                        break;

                    case ValueControlType.DateTime:
                        // No custom conditions
                        break;

                    case ValueControlType.Byte:
                    case ValueControlType.SByte:
                    case ValueControlType.Short:
                    case ValueControlType.UnsignedShort:
                    case ValueControlType.Integer:
                    case ValueControlType.UnsignedInteger:
                    case ValueControlType.Long:
                    case ValueControlType.UnsignedLong:
                    case ValueControlType.Decimal:
                    case ValueControlType.Float:
                    case ValueControlType.Double:
                    case ValueControlType.Numeric:
                        // No custom conditions
                        break;

                    case ValueControlType.TimeSpan:
                        // No custom conditions
                        break;

                    case ValueControlType.Text:
                        conditions = ConditionHelper.GetStringConditions();
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