namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingDictionaryCustomization : DictionaryCustomizationBase
{
    public override IEnumerable<Type> Types => [typeof(CachingDictionary<,>)];

    protected override object Convert<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source) => source.ToCachingDictionary();
}