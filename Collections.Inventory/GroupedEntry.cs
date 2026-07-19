namespace ToolBX.Collections.Inventory;

public sealed record GroupedEntry<T> : EntryBase<T>
{
    public IReadOnlyList<int> Indexes
    {
        get;
        init => field = value?.ToImmutableList() ?? throw new ArgumentNullException(nameof(value));
    } = ImmutableList<int>.Empty;

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

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        foreach (var index in Indexes) hash.Add(index);
        return hash.ToHashCode();
    }

    public override string ToString() => $"{base.ToString()} at indexes {string.Join(", ", Indexes)}";
}