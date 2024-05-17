namespace Collections.Inventory.Tests;

[TestClass]
public sealed class TryAddResultTests : ToolBX.Collections.UnitTesting.Tester
{
    [TestMethod]
    public void Total_Always_ReturnAddedPlusNotAdded()
    {
        //Arrange
        var instance = Dummy.Create<TryAddResult>();

        //Act
        var total = instance.Total;

        //Assert
        total.Should().Be(instance.Added + instance.NotAdded);
    }

    [TestMethod]
    public void ToString_WhenAddedIsZero_ReturnNoneWereAdded()
    {
        //Arrange
        var instance = new TryAddResult(0, Dummy.Create<int>());

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"None of the {instance.Total} items could be added");
    }

    [TestMethod]
    public void ToString_WhenAddedIsNotZeroButNotAddedIs_ReturnAllItemsWereAdded()
    {
        //Arrange
        var instance = new TryAddResult(Dummy.Create<int>(), 0);

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"All of the {instance.Total} items could be added");
    }

    [TestMethod]
    public void ToString_WhenAddedAndNotAddedAreNotZero_ReturnItemsOutOfXAdded()
    {
        //Arrange
        var instance = Dummy.Create<TryAddResult>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Added} items out of {instance.Total} were added");
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TryAddResult>(Dummy);

    [TestMethod]
    public void Ensure_ValueHashCode() => Ensure.ValueHashCode<TryAddResult>(Dummy, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<TryAddResult>(Dummy);
}