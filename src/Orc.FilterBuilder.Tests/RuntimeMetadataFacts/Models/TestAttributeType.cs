namespace Orc.FilterBuilder.Tests;

using System;

public class TestAttributeType
{
    public TestAttributeType(string name, Type type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
}
