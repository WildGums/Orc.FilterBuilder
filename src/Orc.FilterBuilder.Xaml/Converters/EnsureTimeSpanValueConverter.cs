namespace Orc.FilterBuilder.Converters;

using System;
using Catel.MVVM.Converters;

internal class EnsureTimeSpanValueConverter : ValueConverterBase<object, TimeSpan?>
{
    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (value is not TimeSpan time)
        {
            return null;
        }

        return time;
    }

    protected override object? ConvertBack(TimeSpan? value, Type targetType, object? parameter)
    {
        return value;
    }
}
