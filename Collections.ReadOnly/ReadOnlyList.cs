namespace ToolBX.Collections.ReadOnly;

/// <summary>
/// A read-only collection of elements that can be accessed via indexer.
/// </summary>
public record ReadOnlyList<T> : IReadOnlyList<T>, IEquatable<IEnumerable<T>>
{
    public static readonly ReadOnlyList<T> Empty = new();

    public T this[int index] => _items[index];

    public int Count => _items.Count;

    private readonly IReadOnlyList<T> _items;

    private readonly Lazy<int> _hashcode;

    public ReadOnlyList()
    {
        _items = Array.Empty<T>();
        _hashcode = InitializeHashCode();
    }

    public ReadOnlyList(params T[] source) : this(source as IEnumerable<T>)
    {

    }

    public ReadOnlyList(IEnumerable<T> source)
    {
        _items = source?.ToArray() ?? throw new ArgumentNullException(nameof(source));
        _hashcode = InitializeHashCode();
    }

    private Lazy<int> InitializeHashCode() => new(() =>
    {
        unchecked
        {
            return _items.Aggregate(17, (current, item) => current * 31 + (item?.GetHashCode() ?? 0));
        }
    });

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual bool Equals(ReadOnlyList<T>? other) => Equals(other as IEnumerable<T>);

    public bool Equals(IEnumerable<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items.SequenceEqual(other);
    }

    public static bool operator ==(ReadOnlyList<T>? a, IEnumerable<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(ReadOnlyList<T>? a, IEnumerable<T>? b) => !(a == b);

    public override int GetHashCode() => _hashcode.Value;

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} elements";
}

public static class ReadOnlyList
{
    public static ReadOnlyList<T> Create<T>(params T[] items) => new(items);
    public static ReadOnlyList<T> Create<T>(IEnumerable<T> items) => new(items);
}