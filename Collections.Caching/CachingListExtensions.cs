namespace ToolBX.Collections.Caching;

public static class CachingListExtensions
{
    public static CachingList<T> ToCachingList<T>(this IEnumerable<T> source) => new(source);
}