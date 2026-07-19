using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.Inventory.Json;

public sealed class InventoryTableJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(InventoryTable<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(InventoryTableJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public sealed class InventoryTableJsonConverter<T> : InventoryJsonConverterBase<InventoryTable<T>, T>
{

}