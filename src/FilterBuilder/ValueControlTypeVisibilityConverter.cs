using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Orc.FilterBuilder
{
	public class ValueControlTypeVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ValueControlType controlType = (ValueControlType) value;
			ValueControlType parameterType = (ValueControlType) parameter;
			return controlType == parameterType
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
