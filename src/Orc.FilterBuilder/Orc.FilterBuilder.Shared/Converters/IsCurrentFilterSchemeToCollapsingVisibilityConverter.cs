// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsCurrentFilterSchemeToCollapsingVisibilityConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Catel;
    using Models;

    public class IsCurrentFilterSchemeToCollapsingVisibilityConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var filterScheme = values[0] as FilterScheme;
            var selectedFilterScheme = values[1] as FilterScheme;

            var visibility = Visibility.Collapsed;

            if (filterScheme == null)
            {
                return visibility;
            }

            if (ObjectHelper.AreEqual(selectedFilterScheme, filterScheme))
            {
                visibility = Visibility.Visible;
            }

            return visibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}