namespace Orc.FilterBuilder.Tests;

using System.Collections.Generic;

public static class TestAttributeTypeProvider
{
    public static readonly Dictionary<string, TestAttributeType> AttributeTypes;

    static TestAttributeTypeProvider()
    {
        AttributeTypes = new Dictionary<string, TestAttributeType>()
        {
            {"StringAttribute", new TestAttributeType("StringAttribute", typeof (string))},
            {"IntAttribute", new TestAttributeType("IntAttribute", typeof (string))},
            {"DateTimeAttribute", new TestAttributeType("DateTimeAttribute", typeof (string))},
            {"BoolAttribute", new TestAttributeType("BoolAttribute", typeof (string))},
        };
    }
}
