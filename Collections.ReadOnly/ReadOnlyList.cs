namespace ToolBX.Collections.ReadOnly;

/// <summary>
/// A read-only collection of elements that can be accessed via indexer.
/// </summary>
public record ReadOnlyList<T> : IReadOnlyList<T>, IEquatable<IEnumerable<T>>
{
    public static readonly IReadOnlyList<T> Empty = new ReadOnlyList<T>();

    public T this[int index] => _items[index];

    public int Count => _items.Count;

    private readonly IList<T> _items;

    public ReadOnlyList()
    {
        _items = Array.Empty<T>();
    }

    public ReadOnlyList(params T[] source) : this(source as IEnumerable<T>)
    {

    }

    public ReadOnlyList(IEnumerable<T> source)
    {
        _items = source?.ToArray() ?? throw new ArgumentNullException(nameof(source));
    }

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

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} elements";
}