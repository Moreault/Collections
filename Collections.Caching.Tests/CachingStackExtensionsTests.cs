namespace Collections.Caching.Tests;

[TestClass]
public sealed class CachingStackExtensionsTests : Tester
{
    [TestMethod]
    public void WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Dummy> collection = null!;

        //Act
        var action = new Action(() => collection.ToCachingStack());

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void WhenSourceIsNotNull_InstantiateCachingStack()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();

        //Act
        var result = source.ToCachingStack();

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}