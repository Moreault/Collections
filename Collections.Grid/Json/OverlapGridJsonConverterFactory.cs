using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.Grid.Json;

public sealed class OverlapGridJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(OverlapGrid<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(OverlapGridJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public sealed class OverlapGridJsonConverter<T> : JsonConverter<OverlapGrid<T>>
{
    public override OverlapGrid<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token");
        }

        var cells = new List<Cell<T>>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token");
            }

            var cell = JsonSerializer.Deserialize(ref reader, (JsonTypeInfo<Cell<T>>)options.GetTypeInfo(typeof(Cell<T>)));
            cells.Add(cell);
        }

        return new OverlapGrid<T>(cells);
    }

    public override void Write(Utf8JsonWriter writer, OverlapGrid<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var cell in value)
        {
            JsonSerializer.Serialize(writer, cell, (JsonTypeInfo<Cell<T>>)options.GetTypeInfo(typeof(Cell<T>)));
        }

        writer.WriteEndArray();
    }
}