using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Orc.FilterBuilder.NET40
{
	public class ConditionTreeItemConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((string) parameter)
			{
				case "Group":
					return value is ConditionGroup
						? Visibility.Visible
						: Visibility.Collapsed;
				case "Expression":
					return value is PropertyExpression
						? Visibility.Visible
						: Visibility.Collapsed;
				default:
					throw new NotSupportedException(string.Format("Parameter {0} is not supported.", parameter));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
