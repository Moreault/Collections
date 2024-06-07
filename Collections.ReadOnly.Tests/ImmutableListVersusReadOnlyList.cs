namespace Collections.ReadOnly.Tests;

[TestClass]
public sealed class ImmutableListVersusReadOnlyList : Tester
{
    [TestMethod]
    public void ImmutableList_WhenComparedToEquivalentObjectOfSameType_ReturnsTrue()
    {
        //Arrange
        var instance = Dummy.Create<ImmutableList<Garbage>>();
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
        var instance = Dummy.CreateMany<Garbage>().ToReadOnlyList();
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
        var instance = Dummy.Create<ImmutableList<Garbage>>();
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
        var instance = Dummy.CreateMany<Garbage>().ToReadOnlyList();
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
        var instance = Dummy.Create<ImmutableList<Garbage>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("System.Collections.Immutable.ImmutableList`1[Collections.ReadOnly.Tests.Garbage]");
    }

    [TestMethod]
    public void ReadOnlyList_ToStringWhenEmpty_ReturnEmptyReadOnlyListWithGenericType()
    {
        //Arrange
        var instance = ReadOnlyList<Garbage>.Empty;

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("Empty ReadOnlyList<Garbage>");
    }

    [TestMethod]
    public void ReadOnlyList_ToStringWhenContainsItems_ReturnAmountTypeName()
    {
        //Arrange
        var instance = Dummy.CreateMany<Garbage>(3).ToReadOnlyList();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"ReadOnlyList<Garbage> with {instance.Count} elements");
    }

}