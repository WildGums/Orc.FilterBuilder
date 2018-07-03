// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMetadata.cs" company="WildGums">
//     Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
        private readonly PropertyData _propertyData;
        private readonly PropertyInfo _propertyInfo;
        private string _displayName;

        #region Constructors
        public PropertyMetadata(Type ownerType, PropertyInfo propertyInfo)
        {
            Argument.IsNotNull(() => ownerType);
            Argument.IsNotNull(() => propertyInfo);

            _propertyInfo = propertyInfo;

            OwnerType = ownerType;
            Name = propertyInfo.Name;
            DisplayName = propertyInfo.GetDisplayName() ?? Name;
            Type = propertyInfo.PropertyType;
        }

        public PropertyMetadata(Type ownerType, PropertyData propertyData)
        {
            Argument.IsNotNull(() => ownerType);
            Argument.IsNotNull(() => propertyData);

            _propertyData = propertyData;

            OwnerType = ownerType;
            Name = propertyData.Name;
            DisplayName = ownerType.GetProperty(Name).GetDisplayName() ?? Name;
            Type = propertyData.Type;
        }
        #endregion Constructors

        #region Properties
        public string DisplayName
        {
            get
            {
                if (_displayName != null)
                {
                    return _displayName;
                }

                return Name;
            }
            set => _displayName = value;
        }

        public string Name { get; }

        public Type OwnerType { get; }

        public Type Type { get; }
        #endregion Properties

        #region Methods
        private bool Equals(PropertyMetadata other)
        {
            return Equals(_propertyData, other._propertyData) && string.Equals(Name, other.Name) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((PropertyMetadata)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _propertyData != null ? _propertyData.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        public object GetValue(object instance)
        {
            return GetValue<object>(instance);
        }

        public TValue GetValue<TValue>(object instance)
        {
            Argument.IsNotNull(() => instance);

            object value = null;

            if (_propertyInfo != null)
            {
                value = _propertyInfo.GetValue(instance, null);
            }
            else if (_propertyData != null)
            {
                if (instance is IModelEditor modelEditor)
                {
                    value = modelEditor.GetValue(_propertyData.Name);
                }
            }

            if (value == null)
            {
                return default(TValue);
            }

            if (typeof(TValue) == typeof(string))
            {
                value = ObjectToStringHelper.ToString(value);
            }

            return (TValue)value;
        }

        public void SetValue(object instance, object value)
        {
            Argument.IsNotNull(() => instance);

            if (_propertyInfo != null)
            {
                _propertyInfo.SetValue(instance, value, null);
            }

            if (_propertyData == null)
            {
                return;
            }

            if (instance is IModelEditor modelEditor)
            {
                modelEditor.SetValue(_propertyData.Name, value);
            }
        }
        #endregion Methods
    }
}
