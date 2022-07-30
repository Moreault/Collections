namespace Collections.ReadOnly.Tests;

public record Dummy
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
}