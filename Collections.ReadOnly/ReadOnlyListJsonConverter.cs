using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.ReadOnly;

[Obsolete("Use ImmutableList<T> instead which doesn't require a custom JsonConverter. Will be removed in 3.0.0")]
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