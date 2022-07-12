namespace ToolBX.Collections.ObservableList;

/// <summary>
/// An observable, dynamic one-dimensional array.
/// </summary>
public interface IObservableList<T> : IList<T>, IReadOnlyList<T>, IObservableCollection<T>
{
    new int Count { get; }
    new T this[int index] { get; set; }
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
    int FirstIndexOf(T item);
    int FirstIndexOf(Func<T, bool> predicate);
    int LastIndexOf(T item);
    int LastIndexOf(Func<T, bool> predicate);
    ObservableList<int> IndexesOf(T item);
    ObservableList<int> IndexesOf(Func<T, bool> predicate);
    void Swap(int currentIndex, int destinationIndex);

    /// <summary>
    /// Sizes the collection down to maxSize by removing the first elements if needed.
    /// </summary>
    void TrimStartDownTo(int maxSize);

    /// <summary>
    /// Sizes the collection down to maxSize by removing the last elements if needed.
    /// </summary>
    void TrimEndDownTo(int maxSize);
}

/// <inheritdoc cref="IObservableList{T}"/>
public class ObservableList<T> : IObservableList<T>, IEquatable<IEnumerable<T>>
{
    private T[] _items;

    private const int DefaultCapacity = 4;
    private int _version;

    public int Count { get; private set; }

    bool ICollection<T>.IsReadOnly => false;

