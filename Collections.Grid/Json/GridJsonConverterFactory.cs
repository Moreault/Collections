namespace ToolBX.Collections.Grid.Json;

public sealed class GridJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Grid<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(GridJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public sealed class GridJsonConverter<T> : JsonConverter<Grid<T>>
{
    public override Grid<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token.");
        }

        var grid = new Grid<T>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token.");
            }

            int x = 0, y = 0;
            T? value = default;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    grid[new Vector2<int>(x, y)] = value;
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "X":
                            x = reader.GetInt32();
                            break;
                        case "Y":
                            y = reader.GetInt32();
                            break;
                        case "Value":
                            value = JsonSerializer.Deserialize<T>(ref reader, options);
                            break;
                    }
                }
            }
        }

        return grid;
    }

    public override void Write(Utf8JsonWriter writer, Grid<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            writer.WriteStartObject();

            writer.WriteNumber("X", item.Index.X);
            writer.WriteNumber("Y", item.Index.Y);

            writer.WritePropertyName("Value");
            JsonSerializer.Serialize(writer, item.Value, options);

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}