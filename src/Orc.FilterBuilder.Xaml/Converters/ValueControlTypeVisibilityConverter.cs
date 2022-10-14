namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Linq;
    using System.Windows;
    using Catel.MVVM.Converters;
    using Catel.Reflection;

    public class ValueControlTypeVisibilityConverter : VisibilityConverterBase
    {
        public ValueControlTypeVisibilityConverter()
            : base(Visibility.Collapsed)
        {

        }

        protected override bool IsVisible(object? value, Type targetType, object? parameter)
        {
            if (value is not ValueControlType controlType)
            {
                return false;
            }

            if (parameter is not null && parameter.GetType().IsArrayEx())
            {
                var parameterType = (ValueControlType[])parameter;
                return parameterType.Contains(controlType);
            }
            else if (parameter is ValueControlType valueControlType)
            {
                return controlType == valueControlType;
            }

            return false;
        }
    }
}
