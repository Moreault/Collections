﻿namespace ToolBX.Collections.UnitTesting.Inventory;

public record DummyItem
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }

    public override string ToString() => Name;
}