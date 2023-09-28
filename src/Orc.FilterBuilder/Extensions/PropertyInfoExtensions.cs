namespace Orc.FilterBuilder;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Catel.ComponentModel;
using Catel.Reflection;

internal static class PropertyInfoExtensions
{
    public static string? GetDisplayName(this PropertyInfo propertyInfo)
    {
        if (propertyInfo is not null)
        {
            if (propertyInfo.TryGetAttribute<DisplayNameAttribute>(out var catelDispNameAttr))
            {
                return catelDispNameAttr.DisplayName;
            }

            if (propertyInfo.TryGetAttribute<DisplayAttribute>(out var dispAttr))
            {
                return dispAttr.GetName();
            }
        }

        return null;
    }
}