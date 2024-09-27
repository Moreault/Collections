namespace Collections.ObservableStack.Tests;

[TestClass]
public sealed class ObservableStackExtensionsTests : Tester
{
    [TestMethod]
    public void ToObservableStack_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> source = null!;

        //Act
        var action = () => source.ToObservableStack();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToObservableStack_WhenSourceIsNotNull_ReturnEquivalent()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.ToObservableStack();

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}
