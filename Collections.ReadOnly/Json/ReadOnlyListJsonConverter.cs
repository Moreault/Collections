namespace ToolBX.Collections.ReadOnly.Json;

public sealed class ReadOnlyListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ReadOnlyList<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(ReadOnlyListJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ReadOnlyListJsonConverter<T> : JsonConverter<ReadOnlyList<T>>
{
    public override ReadOnlyList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new ReadOnlyList<T>(JsonSerializer.Deserialize<T[]>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, ReadOnlyList<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}