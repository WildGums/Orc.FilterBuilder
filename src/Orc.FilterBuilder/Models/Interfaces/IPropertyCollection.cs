namespace Orc.FilterBuilder;

using System.Collections.Generic;

public interface IPropertyCollection
{
    List<IPropertyMetadata> Properties { get; }

    IPropertyMetadata? GetProperty(string propertyName);
}