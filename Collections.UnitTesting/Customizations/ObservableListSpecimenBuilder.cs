namespace ToolBX.Collections.UnitTesting.Customizations;

[AutoCustomization]
public class ObservableListSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableList<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = context.Resolve(listType);

            var toObservableList = typeof(ObservableListExtensions).GetSingleMethod(nameof(ObservableListExtensions.ToObservableList)).MakeGenericMethod(elementType);
            return toObservableList.Invoke(null, new[] { list })!;
        }

        return new NoSpecimen();
    }
}