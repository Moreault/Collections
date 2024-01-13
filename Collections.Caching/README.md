![Caching](https://github.com/Moreault/Collections/blob/master/caching.png)
# Caching collections
Collections that are limited to a specified size in order to be used as object caches.

## Observability
All collections in this package implement the `IObservableCollection<T>` interface from [ToolBX.Collections.Common](https://github.com/Moreault/Collections/tree/master/Collections.Common).

```c#
var collection = new CachingDictionary<int, string>();
collections.CollectionChanged += DictionaryChanged;
```

## Limit property
Limits the number of items allowed in the collection to this number. The oldest elements are removed first. If you wanted to limit your list to the 10 last elements, you would do this :

```c#
var collection = new CachingList<string>();
collection.Limit = 10;
```

This way, adding an eleventh element will remove the oldest so that your list always has a maximum of 10 elements.

## TrimStartDownTo
Removes the `x` oldest elements that were added to the collection.

```c#
//Removes the five oldest elements in the collection
collection.TrimStartDownTo(5);
```

## TrimEndDownTo
Removes the `x` newest elements that were added to the collection.

```c#
//Removes the eight newest elements in the collection
collection.TrimStartDownTo(8);
```