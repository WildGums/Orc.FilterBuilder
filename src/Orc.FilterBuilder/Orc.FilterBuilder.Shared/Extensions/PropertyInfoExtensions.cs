// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Catel.Reflection;

    internal static class PropertyInfoExtensions
    {
        #region Methods
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                DisplayNameAttribute catelDispNameAttr;
                if (AttributeHelper.TryGetAttribute(propertyInfo, out catelDispNameAttr))
                {
                    return catelDispNameAttr.DisplayName;
                }

                DisplayNameAttribute dispNameAttr;
                if (AttributeHelper.TryGetAttribute(propertyInfo, out dispNameAttr))
                {
                    return dispNameAttr.DisplayName;
                }

                DisplayAttribute dispAttr;
                if (AttributeHelper.TryGetAttribute(propertyInfo, out dispAttr))
                {
                    return dispAttr.GetName();
                }
            }

            return null;
        }
        #endregion
    }
}