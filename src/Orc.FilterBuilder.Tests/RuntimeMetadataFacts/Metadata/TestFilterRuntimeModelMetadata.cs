namespace Orc.FilterBuilder.Tests;

using System;

public class TestFilterRuntimeModelMetadata : IPropertyMetadata
{
    private readonly TestAttributeType _attributeType;

    public TestFilterRuntimeModelMetadata(TestAttributeType attributeType)
    {
        _attributeType = attributeType;

        OwnerType = typeof (TestFilterRuntimeModel);
        Type = attributeType.Type;
        DisplayName = _attributeType.Name;
    }

    public string DisplayName { get; set; }
    public string Name => _attributeType.Name;
    public Type OwnerType { get; }
    public Type Type { get; }

    public object GetValue(object instance)
    {
        var testData = instance as TestFilterRuntimeModel;

        return GetAttributeValue(testData, _attributeType);
    }

    public TValue GetValue<TValue>(object instance)
    {
        return (TValue) GetValue(instance);
    }

    public void SetValue(object instance, object value)
    {
        throw new NotImplementedException();
    }

    private object GetAttributeValue(TestFilterRuntimeModel operation, TestAttributeType attributeType)
    {
        var attributeName = attributeType?.Name;

        return operation.Attributes.TryGetValue(attributeName, out var value)
            ? value.Value 
            : null;
    }
}
