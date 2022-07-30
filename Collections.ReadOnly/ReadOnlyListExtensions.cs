namespace ToolBX.Collections.ReadOnly;

public static class ReadOnlyListExtensions
{
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source) => new ReadOnlyList<T>(source);

    /// <summary>
    /// Returns a new read-only list with the specified elements.
    /// </summary>
    public static IReadOnlyList<T> With<T>(this IReadOnlyList<T> source, params T[] items) => source.With(items as IEnumerable<T>);

    /// <summary>
    /// Returns a new read-only list with the specified elements.
    /// </summary>
    public static IReadOnlyList<T> With<T>(this IReadOnlyList<T> source, IEnumerable<T> items)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (items == null) throw new ArgumentNullException(nameof(items));
        return new ReadOnlyList<T>(source.Concat(items));
    }

    /// <summary>
    /// Returns a new read-only list without the specified elements.
    /// </summary>
    public static IReadOnlyList<T> Without<T>(this IReadOnlyList<T> source, Func<T, bool> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return new ReadOnlyList<T>(source.Where(x => !predicate(x)));
    }

    /// <summary>
    /// Returns a new read-only list without the specified elements.
    /// </summary>
    public static IReadOnlyList<T> Without<T>(this IReadOnlyList<T> source, params T[] items) => source.Without(items as IEnumerable<T>);

    /// <summary>
    /// Returns a new read-only list without the specified elements.
    /// </summary>
    public static IReadOnlyList<T> Without<T>(this IReadOnlyList<T> source, IEnumerable<T> items)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (items == null) throw new ArgumentNullException(nameof(items));
        return new ReadOnlyList<T>(source.Where(x => !items.Contains(x)));
    }
    
    /// <summary>
    /// Returns a new read-only list with the specified elements inserted at index.
    /// </summary>
    public static IReadOnlyList<T> WithAt<T>(this IReadOnlyList<T> source, int index, params T[] items) => source.WithAt(index, items as IEnumerable<T>);

    /// <summary>
    /// Returns a new read-only list with the specified elements inserted at index.
    /// </summary>
    public static IReadOnlyList<T> WithAt<T>(this IReadOnlyList<T> source, int index, IEnumerable<T> items)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (items == null) throw new ArgumentNullException(nameof(items));
        var content = source.ToList();
        content.InsertRange(index, items);
        return content.ToReadOnlyList();
    }

    /// <summary>
    /// Returns a new read-only list without the element at the specified index.
    /// </summary>
    public static IReadOnlyList<T> WithoutAt<T>(this IReadOnlyList<T> source, int index)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var content = source.ToList();
        content.RemoveAt(index);
        return content.ToReadOnlyList();
    }

    /// <summary>
    /// Returns a new read-only with the elements at the specified index swapped.
    /// </summary>
    public static IReadOnlyList<T> WithSwapped<T>(this IReadOnlyList<T> source, int current, int destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var content = source.ToList();
        content[current] = source[destination];
        content[destination] = source[current];
        return content.ToReadOnlyList();
    }
}