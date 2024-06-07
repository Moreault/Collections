namespace Collections.Inventory.Tests;

[TestClass]
public sealed class InventoryListExtensionsTests : Tester
{
    [TestMethod]
    public void ToInventoryListType_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<GarbageItem> collection = null!;

        //Act
        var action = () => collection.ToInventoryList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void ToInventoryListType_WhenCollectionIsNotNull_ReturnInventoryListWithAllItemsWithOneQuantity()
    {
        //Arrange
        var collection = Dummy.CreateMany<GarbageItem>().ToList();
        var stackSize = Dummy.Create<int>();

        //Act
        var result = collection.ToInventoryList(stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection.Select(x => new Entry<GarbageItem>(x, 1)));
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void ToInventoryListEntries_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<Entry<GarbageItem>> collection = null!;

        //Act
        var action = () => collection.ToInventoryList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [TestMethod]
    public void ToInventoryListEntries_WhenCollectionIsNotNull_ReturnNew()
    {
        //Arrange
        var collection = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        var stackSize = collection.Max(x => x.Quantity) + Dummy.Create<int>();

        //Act
        var result = collection.ToInventoryList(stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection);
        result.StackSize.Should().Be(stackSize);
    }
}