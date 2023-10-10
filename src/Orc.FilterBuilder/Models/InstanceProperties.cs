namespace Orc.FilterBuilder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Catel.Data;
using Catel.Reflection;

public class InstanceProperties : IPropertyCollection
{
    public InstanceProperties(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var finalProperties = new Dictionary<string, IPropertyMetadata>();

        var regularProperties = new List<PropertyInfo>();
        regularProperties.AddRange(type.GetProperties().Where(m => m.CanRead && InstancePropertyHelper.IsSupportedType(m)));

        foreach (var property in regularProperties.Distinct())
        {
            finalProperties[property.Name] = new PropertyMetadata(type, property);
        }

        var catelProperties = new List<IPropertyData>();
        if (typeof(ModelBase).IsAssignableFromEx(type))
        {
            var propertyDataManager = PropertyDataManager.Default;
            var catelTypeInfo = propertyDataManager.GetCatelTypeInfo(type);
            catelProperties.AddRange(catelTypeInfo.GetCatelProperties().Values.Where(x => !x.IsModelBaseProperty));
        }

        foreach (var property in catelProperties.Distinct())
        {
            finalProperties[property.Name] = new PropertyMetadata(type, property);
        }

        Properties = new List<IPropertyMetadata>(finalProperties.Values.OrderBy(m => m.Name));
    }

    public List<IPropertyMetadata> Properties { get; }

    public IPropertyMetadata? GetProperty(string propertyName)
    {
        return (from property in Properties
            where string.Equals(property.Name, propertyName)
            select property).FirstOrDefault();
    }
}
