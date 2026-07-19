using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace ToolBX.Collections.PagedList.Json;

public sealed class PagedListJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(PagedList<>);

    [UnconditionalSuppressMessage("AOT", "IL3050",
        Justification = "Open generic converters must build the closed converter type via MakeGenericType, which is inherently dynamic and cannot be annotated on this base override (IL3051). AOT consumers must register a source-generated converter for the concrete type instead.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(PagedListJsonConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public sealed class PagedListJsonConverter<T> : JsonConverter<PagedList<T>>
{
    private const string ItemsPropertyName = nameof(PagedList<T>.Items);
    private const string PageNumberPropertyName = nameof(PagedList<T>.PageNumber);
    private const string PageSizePropertyName = nameof(PagedList<T>.PageSize);
    private const string TotalCountPropertyName = nameof(PagedList<T>.TotalCount);
    private const string PageCountPropertyName = nameof(PagedList<T>.PageCount);

    public override PagedList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException($"Expected {nameof(JsonTokenType.StartObject)} token.");

        var itemsTypeInfo = (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[]));

        var items = Array.Empty<T>();
        var pageNumber = 1;
        var pageSize = 0;
        var totalCount = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new PagedList<T>(items, pageNumber, pageSize, totalCount);

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Expected {nameof(JsonTokenType.PropertyName)} token.");

            var propertyName = reader.GetString();
            reader.Read();

            if (NameMatches(propertyName, ItemsPropertyName, options))
                items = JsonSerializer.Deserialize(ref reader, itemsTypeInfo) ?? Array.Empty<T>();
            else if (NameMatches(propertyName, PageNumberPropertyName, options))
                pageNumber = reader.GetInt32();
            else if (NameMatches(propertyName, PageSizePropertyName, options))
                pageSize = reader.GetInt32();
            else if (NameMatches(propertyName, TotalCountPropertyName, options))
                totalCount = reader.GetInt32();
            else
                reader.Skip();
        }

        throw new JsonException($"Expected {nameof(JsonTokenType.EndObject)} token.");
    }

    public override void Write(Utf8JsonWriter writer, PagedList<T> value, JsonSerializerOptions options)
    {
        var itemsTypeInfo = (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[]));

        writer.WriteStartObject();

        writer.WritePropertyName(ConvertName(ItemsPropertyName, options));
        JsonSerializer.Serialize(writer, value.Items.ToArray(), itemsTypeInfo);

        writer.WriteNumber(ConvertName(PageNumberPropertyName, options), value.PageNumber);
        writer.WriteNumber(ConvertName(PageSizePropertyName, options), value.PageSize);
        writer.WriteNumber(ConvertName(TotalCountPropertyName, options), value.TotalCount);
        // PageCount is derived; emitted as a convenience for clients but ignored when reading.
        writer.WriteNumber(ConvertName(PageCountPropertyName, options), value.PageCount);

        writer.WriteEndObject();
    }

    private static string ConvertName(string name, JsonSerializerOptions options)
        => options.PropertyNamingPolicy?.ConvertName(name) ?? name;

    private static bool NameMatches(string? jsonName, string clrName, JsonSerializerOptions options)
    {
        if (jsonName is null) return false;
        var comparison = options.PropertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return string.Equals(jsonName, ConvertName(clrName, options), comparison);
    }
}
