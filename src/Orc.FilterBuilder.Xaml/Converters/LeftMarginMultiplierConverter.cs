namespace Orc.FilterBuilder.Converters;

using System;
using System.Windows;
using System.Windows.Controls;
using Catel.MVVM.Converters;

public class LeftMarginMultiplierConverter : ValueConverterBase
{
    private static Thickness DefaultThickness = new(0);

    public double Length { get; set; }

    protected override object Convert(object? value, Type targetType, object? parameter)
    {
        return value is not TreeViewItem item
            ? DefaultThickness
            : new Thickness(Length * item.GetDepth(), 0, 0, 0);
    }
}
