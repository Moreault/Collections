![ToolBX.Collections.Common](https://github.com/Moreault/Collections/blob/master/collections.common.png)
# ToolBX.Collections.Common
Common types for the ToolBX.Collections namespaces. This package should not be referenced by your project directly.

## IObservableCollection
Interface that exposes a `CollectionChanged` event.

```c#
var collection = new ObservableList<string>();
collections.CollectionChanged += OnChange;
```

The `CollectionChangeEventArgs<T>` have two properties : `OldValues` and `NewValues`.