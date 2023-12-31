namespace Collections.ReadOnly.Tests;

public sealed record DummyWithListInside
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ReadOnlyList<Dummy> Stuff { get; init; } = [];
}