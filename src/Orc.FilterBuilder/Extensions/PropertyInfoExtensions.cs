namespace Orc.FilterBuilder;

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Catel.ComponentModel;
using Catel.Reflection;

internal static class PropertyInfoExtensions
{
    public static string? GetDisplayName(this PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        if (propertyInfo.TryGetAttribute<DisplayNameAttribute>(out var catelDispNameAttr))
        {
            return catelDispNameAttr.DisplayName;
        }

        return propertyInfo.TryGetAttribute<DisplayAttribute>(out var dispAttr) 
            ? dispAttr.GetName() 
            : null;
    }
}
