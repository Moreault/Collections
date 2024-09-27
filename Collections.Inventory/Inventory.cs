namespace ToolBX.Collections.Inventory;

public interface IInventory<T> : IObservableCollection<Entry<T>>, ICollection<Entry<T>>, IEquatable<IInventory<T>>, IReadOnlyInventory<T>
{
    Entry<T> this[int index] { get; }
    int LastIndex { get; }

    /// <summary>
    /// Sum of all item quantities in collection.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Number of unique items in stock.
    /// </summary>
    int StackCount { get; }

    /// <summary>
    /// Maximum quantity allowed per item stack.
    /// </summary>
    int StackSize { get; set; }

    void Add(T item, int quantity = 1);
    void Add(Func<T, bool> predicate, int quantity = 1);
    int QuantityOf(T item);
    int QuantityOf(Func<T, bool> predicate);
    IReadOnlyList<int> IndexesOf(Func<T, bool> predicate);
    StockSearchResult<T> Search(T item);
    StockSearchResult<T> Search(Func<T, bool> predicate);
    void Remove(T item, int quantity = 1);

    /// <summary>
    /// Attempts to remove a quantity from all items that match the predicate without throwing if no matching item is in stock.
    /// </summary>
    void Remove(Func<T, bool> predicate, int quantity = 1);

    /// <summary>
    /// Attempts to remove a quantity of item without throwing if item is not in stock.
    /// </summary>
    TryRemoveResult TryRemove(T item, int quantity = 1);

    TryRemoveResult TryRemove(Func<T, bool> predicate, int quantity = 1);

    /// <summary>
    /// Removes an entire stack of item.
    /// </summary>
    void Clear(T item);

    /// <summary>
    /// Removes an entire stack of item.
    /// </summary>
    void Clear(Func<T, bool> predicate);

    void Swap(int currentIndex, int destinationIndex);
    void RemoveAt(int index, int count = 1);
}

/// <inheritdoc cref="IObservableList{T}"/>
public abstract class Inventory<T> : IInventory<T>
{
    protected readonly IObservableList<Entry<T>> Items;

    public Entry<T> this[int index] => Items[index];

    public int LastIndex => Items.LastIndex;

    public int TotalCount => Items.Sum(x => x.Quantity);

    public int StackCount => Items.Count;

    int ICollection<Entry<T>>.Count => StackCount;
    bool ICollection<Entry<T>>.IsReadOnly => false;

    public int StackSize
    {
        get => _stackSize;
        set
        {
            if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, GetType().GetHumanReadableName(), value));

