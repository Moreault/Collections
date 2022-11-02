namespace ToolBX.Collections.Grid;

/// <summary>
/// An observable, dynamic two-dimensional array.
/// </summary>
public interface IGrid<T> : IEnumerable<Cell<T>>, IEquatable<IGrid<T>>, IEquatable<T[,]>, IEquatable<IEnumerable<KeyValuePair<Vector2<int>, T>>>, IEquatable<T[][]>, IEquatable<IEnumerable<Cell<T>>>
{
    int ColumnCount { get; }
    int RowCount { get; }

    int FirstColumn { get; }
    int LastColumn { get; }

    int FirstRow { get; }
    int LastRow { get; }

    /// <summary>
    /// Total number of cells in use.
    /// </summary>
    int Count { get; }
    T? this[int columnIndex, int rowIndex] { get; set; }
    T? this[Vector2<int> index] { get; set; }

    Boundaries<int> Boundaries { get; }

    IReadOnlyList<Vector2<int>> IndexesOf(T? item);
    IReadOnlyList<Vector2<int>> IndexesOf(Func<T, bool> match);

    void Add(int x, int y, T? item);
    void TryAdd(int x, int y, T? item);
    void Add(Vector2<int> index, T? item);
    void TryAdd(Vector2<int> index, T? item);

    void Add(params Cell<T>[] cells);
    void Add(IEnumerable<Cell<T>> cells);

    void TryAdd(params Cell<T>[] cells);
    void TryAdd(IEnumerable<Cell<T>> cells);

    void RemoveAt(int x, int y);
    void TryRemoveAt(int x, int y);
    void RemoveAt(Vector2<int> index);
    void TryRemoveAt(Vector2<int> index);
    void RemoveAll(T? item);
    void RemoveAll(Func<T, bool> match);

    bool Contains(Vector2<int> index, T? item);
    bool Contains(int x, int y, T? item);
    bool Contains(T? item);
    bool Contains(int x, int y);
    bool Contains(Vector2<int> index);

    void FloodFill(int x, int y, T? newValue);
    void FloodFill(int x, int y, T? newValue, Boundaries<int> boundaries);
    void FloodFill(Vector2<int> index, T? newValue);
    void FloodFill(Vector2<int> index, T? newValue, Boundaries<int> boundaries);

    void FloodClear(int x, int y);
    void FloodClear(int x, int y, Boundaries<int> boundaries);
    void FloodClear(Vector2<int> index);
    void FloodClear(Vector2<int> index, Boundaries<int> boundaries);

    void Resize(Boundaries<int> boundaries);

    /// <summary>
    /// Translates the entire grid in the specified direction.
    /// </summary>
    void TranslateAll(int x, int y);

    /// <summary>
    /// Translates the entire grid in the specified direction.
    /// </summary>
    void TranslateAll(Vector2<int> translation);

    /// <summary>
    /// Translates part of the grid in the specified direction.
    /// </summary>
    void Translate(Rectangle<int> range, int x, int y);

    /// <summary>
    /// Translates part of the grid in the specified direction.
    /// </summary>
    void Translate(Rectangle<int> range, Vector2<int> translation);

    /// <summary>
    /// Translates part of the grid in the specified direction.
    /// </summary>
    void Translate(Boundaries<int> boundaries, int x, int y);

    /// <summary>
    /// Translates part of the grid in the specified direction.
    /// </summary>
    void Translate(Boundaries<int> boundaries, Vector2<int> translation);

    /// <summary>
    /// Returns an exact copy of the Grid.
    /// </summary>
    IGrid<T> Copy();

    /// <summary>
    /// Returns an exact copy of the Grid within the specified boundaries.
    /// </summary>
    IGrid<T> Copy(Boundaries<int> boundaries);

    void Swap(Vector2<int> current, Vector2<int> destination);

    event GridChangedEvent<T> CollectionChanged;
}

/// <inheritdoc cref="IGrid{T}"/>
public class Grid<T> : IGrid<T>
{
    private readonly IDictionary<Vector2<int>, T?> _items = new Dictionary<Vector2<int>, T?>();

