namespace ToolBX.Collections.Inventory;

public abstract record EntryBase<T>
{
    public T? Item { get; init; }

    public int Quantity
    {
        get => _quantity;
        init => _quantity = value < 0 ? throw new ArgumentOutOfRangeException(nameof(value), value, Exceptions.QuantityMustBePositive) : value;
    }
    private readonly int _quantity = 1;

    protected EntryBase()
    {

    }

    protected EntryBase(T item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public void Deconstruct(out T? item, out int quantity)
    {
        item = Item;
        quantity = Quantity;
    }

    public override string ToString() => $"{(Item is null ? "NULL" : Item.ToString())} x{Quantity}";
}