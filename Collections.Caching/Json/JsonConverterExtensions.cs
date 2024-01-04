namespace ToolBX.Collections.Caching.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ImmutableList.Create<JsonConverter>(
        new CachingListJsonConverterFactory(),
        new CachingStackJsonConverterFactory(),
        new CachingDictionaryJsonConverterFactory()
        ));

    /// <summary>
    /// Returns a <see cref="JsonConverter"/> loaded with all <see cref="JsonSerializerOptions"/>s from ToolBX.Collections.Caching.
    /// </summary>
    public static JsonSerializerOptions WithCachingConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }
}