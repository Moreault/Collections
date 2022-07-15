namespace ToolBX.Collections.Caching;

public interface ICachingDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue> where TKey : notnull
{
    int Limit { get; set; }
    void TrimStartDownTo(int maxSize);
    void TrimEndDownTo(int maxSize);
}

public class CachingDictionary<TKey, TValue> : ICachingDictionary<TKey, TValue> where TKey : notnull
{
    private readonly CachingList<KeyValuePair<TKey, TValue>> _items;

    public int Limit
    {
        get => _items.Limit;
        set => _items.Limit = value;
    }

    public ICollection<TKey> Keys => _items.Select(x => x.Key).ToArray();

    public ICollection<TValue> Values => _items.Select(x => x.Value).ToArray();

    public event CollectionChangeEventHandler<KeyValuePair<TKey, TValue>>? CollectionChanged
    {
        add => _items.CollectionChanged += value;
        remove => _items.CollectionChanged -= value;
    }

    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
        get
        {
            if (!ContainsKey(key)) throw new KeyNotFoundException(string.Format(Exceptions.KeyNotFound, key));
            return _items.First(x => Equals(x.Key, key)).Value;
        }
        set
        {
            var index = _items.FirstIndexOf(x => Equals(x.Key, key));
            if (index < 0)
                Add(key, value);
            else if (!Equals(_items[index].Value, value))
                _items[index] = new KeyValuePair<TKey, TValue>(key, value);
        }
    }

    public CachingDictionary()
    {
        _items = new CachingList<KeyValuePair<TKey, TValue>>();
    }

    public CachingDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        _items = items?.ToCachingList() ?? throw new ArgumentNullException(nameof(items));
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (ContainsKey(item.Key)) throw new ArgumentException(string.Format(Exceptions.AddingKeyThatAlreadyExists, item.Key));
        _items.Add(item);
    }

    public void Clear() => _items.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => _items.Contains(item);

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_items).CopyTo(array, arrayIndex);

    public void Remove(KeyValuePair<TKey, TValue> item)
    {
        var index = _items.FirstIndexOf(item);
        if (index < 0) throw new InvalidOperationException(string.Format(Exceptions.RemovingInexistantKey, item.Key, item.Value));
        _items.RemoveAt(index);
    }

    public void Remove(params TKey[] keys) => Remove(keys as IEnumerable<TKey>);

    public void Remove(IEnumerable<TKey> keys) => _items.RemoveAll(x => keys.Contains(x.Key));

    public void Remove(TKey key) => _items.RemoveAll(x => Equals(key, x.Key));

    public void Remove(Func<KeyValuePair<TKey, TValue>, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.RemoveAll(match);
    }

    public void Remove(Func<TKey, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.RemoveAll(x => match(x.Key));
    }

    public void Remove(Func<TValue, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.RemoveAll(x => match(x.Value));
    }

    public void TryRemove(params KeyValuePair<TKey, TValue>[] items) => TryRemove(items as IEnumerable<KeyValuePair<TKey, TValue>>);

    public void TryRemove(IEnumerable<KeyValuePair<TKey, TValue>> items) => _items.TryRemoveAll(items);

    public void TryRemove(KeyValuePair<TKey, TValue> item) => _items.TryRemoveFirst(item);

    public void TryRemove(params TKey[] keys) => TryRemove(keys as IEnumerable<TKey>);

    public void TryRemove(IEnumerable<TKey> keys) => _items.TryRemoveAll(x => keys.Contains(x.Key));

    public void TryRemove(TKey key) => _items.TryRemoveFirst(x => Equals(x.Key, key));

    public void TryRemove(Func<KeyValuePair<TKey, TValue>, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.TryRemoveAll(match);
    }

    public void TryRemove(Func<TKey, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.TryRemoveAll(x => match(x.Key));
    }

    public void TryRemove(Func<TValue, bool> match)
    {
        if (match is null) throw new ArgumentNullException(nameof(match));
        _items.TryRemoveAll(x => match(x.Value));
    }

    public ObservableDictionary<TKey, TValue> Copy() => new(this);

    public TryGetResult<TValue> TryGetValue(TKey key)
    {
        var index = _items.FirstIndexOf(x => Equals(x.Key, key));
        return index < 0 ? TryGetResult<TValue>.Failure : new TryGetResult<TValue>(true, _items[index].Value);
    }

    public void Add(params KeyValuePair<TKey, TValue>[] items) => Add(items as IEnumerable<KeyValuePair<TKey, TValue>>);

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        var list = items as IList<KeyValuePair<TKey, TValue>> ?? items.ToList();
        if (list.Any(x => ContainsKey(x.Key))) throw new ArgumentException(Exceptions.AddingKeyThatAlreadyExists);
        if (list.Select(x => x.Key).GroupBy(x => x).Any(x => x.Count() > 1))
            throw new InvalidOperationException(string.Format(Exceptions.ItemsContainDuplicateKeys, string.Join(',', list.Select(x => x.Key).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key))));
        _items.Add(list);
    }

    public void Remove(params KeyValuePair<TKey, TValue>[] items) => Remove(items as IEnumerable<KeyValuePair<TKey, TValue>>);

    public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items) => _items.RemoveAll(items);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!Contains(item)) return false;
        _items.TryRemoveAll(item);
        return true;
    }

    public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

    public bool ContainsKey(TKey key) => _items.Any(x => Equals(x.Key, key));

    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
        if (!ContainsKey(key)) return false;
        _items.TryRemoveAll(x => Equals(x.Key));
        return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var result = TryGetValue(key);
        value = result.Value!;
        return result.IsSuccess;
    }

    public bool Equals(IObservableDictionary<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    public void TrimStartDownTo(int maxSize) => _items.TrimStartDownTo(maxSize);

    public void TrimEndDownTo(int maxSize) => _items.TrimEndDownTo(maxSize);

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";
}