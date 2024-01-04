namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class CachingListSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(CachingList<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = context.Resolve(listType);

            return Activator.CreateInstance(typeof(CachingList<>).MakeGenericType(elementType), list)!;
        }

        return new NoSpecimen();
    }
}