namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class OverlapGridCustomization : GridCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(OverlapGrid<>)];

    protected override object Convert<T>(IEnumerable<Cell<T>> source) => source.ToOverlapGrid();
}