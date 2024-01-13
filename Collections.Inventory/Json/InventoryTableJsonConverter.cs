using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolBX.Collections.Inventory.Json;

public sealed class InventoryTableJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(InventoryTable<>);

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