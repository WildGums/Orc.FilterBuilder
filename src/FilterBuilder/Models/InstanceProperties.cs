// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceProperties.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Fasterflect;

    public class InstanceProperties
    {
        public InstanceProperties(Type type)
        {
            Argument.IsNotNull("type", type);

            var properties = new List<PropertyInfo>();

            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && m.Type() == typeof(string)));
            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && (m.Type() == typeof(int) || m.Type() == typeof(int?))));
            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && (m.Type() == typeof(DateTime) || m.Type() == typeof(DateTime?))));
            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && m.Type() == typeof(bool)));
            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && m.Type() == typeof(TimeSpan)));
            properties.AddRange(type.GetProperties().Where(m => m.IsReadable() && (m.Type() == typeof(decimal) || m.Type() == typeof(decimal?))));

            Properties = new List<PropertyInfo>(properties.OrderBy(m => m.Name));
        }

        #region Properties
        public List<PropertyInfo> Properties { get; private set; }
        #endregion

        #region Methods
        public PropertyInfo GetProperty(string name)
        {
            return Properties.Single(pi => pi.Name == name);
        }
        #endregion
    }
}