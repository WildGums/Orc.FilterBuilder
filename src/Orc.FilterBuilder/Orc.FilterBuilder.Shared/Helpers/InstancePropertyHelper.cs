// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstancePropertyHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder
{
    using Catel;
    using Catel.Reflection;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class InstancePropertyHelper
    {
        private static readonly HashSet<Type> _supportedTypes;

        static InstancePropertyHelper()
        {
            _supportedTypes = new HashSet<Type>
            {
                typeof(bool),
                typeof(byte),
                typeof(byte?),
                typeof(sbyte),
                typeof(sbyte?),
                typeof(ushort),
                typeof(ushort?),
                typeof(short),
                typeof(short?),
                typeof(uint),
                typeof(uint?),
                typeof(int),
                typeof(int?),
                typeof(ulong),
                typeof(ulong?),
                typeof(long),
                typeof(long?),
                typeof(float),
                typeof(float?),
                typeof(double),
                typeof(double?),
                typeof(decimal),
                typeof(decimal?),
                typeof(string),
                typeof(DateTime),
                typeof(DateTime?),
                typeof(TimeSpan)
            };
        }

        public static bool IsSupportedType(this IPropertyMetadata property)
        {
            Argument.IsNotNull(() => property);

            return IsSupportedType(property.Type);
        }

        public static bool IsSupportedType(this PropertyInfo property)
        {
            Argument.IsNotNull(() => property);

            return IsSupportedType(property.PropertyType);
        }

        public static bool IsSupportedType(this Type type)
        {
            Argument.IsNotNull(() => type);

            return _supportedTypes.Contains(type) || type.IsEnumEx();
        }
    }
}
