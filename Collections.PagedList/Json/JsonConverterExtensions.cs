namespace ToolBX.Collections.PagedList.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => [new PagedListJsonConverterFactory()]);

    /// <summary>
    /// Returns a <see cref="JsonSerializerOptions"/> loaded with all <see cref="JsonConverter"/>s from ToolBX.Collections.PagedList.
    /// </summary>
    public static JsonSerializerOptions WithPagedListConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }
}
