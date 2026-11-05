namespace Orc.FilterBuilder.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

/// <summary>
/// Returns the properties that the given property can be compared to.
/// Expects the property as the first value and all available properties as the second value.
/// </summary>
public class ComparablePropertiesMultiValueConverter : IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (values is not [IPropertyMetadata property, IEnumerable<IPropertyMetadata> properties])
        {
            return new List<IPropertyMetadata>();
        }

        return properties.Where(x => property.CanCompareTo(x)).ToList();
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        return null;
    }
}
