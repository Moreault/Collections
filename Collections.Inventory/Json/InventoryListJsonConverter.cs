using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.Inventory.Json;

public sealed class InventoryListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(InventoryList<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(InventoryListJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public sealed class InventoryListJsonConverter<T> : InventoryJsonConverterBase<InventoryList<T>, T>
{

}

public abstract class InventoryJsonConverterBase<TInventory, TItem> : JsonConverter<TInventory> where TInventory : Inventory<TItem>, new()
{
    public override TInventory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token.");
        }

        var inventory = new TInventory();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return inventory;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a PropertyName token.");
            }

            var propertyName = reader.GetString();
            reader.Read();

            if (propertyName == nameof(Inventory<TItem>.StackSize))
            {
                var stackSize = reader.GetInt32();
                inventory.StackSize = stackSize;
            }
            else if (propertyName == "Items")
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected StartArray token for 'Items'.");
                }

                var entryTypeInfo = (JsonTypeInfo<Entry<TItem>>)options.GetTypeInfo(typeof(Entry<TItem>));
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    var entry = JsonSerializer.Deserialize(ref reader, entryTypeInfo)!;
                    inventory.Add(entry.Item, entry.Quantity);
                }
            }
        }

        throw new JsonException("Expected EndObject token.");
    }


    public override void Write(Utf8JsonWriter writer, TInventory value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(Inventory<TInventory>.StackSize));
        writer.WriteNumberValue(value.StackSize);

        writer.WritePropertyName("Items");
        writer.WriteStartArray();
        var entryTypeInfo = (JsonTypeInfo<Entry<TItem>>)options.GetTypeInfo(typeof(Entry<TItem>));
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, entryTypeInfo);
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }

}