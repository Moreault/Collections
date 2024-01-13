namespace ToolBX.Collections.UnitTesting.Customizations;

public class ReadOnlyListSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlyList<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = context.Resolve(listType);

            var create = typeof(ReadOnlyListExtensions).GetSingleMethod(nameof(ReadOnlyListExtensions.ToReadOnlyList)).MakeGenericMethod(elementType);
            return create.Invoke(null, new[] { list })!;
        }

        return new NoSpecimen();
    }
}