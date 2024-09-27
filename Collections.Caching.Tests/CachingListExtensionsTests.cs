namespace Collections.Caching.Tests;

[TestClass]
public sealed class CachingListExtensionsTests : Tester
{
    [TestMethod]
    public void WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> collection = null!;

        //Act
        var action = new Action(() => collection.ToCachingList());

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void WhenSourceIsNotNull_InstantiateCachingList()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.ToCachingList();

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}