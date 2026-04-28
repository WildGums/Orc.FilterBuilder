namespace Orc.FilterBuilder.Converters;

using System;
using Catel;
using Catel.Data;
using Catel.Logging;
using Catel.MVVM.Converters;
using Catel.Reflection;
using Microsoft.Extensions.Logging;

public partial class ObjectToValueConverter : ValueConverterBase
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ObjectToValueConverter));

    private readonly IPropertyMetadata? _propertyMetadata;

    public ObjectToValueConverter()
    {
        throw new CatelException("Do not use this ctor");
    }

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
            if (value is IModelEditor modelBase)
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

            return PropertyHelper.GetPropertyValue(value, propertyName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to get property value '{0}'", propertyName);
        }

        return null;
    }
}
