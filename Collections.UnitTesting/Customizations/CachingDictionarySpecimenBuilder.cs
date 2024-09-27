namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingDictionaryCustomization : DictionaryCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(CachingDictionary<,>)];

    protected override object Convert<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source) => source.ToCachingDictionary();
}