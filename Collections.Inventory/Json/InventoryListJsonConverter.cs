using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.Inventory.Json;

public sealed class InventoryListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(InventoryList<>);

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

                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    var entry = JsonSerializer.Deserialize<Entry<TItem>>(ref reader, options)!;
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
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }

}