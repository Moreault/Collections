namespace ToolBX.Collections.ObservableDictionary
{
    public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IObservableCollection<KeyValuePair<TKey, TValue>>, IEquatable<IObservableDictionary<TKey, TValue>> where TKey : notnull
    {
        Result<TValue> TryGetValue(TKey key);
        void Add(params KeyValuePair<TKey, TValue>[] items);
        void Add(IEnumerable<KeyValuePair<TKey, TValue>> items);
        void Remove(params KeyValuePair<TKey, TValue>[] items);
        void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items);
        new void Remove(KeyValuePair<TKey, TValue> item);
        void Remove(params TKey[] keys);
        void Remove(IEnumerable<TKey> keys);
        new void Remove(TKey key);
        void Remove(Func<KeyValuePair<TKey, TValue>, bool> match);
        void Remove(Func<TKey, bool> match);
        void Remove(Func<TValue, bool> match);
        void TryRemove(params KeyValuePair<TKey, TValue>[] items);
        void TryRemove(IEnumerable<KeyValuePair<TKey, TValue>> items);
        void TryRemove(KeyValuePair<TKey, TValue> item);
        void TryRemove(params TKey[] keys);
        void TryRemove(IEnumerable<TKey> keys);
        void TryRemove(TKey key);
        void TryRemove(Func<KeyValuePair<TKey, TValue>, bool> match);
        void TryRemove(Func<TKey, bool> match);
        void TryRemove(Func<TValue, bool> match);
        ObservableDictionary<TKey, TValue> Copy();
    }

    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>, IEquatable<ObservableDictionary<TKey, TValue>> where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> _items;

        public ICollection<TKey> Keys => _items.Keys;
        public ICollection<TValue> Values => _items.Values;

        public TValue this[TKey key]
        {
            get => _items[key];
            set
            {
                if (_items.ContainsKey(key))
                {
                    var oldValue = _items[key];
                    if (!Equals(oldValue, value))
                    {
                        _items[key] = value;
                        CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
                        {
                            OldValues = new List<KeyValuePair<TKey, TValue>> { new(key, oldValue) },
                            NewValues = new List<KeyValuePair<TKey, TValue>> { new(key, value) }
                        });
                    }
                }
                else
                    Add(key, value);
            }
        }

        public event CollectionChangeEventHandler<KeyValuePair<TKey, TValue>>? CollectionChanged;

        public int Count => _items.Count;
        public bool IsReadOnly => _items.IsReadOnly;

        public ObservableDictionary()
        {
            _items = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(int capacity)
        {
            _items = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, TValue>(comparer);
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _items = new Dictionary<TKey, TValue>(dictionary);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            _items = new Dictionary<TKey, TValue>(collection);
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
        {
            _items = new Dictionary<TKey, TValue>(collection, comparer);
        }

        public Result<TValue> TryGetValue(TKey key)
        {
            var isSuccess = _items.TryGetValue(key, out var value);
            return isSuccess ? Result<TValue>.Success(value!) : Result<TValue>.Failure();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _items.Add(item);
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                NewValues = new List<KeyValuePair<TKey, TValue>> { item }
            });
        }

        public void Add(params KeyValuePair<TKey, TValue>[] items) => Add(items as IEnumerable<KeyValuePair<TKey, TValue>>);

        public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var list = items as IList<KeyValuePair<TKey, TValue>> ?? items.ToList();
            if (!list.Any()) return;

            foreach (var item in list)
                _items.Add(item);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                NewValues = list as IReadOnlyList<KeyValuePair<TKey, TValue>> ?? list.ToList()
            });
        }

        public void Clear()
        {
            if (_items.Any())
            {
                IReadOnlyList<KeyValuePair<TKey, TValue>> oldItems = CollectionChanged == null ? Array.Empty<KeyValuePair<TKey, TValue>>() : _items.ToList();
                _items.Clear();
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> { OldValues = oldItems });
            }
        }

        public void Remove(params KeyValuePair<TKey, TValue>[] items) => Remove(items as IEnumerable<KeyValuePair<TKey, TValue>>);

        public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var list = items.ToList();
            if (!_items.All(list.Contains)) throw new InvalidOperationException(Exceptions.RemoveAtLeastOneInexistantItem);

            foreach (var item in list)
                _items.Remove(item);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = list
            });
        }

        public void Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item)) throw new InvalidOperationException(string.Format(Exceptions.RemoveInexistantKeyValue, item.Key, item.Value));
            _items.Remove(item);
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = new List<KeyValuePair<TKey, TValue>> { item }
            });
        }

        public void Remove(params TKey[] keys) => Remove(keys as IEnumerable<TKey>);

        public void Remove(IEnumerable<TKey> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            var items = new List<KeyValuePair<TKey, TValue>>();
            foreach (var key in keys)
            {
                if (!ContainsKey(key)) throw new InvalidOperationException(Exceptions.RemoveAtLeastOneInexistantKey);
                items.Add(new KeyValuePair<TKey, TValue>(key, _items[key]));
            }

            Remove(items);
        }

        public void Remove(TKey key)
        {
            var item = TryGetValue(key);
            if (!item.IsSuccess) throw new InvalidOperationException(string.Format(Exceptions.RemoveInexistantKey, key));
            _items.Remove(key);
            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = new List<KeyValuePair<TKey, TValue>> { new(key, item.Value!) }
            });
        }

        public void Remove(Func<KeyValuePair<TKey, TValue>, bool> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            var removed = new List<KeyValuePair<TKey, TValue>>();

            foreach (var item in _items.Where(match).ToList())
            {
                removed.Add(item);
                _items.Remove(item.Key);
            }

            if (!removed.Any())
                throw new InvalidOperationException(Exceptions.RemoveWithNonInexistantPredicate);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = removed
            });
        }

        public void Remove(Func<TKey, bool> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            var removed = new List<KeyValuePair<TKey, TValue>>();

            foreach (var item in _items.Where(x => match(x.Key)).ToList())
            {
                removed.Add(item);
                _items.Remove(item.Key);
            }

            if (!removed.Any())
                throw new InvalidOperationException(Exceptions.RemoveWithNonInexistantPredicate);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = removed
            });
        }

        public void Remove(Func<TValue, bool> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            var removed = new List<KeyValuePair<TKey, TValue>>();

            foreach (var item in _items.Where(x => match(x.Value)).ToList())
            {
                removed.Add(item);
                _items.Remove(item.Key);
            }

            if (!removed.Any())
                throw new InvalidOperationException(Exceptions.RemoveWithNonInexistantPredicate);

            CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
            {
                OldValues = removed
            });
        }

        public void TryRemove(params KeyValuePair<TKey, TValue>[] items) => TryRemove(items as IEnumerable<KeyValuePair<TKey, TValue>>);

        public void TryRemove(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            try
            {
                Remove(items);
            }
            catch (InvalidOperationException)
            {
                //ignore
            }
        }

        public void TryRemove(KeyValuePair<TKey, TValue> item)
        {
            var wasRemoved = _items.Remove(item);
            if (wasRemoved && CollectionChanged != null)
                CollectionChanged(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
                {
                    OldValues = new List<KeyValuePair<TKey, TValue>> { item }
                });
        }

        public void TryRemove(params TKey[] keys) => TryRemove(keys as IEnumerable<TKey>);

        public void TryRemove(IEnumerable<TKey> keys)
        {
            try
            {
                Remove(keys);
            }
            catch (InvalidOperationException)
            {
                //ignore
            }
        }

        public void TryRemove(TKey key)
        {
            var item = CollectionChanged == null ? default : TryGetValue(key);

            var wasRemoved = _items.Remove(key);
            if (wasRemoved && CollectionChanged != null)
                CollectionChanged(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>
                {
                    OldValues = new List<KeyValuePair<TKey, TValue>> { new(key, item.Value!) }
                });
        }

        public void TryRemove(Func<KeyValuePair<TKey, TValue>, bool> match)
        {
            try
            {
                Remove(match);
            }
            catch (InvalidOperationException)
            {
                //Ignore
            }
        }

        public void TryRemove(Func<TKey, bool> match)
        {
            try
            {
                Remove(match);
            }
            catch (InvalidOperationException)
            {
                //Ignore
            }
        }

        public void TryRemove(Func<TValue, bool> match)
        {
            try
            {
                Remove(match);
            }
            catch (InvalidOperationException)
            {
                //Ignore
            }
        }

        public ObservableDictionary<TKey, TValue> Copy() => new(this);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var result = _items.Remove(item);
            if (result)
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> { OldValues = new List<KeyValuePair<TKey, TValue>> { item } });

            return result;
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            _items.TryGetValue(key, out var oldItem);

            var result = _items.Remove(key);
            if (result)
                CollectionChanged?.Invoke(this, new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> { OldValues = new List<KeyValuePair<TKey, TValue>> { new(key, oldItem!) } });

            return result;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _items.Contains(item);

        public bool ContainsKey(TKey key) => _items.ContainsKey(key);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => _items.TryGetValue(key, out value!);

        public bool Equals(IObservableDictionary<TKey, TValue>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.SequenceEqual(other);
        }

        public bool Equals(ObservableDictionary<TKey, TValue>? other) => Equals(other as IObservableDictionary<TKey, TValue>);

        public static bool operator ==(ObservableDictionary<TKey, TValue>? a, ObservableDictionary<TKey, TValue>? b) => a is null && b is null || a is not null && a.Equals(b);

        public static bool operator !=(ObservableDictionary<TKey, TValue>? a, ObservableDictionary<TKey, TValue>? b) => !(a == b);

        public override bool Equals(object? obj) => Equals(obj as IObservableDictionary<TKey, TValue>);

        public override int GetHashCode() => _items.GetHashCode();

        public override string ToString() => Count == 0 ? $"Empty {GetType().GetHumanReadableName()}" : $"{GetType().GetHumanReadableName()} with {Count} items";
    }
}