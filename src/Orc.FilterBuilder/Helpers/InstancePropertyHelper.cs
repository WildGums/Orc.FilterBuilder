namespace Orc.FilterBuilder
{
    using Catel.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class InstancePropertyHelper
    {
        private static readonly HashSet<Type> SupportedTypes;

        private static readonly HashSet<Type> UnsupportedTypes;

        static InstancePropertyHelper()
        {
            UnsupportedTypes = new HashSet<Type>
            {
                typeof(bool?),
                typeof(TimeSpan?)
            };

            SupportedTypes = new HashSet<Type>
            {
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(ushort),
                typeof(short),
                typeof(uint),
                typeof(int),
                typeof(ulong),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(string),
                typeof(DateTime),
                typeof(TimeSpan)
            };
        }

        public static bool IsSupportedType(this IPropertyMetadata property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return IsSupportedType(property.Type);
        }

        public static bool IsSupportedType(this PropertyInfo property)
        {
            ArgumentNullException.ThrowIfNull(property);

            return IsSupportedType(property.PropertyType);
        }

        public static bool IsSupportedType(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (UnsupportedTypes.Contains(type))
            {
                return false;
            }

            if (type.IsNullableType())
            {
                type = type.GetNonNullable();
            }

            return SupportedTypes.Contains(type) || type.IsEnumEx();
        }
    }
}
