namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingListCustomization : ListCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(CachingList<>)];

    protected override object Convert<T>(IEnumerable<T> source) => source.ToCachingList();
}