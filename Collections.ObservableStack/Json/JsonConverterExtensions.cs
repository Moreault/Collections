namespace ToolBX.Collections.ObservableStack.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ImmutableList.Create<JsonConverter>(new ObservableStackJsonConverterFactory()));

    /// <summary>
    /// Returns a <see cref="JsonConverter"/> loaded with all <see cref="JsonSerializerOptions"/>s from ToolBX.Collections.ObservableStack.
    /// </summary>
    public static JsonSerializerOptions WithObservableStackConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }

}