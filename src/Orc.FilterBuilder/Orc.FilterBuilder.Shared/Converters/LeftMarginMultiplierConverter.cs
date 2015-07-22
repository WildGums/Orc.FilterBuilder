// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LeftMarginMultiplierConverter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM.Converters;
    using Extensions;

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