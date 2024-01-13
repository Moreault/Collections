namespace ToolBX.Collections.UnitTesting.Inventory;

public abstract class EntryTester<TEntry> : RecordTester<TEntry> where TEntry : EntryBase<DummyItem>
{
    [TestMethod]
    public void Deconstructor_Always_Deconstruct()
    {
        //Arrange
        var instance = Fixture.Create<TEntry>();

        //Act
        var (item, quantity) = instance;

        //Assert
        item.Should().Be(instance.Item);
        quantity.Should().Be(instance.Quantity);
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TEntry>(Fixture);

    [TestMethod]
    public void Always_EnsureIsJsonSerializable() => Ensure.IsJsonSerializable<TEntry>(Fixture);

    [TestMethod]
    public void Always_EnsureItemHasBasicGetSetFunctionality() => Ensure.HasBasicGetSetFunctionality<TEntry>(Fixture, nameof(EntryBase<DummyItem>.Item));

    [TestMethod]
    public void Always_EnsureQuantityHasBasicGetSetFunctionality() => Ensure.HasBasicGetSetFunctionality<TEntry>(Fixture, nameof(EntryBase<DummyItem>.Quantity));
}