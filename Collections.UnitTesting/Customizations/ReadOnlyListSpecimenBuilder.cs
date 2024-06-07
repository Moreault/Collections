namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class ReadOnlyListCustomization : ListCustomizationBase
{
    protected override IEnumerable<Type> Types { get; } = [typeof(ReadOnlyList<>)];

    protected override object Convert<T>(IEnumerable<T> source) => source.ToReadOnlyList();
}