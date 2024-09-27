using ToolBX.Dummies;
using ToolBX.Dummies.Customizations;
using ToolBX.Reflection4Humans.Extensions;

namespace Collections.Grid.Tests.Customizations;

public sealed class PositiveIndexCellCustomization : CustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(Cell<>)];

    protected override IDummyBuilder BuildMe(IDummy dummy, Type type) => dummy.Build<object>().FromFactory(() =>
    {
        var position = new Vector2<int>(dummy.Number.Between(0, 20).Create(), dummy.Number.Between(0, 20).Create());
        var elementType = type.GetGenericArguments()[0];
        var cellType = typeof(Cell<>).MakeGenericType(elementType);

        var cell = Activator.CreateInstance(cellType)!;

        cellType.GetSingleProperty(nameof(Cell<int>.Index)).SetValue(cell, position);
        cellType.GetSingleProperty(nameof(Cell<int>.Value)).SetValue(cell, dummy.Create(elementType));

        return cell;
    });
}