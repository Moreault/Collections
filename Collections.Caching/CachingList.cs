namespace ToolBX.Collections.Caching;

/// <summary>
/// A linear collection that can be limited to a certain amount of items.
/// </summary>
public interface ICachingList<T> : IObservableList<T>
{
    int Limit { get; set; }
}

/// <summary>
/// A linear collection that can be limited to a certain amount of items.
/// </summary>
public class CachingList<T> : ObservableList<T>, ICachingList<T>
{
    public int Limit
    {
        get => _limit;
        set
        {
            _limit = Math.Clamp(value, 0, int.MaxValue);
            TrimStartDownTo(_limit);
        }
    }
    private int _limit = int.MaxValue;

    public CachingList()
    {
        CollectionChanged += OnCollectionChanged;
    }

    public CachingList(params T[] items) : base(items)
    {
        CollectionChanged += OnCollectionChanged;
    }

    public CachingList(IEnumerable<T> collection) : base(collection)
    {
        CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, CollectionChangeEventArgs<T> args) => TrimStartDownTo(_limit);
}