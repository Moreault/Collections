namespace ToolBX.Collections.Grid.Json;

public sealed class OverlapGridJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(OverlapGrid<>);

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

            var cell = JsonSerializer.Deserialize<Cell<T>>(ref reader, options);
            cells.Add(cell);
        }

        return new OverlapGrid<T>(cells);
    }

    public override void Write(Utf8JsonWriter writer, OverlapGrid<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var cell in value)
        {
            JsonSerializer.Serialize(writer, cell, options);
        }

        writer.WriteEndArray();
    }
}