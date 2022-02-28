using Collections.Common;
using System.Collections;
using ToolBX.Collections.Inventory.Resources;
using ToolBX.Collections.ObservableList;
using ToolBX.Reflection4Humans.Extensions;

namespace ToolBX.Collections.Inventory;

public interface IInventory<T> : IObservableCollection<InventoryEntry<T>>, IEnumerable<InventoryEntry<T>>, IEquatable<IInventory<T>>
{
    int LastIndex { get; }

    /// <summary>
    /// Maximum quantity allowed per item stack.
    /// </summary>
    int StackSize { get; }

    /// <summary>
    /// Sum of all item quantities in collection.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Number of unique items in stock.
    /// </summary>
    int StackCount { get; }

    InventoryEntry<T> this[int index] { get; }

    void Add(T item, int quantity = 1);

    /// <summary>
    /// Attempts to add a quantity of item without throwing if its stack if full.
    /// </summary>
    TryAddResult TryAdd(T item, int quantity = 1);

    void Add(Predicate<T> predicate, int quantity = 1);

    /// <summary>
    /// Attempts to add a quantity to all items that match the predicate without throwing if its stack if full.
    /// </summary>
    TryAddResult TryAdd(Predicate<T> predicate, int quantity = 1);

    void Remove(T item, int quantity = 1);

    /// <summary>
    /// Attempts to remove a quantity of item without throwing if item is not in stock.
    /// </summary>
    TryRemoveResult TryRemove(T item, int quantity = 1);

    void Remove(Predicate<T> predicate, int quantity = 1);

    /// <summary>
    /// Attempts to remove a quantity from all items that match the predicate without throwing if no matching item is in stock.
    /// </summary>
    TryRemoveResult TryRemove(Predicate<T> predicate, int quantity = 1);

    /// <summary>
    /// Removes an entire stack of item.
    /// </summary>
    void Clear(T item);

    /// <summary>
    /// Removes an entire stack of item.
    /// </summary>
    void Clear(Predicate<T> predicate);

    void Clear();

    int QuantityOf(T item);

    int QuantityOf(Predicate<T> predicate);

    int IndexOf(T item);

    IList<int> IndexesOf(Predicate<T> predicate);

    void Swap(int currentIndex, int destinationIndex);
}

/// <inheritdoc cref="IInventory{T}"/>
public class Inventory<T> : IInventory<T>
{
    private readonly IObservableList<InventoryEntry<T>> _items;

    public int LastIndex => _items.LastIndex;

    public int StackSize { get; }

    public int TotalCount => _items.Sum(x => x.Quantity);
    public int StackCount => _items.Count;

    public InventoryEntry<T> this[int index] => _items[index];

