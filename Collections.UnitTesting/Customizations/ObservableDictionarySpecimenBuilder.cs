namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class ObservableDictionarySpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableDictionary<,>))
        {
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var dictionary = context.Resolve(dictionaryType);

            return Activator.CreateInstance(typeof(ObservableDictionary<,>).MakeGenericType(keyType, valueType), dictionary)!;
        }

        return new NoSpecimen();
    }
}