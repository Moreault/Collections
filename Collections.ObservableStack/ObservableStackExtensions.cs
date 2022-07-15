namespace ToolBX.Collections.ObservableStack;

public static class ObservableStackExtensions
{
    public static ObservableStack<T> ToObservableStack<T>(this IEnumerable<T> source) => new(source);
}