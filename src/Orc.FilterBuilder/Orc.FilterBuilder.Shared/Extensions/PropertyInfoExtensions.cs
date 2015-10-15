using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Orc.FilterBuilder
{
    static class PropertyInfoExtensions
    {
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                var dispAttr = (DisplayAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayAttribute));
                if (dispAttr != null)
                    return dispAttr.GetName();
            }
            return null;
        }
    }
}
