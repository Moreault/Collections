namespace ToolBX.Collections.Inventory;

public readonly record struct TryRemoveResult(int Removed, int NotRemoved)
{
    public int Total => Removed + NotRemoved;

    public override string ToString()
    {
        return Removed == 0 ? $"All {NotRemoved} items could not be removed" : $"{Removed} items out of {Total} were removed";
    }
}