            if (_stackSize > value)
            {
                var oldItems = Items.ToList();
                var changedValues = new List<Entry<T>>();

                foreach (var (item, quantity) in oldItems.Where(x => x.Quantity > value))
                {
                    var toRemove = quantity - value;
                    RemoveSilently(item, toRemove);
                    if (CollectionChanged != null)
                        changedValues.Add(new Entry<T>(item, toRemove));
                }

                if (CollectionChanged != null && changedValues.Any())
                    CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
                    {
                        OldValues = changedValues
                    });

            }
            _stackSize = value;
        }
    }
    private int _stackSize = DefaultValues.StackSize;

    protected void OnCollectionChanged(CollectionChangeEventArgs<Entry<T>> args) => CollectionChanged?.Invoke(this, args);

    public event CollectionChangeEventHandler<Entry<T>>? CollectionChanged;

    protected Inventory()
    {
        Items = new ObservableList<Entry<T>>();
    }

    protected Inventory(int stackSize)
    {
        Items = new ObservableList<Entry<T>>();
        StackSize = stackSize;
    }

    protected Inventory(IEnumerable<T> collection, int stackSize = DefaultValues.StackSize) : this(collection.Select(x => new Entry<T>(x)), stackSize)
    {

    }

    protected Inventory(IEnumerable<Entry<T>> collection, int stackSize = DefaultValues.StackSize) : this(stackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        var entries = collection.ToList();

        var items = new ObservableList<Entry<T>>();
        foreach (var entry in entries)
        {
            if (items.Any(x => Equals(entry.Item, x.Item))) continue;
            items.Add(new Entry<T>(entry.Item, entries.Where(x => Equals(x.Item, entry.Item)).Sum(x => x.Quantity)));
        }

        if (items.Any(x => x.Quantity > stackSize)) throw new InventoryStackFullException(stackSize);
        Items = items;
    }

    void ICollection<Entry<T>>.Add(Entry<T> item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        Add(item.Item, item.Quantity);
    }

    public void Add(T item, int quantity = 1)
    {
        AddSilently(item, quantity);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            NewValues = [new(item, quantity)]
        });
    }

    public void Add(Func<T, bool> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseThereIsNoMatch, predicate));

        foreach (var index in indexesOf)
            AddSilently(Items[index].Item, quantity);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            NewValues = indexesOf.Select(x => new Entry<T>(Items[x].Item, quantity)).ToList()
        });
    }

    protected abstract void AddSilently(T item, int quantity);
    protected abstract void RemoveSilently(T item, int quantity);

    public int QuantityOf(T item) => Items.Where(x => Equals(x.Item, item)).Sum(x => x.Quantity);

    public int QuantityOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var indexes = IndexesOf(predicate);
        return indexes.Sum(x => Items[x].Quantity);
    }

    public IReadOnlyList<int> IndexesOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var items = Items.Select(x => x.Item).ToObservableList();
        return items.IndexesOf(predicate);
    }

    public StockSearchResult<T> Search(T item) => Search(x => Equals(x, item));

    public StockSearchResult<T> Search(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var results = new List<IndexedEntry<T>>();
        for (var i = 0; i < StackCount; i++)
        {
            if (predicate(Items[i].Item))
                results.Add(new IndexedEntry<T>(Items[i].Item, Items[i].Quantity, i));
        }
        return new StockSearchResult<T>(results);
    }

    bool ICollection<Entry<T>>.Contains(Entry<T> item) => Items.Contains(item);

    void ICollection<Entry<T>>.CopyTo(Entry<T>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

    bool ICollection<Entry<T>>.Remove(Entry<T> item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        var result = TryRemove(item.Item, item.Quantity);
        return result.Removed > 0;
    }

    public void Remove(T item, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        RemoveSilently(item, quantity);
        Items.TryRemoveAll(x => x.Quantity == 0);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            OldValues =
            [
                new(item, quantity)
            ]
        });
    }

    public void Remove(Func<T, bool> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var items = Search((Func<T, bool>)predicate).DistinctBy(x => x.Item).Select(x => x.Item).ToList();
        if (!items.Any()) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseThereIsNoMatch, predicate));

        foreach (var item in items)
            RemoveSilently(item, quantity);

        Items.TryRemoveAll(x => x.Quantity == 0);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            OldValues = items.Select(x => new Entry<T>(x, quantity)).ToList()
        });
    }

    public TryRemoveResult TryRemove(T item, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var removals = TryRemoveSilently(item, quantity);
        Items.TryRemoveAll(x => x.Quantity == 0);

        if (CollectionChanged != null && removals.Removed > 0)
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
            {
                OldValues = [new(item, removals.Removed)]
            });

        return removals;
    }

    public TryRemoveResult TryRemove(Func<T, bool> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var items = Search((Func<T, bool>)predicate).DistinctBy(x => x.Item).Select(x => x.Item).ToList();
        if (!items.Any()) return new TryRemoveResult(0, quantity);

        IList<Entry<T>> itemsRemoved = CollectionChanged == null ? Array.Empty<Entry<T>>() : new List<Entry<T>>();
        var removals = new List<TryRemoveResult>();

        foreach (var item in items)
        {
            var removal = TryRemoveSilently(item, quantity);

            removals.Add(removal);
            if (CollectionChanged != null)
                itemsRemoved.Add(new Entry<T>(item, removal.Removed));
        }

        Items.TryRemoveAll(x => x.Quantity == 0);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            OldValues = itemsRemoved.ToList()
        });

        return new TryRemoveResult(removals.Sum(x => x.Removed), removals.Sum(x => x.NotRemoved));
    }

    private TryRemoveResult TryRemoveSilently(T item, int quantity)
    {
        var currentQuantity = QuantityOf(item);
        if (currentQuantity > 0)
        {
            var newQuantity = currentQuantity - quantity;
            var toBeRemoved = Math.Clamp(newQuantity < 0 ? currentQuantity : quantity, 0, int.MaxValue);
            RemoveSilently(item, toBeRemoved);

            return new TryRemoveResult(toBeRemoved, quantity - toBeRemoved);
        }

        return new TryRemoveResult(0, quantity);
    }

    public void Clear()
    {
        if (Items.Any())
        {
            var items = CollectionChanged == null ? null : Items.ToList();

            Items.Clear();
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
            {
                OldValues = items!
            });
        }
    }

    public void Clear(T item)
    {
        var indexOf = Items.FirstIndexOf(x => Equals(x.Item, item));
        if (indexOf >= 0)
        {
            var quantity = Items[indexOf].Quantity;
            Items.RemoveAt(indexOf);
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
            {
                OldValues =
                [
                    new(item, quantity)
                ]
            });
        }
    }

    public void Clear(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var indexesOf = IndexesOf(predicate);

        IList<Entry<T>> removals = CollectionChanged == null ? Array.Empty<Entry<T>>() : new List<Entry<T>>();

        while (indexesOf.Any())
        {
            if (CollectionChanged != null)
                removals.Add(Items[indexesOf.First()]);
            Items.RemoveAt(indexesOf.First());
            indexesOf = IndexesOf(predicate);
        }

        if (CollectionChanged != null && removals.Any())
        {
            CollectionChanged.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
            {
                OldValues = (IReadOnlyList<Entry<T>>)removals
            });
        }
    }

    public void Swap(int currentIndex, int destinationIndex) => Items.Swap(currentIndex, destinationIndex);

    public void RemoveAt(int index, int count = 1)
    {
        var itemsRemoved = CollectionChanged == null ? null : Items.Copy(index, count);

        Items.RemoveAt(index, count);
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<Entry<T>>
        {
            OldValues = itemsRemoved!
        });
    }

    public IEnumerator<Entry<T>> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(IInventory<T>? other) => Equals(other as Inventory<T>);

    public bool Equals(Inventory<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(other, this)) return true;
        return StackSize == other.StackSize && Items.SequenceEqual(other.Items);
    }

    public override bool Equals(object? obj) => Equals(obj as Inventory<T>);

    public override int GetHashCode() => Items.GetHashCode();

    public static bool operator ==(Inventory<T>? a, Inventory<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Inventory<T>? a, Inventory<T>? b) => !(a == b);

    public override string ToString() => Items.Any() ? $"{GetType().GetHumanReadableName()} with {StackCount} stacks of items" : $"Empty {GetType().GetHumanReadableName()}";
}