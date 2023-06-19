namespace ToolBX.Collections.Common;

public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangeEventArgs<T> args);

public sealed record CollectionChangeEventArgs<T>
{
    public IReadOnlyList<T> OldValues
    {
        get => _oldValues;
        init => _oldValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<T> _oldValues = Array.Empty<T>();

    public IReadOnlyList<T> NewValues
    {
        get => _newValues;
        init => _newValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<T> _newValues = Array.Empty<T>();

    public bool Equals(CollectionChangeEventArgs<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _oldValues.SequenceEqual(other._oldValues) && _newValues.SequenceEqual(other._newValues);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_oldValues, _newValues);
    }
}