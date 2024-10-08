﻿namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class InventoryListCustomization : GenericCollectionCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(InventoryList<>)];

    protected override object Factory(IDummy dummy, Type type)
    {
        var elementType = type.GetGenericArguments()[0];
        var entryType = typeof(Entry<>).MakeGenericType(elementType);

        var list = CreateEnumerable(dummy, entryType);

        return Activator.CreateInstance(typeof(InventoryList<>).MakeGenericType(elementType), list, int.MaxValue)!;
    }
}