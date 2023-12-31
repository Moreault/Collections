namespace Collections.ReadOnly.Tests;

[TestClass]
public sealed class ImmutableListVersusReadOnlyList : Tester
{
    [TestMethod]
    public void ImmutableList_WhenComparedToEquivalentObjectOfSameType_ReturnsTrue()
    {
        //Arrange
        var instance = Fixture.Create<ImmutableList<Dummy>>();
        var copy = instance.Select(x => x).ToImmutableList();

        //Act
        var result = instance.Equals(copy);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ReadOnlyList_WhenComparedToEquivalentObjectOfSameType_ReturnsTrue()
    {
        //Arrange
        var instance = Fixture.CreateMany<Dummy>().ToReadOnlyList();
        var copy = instance.Select(x => x).ToReadOnlyList();

        //Act
        var result = instance.Equals(copy);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ImmutableList_WhenComparedToReadOnlyList_ReturnsFalse()
    {
        //Arrange
        var instance = Fixture.Create<ImmutableList<Dummy>>();
        var copy = instance.Select(x => x).ToReadOnlyList();

        //Act
        var result = instance.Equals(copy);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ReadOnlyList_WhenComparedToImmutableList_ReturnsTrue()
    {
        //Arrange
        var instance = Fixture.CreateMany<Dummy>().ToReadOnlyList();
        var copy = instance.Select(x => x).ToImmutableList();

        //Act
        var result = instance.Equals(copy);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ImmutableList_ToString_ReturnTypeName()
    {
        //Arrange
        var instance = Fixture.Create<ImmutableList<Dummy>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("System.Collections.Immutable.ImmutableList`1[Collections.ReadOnly.Tests.Dummy]");
    }

    [TestMethod]
    public void ReadOnlyList_ToStringWhenEmpty_ReturnEmptyReadOnlyListWithGenericType()
    {
        //Arrange
        var instance = ReadOnlyList<Dummy>.Empty;

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("Empty ReadOnlyList<Dummy>");
    }

    [TestMethod]
    public void ReadOnlyList_ToStringWhenContainsItems_ReturnAmountTypeName()
    {
        //Arrange
        var instance = Fixture.CreateMany<Dummy>(3).ToReadOnlyList();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"ReadOnlyList<Dummy> with {instance.Count} elements");
    }

}