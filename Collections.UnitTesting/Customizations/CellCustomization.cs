namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CellCustomization : CustomizationBase
{
    protected override IEnumerable<Type> Types { get; } = [typeof(Cell<>)];

    protected override IDummyBuilder BuildMe(IDummy dummy, Type type) => dummy.Build<object>().FromFactory(() =>
    {
        var position = new Vector2<int>(dummy.Number.Between(-10, 10).Create(), dummy.Number.Between(-10, 10).Create());
        var elementType = type.GetGenericArguments()[0];
        var cellType = typeof(Cell<>).MakeGenericType(elementType);

        var cell = Activator.CreateInstance(cellType)!;

        cellType.GetSingleProperty(nameof(Cell<int>.Index)).SetValue(cell, position);
        cellType.GetSingleProperty(nameof(Cell<int>.Value)).SetValue(cell, dummy.Create(elementType));

        return cell;
    });
}