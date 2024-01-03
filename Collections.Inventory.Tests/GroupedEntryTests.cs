namespace Collections.Inventory.Tests;

[TestClass]
public sealed class GroupedEntryTests : EntryTester<GroupedEntry<DummyItem>>
{
    [TestMethod]
    public void Constructor_WhenIndexesIsNull_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        IEnumerable<int> indexes = null!;

        //Act
        var action = () => new GroupedEntry<DummyItem>(item, quantity, indexes);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(indexes));
    }

    [TestMethod]
    public void Constructor_WhenIndexesIsNotNull_CreateWithIndexes()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        var indexes = Fixture.CreateMany<int>().ToList();

        //Act
        var result = new GroupedEntry<DummyItem>(item, quantity, indexes);

        //Assert
        result.Indexes.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void Constructor_WhenIndexesIsNotNullAndOriginalIndexListIsModified_DoNotAffectGroupedEntry()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        var indexes = Fixture.CreateMany<int>().ToList();
        var original = indexes.ToList();

        //Act
        var result = new GroupedEntry<DummyItem>(item, quantity, indexes);

        //Assert
        indexes.Add(Fixture.Create<int>());
        result.Indexes.Should().BeEquivalentTo(original);
    }

    [TestMethod]
    public void Indexes_WhenIsNull_Throw()
    {
        //Arrange


        //Act
        var action = () => new GroupedEntry<DummyItem> { Indexes = null! };

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }

    [TestMethod]
    public void Indexes_WhenIsNotNullAndOriginalCollectionModified_DoNotAffectGroupedEntry()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        var indexes = Fixture.CreateMany<int>().ToList();
        var original = indexes.ToList();
        var instance = new GroupedEntry<DummyItem>(item, quantity, indexes);

        //Act
        indexes.Add(Fixture.Create<int>());

        //Assert
        instance.Indexes.Should().BeEquivalentTo(original);
    }

    [TestMethod]
    public void Indexes_WhenIsNotNull_ReturnSetValue()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        var indexes = Fixture.CreateMany<int>().ToList();
        var instance = new GroupedEntry<DummyItem>(item, quantity, indexes);

        //Act
        var result = instance.Indexes;

        //Assert
        result.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void ToString_WhenItemIsNull_ReturnNull()
    {
        //Arrange
        var instance = Fixture.Build<GroupedEntry<DummyItem>>().Without(x => x.Item).Create();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"NULL x{instance.Quantity} at indexes {string.Join(", ", instance.Indexes)}");
    }

    [TestMethod]
    public void ToString_WhenItemIsNotNull_ReturnItemWithQuantity()
    {
        //Arrange
        var instance = Fixture.Create<GroupedEntry<DummyItem>>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Item} x{instance.Quantity} at indexes {string.Join(", ", instance.Indexes)}");
    }
}