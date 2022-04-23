namespace ToolBX.Collections.Inventory;

public record GroupedEntry<T> : Entry<T>
{
    public IReadOnlyList<int> Indexes { get; init; } = Array.Empty<int>();

    public GroupedEntry()
    {

    }

    public GroupedEntry(T item, int quantity, IEnumerable<int> indexes) : base(item, quantity)
    {
        if (indexes == null) throw new ArgumentNullException(nameof(indexes));
        Indexes = indexes as IReadOnlyList<int> ?? indexes.ToList();
    }
}