    public int LastIndex => Count - 1;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index > LastIndex) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotGetItemBecauseIndexIsOutOfRange, 0, LastIndex, index));
            return _items[index];
        }
        set
        {
            if (index < 0 || index > LastIndex) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotGetItemBecauseIndexIsOutOfRange, 0, LastIndex, index));
            var oldValue = _items[index];
            _items[index] = value;
            _version++;
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
        _items = Array.Empty<T>();
    }

    public ObservableList(params T[] items) : this(items as IEnumerable<T>)
    {

    }

    public ObservableList(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        _items = collection.ToArray();
        Count = _items.Length;
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

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

        var copy = items.ToArray();

        Grow(copy.Length);

        var oldCount = Count;
        Count += copy.Length;

        foreach (var item in copy)
            _items[oldCount++] = item;

        _version++;
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            NewValues = copy
        });
    }

    private void Grow(int value)
    {
        var newCapacity = _items.Length == 0 ? DefaultCapacity : 2 * _items.Length;
        if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        var originalRequiredCapacity = _items.Length + value;
        if (newCapacity < originalRequiredCapacity) newCapacity = originalRequiredCapacity;

        var newItems = new T[newCapacity];
        if (newCapacity > 0)
            Array.Copy(_items, newItems, Count);
        _items = newItems;
    }

    public void Clear()
    {
        if (Count == 0) return;

        var oldValues = CollectionChanged == null ? Array.Empty<T>() : _items.Take(Count).ToArray();

        Array.Clear(_items, 0, Count);
        Count = 0;
        _version++;
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = oldValues
        });
    }

    bool ICollection<T>.Contains(T item) => _items.Contains(item);

    void ICollection<T>.CopyTo(T[] array, int index) => Array.Copy(_items, 0, array, index, Count);

    public ObservableList<T> Copy(int startingIndex = 0)
    {
        return Copy(startingIndex, Count - startingIndex);
    }

    public ObservableList<T> Copy(int startingIndex, int count)
    {
        if (startingIndex < 0 || startingIndex > LastIndex) throw new ArgumentOutOfRangeException(nameof(startingIndex), string.Format(Exceptions.CannotCopyBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, startingIndex));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), string.Format(Exceptions.CannotCopyBecauseCountIsNegative, GetType().GetHumanReadableName(), count));
        if (Count - startingIndex < count) throw new ArgumentException(string.Format(Exceptions.CannotCopyBecauseRangeFallsOutsideBoundaries, GetType().GetHumanReadableName(), 0, LastIndex, startingIndex, count));

        if (startingIndex == 0 && count == _items.Length) return new ObservableList<T>(this);

        var copy = new ObservableList<T>();
        for (var i = startingIndex; i < startingIndex + count; i++)
            copy.Add(this[i]);
        return copy;
    }

    int IList<T>.IndexOf(T item) => FirstIndexOf(item);

    public int FirstIndexOf(T item) => Array.IndexOf(_items, item, 0, Count);

    public int FirstIndexOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return Array.FindIndex(_items, 0, Count, new Predicate<T>(predicate));
    }

    public int LastIndexOf(T item)
    {
        for (var i = LastIndex; i >= 0; i--)
        {
            if (item == null && _items[i] == null || _items[i] != null && _items[i]!.Equals(item)) return i;
        }
        return -1;
    }

    public int LastIndexOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        for (var i = LastIndex; i >= 0; i--)
        {
            if (predicate(_items[i])) return i;
        }
        return -1;
    }

    public ObservableList<int> IndexesOf(T item)
    {
        var indexes = new ObservableList<int>();

        for (var i = 0; i < Count; i++)
        {
            if (_items[i] is null && item is null || _items[i] != null && _items[i]!.Equals(item))
                indexes.Add(i);
        }

        return indexes;
    }

    public ObservableList<int> IndexesOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var indexes = new ObservableList<int>();
        for (var i = 0; i < Count; i++)
        {
            if (predicate(_items[i]))
                indexes.Add(i);
        }
        return indexes;
    }

    public void Swap(int currentIndex, int destinationIndex)
    {
        if (currentIndex < 0 || currentIndex > LastIndex) throw new ArgumentOutOfRangeException(string.Format(Exceptions.CannotSwapItemsBecauseCurrentIndexIsOutOfRange, LastIndex, currentIndex));
        if (destinationIndex < 0 || destinationIndex > LastIndex) throw new ArgumentOutOfRangeException(string.Format(Exceptions.CannotSwapItemsBecauseDestinationIndexIsOutOfRange, LastIndex, destinationIndex));

        if (currentIndex == destinationIndex) return;

        var firstItem = _items[currentIndex];
        var secondItem = _items[destinationIndex];

        _items[currentIndex] = secondItem;
        _items[destinationIndex] = firstItem;
    }

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
                RemoveAtInternal(0);
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
                RemoveAtInternal(LastIndex);
            }

            if (removed.Any())
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
                {
                    OldValues = (IReadOnlyList<T>)removed
                });
        }
    }

    public void TryRemoveAll(T item)
    {
        var originalCount = Count;
        while (true)
        {
            var index = FirstIndexOf(item);
            if (index == -1) break;

            RemoveAtInternal(index);
        }

        if (originalCount > Count)
        {
            _version++;

            var items = new List<T>();
            for (var i = 0; i < originalCount - Count; i++)
                items.Add(item);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
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
        var list = items.ToList();
        var indexes = list.Select(IndexesOf).SelectMany(x => x).ToList();
        if (indexes.Count != list.Count) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, GetType().GetHumanReadableName()));

        foreach (var index in indexes.OrderByDescending(x => x))
            RemoveAtInternal(index);

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = list
        });
    }

    public void TryRemoveAll(params T[] items) => TryRemoveAll(items as IEnumerable<T>);

    public void TryRemoveAll(IEnumerable<T> items) => RemoveAll(items.Where(x => _items.Contains(x)));

    public void TryRemoveAll(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var removedItems = new List<T>();

        while (true)
        {
            var index = FirstIndexOf(predicate);
            if (index == -1) break;

            removedItems.Add(_items[index]);
            RemoveAtInternal(index);
        }

        if (removedItems.Any())
        {
            _version++;

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
            {
                OldValues = removedItems
            });
        }
    }

    public void RemoveAll(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var firstIndex = FirstIndexOf(predicate);
        if (firstIndex == -1) throw new InvalidOperationException(string.Format(Exceptions.CannotRemoveItemBecauseNoItemFitPredicate, GetType().GetHumanReadableName()));
        TryRemoveAll(predicate);
    }

    void IList<T>.Insert(int index, T item) => Insert(index, item);

    public void Insert(params T[] items) => Insert(0, items);

    public void Insert(IEnumerable<T> items) => Insert(0, items);

    public void Insert(int index, params T[] items) => Insert(index, ReferenceEquals(items, null!) ? new T[] { default! } : (IEnumerable<T>)items);

    public void Insert(int index, IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        //TODO Test LastIndex + 1 behaviour
        if (index < 0 || index > LastIndex + 1) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotInsertItemsBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, index));
        var copy = items.ToArray();

        if (_items.Length <= Count + copy.Length)
            Grow(Count + copy.Length - _items.Length);

        if (index < Count)
        {
            Array.Copy(_items, index, _items, index + copy.Length, Count - index);
        }

        Count += copy.Length;
        _version++;
        foreach (var item in copy)
            _items[index++] = item;

        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            NewValues = copy
        });
    }

    bool ICollection<T>.Remove(T item)
    {
        var wasInCollection = _items.Contains(item);
        TryRemoveFirst(item);
        return wasInCollection;
    }

    public void RemoveAt(int index)
    {
        if ((uint)index > (uint)Count) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotRemoveItemBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, index));

        var item = _items[index];
        _version++;
        RemoveAtInternal(index);
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = new[] { item }
        });
    }

    public void RemoveAt(int index, int count)
    {
        if (index < 0 || index > LastIndex) throw new ArgumentOutOfRangeException(nameof(index), string.Format(Exceptions.CannotRemoveItemBecauseIndexIsOutOfRange, GetType().GetHumanReadableName(), 0, LastIndex, index));
        if (count <= 0) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseCountIsZero, GetType().GetHumanReadableName(), count), nameof(count));
        if (Count - index < count) throw new ArgumentException(string.Format(Exceptions.CannotRemoveItemBecauseRangeFallsOutsideBoundaries, GetType().GetHumanReadableName(), 0, LastIndex, index, count));

        var removedItems = Copy(index, count);

        Count -= count;
        if (index < Count)
            Array.Copy(_items, index + count, _items, index, Count - index);

        _version++;
        Array.Clear(_items, Count, count);
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = removedItems
        });
    }

    private void RemoveAtInternal(int index)
    {
        Count--;
        Array.Copy(_items, index + 1, _items, index, Count - index);
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

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";

    public bool Equals(IEnumerable<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    public override bool Equals(object? obj) => Equals(obj as IEnumerable<T>);

    public static bool operator ==(ObservableList<T>? a, ObservableList<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableList<T>? a, ObservableList<T>? b) => !(a == b);

    public static bool operator ==(ObservableList<T>? a, IEnumerable<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableList<T>? a, IEnumerable<T>? b) => !(a == b);

    public static bool operator ==(ObservableList<T>? a, IList<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableList<T>? a, IList<T>? b) => !(a == b);

    public static bool operator ==(ObservableList<T>? a, ICollection<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableList<T>? a, ICollection<T>? b) => !(a == b);

    public override int GetHashCode() => _items.GetHashCode();

    public struct Enumerator : IEnumerator<T>
    {
        private readonly ObservableList<T> _observableList;
        private int _index;
        private readonly int _version;

        public T Current { get; private set; }
        object? IEnumerator.Current => Current;

        internal Enumerator(ObservableList<T> observableList)
        {
            _observableList = observableList;
            _version = observableList._version;
            _index = 0;
            Current = default!;
        }

        public bool MoveNext()
        {
            if (_version != _observableList._version) throw new InvalidOperationException(string.Format(Exceptions.CannotEnumerateBecauseModified, GetType().GetHumanReadableName()));

            if ((uint)_index < (uint)_observableList.Count)
            {
                Current = _observableList[_index++];
                return true;
            }

            _index = _observableList.Count + 1;
            Current = default!;
            return false;
        }

        public void Reset()
        {
            if (_version != _observableList._version) throw new InvalidOperationException(string.Format(Exceptions.CannotEnumerateBecauseModified, GetType().GetHumanReadableName()));
            _index = 0;
            Current = default!;
        }

        public void Dispose()
        {

        }
    }
}