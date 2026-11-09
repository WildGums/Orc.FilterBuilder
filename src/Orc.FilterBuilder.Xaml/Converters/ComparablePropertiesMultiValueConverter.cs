namespace Orc.FilterBuilder.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

/// <summary>
/// Returns the properties that the given property can be compared to.
/// Expects the property as the first value and all available properties as the second value.
/// </summary>
public class ComparablePropertiesMultiValueConverter : IMultiValueConverter
{
    // Results are cached per list of available properties (held weakly) and per property, so every
    // row and every re-evaluation for the same field reuses one array. The available properties
    // list is expected not to change after it is bound.
    private readonly ConditionalWeakTable<IEnumerable<IPropertyMetadata>, Dictionary<IPropertyMetadata, IPropertyMetadata[]>> _cache = new();

    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (values is not [IPropertyMetadata property, IEnumerable<IPropertyMetadata> properties])
        {
            return Array.Empty<IPropertyMetadata>();
        }

        var comparablePropertiesCache = _cache.GetOrCreateValue(properties);
        if (!comparablePropertiesCache.TryGetValue(property, out var comparableProperties))
        {
            comparableProperties = properties.Where(property.CanCompareTo).ToArray();
            comparablePropertiesCache[property] = comparableProperties;
        }

        return comparableProperties;
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        return null;
    }
}
