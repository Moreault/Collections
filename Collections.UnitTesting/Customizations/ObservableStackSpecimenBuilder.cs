namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class ObservableStackCustomization : GenericStackCustomizationBase
{
    public override IEnumerable<Type> Types { get; } = [typeof(ObservableStack<>), typeof(IObservableStack<>)];

    protected override object Convert<T>(IEnumerable<T> source) => source.ToObservableStack();
}