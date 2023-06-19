using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.ReadOnly;

//TODO .NET 8 : Might not be required anymore ()
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