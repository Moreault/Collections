using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.ObservableList.Json;

public sealed class ObservableListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ObservableList<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(ObservableListJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ObservableListJsonConverter<T> : JsonConverter<ObservableList<T>>
{
    public override ObservableList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        var typeInfo = (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[]));
        return new ObservableList<T>(JsonSerializer.Deserialize(ref reader, typeInfo)!);
    }

    public override void Write(Utf8JsonWriter writer, ObservableList<T> value, JsonSerializerOptions options)
    {
        var typeInfo = (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[]));
        JsonSerializer.Serialize(writer, value.ToArray(), typeInfo);
    }
}