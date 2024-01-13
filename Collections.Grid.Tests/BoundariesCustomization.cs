namespace Collections.Grid.Tests;

[AutoCustomization]
public class BoundariesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Boundaries<int>>(x => x.FromFactory(() =>
        {
            var top = fixture.Create<int>();
            var left = fixture.Create<int>();
            var right = left + fixture.Create<int>();
            var bottom = top + fixture.Create<int>();
            return new Boundaries<int>(top, right, bottom, left);
        }).Without(y => y.Top).Without(y => y.Right).Without(y => y.Bottom).Without(y => y.Left));
    }
}