namespace ToolBX.Collections.Caching;

public interface ICachingStack<T> : IObservableStack<T>, IEquatable<ICachingStack<T>>
{
    int Limit { get; set; }
    void TrimTopDownTo(int limit);
    void TrimBottomDownTo(int limit);
}

public class CachingStack<T> : ICachingStack<T>, IEquatable<CachingStack<T>>
{
    private readonly ObservableList<T> _items;

    public int Count => _items.Count;

    bool ICollection.IsSynchronized => false;
    object ICollection.SyncRoot => this;

    int ICollection.Count => Count;

    int IReadOnlyCollection<T>.Count => Count;

    public int Limit
    {
        get => _limit;
        set
        {
            _limit = Math.Clamp(value, 0, int.MaxValue);
            TrimBottomDownTo(_limit);
        }
    }
    private int _limit = int.MaxValue;

    public event CollectionChangeEventHandler<T>? CollectionChanged
    {
        add => _items.CollectionChanged += value;
        remove => _items.CollectionChanged -= value;
    }

    public CachingStack()
    {
        _items = new ObservableList<T>();
        CollectionChanged += OnCollectionChanged;
    }

    public CachingStack(IEnumerable<T> collection)
    {
        _items = new ObservableList<T>(collection);
        CollectionChanged += OnCollectionChanged;
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    void ICollection.CopyTo(Array array, int index) => _items.ToArray().CopyTo(array, index);

    public void Clear() => _items.Clear();

    public bool Contains(T item) => _items.Contains(item);

    public T Peek()
    {
        if (Count == 0) throw new InvalidOperationException(Exceptions.CannotPeekBecauseEmpty);
        return _items.First();
    }

    public Result<T> TryPeek()
    {
        return Count == 0 ? Result<T>.Failure() : Result<T>.Success(_items.First());
    }

    public T Pop()
    {
        if (Count == 0) throw new InvalidOperationException(Exceptions.CannotUsePopBecauseEmpty);
        var item = _items[0];
        _items.RemoveAt(0);
        return item;
    }

    public Result<T> TryPop()
    {
        return Count == 0 ? Result<T>.Failure() : Result<T>.Success(Pop());
    }

    public void Push(params T[] items) => Push(items as IEnumerable<T>);

    public void Push(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        var list = items.ToList();
        list.Reverse();
        _items.Insert(list);
    }

    public void TrimTopDownTo(int limit) => _items.TrimStartDownTo(limit);

    public void TrimBottomDownTo(int limit) => _items.TrimEndDownTo(limit);

    public bool Equals(IObservableStack<T>? other) => Equals(other as CachingStack<T>);

    public bool Equals(ICachingStack<T>? other) => Equals(other as CachingStack<T>);

    public bool Equals(CachingStack<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items.Equals(other._items) && Limit == other.Limit;
    }

    public override bool Equals(object? obj) => Equals(obj as CachingStack<T>);

    public static bool operator ==(CachingStack<T>? a, CachingStack<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(CachingStack<T>? a, CachingStack<T>? b) => !(a == b);

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";

    private void OnCollectionChanged(object sender, CollectionChangeEventArgs<T> args) => TrimBottomDownTo(_limit);
}