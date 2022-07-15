namespace ToolBX.Collections.Caching;

public static class CachingDictionaryExtensions
{
    public static CachingDictionary<TKey, TSource> ToCachingDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        var dictionary = new CachingDictionary<TKey, TSource>();
        foreach (var element in source)
            dictionary.Add(keySelector(element), element);
        return dictionary;
    }

    public static CachingDictionary<TKey, TElement> ToCachingDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

        var dictionary = new CachingDictionary<TKey, TElement>();
        foreach (var element in source)
            dictionary.Add(keySelector(element), elementSelector(element));
        return dictionary;
    }
}