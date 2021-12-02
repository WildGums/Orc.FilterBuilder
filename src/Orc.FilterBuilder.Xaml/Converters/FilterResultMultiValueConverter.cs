namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Catel;

    public class FilterResultMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null 
                || values.Length != 4
                || values[0] is not bool
                || values[1] is not bool
                || values[2] is not int
                || values[3] is not int)
            {
                return string.Empty;
            }

            var isEnabled = (bool)values[0];
            if (isEnabled)
            {
                var isPreviewVisible = (bool)values[1];

                return $"{(isPreviewVisible ? LanguageHelper.GetString("FilterBuilder_Hide", culture) : LanguageHelper.GetString("FilterBuilder_Show", culture))} {string.Format(LanguageHelper.GetString("FilterBuilder_FilteredItemsOfPattern"), values[2], values[3])}";
            }
            
            return LanguageHelper.GetString("FilterBuilder_NotFiltered", culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
