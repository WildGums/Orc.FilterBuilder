// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LeftMarginMultiplierConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM.Converters;

    public class LeftMarginMultiplierConverter : ValueConverterBase
    {
        #region Properties
        public double Length { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var item = value as TreeViewItem;
            if (item == null)
            {
                return new Thickness(0);
            }

            return new Thickness(Length*item.GetDepth(), 0, 0, 0);
        }
        #endregion
    }
}