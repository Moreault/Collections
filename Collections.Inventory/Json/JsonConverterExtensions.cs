using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.Inventory.Json;

public static class JsonConverterExtensions
{
    private static readonly Lazy<IReadOnlyList<JsonConverter>> All = new(() => ImmutableList.Create<JsonConverter>(
        new InventoryListJsonConverterFactory(), 
        new InventoryTableJsonConverterFactory(),
        new StockSearchResultJsonConverterFactory()));

    /// <summary>
    /// Returns a <see cref="JsonConverter"/> loaded with all <see cref="JsonSerializerOptions"/>s from ToolBX.Collections.Inventory.
    /// </summary>
    public static JsonSerializerOptions WithInventoryConverters(this JsonSerializerOptions options)
    {
        foreach (var converter in All.Value)
            options.Converters.Add(converter);
        return options;
    }

}