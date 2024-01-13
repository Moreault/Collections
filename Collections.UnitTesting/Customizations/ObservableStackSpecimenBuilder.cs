namespace ToolBX.Collections.UnitTesting.Customizations;

public class ObservableStackSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableStack<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var stackType = typeof(List<>).MakeGenericType(elementType);
            var list = context.Resolve(stackType);

            return Activator.CreateInstance(typeof(ObservableStack<>).MakeGenericType(elementType), list)!;
        }

        return new NoSpecimen();
    }
}