namespace Collections.Inventory.Tests;

[TestClass]
public sealed class EntryTests : RecordTester<Entry<DummyItem>>
{
    [TestMethod]
    public void Deconstructor_Always_Deconstruct()
    {
        //Arrange
        var instance = Fixture.Create<Entry<DummyItem>>();

        //Act
        var (item, quantity) = instance;

        //Assert
        item.Should().Be(instance.Item);
        quantity.Should().Be(instance.Quantity);
    }

    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Fixture.Build<Entry<DummyItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"NULL x{instance.Quantity}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Fixture.Create<Entry<DummyItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Item} x{instance.Quantity}");
    }
}