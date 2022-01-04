namespace ToolBX.Collections.Deck;

public static class DeckExtensions
{
    public static IDeck<T> ToDeck<T>(this IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Deck<T>(collection);
    }
}