namespace Orc.FilterBuilder.Tests.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TestFilterRuntimeModelReflectionService : IReflectionService
{
    private readonly IList<TestAttributeType> _attributeTypes;

    private TestFilterRuntimeModelPropertyCollection _propertyCollection;

    public TestFilterRuntimeModelReflectionService(IList<TestAttributeType> attributeTypes)
    {
        ArgumentNullException.ThrowIfNull(attributeTypes);

        _attributeTypes = attributeTypes;
    }

    public IPropertyCollection GetInstanceProperties(Type targetType)
    {
        return _propertyCollection ??= new TestFilterRuntimeModelPropertyCollection(_attributeTypes);
    }

    public Task<IPropertyCollection> GetInstancePropertiesAsync(Type targetType)
    {
        return Task.FromResult<IPropertyCollection>(GetInstanceProperties(targetType));
    }

    public void ClearCache()
    {
        _propertyCollection = null;
    }
}
