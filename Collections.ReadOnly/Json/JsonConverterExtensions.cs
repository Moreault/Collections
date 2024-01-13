namespace ToolBX.Collections.ReadOnly.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ReadOnlyList.Create<JsonConverter>(new ReadOnlyListJsonConverterFactory()));

    /// <summary>
    /// Returns a <see cref="JsonSerializerOptions"/> loaded with all <see cref="JsonConverter"/>s from ToolBX.Collections.ReadOnly.
    /// </summary>
    public static JsonSerializerOptions WithReadOnlyConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }

}