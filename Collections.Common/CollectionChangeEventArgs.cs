namespace Collections.Common;

public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangeEventArgs<T> args);

public class CollectionChangeEventArgs<T> : EventArgs
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
}