namespace Orc.FilterBuilder.Converters;

using System;
using System.Globalization;
using System.Windows.Data;
using Catel.Services;

public partial class FilterResultMultiValueConverter : IMultiValueConverter
{
    private readonly ILanguageService _languageService;

    public FilterResultMultiValueConverter(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentUICulture;

        if (values is not [bool isEnabled, bool isPreviewVisible, int, int])
        {
            return string.Empty;
        }

        return isEnabled 
            ? $"{(isPreviewVisible ? _languageService.GetRequiredString("FilterBuilder_Hide", culture) : _languageService.GetRequiredString("FilterBuilder_Show", culture))} {string.Format(_languageService.GetRequiredString("FilterBuilder_FilteredItemsOfPattern"), values[2], values[3])}" 
            : _languageService.GetRequiredString("FilterBuilder_NotFiltered", culture);
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        return null;
    }
}
