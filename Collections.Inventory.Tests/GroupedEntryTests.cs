namespace Collections.Inventory.Tests;

[TestClass]
public sealed class GroupedEntryTests : EntryTester<GroupedEntry<GarbageItem>>
{
    [TestMethod]
    public void Constructor_WhenIndexesIsNull_Throw()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();
        IEnumerable<int> indexes = null!;

        //Act
        var action = () => new GroupedEntry<GarbageItem>(item, quantity, indexes);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(indexes));
    }

    [TestMethod]
    public void Constructor_WhenIndexesIsNotNull_CreateWithIndexes()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();
        var indexes = Dummy.CreateMany<int>().ToList();

        //Act
        var result = new GroupedEntry<GarbageItem>(item, quantity, indexes);

        //Assert
        result.Indexes.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void Constructor_WhenIndexesIsNotNullAndOriginalIndexListIsModified_DoNotAffectGroupedEntry()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();
        var indexes = Dummy.CreateMany<int>().ToList();
        var original = indexes.ToList();

        //Act
        var result = new GroupedEntry<GarbageItem>(item, quantity, indexes);

        //Assert
        indexes.Add(Dummy.Create<int>());
        result.Indexes.Should().BeEquivalentTo(original);
    }

    [TestMethod]
    public void Indexes_WhenIsNull_Throw()
    {
        //Arrange


        //Act
        var action = () => new GroupedEntry<GarbageItem> { Indexes = null! };

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [TestMethod]
    public void Indexes_WhenIsNotNullAndOriginalCollectionModified_DoNotAffectGroupedEntry()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();
        var indexes = Dummy.CreateMany<int>().ToList();
        var original = indexes.ToList();
        var instance = new GroupedEntry<GarbageItem>(item, quantity, indexes);

        //Act
        indexes.Add(Dummy.Create<int>());

        //Assert
        instance.Indexes.Should().BeEquivalentTo(original);
    }

    [TestMethod]
    public void Indexes_WhenIsNotNull_ReturnSetValue()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();
        var indexes = Dummy.CreateMany<int>().ToList();
        var instance = new GroupedEntry<GarbageItem>(item, quantity, indexes);

        //Act
        var result = instance.Indexes;

        //Assert
        result.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Dummy.Build<GroupedEntry<GarbageItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"NULL x{instance.Quantity} at indexes {string.Join(", ", instance.Indexes)}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Dummy.Create<GroupedEntry<GarbageItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Item} x{instance.Quantity} at indexes {string.Join(", ", instance.Indexes)}");
    }
}