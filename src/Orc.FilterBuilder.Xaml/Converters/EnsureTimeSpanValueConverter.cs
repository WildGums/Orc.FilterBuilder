namespace Orc.FilterBuilder.Converters;

using System;
using Catel.MVVM.Converters;

internal class EnsureTimeSpanValueConverter : ValueConverterBase<object, DateTime?>
{
    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (value is not TimeSpan time)
        {
            return null;
        }

        return time;
    }
}
