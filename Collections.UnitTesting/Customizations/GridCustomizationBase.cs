namespace ToolBX.Collections.UnitTesting.Customizations;

public abstract class GridCustomizationBase : GenericCollectionCustomizationBase
{
    protected override object Factory(IDummy dummy, Type type)
    {
        var elementType = type.GetGenericArguments()[0];
        var cellType = typeof(Cell<>).MakeGenericType(elementType);

        var instance = CreateEnumerable(dummy, cellType);

        return GetType().GetSingleMethod(x => x.Name == nameof(Convert) && !x.IsAbstract).MakeGenericMethod(elementType).Invoke(this, [instance])!;
    }

    protected abstract object Convert<T>(IEnumerable<Cell<T>> source);
}