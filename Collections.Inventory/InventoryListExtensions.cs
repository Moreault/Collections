namespace ToolBX.Collections.Inventory;

public static class InventoryListExtensions
{
    public static InventoryList<T> ToInventoryList<T>(this IEnumerable<T> collection, int stackSize = DefaultValues.StackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new InventoryList<T>(collection, stackSize);
    }

    public static InventoryList<T> ToInventoryList<T>(this IEnumerable<Entry<T>> collection, int stackSize = DefaultValues.StackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new InventoryList<T>(collection, stackSize);
    }
}