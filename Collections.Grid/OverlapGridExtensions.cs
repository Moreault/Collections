namespace ToolBX.Collections.Grid;

public static class OverlapGridExtensions
{
    public static OverlapGrid<T> ToOverlapGrid<T>(this IEnumerable<Cell<T>> cells)
    {
        if (cells == null) throw new ArgumentNullException(nameof(cells));
        return new OverlapGrid<T>(cells);
    }
}