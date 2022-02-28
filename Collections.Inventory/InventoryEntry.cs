namespace ToolBX.Collections.Inventory;

public record InventoryEntry<T>
{
    public T Item { get; init; } = default!;
    public int Quantity { get; init; }

    public InventoryEntry()
    {

    }

    public InventoryEntry(T item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public void Deconstruct(out T item, out int quantity)
    {
        item = Item;
        quantity = Quantity;
    }

    public override string ToString() => $"{Item} x{Quantity}";
}