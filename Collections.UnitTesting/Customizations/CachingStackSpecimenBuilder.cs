namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingStackCustomization : GenericStackCustomizationBase
{
    public override IEnumerable<Type> Types => [typeof(CachingStack<>)];
    protected override object Convert<T>(IEnumerable<T> source) => source.ToCachingStack();
}