namespace ToolBX.Collections.ObservableList;

/// <summary>
/// An observable, dynamic one-dimensional array.
/// </summary>
public interface IObservableList<T> : IList<T>, IReadOnlyList<T>, IObservableCollection<T>
{
    /// <summary>
    /// Number of elements contained in the collection.
    /// </summary>
    new int Count { get; }
    new T this[int index] { get; set; }

    /// <summary>
    /// Index of the last element in the collection.
    /// </summary>
    int LastIndex { get; }

    void TryRemoveFirst(T item);
    void RemoveFirst(T item);
    void TryRemoveFirst(Func<T, bool> predicate);
    void RemoveFirst(Func<T, bool> predicate);

    void TryRemoveLast(T item);
    void RemoveLast(T item);
    void TryRemoveLast(Func<T, bool> predicate);
    void RemoveLast(Func<T, bool> predicate);

    void TryRemoveAll(T item);
    void RemoveAll(T item);
    void RemoveAll(params T[] items);
    void RemoveAll(IEnumerable<T> items);
    void TryRemoveAll(params T[] items);
    void TryRemoveAll(IEnumerable<T> items);
    void TryRemoveAll(Func<T, bool> predicate);
    void RemoveAll(Func<T, bool> predicate);
    void RemoveAt(int index, int count);

    void Add(params T[]? items);
    void Add(IEnumerable<T> items);

    /// <summary>
    /// Inserts items at the beginning of ObservableList.
    /// </summary>
    void Insert(params T[] items);

    /// <summary>
    /// Inserts items at the beginning of ObservableList.
    /// </summary>
    void Insert(IEnumerable<T> items);

    void Insert(int index, params T[] items);
    void Insert(int index, IEnumerable<T> items);

    ObservableList<T> Copy(int startingIndex = 0);
    ObservableList<T> Copy(int startingIndex, int count);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int FirstIndexOf(T item);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int FirstIndexOf(Func<T, bool> predicate);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int LastIndexOf(T item);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int LastIndexOf(Func<T, bool> predicate);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    IReadOnlyList<int> IndexesOf(T item);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    IReadOnlyList<int> IndexesOf(Func<T, bool> predicate);
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    void Swap(int currentIndex, int destinationIndex);

    /// <summary>
    /// Resizes the collection down to <see cref="maxSize"/> by prioritizing keeping the last elements.
    /// </summary>
    void TrimStartDownTo(int maxSize);

    /// <summary>
    /// Resizes the collection down to <see cref="maxSize"/> by prioritizing keeping the first elements.
    /// </summary>
    void TrimEndDownTo(int maxSize);

    /// <summary>
    /// Returns a random element from the collection.
    /// </summary>
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    T? GetRandom();

    /// <summary>
    /// Returns a random index from the collection.
    /// </summary>
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int GetRandomIndex();

    /// <summary>
    /// Randomly rearranges the collection's content order.
    /// </summary>
    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    IObservableList<T> Shuffle();
}

public class ObservableList<T> : IObservableList<T>, IEquatable<IEnumerable<T>>
{
    private readonly List<T> _items;

    public int Count => _items.Count;

    bool ICollection<T>.IsReadOnly => ((ICollection<T>)_items).IsReadOnly;

    public int LastIndex => _items.LastIndex();

