using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.Inventory.Json;

public sealed class StockSearchResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(StockSearchResult<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
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
        if (reader.TokenType == JsonTokenType.Null) return null;
        var typeInfo = (JsonTypeInfo<IndexedEntry<T>[]>)options.GetTypeInfo(typeof(IndexedEntry<T>[]));
        return new StockSearchResult<T>(JsonSerializer.Deserialize(ref reader, typeInfo)!);
    }

    public override void Write(Utf8JsonWriter writer, StockSearchResult<T> value, JsonSerializerOptions options)
    {
        var typeInfo = (JsonTypeInfo<IndexedEntry<T>[]>)options.GetTypeInfo(typeof(IndexedEntry<T>[]));
        JsonSerializer.Serialize(writer, value.ToArray(), typeInfo);
    }
}
