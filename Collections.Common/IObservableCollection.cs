﻿namespace ToolBX.Collections.Common;

public interface IObservableCollection<T>
{
    public event CollectionChangeEventHandler<T> CollectionChanged;
}