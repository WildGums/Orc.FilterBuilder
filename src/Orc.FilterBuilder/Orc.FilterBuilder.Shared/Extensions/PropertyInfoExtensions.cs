// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
                if (propertyInfo.TryGetAttribute(out catelDispNameAttr))
                {
                    return catelDispNameAttr.DisplayName;
                }

                DisplayNameAttribute dispNameAttr;
                if (propertyInfo.TryGetAttribute(out dispNameAttr))
                {
                    return dispNameAttr.DisplayName;
                }

                DisplayAttribute dispAttr;
                if (propertyInfo.TryGetAttribute(out dispAttr))
                {
                    return dispAttr.GetName();
                }
            }

            return null;
        }
        #endregion
    }
}