// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueControlTypeVisibilityConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;

    public class ValueControlTypeVisibilityConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var controlType = (ValueControlType) value;
            var parameterType = (ValueControlType) parameter;
            return controlType == parameterType ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}