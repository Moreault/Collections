namespace ToolBX.Collections.ObservableStack.Json;

public sealed class ObservableStackJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ObservableStack<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(ObservableStackJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ObservableStackJsonConverter<T> : JsonConverter<ObservableStack<T>>
{
    public override ObservableStack<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Null ? null : new ObservableStack<T>(JsonSerializer.Deserialize<T[]>(ref reader, options)!.Reverse());
    }

    public override void Write(Utf8JsonWriter writer, ObservableStack<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
    }
}