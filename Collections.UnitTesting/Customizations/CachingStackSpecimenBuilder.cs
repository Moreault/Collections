namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingStackCustomization : GenericStackCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(CachingStack<>)];
    protected override object Convert<T>(IEnumerable<T> source) => source.ToCachingStack();
}