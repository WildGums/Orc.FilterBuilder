using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Catel.Reflection;

namespace Orc.FilterBuilder
{
    static class PropertyInfoExtensions
    {
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                DisplayNameAttribute dispNameAttr;
                if (AttributeHelper.TryGetAttribute(propertyInfo, out dispNameAttr))
                    return dispNameAttr.DisplayName;

                DisplayAttribute dispAttr;
                if (AttributeHelper.TryGetAttribute(propertyInfo, out dispAttr))
                    return dispAttr.GetName();
            }
            return null;
        }
    }
}
