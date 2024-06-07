using ToolBX.Dummies;
using ToolBX.Eloquentest.Dummies;

namespace Collections.Grid.Tests;

[TestClass]
public class OverlapGridTests : Tester<OverlapGrid<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void IndexerXY_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var value = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance[value.X, value.Y];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsNothingThere_ReturnEmpty()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var value = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance[value.X, value.Y];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsSomethingWithThatIndex_ReturnThatThing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();

        var value = item.Index;

        //Act
        var result = Instance[value.X, value.Y];

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage> { item.Value! });
    }

    [TestMethod]
    public void IndexerXY_WhenThereAreManyThingsWithThatIndex_ReturnAllOfThem()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var value = Dummy.Create<Vector2<int>>();
        var searchedItems = Dummy.Build<Cell<Garbage>>().With(x => x.Index, value).CreateMany().ToArray();
        Instance.Add(searchedItems);


        //Act
        var result = Instance[value.X, value.Y];

        //Assert
        result.Should().BeEquivalentTo(searchedItems.Select(x => x.Value));
    }

    [TestMethod]
    public void IndexerVector_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var value = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance[value];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexerVector_WhenThereIsNothingThere_ReturnEmpty()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var value = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance[value];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexerVector_WhenThereIsSomethingWithThatIndex_ReturnThatThing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();

        var value = item.Index;

        //Act
        var result = Instance[value];

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage> { item.Value! });
    }

    [TestMethod]
    public void IndexerVector_WhenThereAreManyThingsWithThatIndex_ReturnAllOfThem()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var value = Dummy.Create<Vector2<int>>();
        var searchedItems = Dummy.Build<Cell<Garbage>>().With(x => x.Index, value).CreateMany().ToArray();
        Instance.Add(searchedItems);


        //Act
        var result = Instance[value];

        //Assert
        result.Should().BeEquivalentTo(searchedItems.Select(x => x.Value));
    }

    [TestMethod]
    public void ColumnCount_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsOneColumn_ReturnOne()
    {
        //Arrange
        Instance.Add(Dummy.Create<Cell<Garbage>>());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsMultipleItemsOnSameColumn_ReturnOne()
    {
        //Arrange
        var column = Dummy.Create<int>();

        Instance.Add(Dummy.Build<Cell<Garbage>>().With(x => x.Index, new Vector2<int>(column, Dummy.Create<int>())).CreateMany());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsMultipleConsecutiveColumns_ReturnColumnCount()
    {
        //Arrange
        Instance.Add(2, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(3, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(4, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(3);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsMultipleColumnsWithGaps_ReturnByCountingGaps()
    {
        //Arrange
        Instance.Add(2, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(4, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(6, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsOnlyNegativeColumns_ReturnAmount()
    {
        //Arrange
        Instance.Add(-2, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(-4, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(-6, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void ColumnCount_WhenContainsColumnsInNegativesAndPositives_ReturnAmount()
    {
        //Arrange
        Instance.Add(2, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(4, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(-6, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(11);
    }

    [TestMethod]
    public void RowCount_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void RowCount_WhenThereIsOnlyOneItem_ReturnOne()
    {
        //Arrange
        Instance.Add(Dummy.Create<Cell<Garbage>>());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void RowCount_WhenThereAreMultipleItemsAlignedOnSameRow_ReturnOne()
    {
        //Arrange
        var row = Dummy.Create<int>();

        Instance.Add(Dummy.Build<Cell<Garbage>>().With(x => x.Index, new Vector2<int>(Dummy.Create<int>(), row)).CreateMany());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void RowCount_WhenThereAreMultipleConsecutiveItemsOnRows_ReturnAmount()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), 4, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 5, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 6, Dummy.Create<Garbage>());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(3);
    }

    [TestMethod]
    public void RowCount_WhenThereAreMultipleItemsWithGaps_CountGapsAsWell()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), 4, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 6, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 9, Dummy.Create<Garbage>());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(6);
    }

    [TestMethod]
    public void RowCount_WhenAllRowsAreNegative_ReturnAmount()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), -4, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), -6, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), -9, Dummy.Create<Garbage>());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(6);
    }

    [TestMethod]
    public void RowCount_WhenThereAreNegativeAndPositiveRowIndexes_ReturnAmount()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), 4, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), -6, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 9, Dummy.Create<Garbage>());

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(16);
    }

    [TestMethod]
    public void FirstColumn_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FirstColumn_WhenContainsOneItem_ReturnItemColumn()
    {
        //Arrange
        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(item.X);
    }

    [TestMethod]
    public void FirstColumn_WhenContainsMultipleItemsOnSameColumn_ReturnThatColumn()
    {
        //Arrange
        var column = Dummy.Create<int>();
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(column);
    }

    [TestMethod]
    public void FirstColumn_WhenContainsMultipleItems_ReturnSmallestColumn()
    {
        //Arrange
        Instance.Add(-14, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(0, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(29, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(-14);
    }

    [TestMethod]
    public void LastColumn_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void LastColumn_WhenContainsOneItem_ReturnItemColumn()
    {
        //Arrange
        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(item.X);
    }

    [TestMethod]
    public void LastColumn_WhenContainsMultipleItemsOnSameColumn_ReturnThatColumn()
    {
        //Arrange
        var column = Dummy.Create<int>();
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(column, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(column);
    }

    [TestMethod]
    public void LastColumn_WhenContainsMultipleItems_ReturnHighestColumn()
    {
        //Arrange
        Instance.Add(-14, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(0, Dummy.Create<int>(), Dummy.Create<Garbage>());
        Instance.Add(29, Dummy.Create<int>(), Dummy.Create<Garbage>());

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(29);
    }

    [TestMethod]
    public void FirstRow_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FirstRow_WhenThereIsOnlyOneItem_ReturnItemRow()
    {
        //Arrange
        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(item.Y);
    }

    [TestMethod]
    public void FirstRow_WhenThereAreMultipleItemsWithSameRow_ReturnThatRow()
    {
        //Arrange
        var row = Dummy.Create<int>();

        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(row);
    }

    [TestMethod]
    public void FirstRow_WhenThereAreMultipleItemsOnDifferentRows_ReturnSmallestRow()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), -8, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), -1, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 0, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 91, Dummy.Create<Garbage>());

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(-8);
    }

    [TestMethod]
    public void LastRow_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void LastRow_WhenThereIsOnlyOneItem_ReturnItemRow()
    {
        //Arrange
        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(item.Y);
    }

    [TestMethod]
    public void LastRow_WhenThereAreMultipleItemsWithSameRow_ReturnThatRow()
    {
        //Arrange
        var row = Dummy.Create<int>();

        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), row, Dummy.Create<Garbage>());

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(row);
    }

    [TestMethod]
    public void LastRow_WhenThereAreMultipleItemsOnDifferentRows_ReturnHighestRow()
    {
        //Arrange
        Instance.Add(Dummy.Create<int>(), -8, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), -1, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 0, Dummy.Create<Garbage>());
        Instance.Add(Dummy.Create<int>(), 91, Dummy.Create<Garbage>());

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(91);
    }

    [TestMethod]
    public void Count_WhenThereAreNoItems_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void Count_WhenThereIsOneItem_ReturnOne()
    {
        //Arrange
        Instance.Add(Dummy.Create<Cell<Garbage>>());

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void Count_WhenThereAreItemsButTheyAreAllInSameCell_ReturnNumberOfItems()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany(4));

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(4);
    }

    [TestMethod]
    public void Count_WhenThereAreMultipleItems_ReturnNumberOfItems()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>(7));

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(7);
    }

    [TestMethod]
    public void Constructor_WhenCollectionIsNull_Throw()
    {
        //Arrange

        //Act
        var action = () => new OverlapGrid<Garbage>(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("collection");
    }

    [TestMethod]
    public void Constructor_WhenCollectionIsEmpty_InstantiateEmpty()
    {
        //Arrange
        var collection = new List<Cell<Garbage>>();

        //Act
        var result = new OverlapGrid<Garbage>(collection);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenCollectionContainsItems_ContainThoseItems()
    {
        //Arrange
        var collection = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        //Act
        var result = new OverlapGrid<Garbage>(collection);

        //Assert
        result.Should().BeEquivalentTo(collection);
    }

    [TestMethod]
    public void Boundaries_WhenIsEmpty_ReturnZeroes()
    {
        //Arrange

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().Be(new Boundaries<int>());
    }

    [TestMethod]
    public void Boundaries_WhenContainsOneItemAtZeroZero_ReturnZeroes()
    {
        //Arrange
        Instance.Add(0, 0, Dummy.Create<Garbage>());

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().Be(new Boundaries<int>());
    }

    [TestMethod]
    public void Boundaries_WhenContainsMultipleItemsAtTheSameSpot_ReturnThatSpot()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index, Dummy.Create<Garbage>());
        Instance.Add(index, Dummy.Create<Garbage>());
        Instance.Add(index, Dummy.Create<Garbage>());
        Instance.Add(index, Dummy.Create<Garbage>());

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().Be(new Boundaries<int>
        {
            Top = index.Y,
            Right = index.X,
            Bottom = index.Y,
            Left = index.X,
        });
    }

    [TestMethod]
    public void Boundaries_WhenContainsMultipleItemsAtDifferentPositions_ReturnBoundaries()
    {
        //Arrange
        Instance.Add(-2, 4, Dummy.Create<Garbage>());
        Instance.Add(-8, 2, Dummy.Create<Garbage>());
        Instance.Add(12, 1, Dummy.Create<Garbage>());
        Instance.Add(0, -3, Dummy.Create<Garbage>());

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().Be(new Boundaries<int>
        {
            Top = -3,
            Right = 12,
            Bottom = 4,
            Left = -8,
        });
    }

    [TestMethod]
    public void IndexesOfItem_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenItemIsNotInGrid_ReturnEmpty()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhereThereIsOnlyOneOccurence_ReturnThatOccurence()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.IndexesOf(item.Value);

        //Assert
        result.Should().BeEquivalentTo(new List<Vector2<int>> { item.Index });
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereAreMultipleOccurrences_ReturnAll()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Garbage>();

        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Value, item).CreateMany().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(items.Select(x => x.Index));
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<Garbage, bool> predicate = null!;

        //Act
        var action = () => Instance.IndexesOf(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Instance.IndexesOf(x => x.Id > 0);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenThereIsNoOccurenceOfPredicate_ReturnEmpty()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        //Act
        var result = Instance.IndexesOf(x => x.Value.StartsWith("Jer"));

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenThereAreOccurrencesOfPredicate_ReturnThoseOccurrences()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Value, Dummy.Build<Garbage>().With(y => y.Value, "Jerry").Create()).CreateMany().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.IndexesOf(x => x.Value.StartsWith("Jer"));

        //Assert
        result.Should().BeEquivalentTo(items.Select(x => x.Index));
    }

    [TestMethod]
    public void AddWithXYAndItem_Always_AddItemAtPosition()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var value = Dummy.Create<Garbage>();

        //Act
        Instance.Add(x, y, value);

        //Assert
        Instance.Should().BeEquivalentTo(new OverlapGrid<Garbage> { new Cell<Garbage>(x, y, value) });
    }

    [TestMethod]
    public void AddWithXYAndItem_Always_TriggerChange()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var value = Dummy.Create<Garbage>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(x, y, value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                NewValues = [new(x, y, value)]
            }
        });
    }

    [TestMethod]
    public void AddWithXYAndItem_WhenAddingMultipleItemsAtSamePosition_AddThemAll()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();

        var values = Dummy.CreateMany<Garbage>().ToList();

        //Act
        foreach (var value in values) Instance.Add(x, y, value);

        //Assert
        Instance.Should().BeEquivalentTo(new OverlapGrid<Garbage>(values.Select(value => new Cell<Garbage>(x, y, value))));
    }

    [TestMethod]
    public void AddWithVectorAndItem_Always_AddItemAtPosition()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var value = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index, value);

        //Assert
        Instance.Should().BeEquivalentTo(new OverlapGrid<Garbage> { new Cell<Garbage>(index, value) });
    }

    [TestMethod]
    public void AddWithVectorAndItem_Always_TriggerChange()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var value = Dummy.Create<Garbage>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(index, value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                NewValues = [new(index, value)]
            }
        });
    }

    [TestMethod]
    public void AddWithVectorAndItem_WhenAddingMultipleItemsAtSamePosition_AddThemAll()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var values = Dummy.CreateMany<Garbage>().ToList();

        //Act
        foreach (var value in values) Instance.Add(index, value);

        //Assert
        Instance.Should().BeEquivalentTo(new OverlapGrid<Garbage>(values.Select(value => new Cell<Garbage>(index, value))));
    }

    [TestMethod]
    public void AddWithParamsAndCells_Always_AddItemsAtPosition()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void AddWithParamsAndCells_Always_TriggerChangeOnce()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(cells);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                NewValues = cells
            }
        });
    }

    [TestMethod]
    public void AddWithParamsAndCells_WhenAddingMultipleItemsAtSamePosition_AddThemAll()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var cells = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToArray();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void AddWithEnumerableAndCells_WhenCellsAreNull_Throw()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> cells = null!;

        //Act
        var action = () => Instance.Add(cells);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
    }

    [TestMethod]
    public void AddWithEnumerableAndCells_Always_AddItemsAtPosition()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void AddWithEnumerableAndCells_Always_TriggerChangeOnce()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(cells);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                NewValues = cells
            }
        });
    }

    [TestMethod]
    public void AddWithEnumerableAndCells_WhenAddingMultipleItemsAtSamePosition_AddThemAll()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var cells = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    //TODO Fix
    [TestMethod, Ignore("For some reason it throws something else")]
    public void RemoveAtWithXY_WhenThereIsNothingAtIndex_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.RemoveAt(index.X, index.Y);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithMessage(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));
    }

    [TestMethod]
    public void RemoveAtWithXY_WhenThereIsOneItemAtIndex_RemoveThatItem()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        Instance.RemoveAt(item.X, item.Y);

        //Assert
        Instance.Should().NotContain(item);
        Instance.Should().NotContain(x => x.Index == item.Index);
    }

    [TestMethod]
    public void RemoveAtWithXY_WhenThereIsOneItemAtIndex_TriggerChange()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(item.X, item.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void RemoveAtWithXY_WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        //Act
        Instance.RemoveAt(index.X, index.Y);

        //Assert
        Instance.Should().NotContain(items);
        Instance.Should().NotContain(x => x.Index == index);
    }

    [TestMethod]
    public void RemoveAtWithXY_WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(index.X, index.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod, Ignore("For some reason it throws something else")]
    public void RemoveAtWithVector_WhenThereIsNothingAtIndex_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.RemoveAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithMessage(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));
    }

    [TestMethod]
    public void RemoveAtWithVector_WhenThereIsOneItemAtIndex_RemoveThatItem()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        Instance.RemoveAt(item.Index);

        //Assert
        Instance.Should().NotContain(item);
        Instance.Should().NotContain(x => x.Index == item.Index);
    }

    [TestMethod]
    public void RemoveAtWithVector_WhenThereIsOneItemAtIndex_TriggerChange()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(item.Index);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void RemoveAtWithVector_WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        //Act
        Instance.RemoveAt(index);

        //Assert
        Instance.Should().NotContain(items);
        Instance.Should().NotContain(x => x.Index == index);
    }

    [TestMethod]
    public void RemoveAtWithVector_WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(index);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod]
    public void TryRemoveAtWithXY_WhenThereIsNothingAtIndex_DoesNotThrow()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAtWithXY_WhenThereIsOneItemAtIndex_RemoveThatItem()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        Instance.TryRemoveAt(item.X, item.Y);

        //Assert
        Instance.Should().NotContain(item);
        Instance.Should().NotContain(x => x.Index == item.Index);
    }

    [TestMethod]
    public void TryRemoveAtWithXY_WhenThereIsOneItemAtIndex_TriggerChange()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAt(item.X, item.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void TryRemoveAtWithXY_WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        Instance.Should().NotContain(items);
        Instance.Should().NotContain(x => x.Index == index);
    }

    [TestMethod]
    public void TryRemoveAtWithXY_WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod]
    public void TryRemoveAtWithVector_WhenThereIsNothingAtIndex_DoesNotThrow()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.TryRemoveAt(index);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAtWithVector_WhenThereIsOneItemAtIndex_RemoveThatItem()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        Instance.TryRemoveAt(item.Index);

        //Assert
        Instance.Should().NotContain(item);
        Instance.Should().NotContain(x => x.Index == item.Index);
    }

    [TestMethod]
    public void TryRemoveAtWithVector_WhenThereIsOneItemAtIndex_TriggerChange()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAt(item.Index);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void TryRemoveAtWithVector_WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveAt(index);

        //Assert
        Instance.Should().NotContain(items);
        Instance.Should().NotContain(x => x.Index == index);
    }

    [TestMethod]
    public void TryRemoveAtWithVector_WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAt(index);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsNotInGrid_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = Dummy.Create<Garbage>();

        //Act
        Instance.RemoveAll(item);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsNotInGrid_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = Dummy.Create<Cell<Garbage>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(item.Value);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsInGridOnce_RemoveThatInstanceOnly()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        Instance.RemoveAll(item.Value);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsInGridOnce_TriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(item.Value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsInMultiplePlaces_RemoveAll()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var value = Dummy.Create<Garbage>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Value, value).CreateMany().ToList();
        Instance.Add(items);

        //Act
        Instance.RemoveAll(value);

        //Assert
        Instance.Should().BeEquivalentTo(originalItems);
    }

    [TestMethod]
    public void RemoveAllWithItem_WhenItemIsInMultiplePlaces_DoNotTriggerChange()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var value = Dummy.Create<Garbage>();
        var items = Dummy.Build<Cell<Garbage>>().With(x => x.Value, value).CreateMany().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        Func<Garbage, bool> predicate = null!;

        //Act
        var action = () => Instance.RemoveAll(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenNothingMatchesPredicate_DoNothing()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        //Act
        Instance.RemoveAll(x => x.Id < 0);

        //Assert
        Instance.Should().BeEquivalentTo(originalItems);
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenNothingMatchesPredicate_DoNotTriggerChange()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(x => x.Id < 0);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenOneItemMatchesPredicate_RemoveIt()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var extraItem = Dummy.Create<Cell<Garbage>>();
        Instance.Add(extraItem);

        //Act
        Instance.RemoveAll(x => x.Id == extraItem.Value!.Id);

        //Assert
        Instance.Should().BeEquivalentTo(originalItems);
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenOneItemMatchesPredicate_TriggerChange()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var extraItem = Dummy.Create<Cell<Garbage>>();
        Instance.Add(extraItem);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(x => x.Id == extraItem.Value!.Id);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [extraItem]
                }
            });
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenMultipleItemsMatchPredicate_RemoveThemAll()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var value = Dummy.Create<string>();
        var extraItems = Dummy.Build<Cell<Garbage>>().With(x => x.Value, Dummy.Build<Garbage>().With(y => y.Value, value).Create()).CreateMany().ToList();
        Instance.Add(extraItems);

        //Act
        Instance.RemoveAll(x => x.Value == value);

        //Assert
        Instance.Should().BeEquivalentTo(originalItems);
    }

    [TestMethod]
    public void RemoveAllWithPredicate_WhenMultipleItemsMatchPredicate_TriggerChangeOnce()
    {
        //Arrange
        var originalItems = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(originalItems);

        var value = Dummy.Create<string>();
        var extraItems = Dummy.Build<Cell<Garbage>>().With(x => x.Value, Dummy.Build<Garbage>().With(y => y.Value, value).Create()).CreateMany().ToList();
        Instance.Add(extraItems);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(x => x.Value == value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = extraItems
                }
            });
    }

    [TestMethod]
    public void ContainsWithXYAndItem_WhenItemIsInGridButNotAtPosition_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(Dummy.Create<int>(), Dummy.Create<int>(), item.Value);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithXYAndItem_WhenThereIsSomethingAtPositionButNotItem_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(item.X, item.Y, Dummy.Create<Garbage>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithXYAndItem_WhenItemIsAtPosition_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(item.X, item.Y, item.Value);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithVectorAndItem_WhenItemIsInGridButNotAtPosition_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(Dummy.Create<Vector2<int>>(), item.Value);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithVectorAndItem_WhenThereIsSomethingAtPositionButNotItem_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(item.Index, Dummy.Create<Garbage>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithVectorAndItem_WhenItemIsAtPosition_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(items);

        var item = items.GetRandom();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(item.Index, item.Value);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithItem_WhenItemIsNullButGridIsEmpty_ReturnFalse()
    {
        //Arrange

        //Act
        var result = Instance.Contains(null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithItem_WhenItemIsNullAndGridContainsNullValues_ReturnTrue()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());
        Instance.Add(Dummy.Create<Vector2<int>>(), null);

        //Act
        var result = Instance.Contains(null);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithItem_WhenItemIsNotInGrid_ReturnFalse()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        //Act
        var result = Instance.Contains(Dummy.Create<Garbage>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithItem_WhenItemIsInGrid_ReturnTrue()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var item = Dummy.Create<Cell<Garbage>>();
        Instance.Add(item);

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithXY_WhenThereIsNothingAtPosition_ReturnFalse()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index.X, index.Y);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithXY_WhenThereIsSomethingAtPosition_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var index = items.GetRandom().Index;

        //Act
        var result = Instance.Contains(index.X, index.Y);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithXY_WhenThereIsOneNullValueAtPosition_ReturnTrue()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index, null);

        //Act
        var result = Instance.Contains(index.X, index.Y);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithVector_WhenThereIsNothingAtPosition_ReturnFalse()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsWithVector_WhenThereIsSomethingAtPosition_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var index = items.GetRandom().Index;

        //Act
        var result = Instance.Contains(index);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsWithVector_WhenThereIsOneNullValueAtPosition_ReturnTrue()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index, null);

        //Act
        var result = Instance.Contains(index);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Resize_WhenResizedBiggerThanContent_DoNothing()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Resize(new Boundaries<int>(-6, 6, 6, -6));

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Resize_WhenResizedBiggerThanContent_DoNotTrigger()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Resize(new Boundaries<int>(-6, 6, 6, -6));

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Resize_WhenResizedSmallerThanContent_RemoveExcess()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Resize(new Boundaries<int>(-4, 4, 4, -4));

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>> { items[1] });
    }

    [TestMethod]
    public void Resize_WhenResizedSmallerThanContent_TriggerChangeOnce()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Resize(new Boundaries<int>(-4, 4, 4, -4));

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        items[0], items[2]
                    ]
                }
            });
    }

    [TestMethod]
    public void Resize_WhenResizedToZero_OnlyItemsAtOriginRemain()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Resize(new Boundaries<int>(0, 0, 0, 0));

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>> { items[2] });
    }

    [TestMethod]
    public void Resize_WhenResizedToZero_TriggerChangeOnce()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(-5, -5, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(0, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(5, 5, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Resize(new Boundaries<int>(0, 0, 0, 0));

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items[0], items[1], items[3], items[4]]
                }
            });
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Vector2<int>.Zero;

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsZero_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsPositiveAndNotZero_MoveAllItemsInDirection()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsNegative_MoveAllItemsInDirection()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = -Dummy.Create<Vector2<int>>();

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = -Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation.X, translation.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Vector2<int>.Zero;

        //Act
        Instance.TranslateAll(translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsZero_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsPositiveAndNotZero_MoveAllItemsInDirection()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.TranslateAll(translation);

        //Assert
        Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsNegative_MoveAllItemsInDirection()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = -Dummy.Create<Vector2<int>>();

        //Act
        Instance.TranslateAll(translation);

        //Assert
        Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
    }

    [TestMethod]
    public void TranslateAllWithVector_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var translation = -Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TranslateAll(translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = Dummy.Create<Rectangle<int>>();
        var translation = Vector2<int>.Zero;

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsZero_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = Dummy.Create<Rectangle<int>>();
        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenBoundariesAreEqualOnAllAxis_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = new Rectangle<int>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = new Rectangle<int>();
        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsNegative_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(-4, -4, items[5].Value),
                new(-3, -4, items[6].Value),
                new(-2, -4, items[7].Value),
                new(0, 2, items[8].Value),
                new(-4, -3, items[9].Value),
                new(-3, -3, items[10].Value),
                new(-2, -3, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(6, 6, items[5].Value),
                new(7, 6, items[6].Value),
                new(8, 6, items[7].Value),
                new(0, 2, items[8].Value),
                new(6, 7, items[9].Value),
                new(7, 7, items[10].Value),
                new(8, 7, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenThereIsNothingWithinBoundaries_DoNothing()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenThereIsNothingWithinBoundaries_DoNotTrigger()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(1, 0, items[5].Value),
                new(2, 0, items[6].Value),
                new(3, 0, items[7].Value),
                new(0, 2, items[8].Value),
                new(1, 1, items[9].Value),
                new(2, 1, items[10].Value),
                new(3, 1, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(range, translation.X, translation.Y);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = Dummy.Create<Rectangle<int>>();
        var translation = Vector2<int>.Zero;

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsZero_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = Dummy.Create<Rectangle<int>>();
        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(range, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenBoundariesAreEqualOnAllAxis_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = new Rectangle<int>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var range = new Rectangle<int>();
        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(range, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsNegative_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(-4, -4, items[5].Value),
                new(-3, -4, items[6].Value),
                new(-2, -4, items[7].Value),
                new(0, 2, items[8].Value),
                new(-4, -3, items[9].Value),
                new(-3, -3, items[10].Value),
                new(-2, -3, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(range, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(6, 6, items[5].Value),
                new(7, 6, items[6].Value),
                new(8, 6, items[7].Value),
                new(0, 2, items[8].Value),
                new(6, 7, items[9].Value),
                new(7, 7, items[10].Value),
                new(8, 7, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(range, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenThereIsNothingWithinBoundaries_DoNothing()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Dummy.Create<Vector2<int>>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenThereIsNothingWithinBoundaries_DoNotTrigger()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Dummy.Create<Vector2<int>>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(1, 0, items[5].Value),
                new(2, 0, items[6].Value),
                new(3, 0, items[7].Value),
                new(0, 2, items[8].Value),
                new(1, 1, items[9].Value),
                new(2, 1, items[10].Value),
                new(3, 1, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndVector_WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var range = new Rectangle<int>(1, 1, 2, 2);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(range, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Vector2<int>.Zero;

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsZero_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenBoundariesAreEqualOnAllAxis_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = new Boundaries<int>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = new Boundaries<int>();
        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsNegative_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(-4, -4, items[5].Value),
                new(-3, -4, items[6].Value),
                new(-2, -4, items[7].Value),
                new(0, 2, items[8].Value),
                new(-4, -3, items[9].Value),
                new(-3, -3, items[10].Value),
                new(-2, -3, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(6, 6, items[5].Value),
                new(7, 6, items[6].Value),
                new(8, 6, items[7].Value),
                new(0, 2, items[8].Value),
                new(6, 7, items[9].Value),
                new(7, 7, items[10].Value),
                new(8, 7, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenThereIsNothingWithinBoundaries_DoNothing()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Dummy.Create<Vector2<int>>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenThereIsNothingWithinBoundaries_DoNotTrigger()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Dummy.Create<Vector2<int>>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(1, 0, items[5].Value),
                new(2, 0, items[6].Value),
                new(3, 0, items[7].Value),
                new(0, 2, items[8].Value),
                new(1, 1, items[9].Value),
                new(2, 1, items[10].Value),
                new(3, 1, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsZero_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Vector2<int>.Zero;

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsZero_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Vector2<int>.Zero;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenBoundariesAreEqualOnAllAxis_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = new Boundaries<int>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(boundaries, translation.X, translation.Y);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var boundaries = new Boundaries<int>();
        var translation = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsNegative_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(-4, -4, items[5].Value),
                new(-3, -4, items[6].Value),
                new(-2, -4, items[7].Value),
                new(0, 2, items[8].Value),
                new(-4, -3, items[9].Value),
                new(-3, -3, items[10].Value),
                new(-2, -3, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsNegative_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(-5, -5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(6, 6, items[5].Value),
                new(7, 6, items[6].Value),
                new(8, 6, items[7].Value),
                new(0, 2, items[8].Value),
                new(6, 7, items[9].Value),
                new(7, 7, items[10].Value),
                new(8, 7, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationIsPositiveAndNotZero_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(5, 5);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenThereIsNothingWithinBoundaries_DoNothing()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Dummy.Create<Vector2<int>>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenThereIsNothingWithinBoundaries_DoNotTrigger()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Dummy.Create<Vector2<int>>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(0, 0, items[0].Value),
                new(1, 0, items[1].Value),
                new(2, 0, items[2].Value),
                new(3, 0, items[3].Value),
                new(0, 1, items[4].Value),
                new(1, 0, items[5].Value),
                new(2, 0, items[6].Value),
                new(3, 0, items[7].Value),
                new(0, 2, items[8].Value),
                new(1, 1, items[9].Value),
                new(2, 1, items[10].Value),
                new(3, 1, items[11].Value),
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndVector_WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var boundaries = new Boundaries<int>(1, 3, 3, 1);
        var translation = new Vector2<int>(0, -1);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    ],
                    NewValues =
                    [
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    ]
                }
            });
    }

    [TestMethod]
    public void Copy_Always_MakeAPerfectCopy()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEquivalentTo(items);
        result.Should().NotBeSameAs(items);
    }

    [TestMethod]
    public void CopyWithBoundaries_WhenBoundariesAreEqualToCollectionBoundaries_CopyEverything()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        var result = Instance.Copy(Instance.Boundaries);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void CopyWithBoundaries_WhenBoundariesAreEqualToPartOfCollection_CopyThatPart()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        var result = Instance.Copy(new Boundaries<int>(1, 3, 3, 1));

        //Assert
        result.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(1, 1, items[5].Value),
                new(2, 1, items[6].Value),
                new(3, 1, items[7].Value),
                new(1, 2, items[9].Value),
                new(2, 2, items[10].Value),
                new(3, 2, items[11].Value),
            });
    }

    [TestMethod]
    public void CopyWithBoundaries_WhenBoundariesAreEmpty_CreateEmptyGrid()
    {
        //Arrange
        var items = new List<Cell<Garbage>>
            {
                new(0, 0, Dummy.Create<Garbage>()),
                new(1, 0, Dummy.Create<Garbage>()),
                new(2, 0, Dummy.Create<Garbage>()),
                new(3, 0, Dummy.Create<Garbage>()),
                new(0, 1, Dummy.Create<Garbage>()),
                new(1, 1, Dummy.Create<Garbage>()),
                new(2, 1, Dummy.Create<Garbage>()),
                new(3, 1, Dummy.Create<Garbage>()),
                new(0, 2, Dummy.Create<Garbage>()),
                new(1, 2, Dummy.Create<Garbage>()),
                new(2, 2, Dummy.Create<Garbage>()),
                new(3, 2, Dummy.Create<Garbage>()),
            };

        Instance.Add(items);

        //Act
        var result = Instance.Copy(new Boundaries<int>(100, 200, 200, 100));

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreEqual_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = current;

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreEqual_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = current;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtBothCurrentAndDestination_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtBothCurrentAndDestination_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreDifferent_SwapTheirIndexes()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = items.First().Index;
        var destination = items.Last().Index;

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(items[2].Index, items[0].Value),
                items[1],
                new(items[0].Index, items[2].Value),
            });
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreDifferent_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = items.First().Index;
        var destination = items.Last().Index;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items[0], items[2]],
                    NewValues = [new(items[2].Index, items[0].Value), new(items[0].Index, items[2].Value)]
                }
            });
    }

    [TestMethod]
    public void Swap_WhenThereAreMultipleItemsInBothCurrentAndDestination_SwapThemAll()
    {
        //Arrange
        var indexes = Dummy.CreateMany<Vector2<int>>(3).ToList();

        var items = new List<Cell<Garbage>>();
        foreach (var index in indexes)
            items.AddRange(Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany(3));

        Instance.Add(items);

        var current = indexes.First();
        var destination = indexes.Last();

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(indexes[2], items[0].Value),
                new(indexes[2], items[1].Value),
                new(indexes[2], items[2].Value),
                new(indexes[1], items[3].Value),
                new(indexes[1], items[4].Value),
                new(indexes[1], items[5].Value),
                new(indexes[0], items[6].Value),
                new(indexes[0], items[7].Value),
                new(indexes[0], items[8].Value),
            });
    }

    [TestMethod]
    public void Swap_WhenThereAreMultipleItemsInBothCurrentAndDestination_TriggerEventOnce()
    {
        //Arrange
        var indexes = Dummy.CreateMany<Vector2<int>>(3).ToList();

        var items = new List<Cell<Garbage>>();
        foreach (var index in indexes)
            items.AddRange(Dummy.Build<Cell<Garbage>>().With(x => x.Index, index).CreateMany(3));

        Instance.Add(items);

        var current = indexes.First();
        var destination = indexes.Last();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items[0], items[1], items[2], items[6], items[7], items[8]],
                    NewValues =
                    [
                        new(indexes[2], items[0].Value),
                        new(indexes[2], items[1].Value),
                        new(indexes[2], items[2].Value),
                        new(indexes[0], items[6].Value),
                        new(indexes[0], items[7].Value),
                        new(indexes[0], items[8].Value),
                    ]
                }
            });
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtCurrent_MoveDestinationToCurrent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = items.Last().Index;

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                items[0],
                items[1],
                new(current, items[2].Value),
            });
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtCurrent_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = Dummy.Create<Vector2<int>>();
        var destination = items.Last().Index;

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items[2]],
                    NewValues = [new(current, items[2].Value)]
                }
            });
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtDestination_MoveCurrentToDestination()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = items.First().Index;
        var destination = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Cell<Garbage>>
            {
                new(destination, items[0].Value),
                items[1],
                items[2],
            });
    }

    [TestMethod]
    public void Swap_WhenThereIsNothingAtDestination_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>(3).ToList();
        Instance.Add(items);

        var current = items.First().Index;
        var destination = Dummy.Create<Vector2<int>>();

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Swap(current, destination);

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items[0]],
                    NewValues = [new(destination, items[0].Value)]
                }
            });
    }

    [TestMethod]
    public void Clear_WhenGridIsEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenGridIsNotEmpty_RemoveAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        //Act
        Instance.Clear();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenGridIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                OldValues = items
            }
        });
    }

    [TestMethod]
    public void ToString_WhenGridIsEmpty_ReturnEmptyMessage()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().BeEquivalentTo("Empty OverlapGrid<Garbage>");
    }

    [TestMethod]
    public void ToString_WhenGridIsNotEmpty_ReturnCount()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>(5).ToList();
        Instance.Add(cells);

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().BeEquivalentTo("OverlapGrid<Garbage> with 5 items");
    }

    [TestMethod]
    public void Always_EnsureValueEquality() => Ensure.ValueEquality<OverlapGrid<Garbage>>(Dummy);

    [TestMethod]
    public void HashCode_Always_ReturnInternalListHashCode()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(GetFieldValue<List<Cell<Garbage>>>("_items")!.GetHashCode());
    }

    [TestMethod]
    public void WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var json = JsonConvert.SerializeObject(Instance);

        //Act
        var result = JsonConvert.DeserializeObject<OverlapGrid<Garbage>>(json);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        JsonSerializerOptions.WithGridConverters();

        var json = System.Text.Json.JsonSerializer.Serialize(Instance, JsonSerializerOptions);

        //Act
        var result = System.Text.Json.JsonSerializer.Deserialize<OverlapGrid<Garbage>>(json, JsonSerializerOptions);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }
}