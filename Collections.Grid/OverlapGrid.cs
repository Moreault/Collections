using ToolBX.OPEX;

namespace ToolBX.Collections.Grid;

/// <summary>
/// A grid with overlapping elements.
/// </summary>
public class OverlapGrid<T> : IEnumerable<Cell<T>>, IEquatable<OverlapGrid<T>>, IEquatable<IEnumerable<Cell<T>>>
{
    private readonly List<Cell<T>> _items = new();

    public IReadOnlyList<T> this[int columnIndex, int rowIndex] => this[new Vector2<int>(columnIndex, rowIndex)];

    public IReadOnlyList<T> this[Vector2<int> index] => _items.Where(x => x.Index == index && x.Value != null).Select(x => x.Value!).ToArray();

    public int ColumnCount { get; private set; }

    public int RowCount { get; private set; }

    public int FirstColumn { get; private set; }

    public int LastColumn { get; private set; }

    public int FirstRow { get; private set; }

    public int LastRow { get; private set; }

    public int Count => _items.Count;

    public event GridChangedEvent<T>? CollectionChanged;

    public OverlapGrid()
    {
        CollectionChanged += OnCollectionChanged;
    }

    public OverlapGrid(IEnumerable<Cell<T>> collection) : this()
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        _items.AddRange(collection);
    }

    private void OnCollectionChanged(object sender, GridChangedEventArgs<T> args)
    {
        //TODO Apply this new logic to regular Grid<T>
        if (_items.Any())
        {
            FirstColumn = _items.Min(x => x.X);
            LastColumn = _items.Max(x => x.X);

            FirstRow = _items.Min(x => x.Y);
            LastRow = _items.Max(x => x.Y);

            ColumnCount = LastColumn - FirstColumn + 1;

            RowCount = LastRow - FirstRow + 1;
        }
        else
        {
            ColumnCount = 0;
            RowCount = 0;

            FirstColumn = 0;
            LastColumn = 0;
            FirstRow = 0;
            LastRow = 0;
        }
    }

    public Boundaries<int> Boundaries => new()
    {
        Top = FirstRow,
        Right = LastColumn,
        Bottom = LastRow,
        Left = FirstColumn
    };

    public IReadOnlyList<Vector2<int>> IndexesOf(T? item) => IndexesOf(x => Equals(x, item));

    public IReadOnlyList<Vector2<int>> IndexesOf(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return _items.Where(x => predicate(x.Value!)).Select(x => x.Index).ToArray();
    }

    public void Add(int x, int y, T? item) => Add(new Cell<T>(x, y, item));

    public void Add(Vector2<int> index, T? item) => Add(new Cell<T>(index, item));

    public void Add(params Cell<T>[] cells) => Add(cells as IEnumerable<Cell<T>>);

    public void Add(IEnumerable<Cell<T>> cells)
    {
        if (cells == null) throw new ArgumentNullException(nameof(cells));

        var list = cells as IList<Cell<T>> ?? cells.ToArray();
        _items.AddRange(list);
        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            NewValues = list
        });
    }

    public void RemoveAt(int x, int y) => RemoveAt(new Vector2<int>(x, y));

    public void RemoveAt(Vector2<int> index)
    {
        var indexesOf = _items.IndexesOf(x => x.Index == index);
        if (!indexesOf.Any()) throw new ArgumentOutOfRangeException(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));

        var items = indexesOf.Select(x => _items[x]).ToArray();

        foreach (var indexOf in indexesOf.OrderByDescending(x => x))
            _items.RemoveAt(indexOf);

        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            OldValues = items
        });
    }

    public void TryRemoveAt(int x, int y) => TryRemoveAt(new Vector2<int>(x, y));

    public void TryRemoveAt(Vector2<int> index)
    {
        try
        {
            RemoveAt(index);
        }
        catch
        {
            //ignored
        }
    }

    public void RemoveAll(T? item) => RemoveAll(x => Equals(x, item));

    public void RemoveAll(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var indexesOf = new List<int>();
        for (var i = 0; i < _items.Count; i++)
        {
            if (predicate(_items[i].Value!))
                indexesOf.Add(i);
        }

        var items = new List<Cell<T>>();

        foreach (var index in indexesOf.OrderByDescending(x => x))
        {
            items.Add(_items[index]);
            _items.RemoveAt(index);
        }

        if (items.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                OldValues = items
            });
    }

    public bool Contains(int x, int y, T? item) => Contains(new Vector2<int>(x, y), item);

    public bool Contains(Vector2<int> index, T? item) => _items.Any(x => x.Index == index && Equals(x.Value, item));

    public bool Contains(T? item) => _items.Any(x => Equals(x.Value, item));

    public bool Contains(int x, int y) => Contains(new Vector2<int>(x, y));

    public bool Contains(Vector2<int> index) => _items.Any(x => x.Index == index);

    public void Resize(Boundaries<int> boundaries)
    {
        var indexesOfRemoved = _items.IndexesOf(x => x.X < boundaries.Left || x.X > boundaries.Right || x.Y < boundaries.Top || x.Y > boundaries.Bottom).OrderByDescending(x => x);

        var oldValues = new List<Cell<T>>();
        foreach (var index in indexesOfRemoved)
        {
            oldValues.Add(_items[index]);
            _items.RemoveAt(index);
        }

        if (oldValues.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                OldValues = oldValues
            });
    }

    public void TranslateAll(int x, int y) => TranslateAll(new Vector2<int>(x, y));

    public void TranslateAll(Vector2<int> translation)
    {
        if (translation == new Vector2<int>(0, 0) || !_items.Any()) return;

        var original = _items.ToArray();
        _items.Clear();
        _items.AddRange(original.Select(x => x with { Index = x.Index + translation }));

        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T> { OldValues = original, NewValues = _items.ToArray() });
    }

    public void Translate(Rectangle<int> range, int x, int y) => Translate(range, new Vector2<int>(x, y));

    public void Translate(Rectangle<int> range, Vector2<int> translation) => Translate(new Boundaries<int>(range.Top, range.Right, range.Bottom, range.Left), translation);

    public void Translate(Boundaries<int> boundaries, int x, int y) => Translate(boundaries, new Vector2<int>(x, y));

    public void Translate(Boundaries<int> boundaries, Vector2<int> translation)
    {
        if (boundaries.Left == boundaries.Right || boundaries.Top == boundaries.Bottom || translation == new Vector2<int>(0, 0) || !_items.Any()) return;

        var oldItems = new List<Cell<T>>();
        var newItems = new List<Cell<T>>();

        foreach (var item in _items.Where(x => x.X >= boundaries.Left && x.X <= boundaries.Right && x.Y >= boundaries.Top && x.Y <= boundaries.Bottom).ToArray())
        {
            oldItems.Add(item);
            var newItem = item with { Index = item.Index + translation };
            _items.Remove(item);
            _items.Add(newItem);
            newItems.Add(newItem);
        }

        if (oldItems.Any() || newItems.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                NewValues = newItems,
                OldValues = oldItems,
            });
    }

    public OverlapGrid<T> Copy() => Copy(Boundaries);

    public OverlapGrid<T> Copy(Boundaries<int> boundaries)
    {
        var items = _items.Where(x => x.X >= boundaries.Left && x.X <= boundaries.Right && x.Y >= boundaries.Top && x.Y <= boundaries.Bottom).ToList();
        return new OverlapGrid<T>(items);
    }

    public void Swap(Vector2<int> current, Vector2<int> destination)
    {
        if (current == destination) return;

        var firstIndexes = _items.IndexesOf(x => x.Index == current);
        var secondIndexes = _items.IndexesOf(x => x.Index == destination);

        var oldValues = new List<Cell<T>>();
        var newValues = new List<Cell<T>>();
        foreach (var index in firstIndexes)
        {
            oldValues.Add(_items[index]);
            _items[index] = _items[index] with { Index = destination };
            newValues.Add(_items[index]);
        }

        foreach (var index in secondIndexes)
        {
            oldValues.Add(_items[index]);
            _items[index] = _items[index] with { Index = current };
            newValues.Add(_items[index]);
        }

        if (newValues.Any() || oldValues.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                OldValues = oldValues,
                NewValues = newValues
            });
    }

    public void Clear()
    {
        if (!_items.Any()) return;

        var oldItems = _items.ToArray();
        _items.Clear();
        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            OldValues = oldItems
        });
    }

    public IEnumerator<Cell<T>> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Equals(OverlapGrid<T>? other) => Equals(other as IEnumerable<Cell<T>>);

    public bool Equals(IEnumerable<Cell<T>>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    public override bool Equals(object? obj) => Equals(obj as IEnumerable<Cell<T>>);

    public static bool operator ==(OverlapGrid<T>? a, OverlapGrid<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(OverlapGrid<T>? a, OverlapGrid<T>? b) => !(a == b);

    public static bool operator ==(OverlapGrid<T>? a, IEnumerable<Cell<T>>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(OverlapGrid<T>? a, IEnumerable<Cell<T>>? b) => !(a == b);

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => _items.Any() ? $"{GetType().GetHumanReadableName()} with {Count} items" : $"Empty {GetType().GetHumanReadableName()}";
}