using ToolBX.Reflection4Humans.ValueEquality;

namespace ToolBX.Collections.Grid;

public delegate void GridChangedEvent<T>(object sender, GridChangedEventArgs<T> args);

public sealed record GridChangedEventArgs<T>
{
    //TODO 3.0.0 : IReadOnlyList<Cell<T>>
    public IList<Cell<T>> OldValues
    {
        get => _oldValues;
        init => _oldValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IList<Cell<T>> _oldValues = Array.Empty<Cell<T>>();

    //TODO 3.0.0 : IReadOnlyList<Cell<T>>
    public IList<Cell<T>> NewValues
    {
        get => _newValues;
        init => _newValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IList<Cell<T>> _newValues = Array.Empty<Cell<T>>();

    public bool Equals(GridChangedEventArgs<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _oldValues.SequenceEqualOrNull(other._oldValues) && _newValues.SequenceEqualOrNull(other._newValues);
    }

    public override int GetHashCode() => this.GetValueHashCode();
}