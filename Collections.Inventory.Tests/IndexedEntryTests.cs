namespace Collections.Inventory.Tests;

[TestClass]
public sealed class IndexedEntryTests : EntryTester<IndexedEntry<DummyItem>>
{
    //TODO
    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Fixture.Build<IndexedEntry<DummyItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Index}. NULL x{instance.Quantity}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Fixture.Create<IndexedEntry<DummyItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Index}. {instance.Item} x{instance.Quantity}");
    }

    [TestMethod]
    public void Deconstruct_Always_ReturnValues()
    {
        //Arrange
        var instance = Fixture.Create<IndexedEntry<DummyItem>>();

        //Act
        var (item, quantity, index) = instance;

        //Assert
        item.Should().Be(instance.Item);
        quantity.Should().Be(instance.Quantity);
        index.Should().Be(instance.Index);
    }
}