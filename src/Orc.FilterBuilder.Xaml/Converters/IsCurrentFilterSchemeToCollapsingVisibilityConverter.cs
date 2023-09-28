namespace Orc.FilterBuilder.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Catel;

public class IsCurrentFilterSchemeToCollapsingVisibilityConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (values is null)
        {
            return null;
        }

        var filterScheme = values[0] as FilterScheme;
        var selectedFilterScheme = values[1] as FilterScheme;

        var visibility = Visibility.Collapsed;

        if (filterScheme is null)
        {
            return visibility;
        }

        if (ObjectHelper.AreEqual(selectedFilterScheme, filterScheme))
        {
            visibility = Visibility.Visible;
        }

        return visibility;
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}