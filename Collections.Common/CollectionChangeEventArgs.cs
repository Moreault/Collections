﻿namespace ToolBX.Collections.Common;

public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangeEventArgs<T> args);

public sealed record CollectionChangeEventArgs<T>
{
    //TODO 3.0.0 : ImmutableList<T>
    public IReadOnlyList<T> OldValues
    {
        get => _oldValues;
        init => _oldValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<T> _oldValues = Array.Empty<T>();

    //TODO 3.0.0 : ImmutableList<T>
    public IReadOnlyList<T> NewValues
    {
        get => _newValues;
        init => _newValues = value ?? throw new ArgumentNullException(nameof(value));
    }
    private readonly IReadOnlyList<T> _newValues = Array.Empty<T>();

    //TODO 3.0.0 : Remove (equality done automatically via record and ImmutableList<T>)
    public bool Equals(CollectionChangeEventArgs<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _oldValues.SequenceEqualOrNull(other._oldValues) && _newValues.SequenceEqualOrNull(other._newValues);
    }

    public override int GetHashCode() => this.GetValueHashCode();
}