    public T this[int index]
    {
        get => _items[index];
        set
        {
            var oldValue = _items[index];
            _items[index] = value;
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
            {
                NewValues = new[] { value },
                OldValues = new[] { oldValue }
            });
        }
    }

    public event CollectionChangeEventHandler<T>? CollectionChanged;

    public ObservableList()
    {
        _items = new List<T>();
    }

    public ObservableList(params T[] items) : this(items as IEnumerable<T>)
    {

    }

    public ObservableList(IEnumerable<T> collection)
    {
        _items = collection?.ToList() ?? throw new ArgumentNullException(nameof(collection));
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ICollection<T>.Add(T item) => Add(item);

    //Required for XML Serialization
    public void Add(object item) => Add((T)item);

    public void Add(params T[]? items)
    {
        if (items == null) Add(new List<T> { default! });
        else Add(items as IEnumerable<T>);
    }

    public void Add(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        var added = items.ToArray();

        foreach (var item in added)
            _items.Add(item);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            NewValues = added
        });
    }

    public void Clear()
    {
        if (Count == 0) return;

        var oldValues = CollectionChanged == null ? Array.Empty<T>() : _items.ToArray();
        _items.Clear();
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = oldValues
        });
    }

    bool ICollection<T>.Contains(T item) => _items.Contains(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public ObservableList<T> Copy(int startingIndex = 0) => Copy(startingIndex, Count - startingIndex);

    public ObservableList<T> Copy(int startingIndex, int count)
    {
        if (startingIndex < 0 || startingIndex > LastIndex) throw new ArgumentOutOfRangeException(nameof(startingIndex), string.Format(Exceptions.CannotCopyBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, startingIndex));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), string.Format(Exceptions.CannotCopyBecauseCountIsNegative, GetType().GetHumanReadableName(), count));
        if (Count - startingIndex < count) throw new ArgumentException(string.Format(Exceptions.CannotCopyBecauseRangeFallsOutsideBoundaries, GetType().GetHumanReadableName(), 0, LastIndex, startingIndex, count));

        if (startingIndex == 0 && count == Count) return new ObservableList<T>(this);

        var copy = new ObservableList<T>();
        for (var i = startingIndex; i < startingIndex + count; i++)
            copy.Add(this[i]);
        return copy;
    }

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    int IList<T>.IndexOf(T item) => FirstIndexOf(item);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public int FirstIndexOf(T item) => _items.FirstIndexOf(item);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public int FirstIndexOf(Func<T, bool> predicate) => _items.FirstIndexOf(predicate);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public int LastIndexOf(T item) => _items.LastIndexOf(item);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public int LastIndexOf(Func<T, bool> predicate) => _items.LastIndexOf(predicate);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public IReadOnlyList<int> IndexesOf(T item) => _items.IndexesOf(item);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public IReadOnlyList<int> IndexesOf(Func<T, bool> predicate) => _items.IndexesOf(predicate);

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    //TODO Maybe keep Swap to ensure that CollectionChanged only gets called once instead of twice? (also, add an event trigger here if I do this!)
    public void Swap(int currentIndex, int destinationIndex) => _items.Swap(currentIndex, destinationIndex);

    public void TrimStartDownTo(int maxSize)
    {
        if (maxSize < 0) throw new ArgumentException(string.Format(Exceptions.TrimStartRequiresPositiveMaxSize, maxSize));
        if (Count == 0) return;


        if (maxSize == 0)
        {
            Clear();
        }
        else
        {
            IList<T> removed = CollectionChanged == null ? Array.Empty<T>() : new List<T>();

            while (Count > maxSize)
            {
                if (CollectionChanged != null)
                    removed.Add(this[0]);
                _items.RemoveAt(0);
            }

            if (removed.Any())
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
                {
                    OldValues = (IReadOnlyList<T>)removed
                });
        }
    }

    public void TrimEndDownTo(int maxSize)
    {
        if (maxSize < 0) throw new ArgumentException(string.Format(Exceptions.TrimEndRequiresPositiveMaxSize, maxSize));
        if (Count == 0) return;

        if (maxSize == 0)
        {
            Clear();
        }
        else
        {
            IList<T> removed = CollectionChanged == null ? Array.Empty<T>() : new List<T>();

            while (Count > maxSize)
            {
                if (CollectionChanged != null)
                    removed.Add(this[LastIndex]);
                _items.RemoveAt(LastIndex);
            }

            if (removed.Any())
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
                {
                    OldValues = (IReadOnlyList<T>)removed
                });
        }
    }

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public T? GetRandom() => _items.GetRandom();

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public int GetRandomIndex() => _items.GetRandomIndex();

    [Obsolete("Use the OPEX extension method instead. Will be removed in 3.0.0")]
    public IObservableList<T> Shuffle()
    {
        _items.Shuffle();
        return this;
    }

    public void TryRemoveAll(T item)
    {
        var originalCount = Count;
        foreach (var index in IndexesOf(item).OrderByDescending(x => x))
            _items.RemoveAt(index);

        if (CollectionChanged != null && originalCount > Count)
        {
            var items = new List<T>();
            for (var i = 0; i < originalCount - Count; i++)
                items.Add(item);

            CollectionChanged.Invoke(this, new CollectionChangeEventArgs<T>
            {
                OldValues = items
            });
        }
    }

    public void RemoveAll(T item)
    {
        var firstIndex = FirstIndexOf(item);
        if (firstIndex == -1) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, GetType().GetHumanReadableName(), item));
        TryRemoveAll(item);
    }

    public void RemoveAll(params T[] items) => RemoveAll(items as IEnumerable<T>);

    public void RemoveAll(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        var list = items as IReadOnlyList<T> ?? items.ToList();

        var indexes = list.Select(item => IndexesOf(item)).SelectMany(x => x).ToList();
        if (indexes.Count != list.Count) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, GetType().GetHumanReadableName()));

        foreach (var index in indexes.OrderByDescending(x => x))
            _items.RemoveAt(index);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = list
        });
    }

    public void TryRemoveAll(params T[] items) => TryRemoveAll(items as IEnumerable<T>);

    public void TryRemoveAll(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        RemoveAll(_items.Join(items, x => x, y => y, (x, y) => x));
    }

    public void TryRemoveAll(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        IList<T> removedItems = CollectionChanged == null ? Array.Empty<T>() : new List<T>();

        var indexes = IndexesOf(predicate);
        foreach (var index in indexes.OrderByDescending(x => x))
        {
            if (CollectionChanged != null)
                removedItems.Add(_items[index]);
            _items.RemoveAt(index);
        }

        if (CollectionChanged != null && removedItems.Any())
            CollectionChanged.Invoke(this, new CollectionChangeEventArgs<T>
            {
                OldValues = (IReadOnlyList<T>)removedItems
            });
    }

    public void RemoveAll(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var firstIndex = FirstIndexOf(predicate);
        if (firstIndex == -1) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseNoItemFitPredicate, GetType().GetHumanReadableName()));
        TryRemoveAll(predicate);
    }

    void IList<T>.Insert(int index, T item) => Insert(index, item);

    public void Insert(int index, params T[] items) => Insert(index, items as IEnumerable<T>);

    public void Insert(params T[] items) => Insert(items == null! ? new List<T> { default! } : items as IEnumerable<T>);

    public void Insert(IEnumerable<T> items) => Insert(0, items);

    public void Insert(int index, IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var inserted = items.ToArray();

        foreach (var item in inserted)
            _items.Insert(index++, item);

        if (inserted.Any())
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
            {
                NewValues = inserted
            });
    }

    bool ICollection<T>.Remove(T item)
    {
        var wasInCollection = _items.Contains(item);
        TryRemoveFirst(item);
        return wasInCollection;
    }

    //TODO Isn't it LastIndex instead of Count?? And we don't check if it's negative??
    public void RemoveAt(int index)
    {
        if ((uint)index > (uint)Count) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotRemoveItemBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, index));
        var item = _items[index];
        _items.RemoveAt(index);
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = new[] { item }
        });
    }

    public void TryRemoveFirst(T item)
    {
        var index = FirstIndexOf(item);
        if (index > -1)
            RemoveAt(index);
    }

    public void RemoveFirst(T item)
    {
        var index = FirstIndexOf(item);
        RemoveAt(index);
    }

    public void TryRemoveFirst(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var index = FirstIndexOf(predicate);
        if (index > -1)
            RemoveAt(index);
    }

    public void RemoveFirst(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var index = FirstIndexOf(predicate);
        RemoveAt(index);
    }

    public void TryRemoveLast(T item)
    {
        var index = LastIndexOf(item);
        if (index > -1)
            RemoveAt(index);
    }

    public void RemoveLast(T item)
    {
        var index = LastIndexOf(item);
        RemoveAt(index);
    }

    public void TryRemoveLast(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var index = LastIndexOf(predicate);
        if (index > -1)
            RemoveAt(index);
    }

    public void RemoveLast(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var index = LastIndexOf(predicate);
        RemoveAt(index);
    }

    public void RemoveAt(int index, int count)
    {
        if (index < 0 || index > LastIndex) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotRemoveItemBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, index));
        if (count <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseCountIsZero, GetType().GetHumanReadableName(), count), nameof(count));
        if (Count - index < count) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseRangeFallsOutsideBoundaries, GetType().GetHumanReadableName(), 0, LastIndex, index, count));

        var removedItems = Copy(index, count);
        _items.RemoveRange(index, count);
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = removedItems
        });
    }

    public override bool Equals(object? other) => Equals(other as IEnumerable<T>);

    public bool Equals(IEnumerable<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    public static bool operator ==(ObservableList<T>? a, IEnumerable<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableList<T>? a, IEnumerable<T>? b) => !(a == b);

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";
}