namespace Collections.Inventory.Tests;

[TestClass]
public sealed class EntryTests : EntryTester<Entry<GarbageItem>>
{
    [TestMethod]
    public void Quantity_WhenIsZero_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => new Entry<GarbageItem> { Quantity = 0 };

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Quantity_WhenIsNegative_Throw()
    {
        //Arrange

        //Act
        var action = () => new Entry<GarbageItem> { Quantity = -Dummy.Create<int>() };

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithMessage($"{Exceptions.QuantityMustBePositive}*");
    }

    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Dummy.Build<Entry<GarbageItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"NULL x{instance.Quantity}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Dummy.Create<Entry<GarbageItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Item} x{instance.Quantity}");
    }
}