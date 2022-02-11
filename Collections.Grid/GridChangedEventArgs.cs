namespace ToolBX.Collections.Grid;

public delegate void GridChangedEvent<T>(object sender, GridChangedEventArgs<T> args);

public record GridChangedEventArgs<T>
{
    public IList<Cell<T>> OldValues
    {
        get => _oldValues;
        init => _oldValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IList<Cell<T>> _oldValues = Array.Empty<Cell<T>>();

    public IList<Cell<T>> NewValues
    {
        get => _newValues;
        init => _newValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IList<Cell<T>> _newValues = Array.Empty<Cell<T>>();
}