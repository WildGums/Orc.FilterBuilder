namespace Orc.FilterBuilder.Serialization.Json;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catel.Reflection;

public abstract class AbstractTypeJsonConverter<T> : JsonConverter<T>
{
    public override bool CanConvert(Type typeToConvert) =>
        typeof(T).IsAssignableFrom(typeToConvert);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        var propertyName = reader.GetString();
        if (propertyName != "$type")
        {
            throw new JsonException();
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        var itemType = TypeCache.GetType(reader.GetString()!);
        if (itemType is null)
        {
            throw new JsonException();
        }

        // Safety check, we don't want to instantiate any type 
        if (!typeof(T).IsAssignableFrom(itemType))
        {
            throw new JsonException();
        }

        // Now we can deserialize the (same) object now we know the type 
        var typeInfo = options.GetTypeInfo(itemType);

        // Need to skip because of $type 
        typeInfo.Options.UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip;

        var element = JsonElement.ParseValue(ref reader);
        var item = element.Deserialize(typeInfo);

        if (item is not T typedValue)
        {
            throw new JsonException();
        }

        reader.Read();

        return typedValue;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WriteString("$type", value.GetType().GetSafeFullName());

        var typeInfo = options.GetTypeInfo(value.GetType());

        var temp = System.Text.Json.JsonSerializer.SerializeToNode(value, typeInfo)!.AsObject();

        foreach (var kvp in temp)
        {
            writer.WritePropertyName(kvp.Key);
            if (kvp.Value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                kvp.Value.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}