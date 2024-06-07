namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class ObservableListCustomization : ListCustomizationBase
{
    protected override IEnumerable<Type> Types { get; } = [typeof(ObservableList<>), typeof(IObservableList<>)];

    protected override object Convert<T>(IEnumerable<T> source) => source.ToObservableList();
}