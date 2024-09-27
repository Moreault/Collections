using System.Text.Json.Serialization;
using System.Text.Json;

namespace ToolBX.Collections.Inventory.Json;

public sealed class StockSearchResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(StockSearchResult<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(StockSearchResultJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class StockSearchResultJsonConverter<T> : JsonConverter<StockSearchResult<T>>
{
    public override StockSearchResult<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new StockSearchResult<T>(JsonSerializer.Deserialize<IndexedEntry<T>[]>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, StockSearchResult<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}