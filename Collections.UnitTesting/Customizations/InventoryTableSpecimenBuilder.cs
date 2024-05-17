namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class InventoryTableCustomization : GenericCollectionCustomizationBase
{
    public override IEnumerable<Type> Types => [typeof(InventoryTable<>)];

    protected override object Factory(Dummy dummy, Type type)
    {
        var elementType = type.GetGenericArguments()[0];
        var entryType = typeof(Entry<>).MakeGenericType(elementType);

        var list = CreateEnumerable(dummy, entryType);

        return Activator.CreateInstance(typeof(InventoryTable<>).MakeGenericType(elementType), list, int.MaxValue)!;
    }
}