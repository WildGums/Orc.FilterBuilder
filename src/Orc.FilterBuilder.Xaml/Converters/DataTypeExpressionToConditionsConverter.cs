﻿namespace Orc.FilterBuilder.Converters;

using System;
using Catel.MVVM.Converters;
using Catel.Reflection;

public class DataTypeExpressionToConditionsConverter : ValueConverterBase
{
    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (value is not DataTypeExpression dataTypeExpression)
        {
            return null;
        }

        var isNullable = false;

        if (PropertyHelper.IsPropertyAvailable(dataTypeExpression, "IsNullable"))
        {
            isNullable = PropertyHelper.GetPropertyValue<bool>(dataTypeExpression, "IsNullable");
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

            case ValueControlType.Enum:
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
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        return conditions;

    }
}
