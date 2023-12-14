namespace Orc.FilterBuilder.Converters;

using System;
using System.Globalization;
using System.Windows.Data;
using Catel;

public class FilterResultMultiValueConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentUICulture;

        if (values is not [bool isEnabled, bool isPreviewVisible, int, int])
        {
            return string.Empty;
        }

        return isEnabled 
            ? $"{(isPreviewVisible ? LanguageHelper.GetRequiredString("FilterBuilder_Hide", culture) : LanguageHelper.GetRequiredString("FilterBuilder_Show", culture))} {string.Format(LanguageHelper.GetRequiredString("FilterBuilder_FilteredItemsOfPattern"), values[2], values[3])}" 
            : LanguageHelper.GetRequiredString("FilterBuilder_NotFiltered", culture);
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        return null;
    }
}
