namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class ObservableDictionaryCustomization : DictionaryCustomizationBase
{
    public override IEnumerable<Type> Types => [typeof(ObservableDictionary<,>), typeof(IObservableDictionary<,>)];

    protected override object Convert<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source) => source.ToObservableDictionary();
}