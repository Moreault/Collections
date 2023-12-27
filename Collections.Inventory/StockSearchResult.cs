namespace ToolBX.Collections.Inventory;

public sealed record StockSearchResult<T> : IReadOnlyList<IndexedEntry<T>>, IEquatable<IEnumerable<IndexedEntry<T>>>
{
    private readonly IReadOnlyList<IndexedEntry<T>> _items;

    public IndexedEntry<T> this[int index] => _items[index];

    public int Count => _items.Count;

    public StockSearchResult(IEnumerable<IndexedEntry<T>> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        _items = items as IReadOnlyList<IndexedEntry<T>> ?? items.ToList();
    }

    public IEnumerator<IndexedEntry<T>> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(StockSearchResult<T>? other) => Equals(other as IEnumerable<IndexedEntry<T>>);

    public bool Equals(IEnumerable<IndexedEntry<T>>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    public override int GetHashCode()
    {
        return _items.GetHashCode();
    }

    public IReadOnlyList<GroupedEntry<T>> Group()
    {
        if (Count == 0) return Array.Empty<GroupedEntry<T>>();

        var items = this.DistinctBy(x => x.Item);
        var group = new List<GroupedEntry<T>>();
        foreach (var item in items)
        {
            var entries = this.Where(x => Equals(x.Item, item)).ToList();
            var quantity = entries.Sum(x => x.Quantity);
            var indexes = entries.Select(x => x.Index);
            group.Add(new GroupedEntry<T>(item.Item, quantity, indexes));
        }
        return group;
    }
}