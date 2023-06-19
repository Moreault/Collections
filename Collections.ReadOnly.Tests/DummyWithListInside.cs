using System.Text.Json.Serialization;

namespace Collections.ReadOnly.Tests;

public sealed record DummyWithListInside
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    [JsonConverter(typeof(ReadOnlyListJsonConverter<Dummy>))]
    public ReadOnlyList<Dummy> Stuff { get; init; } = ReadOnlyList<Dummy>.Empty;
}