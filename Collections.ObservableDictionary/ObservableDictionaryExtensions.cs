namespace ToolBX.Collections.ObservableDictionary;

public static class ObservableDictionaryExtensions
{
    public static ObservableDictionary<TKey, TSource> ToObservableDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        var dictionary = new ObservableDictionary<TKey, TSource>(comparer!);
        foreach (var element in source)
            dictionary.Add(keySelector(element), element);
        return dictionary;
    }

    public static ObservableDictionary<TKey, TElement> ToObservableDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

        var dictionary = new ObservableDictionary<TKey, TElement>(comparer!);
        foreach (var element in source)
            dictionary.Add(keySelector(element), elementSelector(element));
        return dictionary;
    }
}