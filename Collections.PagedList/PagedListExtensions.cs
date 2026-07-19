namespace ToolBX.Collections.PagedList;

public static class PagedListExtensions
{
    /// <summary>
    /// Paginates an in-memory sequence, returning the requested page along with the total item count.
    /// </summary>
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be greater than zero.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater than zero.");

        var all = source as IReadOnlyCollection<T> ?? source.ToArray();
        var page = all.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return new PagedList<T>(page, pageNumber, pageSize, all.Count);
    }
}
