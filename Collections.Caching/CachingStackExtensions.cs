namespace ToolBX.Collections.Caching;

public static class CachingStackExtensions
{
    public static CachingStack<T> ToCachingStack<T>(this IEnumerable<T> source) => new(source);
}