namespace ToolBX.Collections.Inventory;

public sealed record Entry<T> : EntryBase<T>
{
    public Entry()
    {

    }

    public Entry(T item, int quantity = 1) : base(item, quantity)
    {

    }

    public override string ToString() => base.ToString();
}