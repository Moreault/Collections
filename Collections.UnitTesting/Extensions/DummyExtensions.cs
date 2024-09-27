namespace ToolBX.Collections.UnitTesting.Extensions;

public static class DummyExtensions
{
    public static Dummy WithCollectionCustomizations(this Dummy dummy)
    {
        var customizations = Types.From(Assembly.GetExecutingAssembly()).Where(x => x.Implements<ICustomization>() && !x.IsAbstract).Select(Activator.CreateInstance).Cast<ICustomization>();

        foreach (var customization in customizations)
        {
            dummy.Customize(customization);
        }

        return dummy;
    }
}