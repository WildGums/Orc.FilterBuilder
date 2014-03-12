// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToValueConverter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using Catel.Data;
    using Catel.Reflection;
    using Catel.Windows.Data.Converters;

    public class ObjectToValueConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var propertyName = parameter as string;
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            var modelBase = value as IModelEditor;
            if (modelBase != null)
            {
                return modelBase.GetValue(propertyName);
            }

            if (value != null)
            {
                return PropertyHelper.GetPropertyValue(value, propertyName);
            }

            return null;
        }
        #endregion
    }
}