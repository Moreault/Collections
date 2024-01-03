namespace ToolBX.Collections.Inventory;

public sealed record StockSearchResult<T> : IReadOnlyList<IndexedEntry<T>>, IEquatable<IEnumerable<IndexedEntry<T>>>
{
    private readonly IReadOnlyList<IndexedEntry<T>> _items;

    public IndexedEntry<T> this[int index] => _items[index];

    public int Count => _items.Count;

    public StockSearchResult(IEnumerable<IndexedEntry<T>> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        _items = items.ToImmutableList();
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

    public override int GetHashCode() => _items.GetValueHashCode();

    public IReadOnlyList<GroupedEntry<T>> Group()
    {
        if (Count == 0) return Array.Empty<GroupedEntry<T>>();

        var distinctEntries = this.DistinctBy(x => x.Item);
        var group = new List<GroupedEntry<T>>();
        foreach (var entry in distinctEntries)
        {
            var duplicateEntries = _items.Where(x => Equals(x.Item, entry.Item)).ToList();
            var quantity = duplicateEntries.Sum(x => x.Quantity);
            var indexes = duplicateEntries.Select(x => x.Index);
            group.Add(new GroupedEntry<T>(entry.Item, quantity, indexes));
        }
        return group;
    }
}