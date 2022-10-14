namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Windows;
    using Catel;
    using Catel.MVVM.Converters;

    public class ConditionTreeItemConverter : VisibilityConverterBase
    {
        public ConditionTreeItemConverter()
            : base(Visibility.Collapsed)
        {

        }

        protected override bool IsVisible(object? value, Type targetType, object? parameter)
        {
            if (parameter is null)
            {
                return false;
            }

            switch ((string) parameter)
            {
                case "Group":
                    return value is ConditionGroup;

                case "Expression":
                    return value is PropertyExpression;

                default:
                    throw new NotSupportedException(string.Format(LanguageHelper.GetRequiredString("FilterBuilder_Exception_Message_ParameterIsNotSupported_Pattern"), parameter));
            }
        }
    }
}
