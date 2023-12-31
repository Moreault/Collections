using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.ObservableList.Json;

public sealed class ObservableListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ObservableList<>);

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
        return reader.TokenType == JsonTokenType.Null ? null : new ObservableList<T>(JsonSerializer.Deserialize<T[]>(ref reader, options)!);
    }

    public override void Write(Utf8JsonWriter writer, ObservableList<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}