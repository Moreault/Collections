namespace Collections.Inventory.Tests;

[TestClass]
public sealed class TryAddResultTests : Tester
{
    [TestMethod]
    public void Total_Always_ReturnAddedPlusNotAdded()
    {
        //Arrange
        var instance = Fixture.Create<TryAddResult>();

        //Act
        var total = instance.Total;

        //Assert
        total.Should().Be(instance.Added + instance.NotAdded);
    }

    [TestMethod]
    public void ToString_WhenAddedIsZero_ReturnNoneWereAdded()
    {
        //Arrange
        var instance = new TryAddResult(0, Fixture.Create<int>());

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"None of the {instance.Total} items could be added");
    }

    [TestMethod]
    public void ToString_WhenAddedIsNotZeroButNotAddedIs_ReturnAllItemsWereAdded()
    {
        //Arrange
        var instance = new TryAddResult(Fixture.Create<int>(), 0);

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"All of the {instance.Total} items could be added");
    }

    [TestMethod]
    public void ToString_WhenAddedAndNotAddedAreNotZero_ReturnItemsOutOfXAdded()
    {
        //Arrange
        var instance = Fixture.Create<TryAddResult>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Added} items out of {instance.Total} were added");
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TryAddResult>(Fixture);

    [TestMethod]
    public void Ensure_ValueHashCode() => Ensure.ValueHashCode<TryAddResult>(Fixture);

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<TryAddResult>(Fixture);
}