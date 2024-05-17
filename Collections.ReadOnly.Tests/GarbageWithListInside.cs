namespace Collections.ReadOnly.Tests;

public sealed record GarbageWithListInside
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ReadOnlyList<Garbage> Stuff { get; init; } = [];
}