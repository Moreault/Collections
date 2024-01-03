﻿namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class InventoryTableSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(InventoryTable<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var entryType = typeof(Entry<>).MakeGenericType(elementType);
            var listType = typeof(List<>).MakeGenericType(entryType);
            var list = context.Resolve(listType);

            return Activator.CreateInstance(typeof(InventoryTable<>).MakeGenericType(elementType), list, int.MaxValue)!;
        }

        return new NoSpecimen();
    }
}