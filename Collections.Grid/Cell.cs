namespace ToolBX.Collections.Grid;

public readonly record struct Cell<T>
{
    public Coordinates Index { get; init; }
    public T? Value { get; init; }

    public Cell(int x, int y, T? value) : this(new Coordinates(x, y), value)
    {

    }

    public Cell(Coordinates index, T? value)
    {
        Index = index;
        Value = value;
    }

    public void Deconstruct(out Coordinates index, out T? value)
    {
        index = Index;
        value = Value;
    }
}