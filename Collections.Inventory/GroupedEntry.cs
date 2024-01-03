namespace ToolBX.Collections.Inventory;

public sealed record GroupedEntry<T> : EntryBase<T>
{
    public IReadOnlyList<int> Indexes
    {
        get => _indexes;
        init => _indexes = value?.ToImmutableList() ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<int> _indexes = ImmutableList<int>.Empty;

    public GroupedEntry()
    {

    }

    public GroupedEntry(T item, int quantity, IEnumerable<int> indexes) : base(item, quantity)
    {
        if (indexes == null) throw new ArgumentNullException(nameof(indexes));
        Indexes = indexes.ToImmutableList();
    }

    public bool Equals(GroupedEntry<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Indexes.SequenceEqualOrNull(other.Indexes);
    }

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Indexes.GetValueHashCode());

    public override string ToString() => $"{base.ToString()} at indexes {string.Join(", ", Indexes)}";
}