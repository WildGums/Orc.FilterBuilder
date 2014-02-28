// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMetadata.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FilterBuilder.Models
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Catel;
    using Catel.Data;

    [DebuggerDisplay("{OwnerType}.{Name}")]
    public class PropertyMetadata : IPropertyMetadata
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly PropertyData _propertyData;

        #region Constructors
        public PropertyMetadata(Type ownerType, PropertyInfo propertyInfo)
        {
            Argument.IsNotNull("ownerType", ownerType);
            Argument.IsNotNull("propertyInfo", propertyInfo);

            _propertyInfo = propertyInfo;

            OwnerType = ownerType;
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
        }

        public PropertyMetadata(Type ownerType, PropertyData propertyData)
        {
            Argument.IsNotNull("ownerType", ownerType);
            Argument.IsNotNull("propertyData", propertyData);

            _propertyData = propertyData;

            OwnerType = ownerType;
            Name = propertyData.Name;
            Type = propertyData.Type;
        }
        #endregion

        #region Properties
        public Type OwnerType { get; private set; }

        public string Name { get; private set; }

        public Type Type { get; private set; }
        #endregion

        #region Methods
        public void SetValue(object instance, object value)
        {
            Argument.IsNotNull("instance", instance);

            if (_propertyInfo != null)
            {
                _propertyInfo.SetValue(instance, value, null);
            }

            if (_propertyData != null)
            {
                var modelEditor = instance as IModelEditor;
                if (modelEditor != null)
                {
                    modelEditor.SetValue(_propertyData.Name, value);
                }
            }
        }

        public object GetValue(object instance)
        {
            return GetValue<object>(instance);
        }

        public TValue GetValue<TValue>(object instance)
        {
            Argument.IsNotNull("instance", instance);

            if (_propertyInfo != null)
            {
                return (TValue) _propertyInfo.GetValue(instance, null);
            }

            if (_propertyData != null)
            {
                var modelEditor = instance as IModelEditor;
                if (modelEditor != null)
                {
                    return modelEditor.GetValue<TValue>(_propertyData.Name);
                }
            }

            return default(TValue);
        }
        #endregion
    }
}