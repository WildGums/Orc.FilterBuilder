// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Converters
{
    using System;
    using Catel.Data;
    using Catel.Logging;
    using Catel.MVVM.Converters;
    using Catel.Reflection;
    using Models;

    public class ObjectToValueConverter : ValueConverterBase
    {
        private readonly IPropertyMetadata _propertyMetadata;

        public ObjectToValueConverter(IPropertyMetadata propertyMetadata)
        {
            _propertyMetadata = propertyMetadata;
        }

        public ObjectToValueConverter()
        {

        }

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var propertyName = parameter as string;
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            try
            {
                var modelBase = value as IModelEditor;
                if (modelBase != null)
                {
                    return modelBase.GetValue(propertyName);
                }

                if (_propertyMetadata != null)
                {
                    return _propertyMetadata.GetValue(value);
                }

                if (value != null)
                {
                    return PropertyHelper.GetPropertyValue(value, propertyName, false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get property value '{0}'", propertyName);
            }

            return null;
        }
        #endregion
    }
}