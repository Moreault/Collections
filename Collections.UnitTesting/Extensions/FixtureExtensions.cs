using System.Reflection;
using ToolBX.Reflection4Humans.TypeFetcher;

namespace ToolBX.Collections.UnitTesting.Extensions;

public static class FixtureExtensions
{
    public static IFixture WithCollectionCustomizations(this IFixture fixture)
    {
        var customizations = Types.From(Assembly.GetExecutingAssembly()).Where(x => x.Implements<ICustomization>() || x.Implements<ISpecimenBuilder>()).Select(Activator.CreateInstance);

        foreach (var customization in customizations)
        {
            if (customization is ICustomization cucu)
                fixture.Customize(cucu);
            else if (customization is ISpecimenBuilder specimenBuilder)
                fixture.Customizations.Add(specimenBuilder);
        }

        
        return fixture;
    }

}