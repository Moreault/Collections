namespace ToolBX.Collections.Inventory;

/// <summary>
/// A collection of stackable items with unique slots.
/// </summary>
public class InventoryTable<T> : Inventory<T>
{
    public InventoryTable()
    {

    }

    public InventoryTable(int stackSize) : base(stackSize)
    {

    }

    public InventoryTable(IEnumerable<T> collection, int stackSize = DefaultValues.StackSize) : base(collection, stackSize)
    {

    }

    public InventoryTable(IEnumerable<Entry<T>> collection, int stackSize = DefaultValues.StackSize) : base(collection, stackSize)
    {

    }

    protected override void AddSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var index = Items.FirstIndexOf(x => Equals(x.Item, item));

        if (index < 0)
        {
            if (quantity > StackSize) throw new InventoryStackFullException(quantity, StackSize);
            Items.Add(new Entry<T>(item, quantity));
        }
        else
        {
            var newQuantity = Items[index].Quantity + quantity;
            if (newQuantity > StackSize) throw new InventoryStackFullException(quantity, StackSize);
            Items[index] = Items[index] with { Quantity = newQuantity };
        }
    }

    /// <summary>
    /// Attempts to add a quantity of item without throwing if its stack if full.
    /// </summary>
    public TryAddResult TryAdd(T item, int quantity = 1)
    {
        var result = TryAddSilently(item, quantity);
        if (result.Added > 0)
            OnCollectionChanged(new CollectionChangeEventArgs<Entry<T>>
            {
                NewValues = new List<Entry<T>>
            {
                new(item, result.Added)
            }
            });
        return result;
    }

    private TryAddResult TryAddSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var currentQuantity = QuantityOf(item);
        var newQuantity = quantity + currentQuantity;

        var addedQuantity = Math.Clamp(newQuantity > StackSize ? StackSize - QuantityOf(item) : quantity, 0, int.MaxValue);

        if (addedQuantity > 0)
            AddSilently(item, addedQuantity);

        return new TryAddResult(addedQuantity, quantity - addedQuantity);
    }

    /// <summary>
    /// Attempts to add a quantity to all items that match the predicate without throwing if its stack if full.
    /// </summary>
    public TryAddResult TryAdd(Predicate<T> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) return new TryAddResult(0, quantity);

        var adds = indexesOf.Select(x => new { Result = TryAddSilently(Items[x].Item, quantity), Items[x].Item }).ToList();

        OnCollectionChanged(new CollectionChangeEventArgs<Entry<T>>
        {
            NewValues = adds.Select(x => new Entry<T>(x.Item, x.Result.Added)).ToList()
        });

        return new TryAddResult(adds.Sum(x => x.Result.Added), adds.Sum(x => x.Result.NotAdded));
    }

    protected override void RemoveSilently(T item, int quantity)
    {
        var indexOf = Items.FirstIndexOf(x => Equals(x.Item, item));
        if (indexOf < 0) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, item));

        var oldQuantity = Items[indexOf].Quantity;
        if (oldQuantity < quantity) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item, quantity, oldQuantity));

        var newQuantity = oldQuantity - quantity;
        if (newQuantity == 0)
            Items.RemoveAt(indexOf);
        else
            Items[indexOf] = Items[indexOf] with { Quantity = newQuantity };
    }

    public int IndexOf(T item) => Items.FirstIndexOf(x => Equals(x.Item, item));
}