namespace Collections.ReadOnly.Tests;

public record Garbage
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
}