namespace ToolBX.Collections.Grid;

public readonly record struct Coordinates
{
    public int X { get; init; }
    public int Y { get; init; }

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";

    public static Coordinates operator +(Coordinates a, Coordinates b) => new() { X = a.X + b.X, Y = a.Y + b.Y };
    public static Coordinates operator -(Coordinates a, Coordinates b) => new() { X = a.X - b.X, Y = a.Y - b.Y };

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}