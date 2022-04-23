namespace ToolBX.Collections.Inventory;

public record IndexedEntry<T> : Entry<T>
{
    public int Index { get; init; }

    public IndexedEntry()
    {

    }

    public IndexedEntry(T item, int quantity, int index) : base(item, quantity)
    {
        Index = index;
    }

    public void Deconstruct(out T item, out int quantity, out int index)
    {
        item = Item;
        quantity = Quantity;
        index = Index;
    }

    public override string ToString() => $"{Index}. {base.ToString()}";
}