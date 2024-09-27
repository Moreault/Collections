namespace ToolBX.Collections.Inventory;

/// <summary>
/// A collection of stackable items with non-unique slots.
/// </summary>
public class InventoryList<T> : Inventory<T>
{
    public InventoryList()
    {

    }

    public InventoryList(int stackSize) : base(stackSize)
    {

    }

    public InventoryList(IEnumerable<T> collection, int stackSize = DefaultValues.StackSize) : base(collection, stackSize)
    {

    }

    public InventoryList(IEnumerable<Entry<T>> collection, int stackSize = DefaultValues.StackSize) : base(collection, stackSize)
    {

    }

    protected override void AddSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var indexes = Items.IndexesOf(x => Equals(x.Item, item));

        if (indexes.Any())
        {
            var remaining = quantity;
            foreach (var index in indexes)
            {
                if (remaining == 0) break;

                if (Items[index].Quantity < StackSize)
                {
                    var maximumPossibleQuantity = StackSize - Items[index].Quantity;
                    if (maximumPossibleQuantity > 0)
                    {
                        var amountToAdd = maximumPossibleQuantity >= remaining ? remaining : maximumPossibleQuantity;
                        remaining -= amountToAdd;
                        Items[index] = Items[index] with { Quantity = Items[index].Quantity + amountToAdd };
                    }
                }
            }

            if (remaining > 0)
            {
                InsertSilently(LastIndex + 1, item, remaining);
            }
        }
        else
        {
            InsertSilently(LastIndex + 1, item, quantity);
        }
    }

    protected override void RemoveSilently(T item, int quantity)
    {
        var indexes = Items.IndexesOf(x => Equals(x.Item, item)).OrderByDescending(x => x).ToList();
        if (!indexes.Any()) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, item));

        var total = Items.Where(x => Equals(x.Item, item)).Sum(x => x.Quantity);
        if (quantity > total) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item, quantity, total));

        var remaining = quantity;

        foreach (var index in indexes)
        {
            if (remaining == 0) break;
            var amountToRemove = Items[index].Quantity >= remaining ? remaining : Items[index].Quantity;
            remaining -= amountToRemove;

            Items[index] = Items[index] with { Quantity = Items[index].Quantity - amountToRemove };
        }
    }

    /// <summary>
    /// Inserts items at the beginning of collection.
    /// </summary>
    public void InsertFirst(T item, int quantity = 1) => Insert(0, item, quantity);

    public void Insert(int index, T item, int quantity = 1)
    {
        InsertSilently(index, item, quantity);

        OnCollectionChanged(new CollectionChangeEventArgs<Entry<T>>
        {
            NewValues =
            [
                new(item, quantity)
            ]
        });
    }

    private void InsertSilently(int index, T item, int quantity)
    {
        if (quantity < 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var remaining = quantity;
        var currentIndex = index;

        while (remaining > 0)
        {
            var toAdd = remaining >= StackSize ? StackSize : remaining;
            remaining -= toAdd;
            Items.Insert(currentIndex, new Entry<T>(item, toAdd));
            currentIndex++;
        }
    }

    /// <summary>
    /// Inserts items at the end of collection.
    /// </summary>
    public void InsertLast(T item, int quantity = 1) => Insert(LastIndex + 1, item, quantity);

    public IReadOnlyList<int> IndexesOf(T item) => Items.IndexesOf(x => Equals(x.Item, item));
}