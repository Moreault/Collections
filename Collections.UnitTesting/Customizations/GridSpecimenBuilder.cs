namespace ToolBX.Collections.UnitTesting.Customizations;

public class GridSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Grid<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var cellType = typeof(Cell<>).MakeGenericType(elementType);

            var listType = typeof(List<>).MakeGenericType(cellType);
            var list = context.Resolve(listType);

            return Activator.CreateInstance(typeof(Grid<>).MakeGenericType(elementType), list)!;
        }

        return new NoSpecimen();
    }
}