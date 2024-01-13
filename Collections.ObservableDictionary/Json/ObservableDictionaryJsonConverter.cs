namespace ToolBX.Collections.ObservableDictionary.Json;

public sealed class ObservableDictionaryJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ObservableDictionary<,>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var keyType = typeToConvert.GetGenericArguments()[0];
        var valueType = typeToConvert.GetGenericArguments()[1];
        var converterType = typeof(ObservableDictionaryJsonConverter<,>).MakeGenericType(keyType, valueType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ObservableDictionaryJsonConverter<TKey, TValue> : JsonConverter<ObservableDictionary<TKey, TValue>> where TKey : notnull
{
    public override ObservableDictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new ObservableDictionary<TKey, TValue>(JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, ObservableDictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}