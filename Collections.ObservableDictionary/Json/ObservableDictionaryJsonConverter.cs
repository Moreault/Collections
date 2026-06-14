using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.ObservableDictionary.Json;

public sealed class ObservableDictionaryJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ObservableDictionary<,>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
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
        if (reader.TokenType == JsonTokenType.Null) return null;
        var typeInfo = (JsonTypeInfo<Dictionary<TKey, TValue>>)options.GetTypeInfo(typeof(Dictionary<TKey, TValue>));
        return new ObservableDictionary<TKey, TValue>(JsonSerializer.Deserialize(ref reader, typeInfo)!);
    }

    public override void Write(Utf8JsonWriter writer, ObservableDictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        var typeInfo = (JsonTypeInfo<Dictionary<TKey, TValue>>)options.GetTypeInfo(typeof(Dictionary<TKey, TValue>));
        JsonSerializer.Serialize(writer, new Dictionary<TKey, TValue>(value), typeInfo);
    }
}
