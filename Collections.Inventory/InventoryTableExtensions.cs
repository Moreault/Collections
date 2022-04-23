namespace ToolBX.Collections.Inventory;

public static class InventoryTableExtensions
{
    public static InventoryTable<T> ToInventoryTable<T>(this IEnumerable<T> collection, int stackSize = DefaultValues.StackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new InventoryTable<T>(collection, stackSize);
    }

    public static InventoryTable<T> ToInventoryTable<T>(this IEnumerable<Entry<T>> collection, int stackSize = DefaultValues.StackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new InventoryTable<T>(collection, stackSize);
    }
}