namespace ToolBX.Collections.Common;

public interface IObservableCollection<T>
{
    event CollectionChangeEventHandler<T> CollectionChanged;
}