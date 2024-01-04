namespace ToolBX.Collections.ObservableDictionary.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ImmutableList.Create<JsonConverter>(
        new ObservableDictionaryJsonConverterFactory()));

    /// <summary>
    /// Returns a <see cref="JsonConverter"/> loaded with all <see cref="JsonSerializerOptions"/>s from ToolBX.Collections.ObservableDictionary.
    /// </summary>
    public static JsonSerializerOptions WithObservableDictionaryConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }

}