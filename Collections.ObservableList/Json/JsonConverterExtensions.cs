using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.ObservableList.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ImmutableList.Create<JsonConverter>(new ObservableListJsonConverterFactory()));

    /// <summary>
    /// Returns a <see cref="JsonSerializerOptions"/> loaded with all <see cref="JsonConverter"/>s from ToolBX.Collections.ObservableList.
    /// </summary>
    public static JsonSerializerOptions WithObservableList(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }

}