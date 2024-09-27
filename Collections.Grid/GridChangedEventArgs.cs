namespace ToolBX.Collections.Grid;

public delegate void GridChangedEvent<T>(object sender, GridChangedEventArgs<T> args);

public sealed record GridChangedEventArgs<T>
{
    public IReadOnlyList<Cell<T>> OldValues
    {
        get => _oldValues;
        init => _oldValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<Cell<T>> _oldValues = [];

    public IReadOnlyList<Cell<T>> NewValues
    {
        get => _newValues;
        init => _newValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<Cell<T>> _newValues = [];

    public bool Equals(GridChangedEventArgs<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _oldValues.SequenceEqualOrNull(other._oldValues) && _newValues.SequenceEqualOrNull(other._newValues);
    }

    public override int GetHashCode() => this.GetValueHashCode();
}