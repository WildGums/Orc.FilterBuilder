namespace Orc.FilterBuilder.Converters
{
    using System;
    using Catel.Data;
    using Catel.Logging;
    using Catel.MVVM.Converters;
    using Catel.Reflection;

    public class ObjectToValueConverter : ValueConverterBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IPropertyMetadata? _propertyMetadata;

        public ObjectToValueConverter(IPropertyMetadata? propertyMetadata)
        {
            _propertyMetadata = propertyMetadata;
        }

        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (value is null)
            {
                return null;
            }

            var propertyName = parameter as string;
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            try
            {
                var modelBase = value as IModelEditor;
                if (modelBase is not null)
                {
                    var propertyDataManager = PropertyDataManager.Default;
                    if (propertyDataManager.IsPropertyRegistered(modelBase.GetType(), propertyName))
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        return modelBase.GetValueFastButUnsecure<object>(propertyName);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }

                if (_propertyMetadata is not null)
                {
                    return _propertyMetadata.GetValue(value);
                }

                if (value is not null)
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
    }
}
