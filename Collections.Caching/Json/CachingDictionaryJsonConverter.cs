namespace ToolBX.Collections.Caching.Json;

public sealed class CachingDictionaryJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(CachingDictionary<,>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var keyType = typeToConvert.GetGenericArguments()[0];
        var valueType = typeToConvert.GetGenericArguments()[1];
        var converterType = typeof(CachingDictionaryJsonConverter<,>).MakeGenericType(keyType, valueType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class CachingDictionaryJsonConverter<TKey, TValue> : JsonConverter<CachingDictionary<TKey, TValue>> where TKey : notnull
{
    public override CachingDictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new CachingDictionary<TKey, TValue>(JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, CachingDictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}