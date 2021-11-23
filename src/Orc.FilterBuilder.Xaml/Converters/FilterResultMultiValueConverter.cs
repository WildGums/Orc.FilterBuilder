namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class FilterResultMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length != 2)
            {
                return string.Empty;
            }

            return $"{values[0]} items of {values[1]} filtered";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
