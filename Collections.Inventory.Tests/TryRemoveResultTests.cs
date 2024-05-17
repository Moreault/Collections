namespace Collections.Inventory.Tests;

[TestClass]
public sealed class TryRemoveResultTests : ToolBX.Collections.UnitTesting.Tester
{
    [TestMethod]
    public void Total_Always_ReturnRemovedPlusNotRemoved()
    {
        //Arrange
        var instance = Dummy.Create<TryRemoveResult>();

        //Act
        var result = instance.Total;

        //Assert
        result.Should().Be(instance.Removed + instance.NotRemoved);
    }

    [TestMethod]
    public void ToString_WhenNoItemsWereRemoved_ReturnAllItemsCouldNotBeRemoved()
    {
        //Arrange
        var instance = new TryRemoveResult(0, Dummy.Create<int>());

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"All {instance.NotRemoved} items could not be removed");
    }

    [TestMethod]
    public void ToString_WhenAtLeastOneItemWasRemoved_ReturnHowManyOutOfTotalWereRemoved()
    {
        //Arrange
        var instance = new TryRemoveResult(Dummy.Create<int>(), Dummy.Create<int>());

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Removed} items out of {instance.Total} were removed");
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TryRemoveResult>(Dummy);

    [TestMethod]
    public void Ensure_ValueHashCode() => Ensure.ValueHashCode<TryRemoveResult>(Dummy, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<TryRemoveResult>(Dummy);
}