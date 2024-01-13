namespace ToolBX.Collections.Caching.Json;

public sealed class CachingStackJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(CachingStack<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(CachingStackJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class CachingStackJsonConverter<T> : JsonConverter<CachingStack<T>>
{
    public override CachingStack<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new CachingStack<T>(JsonSerializer.Deserialize<T[]>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, CachingStack<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}