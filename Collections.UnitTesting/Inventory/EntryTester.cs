namespace ToolBX.Collections.UnitTesting.Inventory;

public abstract class EntryTester<TEntry> : RecordTester<TEntry> where TEntry : EntryBase<GarbageItem>
{
    [TestMethod]
    public void Deconstructor_Always_Deconstruct()
    {
        //Arrange
        var instance = Dummy.Create<TEntry>();

        //Act
        var (item, quantity) = instance;

        //Assert
        item.Should().Be(instance.Item);
        quantity.Should().Be(instance.Quantity);
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TEntry>(Dummy);

    [TestMethod]
    public void Always_EnsureIsJsonSerializable() => Ensure.IsJsonSerializable<TEntry>(Dummy);

    [TestMethod]
    public void Always_EnsureItemHasBasicGetSetFunctionality() => Ensure.HasBasicGetSetFunctionality<TEntry>(Dummy, nameof(EntryBase<GarbageItem>.Item));

    [TestMethod]
    public void Always_EnsureQuantityHasBasicGetSetFunctionality() => Ensure.HasBasicGetSetFunctionality<TEntry>(Dummy, nameof(EntryBase<GarbageItem>.Quantity));
}