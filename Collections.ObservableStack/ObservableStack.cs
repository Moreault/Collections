namespace ToolBX.Collections.ObservableStack;

public interface IObservableStack<T> : IObservableCollection<T>, ICollection, IReadOnlyCollection<T>, IEquatable<IObservableStack<T>>
{
    new int Count { get; }
    void Clear();
    bool Contains(T item);
    T Peek();
    TryGetResult<T> TryPeek();
    T Pop();
    TryGetResult<T> TryPop();
    void Push(params T[] items);
    void Push(IEnumerable<T> items);
}

public class ObservableStack<T> : IObservableStack<T>, IEquatable<ObservableStack<T>>
{
    private readonly Stack<T> _items;

    public int Count => _items.Count;

    int IObservableStack<T>.Count => Count;
    int IReadOnlyCollection<T>.Count => Count;

    bool ICollection.IsSynchronized => ((ICollection)_items).IsSynchronized;
    object ICollection.SyncRoot => ((ICollection)_items).SyncRoot;

    public event CollectionChangeEventHandler<T>? CollectionChanged;

    public ObservableStack()
    {
        _items = new Stack<T>();
    }

    public ObservableStack(IEnumerable<T> collection)
    {
        _items = new Stack<T>(collection);
    }

    public void Clear()
    {
        if (Count > 0)
        {
            var oldItems = CollectionChanged == null ? Array.Empty<T>() : _items.ToArray();
            _items.Clear();
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
            {
                OldValues = oldItems
            });
        }
    }

    public bool Contains(T item) => _items.Contains(item);

    public T Peek() => _items.Peek();

    public TryGetResult<T> TryPeek()
    {
        var isSucess = _items.TryPeek(out var result);
        return new TryGetResult<T>(isSucess, result);
    }

    public T Pop()
    {
        var item = _items.Pop();
        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
        {
            OldValues = new List<T> { item }
        });
        return item;
    }

    public TryGetResult<T> TryPop()
    {
        var isSucess = _items.TryPop(out var result);
        var item = new TryGetResult<T>(isSucess, result);
        if (item.IsSuccess)
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<T>
            {
                OldValues = new List<T> { item.Value! }
            });
        return item;
    }

    public void Push(params T[] items) => Push(items as IEnumerable<T>);

    public void Push(IEnumerable<T> items)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        var list = items as IReadOnlyList<T> ?? items.ToList();
        foreach (var item in list)
            _items.Push(item);
        if (CollectionChanged != null && list.Any())
        CollectionChanged.Invoke(this, new CollectionChangeEventArgs<T>
        {
            NewValues = list
        });
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ICollection.CopyTo(Array array, int index) => ((ICollection)_items).CopyTo(array, index);

    public bool Equals(IObservableStack<T>? other) => Equals(other as ObservableStack<T>);

    public bool Equals(ObservableStack<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items.SequenceEqual(other._items);
    }

    public override bool Equals(object? obj) => Equals(obj as ObservableStack<T>);

    public static bool operator ==(ObservableStack<T>? a, ObservableStack<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ObservableStack<T>? a, ObservableStack<T>? b) => !(a == b);

    public override int GetHashCode()
    {
        return _items.GetHashCode();
    }

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";
}