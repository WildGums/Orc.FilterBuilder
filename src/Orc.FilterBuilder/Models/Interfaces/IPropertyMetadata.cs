namespace Orc.FilterBuilder;

using System;
using System.Text.Json.Serialization;
using Orc.Serialization.Json;

public interface IPropertyMetadata
{
    string DisplayName { get; set; }

    string Name { get; }

    [JsonConverter(typeof(TypeJsonConverter))]
    Type OwnerType { get; }

    [JsonConverter(typeof(TypeJsonConverter))]
    Type Type { get; }

    object? GetValue(object instance);

    TValue? GetValue<TValue>(object instance);

    void SetValue(object instance, object? value);
}