    public T? this[int columnIndex, int rowIndex]
    {
        get => this[new Vector2<int>(columnIndex, rowIndex)];
        set => this[new Vector2<int>(columnIndex, rowIndex)] = value;
    }

    public T? this[Vector2<int> index]
    {
        get
        {
            _items.TryGetValue(index, out var value);
            return value;
        }
        set
        {
            var oldItem = this[index];
            _items[index] = value;

            if (!Equals(oldItem, value))
                CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
                {
                    OldValues = oldItem == null ? Array.Empty<Cell<T>>() : new Cell<T>[] { new() { Index = index, Value = oldItem } },
                    NewValues = value == null ? Array.Empty<Cell<T>>() : new Cell<T>[] { new() { Index = index, Value = value } },
                });
        }
    }

    public int ColumnCount { get; private set; }

    public int RowCount { get; private set; }

    public int FirstColumn { get; private set; }

    public int LastColumn { get; private set; }

    public int FirstRow { get; private set; }

    public int LastRow { get; private set; }

    public int Count => _items.Count;

    public Grid()
    {
        CollectionChanged += OnCollectionChanged;
    }

    public Grid(IEnumerable<Cell<T>> cells) : this()
    {
        if (cells == null) throw new ArgumentNullException(nameof(cells));
        foreach (var (index, value) in cells)
            this[index] = value;
    }

    public Grid(IEnumerable<KeyValuePair<Vector2<int>, T>> collection) : this()
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        foreach (var (key, value) in collection)
            this[key] = value;

    }

    public Grid(T[,] collection) : this()
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        for (var x = 0; x < collection.GetLength(0); x++)
        {
            for (var y = 0; y < collection.GetLength(1); y++)
            {
                this[new Vector2<int>(x, y)] = collection[x, y];
            }
        }

    }

    public Grid(T[][] collection) : this()
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        for (var y = 0; y < collection.Length; y++)
        {
            for (var x = 0; x < collection[y].Length; x++)
            {
                this[new Vector2<int>(x, y)] = collection[x][y];
            }
        }
    }

    private void OnCollectionChanged(object sender, GridChangedEventArgs<T> args)
    {
        if (_items.Any())
        {
            ColumnCount = (_items.Keys.Any(x => x.X < 0) ? Math.Abs(_items.Keys.Min(x => x.X)) : 0) +
                          (_items.Keys.Any(x => x.X > 0) ? _items.Keys.Max(x => x.X) : 0) + 1;

            RowCount = (_items.Keys.Any(x => x.Y < 0) ? Math.Abs(_items.Keys.Min(x => x.Y)) : 0) +
                       (_items.Keys.Any(x => x.Y > 0) ? _items.Keys.Max(x => x.Y) : 0) + 1;

            FirstColumn = _items.Keys.Min(x => x.X);
            LastColumn = _items.Keys.Max(x => x.X);

            FirstRow = _items.Keys.Min(x => x.Y);
            LastRow = _items.Keys.Max(x => x.Y);
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

    public Boundaries<int> Boundaries => _items.Any() ? new Boundaries<int>
    {
        Top = _items.Keys.Min(x => x.Y),
        Right = _items.Keys.Max(x => x.X),
        Bottom = _items.Keys.Max(x => x.Y),
        Left = _items.Keys.Min(x => x.X)
    } : new Boundaries<int>();

    public IReadOnlyList<Vector2<int>> IndexesOf(T? item) => _items.Where(x => x.Value == null && item == null || x.Value != null && x.Value.Equals(item)).Select(x => x.Key).ToList();

    public IReadOnlyList<Vector2<int>> IndexesOf(Func<T, bool> match)
    {
        if (match == null) throw new ArgumentNullException(nameof(match));
        return _items.Where(x => match.Invoke(x.Value!)).Select(x => x.Key).ToList();
    }

    public void Add(int x, int y, T? item) => Add(new Vector2<int>(x, y), item);

    public void TryAdd(int x, int y, T? item) => TryAdd(new Vector2<int>(x, y), item);

    public void Add(Vector2<int> index, T? item)
    {
        if (_items.ContainsKey(index))
            throw new InvalidOperationException(string.Format(Exceptions.CannotAddItemAtIndexBecauseItIsTaken, index));
        this[index] = item;
    }

    public void TryAdd(Vector2<int> index, T? item)
    {
        try
        {
            Add(index, item);
        }
        catch
        {
            // ignored
        }
    }

    public void Add(params Cell<T>[] cells) => Add(cells as IEnumerable<Cell<T>>);

    public void Add(IEnumerable<Cell<T>> cells)
    {
        if (cells == null) throw new ArgumentNullException(nameof(cells));

        var cellsList = cells as IReadOnlyList<Cell<T>> ?? cells.ToList();

        //TODO Message
        if (cellsList.Any(x => Contains(x.Index)))
            throw new InvalidOperationException();

        foreach (var cell in cellsList)
            _items[cell.Index] = cell.Value;

        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            NewValues = cellsList.ToList()
        });
    }

    public void TryAdd(params Cell<T>[] cells) => TryAdd(cells as IEnumerable<Cell<T>>);

    public void TryAdd(IEnumerable<Cell<T>> cells)
    {
        var cellsList = cells?.ToList() ?? new List<Cell<T>>();
        cellsList = cellsList.Where(x => !Contains(x.Index)).ToList();
        if (!cellsList.Any()) return;
        Add(cellsList);
    }

    public void RemoveAt(int x, int y) => RemoveAt(new Vector2<int>(x, y));

    public void TryRemoveAt(int x, int y) => TryRemoveAt(new Vector2<int>(x, y));

    public void RemoveAt(Vector2<int> index)
    {
        var isIndexFound = _items.TryGetValue(index, out var item);
        if (!isIndexFound) throw new ArgumentOutOfRangeException(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));
        _items.Remove(index);
        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            OldValues = new Cell<T>[] { new() { Index = index, Value = item } }
        });
    }

    public void TryRemoveAt(Vector2<int> index)
    {
        try
        {
            RemoveAt(index);
        }
        catch
        {
            // ignored
        }
    }

    public void RemoveAll(T? item)
    {
        var indexesOf = IndexesOf(item);
        var cells = indexesOf.Select(x => new Cell<T> { Index = x, Value = _items[x] }).ToArray();

        foreach (var pair in cells)
            _items.Remove(pair.Index);

        if (cells.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                OldValues = cells
            });
    }

    public void RemoveAll(Func<T, bool> match)
    {
        if (match == null) throw new ArgumentNullException(nameof(match));
        var indexesOf = IndexesOf(match);
        var cells = indexesOf.Select(x => new Cell<T> { Index = x, Value = _items[x] }).ToArray();

        foreach (var pair in cells)
            _items.Remove(pair.Index);

        if (cells.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
            {
                OldValues = cells
            });
    }

    public bool Contains(Vector2<int> index, T? item)
    {
        var itemAtIndex = this[index];
        return itemAtIndex == null && item == null || itemAtIndex != null && itemAtIndex.Equals(item);
    }

    public bool Contains(int x, int y, T? item) => Contains(new Vector2<int>(x, y), item);

    public bool Contains(T? item) => _items.Values.Any(x => x is null && item is null || x != null && x.Equals(item));

    public bool Contains(int x, int y) => Contains(new Vector2<int>(x, y));

    public bool Contains(Vector2<int> index) => _items.ContainsKey(index);

    public void FloodFill(int x, int y, T? newValue) => FloodFill(new Vector2<int>(x, y), newValue);

    public void FloodFill(int x, int y, T? newValue, Boundaries<int> boundaries) => FloodFill(new Vector2<int>(x, y), newValue, boundaries);

    public void FloodFill(Vector2<int> index, T? newValue) => FloodFill(index, newValue, Boundaries);

    //TODO Throw if they try to flood outside boundaries? Add TryFloodFill to remain consistent?
    public void FloodFill(Vector2<int> index, T? newValue, Boundaries<int> boundaries)
    {
        var before = CollectionChanged == null ? null : _items.ToDictionary(x => x.Key, x => x.Value);
        FloodFill(index, this[index], newValue, boundaries);

        if (CollectionChanged != null)
        {
            var oldValues = new List<Cell<T>>();
            var newValues = new List<Cell<T>>();

            if (before == null || !before.Any())
            {
                newValues.AddRange(_items.Select(item => new Cell<T>(item.Key, item.Value)));
            }
            else
            {
                foreach (var (key, oldValue) in before.Where(x => !Equals(this[x.Key], x.Value)))
                {
                    oldValues.Add(new Cell<T>(key, oldValue));
                    newValues.Add(new Cell<T>(key, this[key]));
                }
            }

            if (oldValues.Any() || newValues.Any())
                CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
                {
                    NewValues = newValues,
                    OldValues = oldValues
                });
        }
    }

    private void FloodFill(Vector2<int> index, T? replacedValue, T? newValue, Boundaries<int> boundaries)
    {
        if (index.X < boundaries.Left || index.Y < boundaries.Top ||
            index.X > boundaries.Right || index.Y > boundaries.Bottom || Equals(this[index], newValue) || !Equals(this[index], replacedValue)) return;

        _items[index] = newValue;

        FloodFill(new Vector2<int>(index.X + 1, index.Y), replacedValue, newValue, boundaries);
        FloodFill(new Vector2<int>(index.X - 1, index.Y), replacedValue, newValue, boundaries);
        FloodFill(new Vector2<int>(index.X, index.Y + 1), replacedValue, newValue, boundaries);
        FloodFill(new Vector2<int>(index.X, index.Y - 1), replacedValue, newValue, boundaries);
    }

    public void FloodClear(int x, int y) => FloodClear(new Vector2<int>(x, y));

    public void FloodClear(Vector2<int> index) => FloodClear(index, Boundaries);

    public void FloodClear(int x, int y, Boundaries<int> boundaries) => FloodClear(new Vector2<int>(x, y), boundaries);

    public void FloodClear(Vector2<int> index, Boundaries<int> boundaries)
    {
        var before = CollectionChanged == null ? null : _items.ToDictionary(x => x.Key, x => x.Value);
        if (Contains(index))
            FloodClear(index, this[index], boundaries);

        if (CollectionChanged != null)
        {
            var oldValues = new List<Cell<T>>();

            if (before != null && before.Any())
            {
                foreach (var (key, oldValue) in before.Where(x => !Equals(this[x.Key], x.Value)))
                {
                    oldValues.Add(new Cell<T>(key, oldValue));
                }
            }

            if (oldValues.Any())
                CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
                {
                    OldValues = oldValues
                });
        }
    }

    private void FloodClear(Vector2<int> index, T? deletedValue, Boundaries<int> boundaries)
    {
        if (index.X < boundaries.Left || index.Y < boundaries.Top ||
            index.X > boundaries.Right || index.Y > boundaries.Bottom || !Equals(this[index], deletedValue)) return;

        _items.Remove(index);

        FloodClear(new Vector2<int>(index.X + 1, index.Y), deletedValue, boundaries);
        FloodClear(new Vector2<int>(index.X - 1, index.Y), deletedValue, boundaries);
        FloodClear(new Vector2<int>(index.X, index.Y + 1), deletedValue, boundaries);
        FloodClear(new Vector2<int>(index.X, index.Y - 1), deletedValue, boundaries);
    }

    public void Resize(Boundaries<int> boundaries)
    {
        var original = _items.ToDictionary(x => x.Key, x => x.Value);

        foreach (var item in original.Where(x => x.Key.X < boundaries.Left || x.Key.X > boundaries.Right || x.Key.Y < boundaries.Top || x.Key.Y > boundaries.Bottom))
        {
            _items.Remove(item.Key);
        }

        if (CollectionChanged != null)
        {
            var oldValues = new List<Cell<T>>();
            foreach (var (index, value) in original.Where(x => !Contains(x.Key)))
                oldValues.Add(new Cell<T>(index, value));

            if (oldValues.Any())
                CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
                {
                    OldValues = oldValues
                });
        }
    }

    public void TranslateAll(int x, int y) => TranslateAll(new Vector2<int>(x, y));

    public void TranslateAll(Vector2<int> translation)
    {
        if (translation == new Vector2<int>(0, 0) || !_items.Any()) return;
        var originalItems = _items.ToDictionary(x => x.Key, x => x.Value);

        _items.Clear();
        var oldItems = new List<Cell<T>>();
        var newItems = new List<Cell<T>>();
        foreach (var (key, value) in originalItems)
        {
            oldItems.Add(new Cell<T> { Index = key, Value = value });
            newItems.Add(new Cell<T> { Index = key + translation, Value = value });

            _items.Add(key + translation, value);
        }

        if (oldItems.Any() || newItems.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T> { OldValues = oldItems, NewValues = newItems });
    }

    public void Translate(Rectangle<int> range, int x, int y) => Translate(range, new Vector2<int>(x, y));

    public void Translate(Rectangle<int> range, Vector2<int> translation) => Translate(new Boundaries<int>(range.Top, range.Right, range.Bottom, range.Left), translation);

    public void Translate(Boundaries<int> boundaries, int x, int y) => Translate(boundaries, new Vector2<int>(x, y));

    public void Translate(Boundaries<int> boundaries, Vector2<int> translation)
    {
        if (boundaries.Left == boundaries.Right || boundaries.Top == boundaries.Bottom || translation == new Vector2<int>(0, 0)) return;
        var originalItems = _items.ToDictionary(x => x.Key, x => x.Value);

        var oldItems = new List<Cell<T>>();
        var newItems = new List<Cell<T>>();

        foreach (var item in originalItems)
        {
            if (item.Key.X >= boundaries.Left && item.Key.X < boundaries.Right && item.Key.Y >= boundaries.Top && item.Key.Y < boundaries.Bottom)
            {
                var newIndex = item.Key + translation;

                if (CollectionChanged != null)
                {
                    oldItems.Add(new Cell<T>(item.Key, item.Value));
                    if (_items.ContainsKey(newIndex)) oldItems.Add(new Cell<T>(newIndex, _items[newIndex]));
                    newItems.Add(new Cell<T>(newIndex, item.Value));
                }
                _items.Remove(item.Key);
                _items[newIndex] = item.Value;
            }
        }

        if (oldItems.Any() || newItems.Any())
            CollectionChanged?.Invoke(this, new GridChangedEventArgs<T> { OldValues = oldItems, NewValues = newItems });
    }
    //TODO Return Grid<T>
    public IGrid<T> Copy() => Copy(Boundaries);

    public IGrid<T> Copy(Boundaries<int> boundaries)
    {
        var copy = new Grid<T>();
        foreach (var (key, value) in _items.Where(x => x.Key.X >= boundaries.Left && x.Key.X <= boundaries.Right && x.Key.Y >= boundaries.Top && x.Key.Y <= boundaries.Bottom))
        {
            copy[key] = value;
        }
        return copy;
    }

    public void Swap(Vector2<int> current, Vector2<int> destination)
    {
        if (current == destination) return;

        var firstItem = _items[current];
        var secondItem = _items[destination];

        _items[current] = secondItem;
        _items[destination] = firstItem;

        CollectionChanged?.Invoke(this, new GridChangedEventArgs<T>
        {
            NewValues = new List<Cell<T>> { new(destination, firstItem), new(current, secondItem) },
            OldValues = new List<Cell<T>> { new(destination, secondItem), new(current, firstItem) }
        });
    }

    public event GridChangedEvent<T>? CollectionChanged;

    public IEnumerator<Cell<T>> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private readonly struct Enumerator : IEnumerator<Cell<T>>
    {
        private readonly IEnumerator<KeyValuePair<Vector2<int>, T>> _parentEnumerator;

        public Cell<T> Current
        {
            get
            {
                var (key, value) = _parentEnumerator.Current;
                return new Cell<T>(key, value);
            }
        }

        object IEnumerator.Current => Current;

        public Enumerator(Grid<T> grid)
        {
            _parentEnumerator = grid._items.GetEnumerator()!;
        }

        public bool MoveNext() => _parentEnumerator.MoveNext();

        public void Reset() => _parentEnumerator.Reset();

        public void Dispose() => _parentEnumerator.Dispose();
    }

    public static bool operator ==(Grid<T>? a, Grid<T>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Grid<T>? a, Grid<T>? b) => !(a == b);

    public static bool operator ==(Grid<T>? a, T[,]? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Grid<T>? a, T[,]? b) => !(a == b);

    public static bool operator ==(Grid<T>? a, T[][]? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Grid<T>? a, T[][]? b) => !(a == b);

    public static bool operator ==(Grid<T>? a, IEnumerable<Cell<T>>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Grid<T>? a, IEnumerable<Cell<T>>? b) => !(a == b);

    public static bool operator ==(Grid<T>? a, IEnumerable<KeyValuePair<Vector2<int>, T>>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(Grid<T>? a, IEnumerable<KeyValuePair<Vector2<int>, T>>? b) => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is IGrid<T> grid) return Equals(grid);
        if (obj is T[,] array) return Equals(array);
        if (obj is T[][] jagged) return Equals(jagged);
        if (obj is IEnumerable<Cell<T>> cells) return Equals(cells);
        if (obj is IEnumerable<KeyValuePair<Vector2<int>, T>> keyValuePairs) return Equals(keyValuePairs);
        return false;
    }

    public bool Equals(IEnumerable<Cell<T>>? other)
    {
        if (ReferenceEquals(other, null)) return false;
        return this.SequenceEqual(other);
    }

    public bool Equals(IGrid<T>? other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.SequenceEqual(other);
    }

    //TODO There has to be a better way than to compare both like this (comparing lengths or one-way is not enough since arrays tend to declare more than they need to avoid having to expand often)
    public bool Equals(T[,]? other)
    {
        if (ReferenceEquals(other, null)) return false;

        for (var x = 0; x < ColumnCount; x++)
        {
            if (x > other.GetLength(0) - 1) return false;
            for (var y = 0; y < RowCount; y++)
            {
                if (y > other.GetLength(1) - 1 || !Equals(other[x, y], this[x, y]))
                {
                    return false;
                }
            }
        }

        for (var x = 0; x < other.GetLength(0); x++)
        {
            if (x > ColumnCount - 1) return false;
            for (var y = 0; y < other.GetLength(1); y++)
            {
                if (y > RowCount - 1 || !Equals(other[x, y], this[x, y]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //TODO There has to be a better way than to compare both like this (comparing lengths or one-way is not enough since arrays tend to declare more than they need to avoid having to expand often)
    public bool Equals(T[][]? other)
    {
        if (ReferenceEquals(other, null)) return false;

        for (var x = 0; x < ColumnCount; x++)
        {
            if (x > other.Length - 1) return false;

            for (var y = 0; y < RowCount; y++)
            {
                if (y > other[x].Length - 1 || !Equals(other[x][y], this[x, y]))
                {
                    return false;
                }
            }
        }

        for (var x = 0; x < other.Length; x++)
        {
            if (x > ColumnCount - 1) return false;

            for (var y = 0; y < other[x].Length; y++)
            {
                if (y > RowCount - 1 || !Equals(other[x][y], this[x, y]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool Equals(IEnumerable<KeyValuePair<Vector2<int>, T>>? other)
    {
        if (ReferenceEquals(other, null)) return false;
        return this.SequenceEqual(other.Select(x => new Cell<T>(x.Key, x.Value)));
    }

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => _items.Any() ? $"{GetType().GetHumanReadableName()} with {Count} items" : $"Empty {GetType().GetHumanReadableName()}";
}