using ToolBX.Mathemancy;

namespace ToolBX.Collections.UnitTesting.Customizations;

public class BoundariesCustomization : ICustomization
{
    public IEnumerable<Type> Types { get; } = [typeof(Boundaries<>)];

    public IDummyBuilder Build(Dummy dummy, Type type)
    {
        if (dummy is null) throw new ArgumentNullException(nameof(dummy));
        if (type is null) throw new ArgumentNullException(nameof(type));

        return dummy.Build<object>().FromFactory(() =>
        {
            var top = dummy.Create<int>();
            var left = dummy.Create<int>();
            var right = left + dummy.Create<int>();
            var bottom = top + dummy.Create<int>();
            return new Boundaries<int>(top, right, bottom, left);
            //TODO These shouldn't be required but were required by AutoFixture
        })/*.Without(y => y.Top).Without(y => y.Right).Without(y => y.Bottom).Without(y => y.Left));*/;
    }
}