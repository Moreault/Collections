using ToolBX.Dummies;
using ToolBX.Eloquentest.Dummies;

namespace Collections.Inventory.Tests;

[TestClass]
public class StockSearchResultTests : ToolBX.Collections.UnitTesting.RecordTester<StockSearchResult<GarbageItem>>
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
        var instance = Dummy.Create<StockSearchResult<IndexedEntry<GarbageItem>>>();

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
        var action = () => new StockSearchResult<IndexedEntry<GarbageItem>>(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("items");
    }

    [TestMethod]
    public void Constructor_WhenItemsIsEmpty_ShouldBeEmpty()
    {
        //Arrange
        var items = new List<IndexedEntry<GarbageItem>>();

        //Act
        var result = new StockSearchResult<GarbageItem>(items);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenItemsIsNotEmpty_ReturnWithItems()
    {
        //Arrange
        var items = Dummy.CreateMany<IndexedEntry<GarbageItem>>().ToList();

        //Act
        var result = new StockSearchResult<GarbageItem>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Enumerator_Always_CorrectlyEnumeratesEveryItem()
    {
        //Arrange
        var searchResult = Dummy.Create<StockSearchResult<GarbageItem>>();

        var enumeratedItems = new List<IndexedEntry<GarbageItem>>();

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
        var searchResult = Dummy.Create<StockSearchResult<GarbageItem>>();

        //Act
        var enumerator = searchResult.GetEnumerator();

        //Assert
        enumerator.Should().NotBeNull();
    }

    [TestMethod]
    public void InterfaceGetEnumerator_Always_ReturnsInternalCollectionEnumerator()
    {
        //Arrange
        var searchResult = Dummy.Create<StockSearchResult<GarbageItem>>();

        //Act
        var enumerator = ((IEnumerable)searchResult).GetEnumerator();

        //Assert
        enumerator.Should().NotBeNull();
    }

    [TestMethod]
    public void Group_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var searchResult = new StockSearchResult<GarbageItem>(Array.Empty<IndexedEntry<GarbageItem>>());

        //Act
        var result = searchResult.Group();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Group_WhenHasUniqueItems_DoNotGroup()
    {
        //Arrange
        var searchResult = Dummy.Create<StockSearchResult<GarbageItem>>();

        //Act
        var result = searchResult.Group();

        //Assert
        result.Should().BeEquivalentTo(searchResult.Select(x => new GroupedEntry<GarbageItem>(x.Item, x.Quantity, new List<int> { x.Index })));
    }

    [TestMethod]
    public void Group_WhenHasDuplicateItems_GroupThem()
    {
        //Arrange
        var entries = Dummy.CreateMany<IndexedEntry<GarbageItem>>().ToList();
        var duplicate = entries.GetRandom() with { Quantity = Dummy.Create<int>(), Index = Dummy.Create<int>() };
        entries.Add(duplicate);

        var searchResult = new StockSearchResult<GarbageItem>(entries);

        //Act
        var result = searchResult.Group();

        //Assert
        var expectedGroupedEntry = new GroupedEntry<GarbageItem>(duplicate.Item, entries.Where(x => x.Item == duplicate.Item).Sum(x => x.Quantity), entries.Where(x => x.Item.Equals(duplicate.Item)).Select(x => x.Index).ToList());
        var expectedEntries = searchResult.Where(x => !x.Item.Equals(duplicate.Item)).Select(x => new GroupedEntry<GarbageItem>(x.Item, x.Quantity, new List<int> { x.Index })).Append(expectedGroupedEntry).ToList();
        result.Should().BeEquivalentTo(expectedEntries);
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<StockSearchResult<IndexedEntry<GarbageItem>>>(Dummy, JsonSerializerOptions.WithInventoryConverters());
}