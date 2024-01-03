namespace Collections.Inventory.Tests;

[TestClass]
public sealed class EntryTests : EntryTester<Entry<DummyItem>>
{
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