namespace Collections.Inventory.Tests;

[TestClass]
public sealed class InventoryListExtensionsTests : Tester
{
    [TestMethod]
    public void ToInventoryListType_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<DummyItem> collection = null!;

        //Act
        var action = () => collection.ToInventoryList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void ToInventoryListType_WhenCollectionIsNotNull_ReturnInventoryListWithAllItemsWithOneQuantity()
    {
        //Arrange
        var collection = Fixture.CreateMany<DummyItem>().ToList();
        var stackSize = Fixture.Create<int>();

        //Act
        var result = collection.ToInventoryList(stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection.Select(x => new Entry<DummyItem>(x, 1)));
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void ToInventoryListEntries_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<Entry<DummyItem>> collection = null!;

        //Act
        var action = () => collection.ToInventoryList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void ToInventoryListEntries_WhenCollectionIsNotNull_ReturnNew()
    {
        //Arrange
        var collection = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        var stackSize = collection.Max(x => x.Quantity) + Fixture.Create<int>();

        //Act
        var result = collection.ToInventoryList(stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection);
        result.StackSize.Should().Be(stackSize);
    }
}