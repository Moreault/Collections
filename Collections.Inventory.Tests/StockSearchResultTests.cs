namespace Collections.Inventory.Tests;

[TestClass]
public class StockSearchResultTests : RecordTester<StockSearchResult<DummyItem>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        JsonSerializerOptions.WithInventoryConverters();
    }

    [TestMethod]
    public void Indexer_WhenIndexIsWithinBoundaries_ReturnItemAtIndex()
    {
        //Arrange
        var instance = Fixture.Create<StockSearchResult<IndexedEntry<DummyItem>>>();

        var index = instance.GetRandomIndex();

        //Act
        var result = instance[index];

        //Assert
        result.Should().Be(instance[index]);
    }

    [TestMethod]
    public void Constructor_WhenItemsIsNull_Throw()
    {
        //Arrange

        //Act
        var action = () => new StockSearchResult<IndexedEntry<DummyItem>>(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("items");
    }

    [TestMethod]
    public void Constructor_WhenItemsIsEmpty_ShouldBeEmpty()
    {
        //Arrange
        var items = new List<IndexedEntry<DummyItem>>();

        //Act
        var result = new StockSearchResult<DummyItem>(items);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenItemsIsNotEmpty_ReturnWithItems()
    {
        //Arrange
        var items = Fixture.CreateMany<IndexedEntry<DummyItem>>().ToList();

        //Act
        var result = new StockSearchResult<DummyItem>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Enumerator_Always_CorrectlyEnumeratesEveryItem()
    {
        //Arrange
        var searchResult = Fixture.Create<StockSearchResult<DummyItem>>();

        var enumeratedItems = new List<IndexedEntry<DummyItem>>();

        //Act
        foreach (var item in searchResult)
            enumeratedItems.Add(item);

        //Assert
        enumeratedItems.Should().NotBeEmpty();
        enumeratedItems.Should().BeEquivalentTo(searchResult);
        enumeratedItems.Should().HaveCount(searchResult.Count);
    }

    [TestMethod]
    public void GetEnumerator_Always_ReturnsEnumerator()
    {
        //Arrange
        var searchResult = Fixture.Create<StockSearchResult<DummyItem>>();

        //Act
        var enumerator = searchResult.GetEnumerator();

        //Assert
        enumerator.Should().NotBeNull();
    }

    [TestMethod]
    public void InterfaceGetEnumerator_Always_ReturnsInternalCollectionEnumerator()
    {
        //Arrange
        var searchResult = Fixture.Create<StockSearchResult<DummyItem>>();

        //Act
        var enumerator = ((IEnumerable)searchResult).GetEnumerator();

        //Assert
        enumerator.Should().NotBeNull();
    }

    [TestMethod]
    public void Group_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var searchResult = new StockSearchResult<DummyItem>(Array.Empty<IndexedEntry<DummyItem>>());

        //Act
        var result = searchResult.Group();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Group_WhenHasUniqueItems_DoNotGroup()
    {
        //Arrange
        var searchResult = Fixture.Create<StockSearchResult<DummyItem>>();

        //Act
        var result = searchResult.Group();

        //Assert
        result.Should().BeEquivalentTo(searchResult.Select(x => new GroupedEntry<DummyItem>(x.Item, x.Quantity, new List<int> { x.Index })));
    }

    [TestMethod]
    public void Group_WhenHasDuplicateItems_GroupThem()
    {
        //Arrange
        var entries = Fixture.CreateMany<IndexedEntry<DummyItem>>().ToList();
        var duplicate = entries.GetRandom() with { Quantity = Fixture.Create<int>(), Index = Fixture.Create<int>() };
        entries.Add(duplicate);

        var searchResult = new StockSearchResult<DummyItem>(entries);

        //Act
        var result = searchResult.Group();

        //Assert
        var expectedGroupedEntry = new GroupedEntry<DummyItem>(duplicate.Item, entries.Where(x => x.Item == duplicate.Item).Sum(x => x.Quantity), entries.Where(x => x.Item.Equals(duplicate.Item)).Select(x => x.Index).ToList());
        var expectedEntries = searchResult.Where(x => !x.Item.Equals(duplicate.Item)).Select(x => new GroupedEntry<DummyItem>(x.Item, x.Quantity, new List<int> { x.Index })).Append(expectedGroupedEntry).ToList();
        result.Should().BeEquivalentTo(expectedEntries);
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<StockSearchResult<IndexedEntry<DummyItem>>>(Fixture, new JsonSerializerOptions().WithInventoryConverters());
}