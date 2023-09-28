namespace Orc.FilterBuilder;

using System;
using System.Diagnostics;
using System.Reflection;
using Catel;
using Catel.Data;

[DebuggerDisplay("{OwnerType}.{Name}")]
public class PropertyMetadata : IPropertyMetadata
{
    private readonly IPropertyData? _propertyData;
    private readonly PropertyInfo? _propertyInfo;
    private string? _displayName;

    public PropertyMetadata(Type ownerType, PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(ownerType);
        ArgumentNullException.ThrowIfNull(propertyInfo);

        _propertyInfo = propertyInfo;

        OwnerType = ownerType;
        Name = propertyInfo.Name;
        DisplayName = propertyInfo.GetDisplayName() ?? Name;
        Type = propertyInfo.PropertyType;
    }

    public PropertyMetadata(Type ownerType, IPropertyData propertyData)
    {
        ArgumentNullException.ThrowIfNull(ownerType);
        ArgumentNullException.ThrowIfNull(propertyData);

        _propertyData = propertyData;

        OwnerType = ownerType;
        Name = propertyData.Name;
        DisplayName = ownerType.GetProperty(propertyData.Name)?.GetDisplayName() ?? Name;
        Type = propertyData.Type;
    }

    public string DisplayName
    {
        get => _displayName ?? Name;
        set => _displayName = value;
    }

    public string Name { get; }

    public Type OwnerType { get; }

    public Type Type { get; }

    private bool Equals(PropertyMetadata other)
    {
        return Equals(_propertyData, other._propertyData) && string.Equals(Name, other.Name) && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() 
               && Equals((PropertyMetadata)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = _propertyData is not null ? _propertyData.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            hashCode = (hashCode * 397) ^ Type.GetHashCode();
            return hashCode;
        }
    }

    public object? GetValue(object instance)
    {
        return GetValue<object?>(instance);
    }

    public TValue? GetValue<TValue>(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        object? value = null;

        if (_propertyInfo is not null)
        {
            value = _propertyInfo.GetValue(instance, null);
        }
        else if (_propertyData is not null && instance is IModelEditor modelEditor)
        {
            value = modelEditor.GetValue<TValue>(_propertyData.Name);
        }

        if (value is null)
        {
            return default;
        }

        if (typeof(TValue) == typeof(string))
        {
            value = ObjectToStringHelper.ToString(value);
        }

        return (TValue)value;
    }

    public void SetValue(object instance, object? value)
    {
        ArgumentNullException.ThrowIfNull(instance);

        _propertyInfo?.SetValue(instance, value, null);

        if (_propertyData is null)
        {
            return;
        }

        if (instance is IModelEditor modelEditor)
        {
            modelEditor.SetValue(_propertyData.Name, value);
        }
    }
}
