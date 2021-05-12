// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Catel.ComponentModel;
    using Catel.Reflection;

    internal static class PropertyInfoExtensions
    {
        #region Methods
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            if (propertyInfo is not null)
            {
                if (propertyInfo.TryGetAttribute(out DisplayNameAttribute catelDispNameAttr))
                {
                    return catelDispNameAttr.DisplayName;
                }

                if (propertyInfo.TryGetAttribute(out DisplayAttribute dispAttr))
                {
                    return dispAttr.GetName();
                }
            }

            return null;
        }
        #endregion
    }
}