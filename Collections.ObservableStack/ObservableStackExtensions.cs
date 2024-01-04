namespace ToolBX.Collections.ObservableStack;

public static class ObservableStackExtensions
{
    public static ObservableStack<T> ToObservableStack<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        return new ObservableStack<T>(source);
    }
}