namespace ToolBX.Collections.PagedList;

/// <summary>
/// An immutable, read-only slice of a larger collection along with the pagination metadata
/// required to navigate it. Ideal as the result type for paginated REST endpoints or any
/// other scenario that returns paged data.
/// </summary>
public record PagedList<T> : IReadOnlyList<T>
{
    /// <summary>
    /// An empty page (page 1 of 0, with no items and a total count of zero.)
    /// </summary>
    public static readonly PagedList<T> Empty = new();

    private readonly Lazy<int> _hashCode;

    /// <summary>
    /// The items on the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; }

    /// <summary>
    /// The one-based index of the current page.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// The maximum number of items that a single page may contain.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// The total number of items across every page.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// The total number of pages required to contain <see cref="TotalCount"/> items given <see cref="PageSize"/>.
    /// </summary>
    public int PageCount => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// The number of items on the current page.
    /// </summary>
    public int Count => Items.Count;

    /// <summary>
    /// Whether there is a page before the current one.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Whether there is a page after the current one.
    /// </summary>
    public bool HasNextPage => PageNumber < PageCount;

    /// <summary>
    /// Whether the current page is the first page.
    /// </summary>
    public bool IsFirstPage => PageNumber <= 1;

    /// <summary>
    /// Whether the current page is the last page.
    /// </summary>
    public bool IsLastPage => PageNumber >= PageCount;

    public T this[int index] => Items[index];

    public PagedList()
    {
        Items = [];
        PageNumber = 1;
        PageSize = 0;
        TotalCount = 0;
        _hashCode = InitializeHashCode();
    }

    public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be greater than zero.");
        if (pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater than or equal to zero.");
        if (totalCount < 0) throw new ArgumentOutOfRangeException(nameof(totalCount), totalCount, "Total count must be greater than or equal to zero.");

        Items = items.ToArray();
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        _hashCode = InitializeHashCode();
    }

    private Lazy<int> InitializeHashCode() => new(() =>
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + PageNumber;
            hash = hash * 31 + PageSize;
            hash = hash * 31 + TotalCount;
            return Items.Aggregate(hash, (current, item) => current * 31 + (item?.GetHashCode() ?? 0));
        }
    });

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual bool Equals(PagedList<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return PageNumber == other.PageNumber &&
               PageSize == other.PageSize &&
               TotalCount == other.TotalCount &&
               Items.SequenceEqual(other.Items);
    }

    public override int GetHashCode() => _hashCode.Value;

    public override string ToString() => Count == 0
        ? $"Empty {GetType().GetHumanReadableName()} (page {PageNumber} of {PageCount})"
        : $"{GetType().GetHumanReadableName()} page {PageNumber} of {PageCount} with {Count} of {TotalCount} elements";
}