    public Inventory(int stackSize = 99)
    {
        if (stackSize <= 0) throw new ArgumentException(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, GetType().GetHumanReadableName(), stackSize));
        StackSize = stackSize;
        _items = new ObservableList<InventoryEntry<T>>();
    }

    public Inventory(IEnumerable<T> collection, int stackSize = 99) : this(collection.Select(x => new InventoryEntry<T>(x)), stackSize)
    {

    }

    public Inventory(IEnumerable<InventoryEntry<T>> collection, int stackSize = 99) : this(stackSize)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        var entries = collection.ToList();

        var items = new ObservableList<InventoryEntry<T>>();
        foreach (var entry in entries)
        {
            if (items.Any(x => Equals(entry.Item, x.Item))) continue;
            items.Add(new InventoryEntry<T>(entry.Item, entries.Where(x => Equals(x.Item, entry.Item)).Sum(x => x.Quantity)));
        }

        if (items.Any(x => x.Quantity > stackSize)) throw new InventoryStackFullException(stackSize);
        _items = items;
    }

    public void Add(T item, int quantity = 1)
    {
        AddSilently(item, quantity);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            NewValues = new List<InventoryEntry<T>> { new(item, quantity) }
        });
    }

    private void AddSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var index = _items.FirstIndexOf(x => Equals(x.Item, item));

        if (index < 0)
        {
            if (quantity > StackSize) throw new InventoryStackFullException(quantity, StackSize);
            _items.Add(new InventoryEntry<T>(item, quantity));
        }
        else
        {
            var newQuantity = _items[index].Quantity + quantity;
            if (newQuantity > StackSize) throw new InventoryStackFullException(quantity, StackSize);
            _items[index] = _items[index] with { Quantity = newQuantity };
        }
    }

    public TryAddResult TryAdd(T item, int quantity = 1)
    {
        var result = TryAddSilently(item, quantity);
        if (result.Added > 0)
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
            {
                NewValues = new List<InventoryEntry<T>>
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

    public void Add(Predicate<T> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseThereIsNoMatch, predicate));

        foreach (var index in indexesOf)
            AddSilently(_items[index].Item, quantity);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            NewValues = indexesOf.Select(x => new InventoryEntry<T>(_items[x].Item, quantity)).ToList()
        });
    }

    public TryAddResult TryAdd(Predicate<T> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) return new TryAddResult(0, quantity);

        var adds = indexesOf.Select(x => new { Result = TryAddSilently(_items[x].Item, quantity), _items[x].Item }).ToList();

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            NewValues = adds.Select(x => new InventoryEntry<T>(x.Item, x.Result.Added)).ToList()
        });

        return new TryAddResult(adds.Sum(x => x.Result.Added), adds.Sum(x => x.Result.NotAdded));
    }

    public void Remove(T item, int quantity = 1)
    {
        RemoveSilently(item, quantity);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            OldValues = new List<InventoryEntry<T>>
            {
                new(item, quantity)
            }
        });
    }

    private void RemoveSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

        var indexOf = _items.FirstIndexOf(x => Equals(x.Item, item));
        if (indexOf < 0) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, item));

        var oldQuantity = _items[indexOf].Quantity;
        if (oldQuantity < quantity) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item, quantity, oldQuantity));

        var newQuantity = oldQuantity - quantity;
        if (newQuantity == 0)
            _items.RemoveAt(indexOf);
        else
            _items[indexOf] = _items[indexOf] with { Quantity = newQuantity };
    }

    public TryRemoveResult TryRemove(T item, int quantity = 1)
    {
        var copy = CollectionChanged == null ? null : _items.Copy();

        var removals = TryRemoveSilently(item, quantity);

        if (copy != null && copy.Any())
        {
            var oldValues = new List<InventoryEntry<T>>();
            foreach (var thingy in copy)
            {
                var quantityOf = QuantityOf(thingy.Item);
                if (quantityOf != thingy.Quantity)
                    oldValues.Add(new InventoryEntry<T>(thingy.Item, thingy.Quantity - quantityOf));
            }
            if (oldValues.Any())
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
                {
                    OldValues = oldValues
                });
        }

        return removals;
    }

    private TryRemoveResult TryRemoveSilently(T item, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));

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

    public void Remove(Predicate<T> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseThereIsNoMatch, predicate));

        IList<InventoryEntry<T>> itemsRemoved = CollectionChanged == null ? Array.Empty<InventoryEntry<T>>() : new List<InventoryEntry<T>>();

        foreach (var index in indexesOf)
        {
            if (CollectionChanged != null)
                itemsRemoved.Add(new InventoryEntry<T>(_items[index].Item, quantity));

            RemoveSilently(_items[index].Item, quantity);
        }

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            OldValues = itemsRemoved.ToList()
        });
    }

    public TryRemoveResult TryRemove(Predicate<T> predicate, int quantity = 1)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (quantity <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));

        var indexesOf = IndexesOf(predicate);
        if (!indexesOf.Any()) return new TryRemoveResult(0, quantity);

        IList<InventoryEntry<T>> itemsRemoved = CollectionChanged == null ? Array.Empty<InventoryEntry<T>>() : new List<InventoryEntry<T>>();

        var removals = new List<TryRemoveResult>();

        foreach (var index in indexesOf)
        {
            var item = _items[index].Item;

            var removal = TryRemoveSilently(item, quantity);

            removals.Add(removal);

            if (CollectionChanged != null)
                itemsRemoved.Add(new InventoryEntry<T>(item, removal.Removed));
        }

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
        {
            OldValues = itemsRemoved.ToList()
        });

        return new TryRemoveResult(removals.Sum(x => x.Removed), removals.Sum(x => x.NotRemoved));
    }

    public void Clear(T item)
    {
        var indexOf = _items.FirstIndexOf(x => Equals(x.Item, item));
        if (indexOf >= 0)
        {
            var quantity = _items[indexOf].Quantity;
            _items.RemoveAt(indexOf);
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
            {
                OldValues = new List<InventoryEntry<T>>
                {
                    new(item, quantity)
                }
            });
        }
    }

    public void Clear(Predicate<T> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var indexesOf = IndexesOf(predicate);

        IList<InventoryEntry<T>> removals = CollectionChanged == null ? Array.Empty<InventoryEntry<T>>() : new List<InventoryEntry<T>>();

        while (indexesOf.Any())
        {
            if (CollectionChanged != null)
                removals.Add(_items[indexesOf.First()]);
            _items.RemoveAt(indexesOf.First());
            indexesOf = IndexesOf(predicate);
        }

        if (CollectionChanged != null && removals.Any())
        {
            CollectionChanged.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
            {
                OldValues = (IReadOnlyList<InventoryEntry<T>>)removals
            });
        }
    }

    public void Clear()
    {
        if (_items.Any())
        {
            var items = CollectionChanged == null ? null : _items.ToList();

            _items.Clear();
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<InventoryEntry<T>>
            {
                OldValues = items!
            });
        }
    }

    public int QuantityOf(T item)
    {
        var index = _items.FirstIndexOf(x => Equals(x.Item, item));
        return index < 0 ? 0 : _items[index].Quantity;
    }

    public int QuantityOf(Predicate<T> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var indexes = IndexesOf(predicate);
        return indexes.Sum(x => _items[x].Quantity);
    }

    public int IndexOf(T item) => _items.FirstIndexOf(x => Equals(x.Item, item));

    public IList<int> IndexesOf(Predicate<T> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var items = _items.Select(x => x.Item).ToObservableList();
        return items.IndexesOf(predicate);
    }

    public IEnumerator<InventoryEntry<T>> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Swap(int currentIndex, int destinationIndex) => _items.Swap(currentIndex, destinationIndex);

    public event CollectionChangeEventHandler<InventoryEntry<T>>? CollectionChanged;

    public override string ToString() => _items.Any() ? $"{GetType().GetHumanReadableName()} with {StackCount} unique items" : $"Empty {GetType().GetHumanReadableName()}";

    public bool Equals(Inventory<T>? other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(other, this)) return true;
        return StackSize == other.StackSize && _items.SequenceEqual(other._items);
    }

    public bool Equals(IInventory<T>? other) => Equals(other as Inventory<T>);

    public override bool Equals(object? obj) => Equals(obj as Inventory<T>);

    public override int GetHashCode()
    {
        return HashCode.Combine(_items, StackSize);
    }

    public static bool operator ==(Inventory<T>? a, Inventory<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Inventory<T>? a, Inventory<T>? b) => !(a == b);
}