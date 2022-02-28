namespace ToolBX.Collections.Inventory;

public readonly record struct TryAddResult(int Added, int NotAdded)
{
    public int Total => Added + NotAdded;

    public override string ToString()
    {
        if (Added == 0) return $"None of the {Total} items could be added";
        if (NotAdded == 0) return $"All of the {Total} items could be added";
        return $"{Added} items out of {Total} were added";
    }
}