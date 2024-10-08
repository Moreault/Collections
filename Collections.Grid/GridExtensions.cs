﻿namespace ToolBX.Collections.Grid;

public static class GridExtensions
{
    public static Grid<T> ToGrid<T>(this IEnumerable<Cell<T>> cells)
    {
        if (cells == null) throw new ArgumentNullException(nameof(cells));
        return new Grid<T>(cells);
    }

    public static Grid<T> ToGrid<T>(this IEnumerable<KeyValuePair<Vector2<int>, T>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Grid<T>(collection);
    }

    public static Grid<T> ToGrid<T>(this T[,] collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Grid<T?>(collection)!;
    }

    public static Grid<T> ToGrid<T>(this T[][] collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new Grid<T?>(collection)!;
    }

    public static Grid<T> ToGrid<T>(this IEnumerable<T> collection, int columnCount)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (columnCount <= 0) throw new ArgumentException(string.Format(Exceptions.CannotConvertToGridFromColumnCount, columnCount));

        var grid = new Grid<T>();

        var x = 0;
        var y = 0;

        foreach (var item in collection)
        {
            grid[x, y] = item;
            if (x >= columnCount)
            {
                x = 0;
                y++;
            }
            else
                x++;
        }

        return grid;
    }

    public static T?[,] To2dArray<T>(this IGrid<T> grid)
    {
        if (grid == null) throw new ArgumentNullException(nameof(grid));
        if (grid.FirstColumn < 0 || grid.FirstRow < 0) throw new InvalidOperationException(Exceptions.To2dArrayNegativeIndexes);

        var array = new T[grid.ColumnCount, grid.RowCount];
        foreach (var ((x, y), value) in grid)
            array[x, y] = value!;

        return array;
    }

    public static T[][] ToJaggedArray<T>(this IGrid<T> grid)
    {
        if (grid == null) throw new ArgumentNullException(nameof(grid));
        if (grid.FirstColumn < 0 || grid.FirstRow < 0) throw new InvalidOperationException(Exceptions.ToJaggedArrayNegativeIndexes);

        var array = new T[grid.ColumnCount][];
        for (var x = grid.FirstColumn; x < array.Length; x++)
        {
            array[x] = new T[grid.RowCount];
            for (var y = 0; y < grid.RowCount; y++)
            {
                array[x][y] = grid[x, y]!;
            }
        }
        return array;
    }

    public static Dictionary<Vector2<int>, T?> ToDictionary<T>(this IGrid<T> grid)
    {
        if (grid == null) throw new ArgumentNullException(nameof(grid));
        return grid.ToDictionary(x => x.Index, x => x.Value);
    }

    public static bool SequenceEqual<TSource>(this IGrid<TSource> first, IEnumerable<Cell<TSource>> second, IEqualityComparer<TSource>? comparer = default)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        if (ReferenceEquals(first, second)) return true;

        var secondAsGrid = second as IGrid<TSource> ?? second.ToGrid();
        if (first.Count != secondAsGrid.Count) return false;

        comparer ??= EqualityComparer<TSource>.Default;

        foreach (var cell in first)
        {
            if (!comparer.Equals(cell.Value, secondAsGrid[cell.Index]))
            {
                return false;
            }
        }
        return true;
    }
}