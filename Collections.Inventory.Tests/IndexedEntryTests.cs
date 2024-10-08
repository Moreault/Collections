﻿namespace Collections.Inventory.Tests;

[TestClass]
public sealed class IndexedEntryTests : EntryTester<IndexedEntry<GarbageItem>>
{
    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Dummy.Build<IndexedEntry<GarbageItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Index}. NULL x{instance.Quantity}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Dummy.Create<IndexedEntry<GarbageItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Index}. {instance.Item} x{instance.Quantity}");
    }

    [TestMethod]
    public void Deconstruct_Always_ReturnValues()
    {
        //Arrange
        var instance = Dummy.Create<IndexedEntry<GarbageItem>>();

        //Act
        var (item, quantity, index) = instance;

        //Assert
        item.Should().Be(instance.Item);
        quantity.Should().Be(instance.Quantity);
        index.Should().Be(instance.Index);
    }
}