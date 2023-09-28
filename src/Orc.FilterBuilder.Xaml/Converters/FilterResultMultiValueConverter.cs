namespace Orc.FilterBuilder.Converters;

using System;
using System.Globalization;
using System.Windows.Data;
using Catel;

public class FilterResultMultiValueConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (culture is null)
        {
            culture = CultureInfo.CurrentUICulture;
        }

        if (values is null 
            || values.Length != 4
            || values[0] is not bool isEnabled
            || values[1] is not bool isPreviewVisible
            || values[2] is not int
            || values[3] is not int)
        {
            return string.Empty;
        }

        if (isEnabled)
        {
            return $"{(isPreviewVisible ? LanguageHelper.GetRequiredString("FilterBuilder_Hide", culture) : LanguageHelper.GetRequiredString("FilterBuilder_Show", culture))} {string.Format(LanguageHelper.GetRequiredString("FilterBuilder_FilteredItemsOfPattern"), values[2], values[3])}";
        }
            
        return LanguageHelper.GetRequiredString("FilterBuilder_NotFiltered", culture);
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        return null;
    }
}