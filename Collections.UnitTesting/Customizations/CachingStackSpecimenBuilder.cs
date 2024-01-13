namespace ToolBX.Collections.UnitTesting.Customizations;

public class CachingStackSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(CachingStack<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var stackType = typeof(List<>).MakeGenericType(elementType);
            var list = context.Resolve(stackType);

            return Activator.CreateInstance(typeof(CachingStack<>).MakeGenericType(elementType), list)!;
        }

        return new NoSpecimen();
    }
}