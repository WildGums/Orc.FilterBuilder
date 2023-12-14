namespace Orc.FilterBuilder.Tests;

using System;
using System.Collections.Generic;
using System.Linq;

public class TestFilterRuntimeModelPropertyCollection : IPropertyCollection
{
    public TestFilterRuntimeModelPropertyCollection(IList<TestAttributeType> attributeTypes)
    {
        ArgumentNullException.ThrowIfNull(attributeTypes);

        Properties = attributeTypes.Select(x => new TestFilterRuntimeModelMetadata(x))
            .Cast<IPropertyMetadata>()
            .ToList();
    }

    public List<IPropertyMetadata> Properties { get; }

    public IPropertyMetadata GetProperty(string propertyName)
    {
        return Properties.FirstOrDefault(x => x.Name == propertyName);
    }
}
