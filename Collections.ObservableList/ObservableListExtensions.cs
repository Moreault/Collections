namespace ToolBX.Collections.ObservableList;

public static class ObservableListExtensions
{
    public static IObservableList<T> ToObservableList<T>(this IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new ObservableList<T>(collection);
    }
}