namespace ToolBX.Collections.Grid;

public readonly record struct Cell<T>
{
    public Vector2<int> Index { get; init; }
    public T? Value { get; init; }

    public int X => Index.X;
    public int Y => Index.Y;

    public Cell(int x, int y, T? value) : this(new Vector2<int>(x, y), value)
    {

    }

    public Cell(Vector2<int> index, T? value)
    {
        Index = index;
        Value = value;
    }

    public void Deconstruct(out Vector2<int> index, out T? value)
    {
        index = Index;
        value = Value;
    }
}