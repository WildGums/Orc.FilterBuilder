namespace Orc.FilterBuilder.Converters;

using System;
using Catel;
using Catel.MVVM.Converters;

public class ConditionTreeItemConverter : VisibilityConverterBase
{
    public ConditionTreeItemConverter()
    {

    }

    protected override bool IsVisible(object? value, Type targetType, object? parameter)
    {
        if (parameter is null)
        {
            return false;
        }

        return (string)parameter switch
        {
            "Group" => value is ConditionGroup,
            "Expression" => value is PropertyExpression,
            _ => throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ParameterIsNotSupported_Pattern"), parameter))
        };
    }
}
