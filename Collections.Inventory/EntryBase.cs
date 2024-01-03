namespace ToolBX.Collections.Inventory;

public abstract record EntryBase<T>
{
    public T Item { get; init; } = default!;
    public int Quantity { get; init; }

    protected EntryBase()
    {

    }

    protected EntryBase(T item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public void Deconstruct(out T item, out int quantity)
    {
        item = Item;
        quantity = Quantity;
    }

    public override string ToString() => $"{(Item is null ? "NULL" : Item.ToString())} x{Quantity}";
}