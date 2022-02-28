namespace ToolBX.Collections.Inventory;

public static class InventoryExtensions
{
    public static IInventory<T> ToInventory<T>(this IEnumerable<T> collection, int stackSize = 99)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Inventory<T>(collection, stackSize);
    }

    public static IInventory<T> ToInventory<T>(this IEnumerable<InventoryEntry<T>> collection, int stackSize = 99)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Inventory<T>(collection, stackSize);
    }
}