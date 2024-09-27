namespace ToolBX.Collections.UnitTesting.Customizations;

public class BoundariesCustomization : CustomizationBase
{
    protected override IEnumerable<Type> Types { get; } = [typeof(Boundaries<>)];

    protected override IDummyBuilder BuildMe(IDummy dummy, Type type)
    {
        return dummy.Build<object>().FromFactory(() =>
        {
            var top = dummy.Create<int>();
            var left = dummy.Create<int>();
            var right = left + dummy.Create<int>();
            var bottom = top + dummy.Create<int>();
            return new Boundaries<int>(top, right, bottom, left);
        });
    }
}