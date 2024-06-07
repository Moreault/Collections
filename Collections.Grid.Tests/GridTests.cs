using Collections.Grid.Tests.Customizations;

namespace Collections.Grid.Tests;

[TestClass]
public class GridTests : Tester<Grid<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
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
    public void ColumnCount_WhenHasOnlyOneItemAtColumnZero_ReturnOne()
    {
        //Arrange
        Instance[0, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void ColumnCount_WhenHasOneItemAtNegativeColumnIndex_ReturnDifferenceBetweenThatAndZero()
    {
        //Arrange
        Instance[-3, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(4);
    }

    [TestMethod]
    public void ColumnCount_WhenHasOneItemAtColumnIndexGreaterThanZero_ReturnNumberOfColumns()
    {
        //Arrange
        Instance[5, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(6);
    }

    [TestMethod]
    public void ColumnCount_WhenHasOneItemInNegativeColumnIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
    {
        //Arrange
        Instance[-3, Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[5, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(9);
    }

    [TestMethod]
    public void ColumnCount_WhenHasABunchOfColumns_ReturnDifferenceBetweenMinimumAndMaximum()
    {
        //Arrange
        Instance[-3, Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[-5, Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[5, Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[7, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.ColumnCount;

        //Assert
        result.Should().Be(13);
    }

    [TestMethod]
    public void WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void WhenHasOnlyOneItemAtRowZero_ReturnOne()
    {
        //Arrange
        Instance[Dummy.Create<int>(), 0] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void WhenHasOneItemAtNegativeRowIndex_ReturnDifferenceBetweenThatAndZero()
    {
        //Arrange
        Instance[Dummy.Create<int>(), -3] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(4);
    }

    [TestMethod]
    public void WhenHasOneItemAtRowIndexGreaterThanZero_ReturnNumberOfRows()
    {
        //Arrange
        Instance[Dummy.Create<int>(), 5] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(6);
    }

    [TestMethod]
    public void WhenHasOneItemInNegativeRowIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
    {
        //Arrange
        Instance[Dummy.Create<int>(), -3] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), 5] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(9);
    }

    [TestMethod]
    public void WhenHasABunchOfRows_ReturnDifferenceBetweenMinimumAndMaximum()
    {
        //Arrange
        Instance[Dummy.Create<int>(), -3] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), -5] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), 5] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), 7] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.RowCount;

        //Assert
        result.Should().Be(13);
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
    public void FirstColumn_WhenOnlyContainsItemsAtColumnZero_ReturnZero()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[0, Dummy.Create<int>()] = item;

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FirstColumn_WhenContainsSomethingInTheNegatives_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = item;

        var negativeIndex = -Dummy.Create<int>();
        Instance[negativeIndex, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(negativeIndex);
    }

    [TestMethod]
    public void FirstColumn_WhenContainsSomethingGreaterThanZero_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Number.Between(10, 30).Create(), Dummy.Create<int>()] = item;

        Instance[5, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.FirstColumn;

        //Assert
        result.Should().Be(5);
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
    public void LastColumn_WhenOnlyContainsItemsAtColumnZero_ReturnZero()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[0, Dummy.Create<int>()] = item;

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void LastColumn_WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[-20, Dummy.Create<int>()] = item;

        var negativeIndex = -5;
        Instance[negativeIndex, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(negativeIndex);
    }

    [TestMethod]
    public void WhenHighestIsSomethingGreaterThanZero_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Number.Between(0, 20).Create(), Dummy.Create<int>()] = item;

        Instance[35, Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.LastColumn;

        //Assert
        result.Should().Be(35);
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
    public void FirstRow_WhenOnlyContainsItemsAtRowZero_ReturnZero()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), 0] = item;

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FirstRow_WhenContainsSomethingInTheNegatives_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = item;

        var negativeIndex = -Dummy.Create<int>();
        Instance[Dummy.Create<int>(), negativeIndex] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(negativeIndex);
    }

    [TestMethod]
    [Ignore("Flaky")]
    public void FirstRow_WhenContainsSomethingGreaterThanZero_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Number.Between(10, 30).Create(), Dummy.Create<int>()] = item;

        Instance[Dummy.Create<int>(), 5] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.FirstRow;

        //Assert
        result.Should().Be(5);
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
    public void LastRow_WhenOnlyContainsItemsAtRowZero_ReturnZero()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), 0] = item;

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void LastRow_WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), -15] = item;

        var negativeIndex = -2;
        Instance[Dummy.Create<int>(), negativeIndex] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(negativeIndex);
    }

    [TestMethod]
    public void LastRow_WhenHighestIsSomethingGreaterThanZero_ReturnThatIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>();
        foreach (var item in items)
            Instance[Dummy.Create<int>(), Dummy.Number.Between(10, 30).Create()] = item;

        Instance[Dummy.Create<int>(), 35] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.LastRow;

        //Assert
        result.Should().Be(35);
    }

    [TestMethod]
    public void Count_WhenIsEmpty_ReturnZero()
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
        Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void Count_WhenThereAreTwoItemsAtOppositeSidesOfTheGrid_ReturnTwo()
    {
        //Arrange
        Instance[-10, -10] = Dummy.Create<Garbage>();
        Instance[10, 10] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void Count_WhenThereIsABunchOfItems_ReturnExactNumberOfItemsRegardlessOfColumnAndRowCount()
    {
        //Arrange
        Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();
        Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(3);
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsNothingAtIndex_ReturnDefaultValue()
    {
        //Arrange

        //Act
        var result = Instance[2, 3];

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsValueAtIndex_ReturnValue()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var value = Dummy.Create<Garbage>();
        Instance[x, y] = value;

        //Act
        var result = Instance[x, y];

        //Assert
        result.Should().Be(value);
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsNothingAtGivenIndex_AddValueAtIndex()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var value = Dummy.Create<Garbage>();

        //Act
        Instance[x, y] = value;

        //Assert
        Instance[x, y].Should().Be(value);
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsNothingAtGivenIndex_TriggerOnChange()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var value = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance[x, y] = value;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(x, y, value)] }
            });
    }

    [TestMethod]
    public void IndexerXY_WhenThereIsSomethingAtGivenIndex_ReplaceExistingValueByNewValue()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var oldValue = Dummy.Create<Garbage>();
        Instance[x, y] = oldValue;
        var newValue = Dummy.Create<Garbage>();

        //Act
        Instance[x, y] = newValue;

        //Assert
        Instance[x, y].Should().Be(newValue);

    }

    [TestMethod]
    public void IndexerXY_WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
    {
        //Arrange
        var x = Dummy.Create<int>();
        var y = Dummy.Create<int>();
        var oldValue = Dummy.Create<Garbage>();
        Instance[x, y] = oldValue;
        var newValue = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance[x, y] = newValue;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues = [new(x, y, newValue)],
                    OldValues = [new(x, y, oldValue)]
                }
            });
    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsNothingAtIndex_ReturnDefaultValue()
    {
        //Arrange

        //Act
        var result = Instance[new Vector2<int>(2, 3)];

        //Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsValueAtIndex_ReturnValue()
    {
        //Arrange
        var coordinates = Dummy.Create<Vector2<int>>();
        var value = Dummy.Create<Garbage>();
        Instance[coordinates] = value;

        //Act
        var result = Instance[coordinates];

        //Assert
        result.Should().Be(value);
    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsNothingAtGivenIndex_AddValueAtIndex()
    {
        //Arrange
        var coordinates = Dummy.Create<Vector2<int>>();
        var value = Dummy.Create<Garbage>();

        //Act
        Instance[coordinates] = value;

        //Assert
        Instance[coordinates].Should().Be(value);
    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsNothingAtGivenIndex_TriggerOnChange()
    {
        //Arrange
        var coordinates = Dummy.Create<Vector2<int>>();
        var value = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance[coordinates] = value;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(coordinates, value)] }
            });
    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsSomethingAtGivenIndex_ReplaceExistingValueByNewValue()
    {
        //Arrange
        var coordinates = Dummy.Create<Vector2<int>>();
        var oldValue = Dummy.Create<Garbage>();
        Instance[coordinates] = oldValue;
        var newValue = Dummy.Create<Garbage>();

        //Act
        Instance[coordinates] = newValue;

        //Assert
        Instance[coordinates].Should().Be(newValue);

    }

    [TestMethod]
    public void IndexerCoordinates_WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
    {
        //Arrange
        var coordinates = Dummy.Create<Vector2<int>>();
        var oldValue = Dummy.Create<Garbage>();
        Instance[coordinates] = oldValue;
        var newValue = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance[coordinates] = newValue;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues = [new(coordinates, newValue)],
                    OldValues = [new(coordinates, oldValue)]
                }
            });
    }

    [TestMethod]
    public void IndexesOfItem_WhenGridIsEmpty_ReturnEmpty()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereAreNoOccurrences_ReturnEmpty()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance.Add(cell.Index, cell.Value);

        var item = Dummy.Create<Garbage>();

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereIsOnlyOneOccurence_ReturnListWithOnlyThatOneItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = item;

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(new List<Vector2<int>> { index });
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereAreMultipleOccurences_ReturnAllOccurences()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance.Add(cell.Index, cell.Value);

        var indexes = Dummy.CreateMany<Vector2<int>>().ToList();
        var item = Dummy.Create<Garbage>();
        foreach (var index in indexes)
            Instance.Add(index, item);

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void IndexesOfItem_WhenSeekingNullAndThereAreNoOccurrences_ReturnEmpty()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance.Add(cell.Index, cell.Value);

        //Act
        var result = Instance.IndexesOf((Garbage)null!);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenSeekingNullAndThereIsOnlyOneOccurence_ReturnListWithOnlyThatOneItem()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = null;

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance.Add(cell.Index, cell.Value);

        //Act
        var result = Instance.IndexesOf((Garbage)null!);

        //Assert
        result.Should().BeEquivalentTo(new List<Vector2<int>> { index });
    }

    [TestMethod]
    public void IndexesOfItem_WhenSeekingNullAndThereAreMultipleOccurrences_ReturnAllOccurrences()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance.Add(cell.Index, cell.Value);

        var indexes = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var index in indexes)
            Instance.Add(index, null);

        //Act
        var result = Instance.IndexesOf((Garbage)null!);

        //Assert
        result.Should().BeEquivalentTo(indexes);
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenMatchIsNull_Throw()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        Func<Garbage, bool> match = null!;

        //Act
        var action = () => Instance.IndexesOf(match!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenGridIsEmpty_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Instance.IndexesOf(x => x == Dummy.Create<Garbage>());

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenNothingMatches_ReturnEmpty()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.IndexesOf(x => x == Dummy.Create<Garbage>());

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenOneThingMatches_ReturnItsIndex()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        var targetCell = cells.GetRandom();

        //Act
        var result = Instance.IndexesOf(x => x == targetCell.Value);

        //Assert
        result.Should().BeEquivalentTo(new List<Vector2<int>> { targetCell.Index });
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenMultipleThingsMatch_ReturnAllTheIndexes()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        var sameValue = Dummy.Create<Garbage>();
        var sameCells = Dummy.Build<Cell<Garbage>>().With(x => x.Value, sameValue).CreateMany().ToList();
        foreach (var cell in sameCells)
            Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.IndexesOf(x => x == sameValue);

        //Assert
        result.Should().BeEquivalentTo(sameCells.Select(x => x.Index));
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenGridContainsNullValues_DoNotThrow()
    {
        //Arrange
        var indexesOfNulls = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var index in indexesOfNulls)
            Instance[index] = null;

        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        var targetCell = cells.GetRandom();

        //Act
        var result = Instance.IndexesOf(x => x == targetCell.Value);

        //Assert
        result.Should().BeEquivalentTo(new List<Vector2<int>> { targetCell.Index });
    }

    [TestMethod]
    public void IndexesOfPredicate_WhenGridContainsNullValuesAndMatchIsNull_ReturnIndexesOfNullValues()
    {
        //Arrange
        var indexesOfNulls = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var index in indexesOfNulls)
            Instance[index] = null;

        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        var targetCell = cells.GetRandom();

        //Act
        var result = Instance.IndexesOf(x => x == null);

        //Assert
        result.Should().BeEquivalentTo(indexesOfNulls);
    }

    [TestMethod]
    public void Boundaries_WhenGridIsEmpty_AllValuesAreZero()
    {
        //Arrange

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().BeEquivalentTo(new Boundaries<int>());
    }

    [TestMethod]
    public void Boundaries_WhenGridHasItems_ReturnBoundaries()
    {
        //Arrange
        Instance[3, 5] = Dummy.Create<Garbage>();
        Instance[-4, 9] = Dummy.Create<Garbage>();
        Instance[2, -14] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Boundaries;

        //Assert
        result.Should().BeEquivalentTo(new Boundaries<int>
        {
            Top = -14,
            Right = 3,
            Bottom = 9,
            Left = -4
        });
    }

    [TestMethod]
    public void ConstructorCells_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> cells = null!;

        //Act
        var action = () => new Grid<Garbage>(cells);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ConstructorCells_WhenCollectionIsEmpty_InstantiateEmptyGrid()
    {
        //Arrange
        var cells = Array.Empty<Cell<Garbage>>();

        //Act
        var result = new Grid<Garbage>(cells);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorCells_WhenCollectionIsNotEmpty_InstantiateWithContents()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Act
        var result = new Grid<Garbage>(cells);

        //Assert
        result.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void ConstructorKeyValuePairs_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<Vector2<int>, Garbage>> pairs = null!;

        //Act
        var action = () => new Grid<Garbage>(pairs);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ConstructorKeyValuePairs_WhenCollectionIsEmpty_InstantiateEmptyGrid()
    {
        //Arrange
        var pairs = Array.Empty<KeyValuePair<Vector2<int>, Garbage>>();

        //Act
        var result = new Grid<Garbage>(pairs);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorKeyValuePairs_WhenCollectionIsNotEmpty_InstantiateWithContents()
    {
        //Arrange
        var pairs = Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        //Act
        var result = new Grid<Garbage>(pairs);

        //Assert
        result.Should().BeEquivalentTo(pairs.Select(x => new Cell<Garbage>(x.Key, x.Value)));
    }

    [TestMethod]
    public void Constructor2dArray_WhenCollectionIsNull_Throw()
    {
        //Arrange
        Garbage[,] array = null!;

        //Act
        var action = () => new Grid<Garbage>(array);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Constructor2dArray_WhenCollectionIsEmpty_InstantiateEmptyGrid()
    {
        //Arrange
        var array = new Garbage[0, 0];

        //Act
        var result = new Grid<Garbage>(array);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor2dArray_WhenCollectionIsNotEmpty_InstantiateWithContents()
    {
        //Arrange
        var array = Dummy.Create<Garbage[,]>();

        //Act
        var result = new Grid<Garbage>(array);

        //Assert
        result.Should().BeEquivalentTo(array.ToGrid());
    }

    [TestMethod]
    public void ConstructorJaggedArray_WhenCollectionIsNull_Throw()
    {
        //Arrange
        Garbage[][] array = null!;

        //Act
        var action = () => new Grid<Garbage>(array);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ConstructorJaggedArray_WhenCollectionIsEmpty_InstantiateEmptyGrid()
    {
        //Arrange
        var array = Array.Empty<Garbage[]>();

        //Act
        var result = new Grid<Garbage>(array);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorJaggedArray_WhenCollectionIsNotEmpty_InstantiateWithContents()
    {
        //Arrange
        var array = Dummy.Create<Garbage[][]>();

        //Act
        var result = new Grid<Garbage>(array);

        //Assert
        result.Should().BeEquivalentTo(array.ToGrid());
    }

    [TestMethod]
    public void AddXY_WhenIndexIsNegative_AddAtIndex()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddXY_WhenIndexIsNegative_TriggerEvent()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddXY_WhenIndexIsPositive_AddAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddXY_WhenIndexIsPositive_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddXY_WhenItemIsNull_AddNullAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Add(index.X, index.Y, null);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void AddXY_WhenItemIsNull_DoNotTriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, null);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<Garbage>>(),
                    NewValues = [new(index, null)]
                }
            });
    }

    [TestMethod]
    public void AddXY_WhenThereIsAlreadySomethingAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.Add(index.X, index.Y, item);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddXY_WhenAddingNullAtExistingIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.Add(index.X, index.Y, null);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddCoordinates_WhenIndexIsNegative_AddAtIndex()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddCoordinates_WhenIndexIsNegative_TriggerEvent()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddCoordinates_WhenIndexIsPositive_AddAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddCoordinates_WhenIndexIsPositive_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddCoordinates_WhenItemIsNull_AddNullAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Add(index, null);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void AddCoordinates_WhenItemIsNullButInNewCell_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index, null);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<Garbage>>(),
                    NewValues = [new(index, null)]
                }
            });
    }

    [TestMethod]
    public void AddCoordinates_WhenThereIsAlreadySomethingAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.Add(index, item);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddCoordinates_WhenAddingNullAtExistingIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.Add(index, null);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddValueType_WhenIndexIsNegative_AddAtIndex()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddValueType_WhenIndexIsNegative_TriggerEvent()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddValueType_WhenIndexIsPositive_AddAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void AddValueType_WhenIndexIsPositive_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void AddValueType_WhenItemIsDefaultValue_AddDefaultValueAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Add(index.X, index.Y, default);

        //Assert
        Instance[index].Should().Be(default);
    }

    [TestMethod]
    public void AddValueType_WhenItemIsDefaultValue_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(index.X, index.Y, default);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<Garbage>>(),
                    NewValues = [new(index, default)]
                }
            });
    }

    [TestMethod]
    public void AddValueType_WhenThereIsAlreadySomethingAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.Add(index.X, index.Y, item);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddValueType_WhenAddingNegativeIndexWithDefaultValue_UpdateFirstRow()
    {
        //Arrange
        var index = -Dummy.Create<Vector2<int>>();

        //Act
        Instance.Add(index, default);

        //Assert
        Instance.FirstRow.Should().Be(index.Y);
    }

    [TestMethod]
    public void AddValueType_WhenAddingNegativeIndexWithDefaultValue_UpdateFirstColumn()
    {
        //Arrange
        var index = -Dummy.Create<Vector2<int>>();

        //Act
        Instance.Add(index, default);

        //Assert
        Instance.FirstColumn.Should().Be(index.X);
    }

    [TestMethod]
    public void TryAddXY_WhenIndexIsNegative_AddAtIndex()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.TryAdd(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void TryAddXY_WhenIndexIsNegative_TriggerEvent()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void TryAddXY_WhenIndexIsPositive_AddAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.TryAdd(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void TryAddXY_WhenIndexIsPositive_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index.X, index.Y, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void TryAddXY_WhenItemIsNull_AddNullAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        Instance.TryAdd(index.X, index.Y, null);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void TryAddXY_WhenItemIsNull_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index.X, index.Y, null);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<Garbage>>(),
                    NewValues = [new(index, null)]
                }
            });
    }

    [TestMethod]
    public void TryAddXY_WhenThereIsAlreadySomethingAtIndex_DoNotReplace()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var originalItem = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, originalItem);

        //Act
        Instance.TryAdd(index.X, index.Y, item);

        //Assert
        Instance[index].Should().Be(originalItem);
    }

    [TestMethod]
    public void TryAddXY_WhenThereIsAlreadySomethingAtIndex_DoNotThrow()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.TryAdd(index.X, index.Y, item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddXY_WhenAddingNullAtExistingIndex_DoNotThrow()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.TryAdd(index.X, index.Y, null);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddXY_WhenAddingNullAtExistingIndex_DoNotReplace()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var originalItem = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, originalItem);

        //Act
        Instance.TryAdd(index.X, index.Y, null);

        //Assert
        Instance[index].Should().Be(originalItem);
    }

    [TestMethod]
    public void TryAddCoordinates_WhenIndexIsNegative_AddAtIndex()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.TryAdd(index, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void TryAddCoordinates_WhenIndexIsNegative_TriggerEvent()
    {
        //Arrange
        var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void TryAddCoordinates_WhenIndexIsPositive_AddAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        //Act
        Instance.TryAdd(index, item);

        //Assert
        Instance[index].Should().Be(item);
    }

    [TestMethod]
    public void TryAddCoordinates_WhenIndexIsPositive_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index, item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new() { NewValues = [new(index, item)] }
            });
    }

    [TestMethod]
    public void TryAddCoordinates_WhenItemIsNull_AddNullAtIndex()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        Instance.TryAdd(index, null);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void TryAddCoordinates_WhenItemIsNull_TriggerEvent()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(index, null);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<Garbage>>(),
                    NewValues = [new(index, null)]
                }
            });
    }

    [TestMethod]
    public void TryAddCoordinates_WhenThereIsAlreadySomethingAtIndex_DoNotReplace()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();

        var originalItem = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, originalItem);

        //Act
        Instance.TryAdd(index, item);

        //Assert
        Instance[index].Should().Be(originalItem);
    }

    [TestMethod]
    public void TryAddCoordinates_WhenThereIsAlreadySomethingAtIndex_DoNotThrow()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.TryAdd(index, item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCoordinates_WhenAddingNullAtExistingIndex_DoNotThrow()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance.Add(index.X, index.Y, Dummy.Create<Garbage>());

        //Act
        var action = () => Instance.TryAdd(index, null);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCoordinates_WhenAddingNullAtExistingIndex_DoNotReplace()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var originalItem = Dummy.Create<Garbage>();
        Instance.Add(index.X, index.Y, originalItem);

        //Act
        Instance.TryAdd(index, null);

        //Assert
        Instance[index].Should().Be(originalItem);
    }

    [TestMethod]
    public void AddCellsParams_WhenCellsIsEmpty_DoNotModify()
    {
        //Arrange
        var cells = Array.Empty<Cell<Garbage>>();
        var copy = Instance.Copy();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void AddCellsParams_WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();
        Instance.Add(cells);

        //Act
        var action = () => Instance.Add(cells);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddCellsParams_WhenAddingToNewCoordinates_Add()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().Contain(cells);
    }

    [TestMethod]
    public void AddCellsParams_WhenAddingToNewCoordinates_TriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        var triggered = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggered.Add(args);

        //Act
        Instance.Add(cells);

        //Assert
        triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new(){NewValues = cells}
        });
    }

    [TestMethod]
    public void AddCellsEnumerable_WhenCellsIsNull_Throw()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> cells = null!;

        //Act
        var action = () => Instance.Add(cells);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
    }

    [TestMethod]
    public void AddCellsEnumerable_WhenCellsIsEmpty_DoNotModify()
    {
        //Arrange
        var cells = new List<Cell<Garbage>>();
        var copy = Instance.Copy();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void AddCellsEnumerable_WhenThereIsAlreadySomethingAtCoordinates_Throw()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(cells);

        //Act
        var action = () => Instance.Add(cells);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddCellsEnumerable_WhenAddingToNewCoordinates_Add()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Act
        Instance.Add(cells);

        //Assert
        Instance.Should().Contain(cells);
    }

    [TestMethod]
    public void AddCellsEnumerable_WhenAddingToNewCoordinates_TriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var triggered = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggered.Add(args);

        //Act
        Instance.Add(cells);

        //Assert
        triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new(){NewValues = cells}
        });
    }

    [TestMethod]
    public void TryAddCellsParams_WhenCellsIsNull_DoNotThrow()
    {
        //Arrange
        Cell<Garbage>[] cells = null!;

        //Act
        var action = () => Instance.TryAdd(cells);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCellsParams_WhenCellsIsEmpty_DoNotModify()
    {
        //Arrange
        var cells = Array.Empty<Cell<Garbage>>();
        var copy = Instance.Copy();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TryAddCellsParams_WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
    {
        //Arrange
        var alreadyThereCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(alreadyThereCells);

        var newCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        var cells = alreadyThereCells.Concat(newCells).ToArray();

        //Act
        var action = () => Instance.TryAdd(cells);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCellsParams_WhenThereIsAlreadySomethingAtCoordinates_StillAddThoseThatAreNotAlreadyIn()
    {
        //Arrange
        var alreadyThereCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(alreadyThereCells);

        var newCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        var cells = alreadyThereCells.Concat(newCells).ToArray();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void TryAddCellsParams_WhenAddingToNewCoordinates_Add()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().Contain(cells);
    }

    [TestMethod]
    public void TryAddCellsParams_WhenAddingToNewCoordinates_TriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

        var triggered = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggered.Add(args);

        //Act
        Instance.TryAdd(cells);

        //Assert
        triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new(){NewValues = cells}
            });
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenCellsIsNull_DoNotThrow()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> cells = null!;

        //Act
        var action = () => Instance.TryAdd(cells);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenCellsIsEmpty_DoNotModify()
    {
        //Arrange
        var cells = new List<Cell<Garbage>>();
        var copy = Instance.Copy();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
    {
        //Arrange
        var alreadyThereCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(alreadyThereCells);

        var newCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        var cells = alreadyThereCells.Concat(newCells).ToList();

        //Act
        var action = () => Instance.TryAdd(cells);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenThereIsAlreadySomethingAtCoordinates_StillAddThoseThatAreNotAlreadyIn()
    {
        //Arrange
        var alreadyThereCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(alreadyThereCells);

        var newCells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        var cells = alreadyThereCells.Concat(newCells).ToList();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenAddingToNewCoordinates_Add()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Act
        Instance.TryAdd(cells);

        //Assert
        Instance.Should().Contain(cells);
    }

    [TestMethod]
    public void TryAddCellsEnumerable_WhenAddingToNewCoordinates_TriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var triggered = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggered.Add(args);

        //Act
        Instance.TryAdd(cells);

        //Assert
        triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new(){NewValues = cells}
            });
    }

    [TestMethod]
    public void RemoveAtXY_WhenThereIsNoItemAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.RemoveAt(index.X, index.Y);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAtXY_WhenThereIsItemAtIndex_RemoveItem()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        Instance.RemoveAt(index.X, index.Y);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void RemoveAtXY_WhenThereIsItemAtIndex_TriggerChange()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAt(index.X, index.Y);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>> { new() { OldValues = [new(index, item)] } });
    }

    [TestMethod]
    public void TryRemoveAtXY_WhenThereIsNoItemAtIndex_DoNotThrow()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAtXY_WhenThereIsItemAtIndex_RemoveItem()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void TryRemoveAtXY_WhenThereIsItemAtIndex_TriggerChange()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemoveAt(index.X, index.Y);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>> { new() { OldValues = [new(index, item)] } });
    }

    [TestMethod]
    public void RemoveAtCoordinates_WhenThereIsNoItemAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.RemoveAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAtCoordinates_WhenThereIsItemAtIndex_RemoveItem()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        Instance.RemoveAt(index);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void RemoveAtCoordinates_WhenThereIsItemAtIndex_TriggerChange()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAt(index);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>> { new() { OldValues = [new(index, item)] } });
    }

    [TestMethod]
    public void TryRemoveAtCoordinates_WhenThereIsNoItemAtIndex_Throw()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var action = () => Instance.TryRemoveAt(index);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAtCoordinates_WhenThereIsItemAtIndex_RemoveItem()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        Instance.TryRemoveAt(index);

        //Assert
        Instance[index].Should().BeNull();
    }

    [TestMethod]
    public void TryRemoveAtCoordinates_WhenThereIsItemAtIndex_TriggerChange()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemoveAt(index);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>> { new() { OldValues = [new(index, item)] } });
    }

    [TestMethod]
    public void RemoveAllItem_WhenItemIsNull_RemoveAllKeysThatPointToNullReferences()
    {
        //Arrange
        var keys = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var key in keys)
            Instance[key] = null;

        var nonNullEntries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in nonNullEntries)
            Instance[entry.Index] = entry.Value;

        //Act
        Instance.RemoveAll((Garbage)null!);

        //Assert
        Instance.Should().NotContain(x => x.Value == null);
    }

    [TestMethod]
    public void RemoveAllItem_WhenItemIsNull_DoNotRemoveNonNullReferences()
    {
        //Arrange
        var keys = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var key in keys)
            Instance[key] = null;

        var nonNullEntries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in nonNullEntries)
            Instance[entry.Index] = entry.Value;

        //Act
        Instance.RemoveAll((Garbage)null!);

        //Assert
        Instance.Should().Contain(new Grid<Garbage>(nonNullEntries));
    }

    [TestMethod]
    public void RemoveAllItem_WhenItemIsNull_TriggerEvent()
    {
        //Arrange
        var keys = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var key in keys)
            Instance[key] = null;

        var nonNullEntries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in nonNullEntries)
            Instance[entry.Index] = entry.Value;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAll((Garbage)null!);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = keys.Select(x => new Cell<Garbage>(x, null)).ToList()
                }
            });
    }

    [TestMethod]
    public void RemoveAllItem_WhenThereIsNoOccurenceOfItemInGrid_DoNotModifyCollection()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var copy = Instance.Copy();

        var item = Dummy.Create<Garbage>();

        //Act
        Instance.RemoveAll(item);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void RemoveAllItem_WhenThereIsNoOccurenceOfItemInGrid_DoNotTriggerEvent()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        var item = Dummy.Create<Garbage>();

        //Act
        Instance.RemoveAll(item);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllItem_WhenThereAreOccurrencesOfItemInGrid_RemoveAllThoseOccurences()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var item = Dummy.Create<Garbage>();
        var keys = Dummy.CreateMany<Vector2<int>>();
        foreach (var key in keys)
            Instance[key] = item;

        //Act
        Instance.RemoveAll(item);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>(entries));
    }

    [TestMethod]
    public void RemoveAllItem_WhenThereAreOccurrencesOfItemInGrid_TriggerEvent()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var item = Dummy.Create<Garbage>();
        var keys = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var key in keys)
            Instance[key] = item;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAll(item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
        {
            new()
            {
                OldValues = keys.Select(x => new Cell<Garbage>(x, item)).ToList()
            }
        });
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<Garbage?, bool> match = null!;

        //Act
        var action = () => Instance.RemoveAll(match);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenNoItemsMatch_DoNotModifyGrid()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var copy = Instance.Copy();

        //Act
        Instance.RemoveAll(x => x.Id > int.MaxValue);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenNoItemsMatch_DoNotTriggerEvent()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAll(x => x.Id > int.MaxValue);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenItemsMatch_RemoveThoseItems()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var value = Dummy.Create<string>();
        var dummies = Dummy.Build<Garbage>().With(x => x.Value, value).CreateMany();
        var items = dummies.Select(x => new Cell<Garbage>(Dummy.Create<Vector2<int>>(), x)).ToList();
        foreach (var item in items)
            Instance[item.Index] = item.Value;

        //Act
        Instance.RemoveAll(x => x.Value == value);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>(entries));
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenItemsMatch_TriggerEvent()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var value = Dummy.Create<string>();
        var dummies = Dummy.Build<Garbage>().With(x => x.Value, value).CreateMany();
        var items = dummies.Select(x => new Cell<Garbage>(Dummy.Create<Vector2<int>>(), x)).ToList();
        foreach (var item in items)
            Instance[item.Index] = item.Value;

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAll(x => x.Value == value);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = items
                }
            });
    }

    [TestMethod]
    public void ContainsCoordinatesItem_WhenThereIsItemAtIndex_ReturnTrue()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        //Act
        var result = Instance.Contains(index, item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsCoordinatesItem_WhenSomethingElseIsAtIndex_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Contains(index, item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsCoordinatesItem_WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = entries.GetRandom().Value;

        //Act
        var result = Instance.Contains(index, item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsCoordinatesItem_WhenThereIsNothingAtIndexAndItemIsNull_ReturnTrue()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index, null);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsCoordinatesItem_WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = entries.GetRandom().Index;


        //Act
        var result = Instance.Contains(index, null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsXYItem_WhenThereIsItemAtIndex_ReturnTrue()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = item;

        //Act
        var result = Instance.Contains(index.X, index.Y, item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsXYItem_WhenSomethingElseIsAtIndex_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = Dummy.Create<Garbage>();
        Instance[index] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Contains(index.X, index.Y, item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsXYItem_WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();
        var item = entries.GetRandom().Value;

        //Act
        var result = Instance.Contains(index.X, index.Y, item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsXYItem_WhenThereIsNothingAtIndexAndItemIsNull_ReturnTrue()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index.X, index.Y, null);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsXYItem_WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
    {
        //Arrange
        var entries = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var entry in entries)
            Instance[entry.Index] = entry.Value;

        var index = entries.GetRandom().Index;


        //Act
        var result = Instance.Contains(index.X, index.Y, null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsItem_WhenGridIsEmptyAndSeekingNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = Instance.Contains(null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsItem_WhenGridIsEmptyAndSeekingSomething_ReturnFalse()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsItem_WhenThereIsAtLeastOneNullValueSomewhereAndSeekingNull_ReturnTrue()
    {
        //Arrange
        var indexes = Dummy.CreateMany<Vector2<int>>().ToList();
        foreach (var index in indexes)
            Instance[index] = null;

        //Act
        var result = Instance.Contains(null);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsItem_WhenThereAreNoNullValuesAndSeekingNull_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.Contains(null);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsItem_WhenThereIsAtLeastOneOccurenceOfItemInGrid_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.Contains(cells.GetRandom().Value);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsXY_WhenGridIsEmpty_ReturnFalse()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index.X, index.Y);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsXY_WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
    {
        //Arrange
        var cell = Dummy.Create<Cell<Garbage>>();
        Instance[cell.Index] = cell.Value;

        Instance.RemoveAt(cell.Index);

        //Act
        var result = Instance.Contains(cell.Index.X, cell.Index.Y);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsXY_WhenThereIsSomethingAtIndex_ReturnTrue()
    {
        //Arrange
        var cell = Dummy.Create<Cell<Garbage>>();
        Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.Contains(cell.Index.X, cell.Index.Y);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsXY_WhenThereIsNullAtIndex_ReturnTrue()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = null;

        //Act
        var result = Instance.Contains(index.X, index.Y);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsCoordinates_WhenGridIsEmpty_ReturnFalse()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();

        //Act
        var result = Instance.Contains(index);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsCoordinates_WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
    {
        //Arrange
        var cell = Dummy.Create<Cell<Garbage>>();
        Instance[cell.Index] = cell.Value;

        Instance.RemoveAt(cell.Index);

        //Act
        var result = Instance.Contains(cell.Index);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsCoordinates_WhenThereIsSomethingAtIndex_ReturnTrue()
    {
        //Arrange
        var cell = Dummy.Create<Cell<Garbage>>();
        Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.Contains(cell.Index);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsCoordinates_WhenThereIsNullAtIndex_ReturnTrue()
    {
        //Arrange
        var index = Dummy.Create<Vector2<int>>();
        Instance[index] = null;

        //Act
        var result = Instance.Contains(index);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenRectangleHasZeroSize_DoNotModify()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var copy = Instance.Copy();

        var range = new Rectangle<int>(Dummy.Create<Vector2<int>>(), 0, 0);

        //Act
        Instance.Translate(range, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenRectangleHasZeroSize_DoNotTriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var range = new Rectangle<int>(Dummy.Create<Vector2<int>>(), 0, 0);

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(range, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange
        var range = Dummy.Create<Rectangle<int>>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenGridIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var range = Dummy.Create<Rectangle<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(range, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 1, 0);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[0, 0] },
                { 1, 1, copy[0, 1] },
                { 1, 2, copy[0, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 1, 0);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    ],
                    NewValues =
                    [
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 3, 0);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[1, 0] },
                { 1, 1, copy[1, 1] },
                { 1, 2, copy[1, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
                { 3, 0, copy[0, 0] },
                { 3, 1, copy[0, 1] },
                { 3, 2, copy[0, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndXY_WhenMovingRectangleOverEmptyArea_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 3, 0);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues =
                    [
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    ],
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenRectangleHasZeroSize_DoNotModify()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var copy = Instance.Copy();

        var range = new Rectangle<int>(Dummy.Create<Vector2<int>>(), 0, 0);
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenRectangleHasZeroSize_DoNotTriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var range = new Rectangle<int>(Dummy.Create<Vector2<int>>(), 0, 0);
        var translation = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(range, translation);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange
        var range = Dummy.Create<Rectangle<int>>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(range, translation);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenGridIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var range = Dummy.Create<Rectangle<int>>();
        var translation = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(range, translation);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(1, 0));

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[0, 0] },
                { 1, 1, copy[0, 1] },
                { 1, 2, copy[0, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(1, 0));

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    ],
                    NewValues =
                    [
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(3, 0));

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[1, 0] },
                { 1, 1, copy[1, 1] },
                { 1, 2, copy[1, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
                { 3, 0, copy[0, 0] },
                { 3, 1, copy[0, 1] },
                { 3, 2, copy[0, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithRangeAndCoordinates_WhenMovingRectangleOverEmptyArea_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(3, 0));

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues =
                    [
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    ],
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenRectangleHasZeroBoundaries_DoNotModify()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var copy = Instance.Copy();

        var boundaries = new Boundaries<int>(0, 0, 0, 0);

        //Act
        Instance.Translate(boundaries, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenRectangleHasZeroSize_DoNotTriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var boundaries = new Boundaries<int>(0, 0, 0, 0);

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(boundaries, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange
        var boundaries = Dummy.Create<Boundaries<int>>();

        //Act
        Instance.Translate(boundaries, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenGridIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var boundaries = Dummy.Create<Boundaries<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(boundaries, Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 1, 0);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[0, 0] },
                { 1, 1, copy[0, 1] },
                { 1, 2, copy[0, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 1, 0);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    ],
                    NewValues =
                    [
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 3, 0);

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[1, 0] },
                { 1, 1, copy[1, 1] },
                { 1, 2, copy[1, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
                { 3, 0, copy[0, 0] },
                { 3, 1, copy[0, 1] },
                { 3, 2, copy[0, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndXY_WhenMovingRectangleOverEmptyArea_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 3, 0);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues =
                    [
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    ],
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenRectangleHasZeroBoundaries_DoNotModify()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var copy = Instance.Copy();

        var boundaries = new Boundaries<int>(0, 0, 0, 0);
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenRectangleHasZeroSize_DoNotTriggerChange()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var boundaries = new Boundaries<int>(0, 0, 0, 0);
        var translation = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange
        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Dummy.Create<Vector2<int>>();

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenGridIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var boundaries = Dummy.Create<Boundaries<int>>();
        var translation = Dummy.Create<Vector2<int>>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(boundaries, translation);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(1, 0));

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[0, 0] },
                { 1, 1, copy[0, 1] },
                { 1, 2, copy[0, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(1, 0));

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    ],
                    NewValues =
                    [
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(3, 0));

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 1, 0, copy[1, 0] },
                { 1, 1, copy[1, 1] },
                { 1, 2, copy[1, 2] },
                { 2, 0, copy[2, 0] },
                { 2, 1, copy[2, 1] },
                { 2, 2, copy[2, 2] },
                { 3, 0, copy[0, 0] },
                { 3, 1, copy[0, 1] },
                { 3, 2, copy[0, 2] },
            });
    }

    [TestMethod]
    public void TranslateWithBoundariesAndCoordinates_WhenMovingRectangleOverEmptyArea_TriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(3, 0));

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues =
                    [
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    ],
                    OldValues =
                    [
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    ]
                }
            });
    }

    [TestMethod]
    public void Copy_WhenGridIsEmpty_ReturnEmptyGrid()
    {
        //Arrange

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Copy_WhenGridIsNotEmpty_ReturnExactCopy()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEquivalentTo(Instance);
        result.Should().NotBeSameAs(Instance);
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreEqual_DoNotModifyGrid()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var copy = Instance.Copy();

        //Act
        Instance.Swap(new Vector2<int>(1, 0), new Vector2<int>(1, 0));

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreEqual_DoNotTriggerChange()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Swap(new Vector2<int>(1, 0), new Vector2<int>(1, 0));

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreDifferent_Swap()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var firstItem = Instance[2, 0];
        var secondItem = Instance[1, 2];

        //Act
        Instance.Swap(new Vector2<int>(2, 0), new Vector2<int>(1, 2));

        //Assert
        Instance.Should().BeEquivalentTo(new Grid<Garbage>
            {
                { 0, 0, Instance[0, 0] },
                { 1, 0, Instance[1, 0] },
                { 2, 0, secondItem },
                { 0, 1, Instance[0, 1] },
                { 1, 1, Instance[1, 1] },
                { 2, 1, Instance[2, 1] },
                { 0, 2, Instance[0, 2] },
                { 1, 2, firstItem },
                { 2, 2, Instance[2, 2] },
            });
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreDifferent_TriggerChangeWithNewIndexes()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();
        Instance[0, 1] = Dummy.Create<Garbage>();
        Instance[1, 1] = Dummy.Create<Garbage>();
        Instance[2, 1] = Dummy.Create<Garbage>();
        Instance[0, 2] = Dummy.Create<Garbage>();
        Instance[1, 2] = Dummy.Create<Garbage>();
        Instance[2, 2] = Dummy.Create<Garbage>();

        var firstItem = Instance[2, 0];
        var secondItem = Instance[1, 2];

        var eventArgs = new List<GridChangedEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Swap(new Vector2<int>(2, 0), new Vector2<int>(1, 2));

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Garbage>>
            {
                new()
                {
                    NewValues = [new(2,0, secondItem), new(1, 2, firstItem)],
                    OldValues = [new(2,0, firstItem), new(1, 2, secondItem)],
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
        result.Should().BeEquivalentTo("Empty Grid<Garbage>");
    }

    [TestMethod]
    public void ToString_WhenGridIsNotEmpty_ReturnCount()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>(5).ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().BeEquivalentTo("Grid<Garbage> with 5 items");
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsSimilarGrid_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.Build<Cell<Garbage>>().With(x => x.Index, new Vector2<int>(Dummy.Number.Between(-5, 5).Create(), Dummy.Number.Between(-5, 5).Create())).CreateMany().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Copy();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferentGrid_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.Build<Cell<Garbage>>().With(x => x.Index, new Vector2<int>(Dummy.Number.Between(-5, 5).Create(), Dummy.Number.Between(-5, 5).Create())).CreateMany().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsSimilar2dArray_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.To2dArray();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferent2dArray_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.Create<Garbage[,]>();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsSimilarJaggedArray_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToJaggedArray();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferentJaggedArray_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.Create<Garbage[][]>();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsSimilarCells_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.Equals((object)cells);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferentCells_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsSimilarKeyValuePairs_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferentKeyValuePairs_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.Create<Dictionary<Vector2<int>, Garbage>>();

        //Act
        var result = Instance.Equals((object)other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithObject_WhenOtherIsDifferentUnsupportedType_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Dummy.Create<int>();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridIsNull_ReturnFalse()
    {
        //Arrange
        Grid<Garbage> other = null!;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridIsSameReference_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.Equals(Instance);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToGrid();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToGrid();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Copy();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherIsGridButOrderedDifferently_ReturnTrue()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();

        var other = new Grid<Garbage>
            {
                { 1,0, Instance[1,0] },
                { 0,0, Instance[0,0] },
                { 2,0, Instance[2,0] },
            };

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithGrid_WhenOtherIsCellsButOrderedDifferently_ReturnTrue()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 0] = Dummy.Create<Garbage>();
        Instance[2, 0] = Dummy.Create<Garbage>();

        var other = new List<Cell<Garbage>>
            {
                new(1, 0, Instance[1, 0]),
                new(0, 0, Instance[0, 0]),
                new(2, 0, Instance[2, 0]),
            };

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenBothAreNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == (Grid<Garbage>)null!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenGridIsNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenOtherGridIsNull_ReturnFalse()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> other = null!;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenOtherGridIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToGrid();

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToGrid();

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithGrid_WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Copy();

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenBothAreNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != (Grid<Garbage>)null!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenGridIsNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenOtherGridIsNull_ReturnTrue()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> other = null!;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenOtherGridIsDifferent_ReturnTrue()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToGrid();

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToGrid();

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithGrid_WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Copy();

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithCells_WhenCellsIsNull_ReturnFalse()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> other = null!;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithCells_WhenCellsIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithCells_WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();

        //Act
        var result = Instance.Equals(differentCells);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithCells_WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToList();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithCells_WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToList();

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenBothAreNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == (IEnumerable<Cell<Garbage>>)null!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenGridIsNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenCellsIsNull_ReturnFalse()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> other = null!;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenCellsIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();

        //Act
        var result = Instance == differentCells;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToList();

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithCells_WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToList();

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenBothAreNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != (IEnumerable<Cell<Garbage>>)null!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenGridIsNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != Dummy.CreateMany<Cell<Garbage>>().ToList();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenCellsIsNull_ReturnTrue()
    {
        //Arrange
        IEnumerable<Cell<Garbage>> other = null!;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenCellsIsDifferent_ReturnTrue()
    {
        //Arrange
        var other = Dummy.CreateMany<Cell<Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();

        //Act
        var result = Instance != differentCells;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.Select(x => new Cell<Garbage>(x.Index, Dummy.Create<Garbage>())).ToList();

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithCells_WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToList();

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithKeyValuePairs_WhenKeyValuePairsIsNull_ReturnFalse()
    {
        //Arrange
        IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithKeyValuePairs_WhenKeyValuePairsIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance.Equals(other);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();
        var other = new Grid<Garbage>(differentCells).ToDictionary();

        //Act
        var result = Instance.Equals(other!);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithKeyValuePairs_WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();
        foreach (var ((x, y), _) in Instance)
            other[new Vector2<int>(x, y)] = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Equals(other!);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();

        //Act
        var result = Instance.Equals(other!);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenBothAreNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == (IEnumerable<KeyValuePair<Vector2<int>, Garbage>>)null!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenGridIsNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenKeyValuePairsIsNull_ReturnFalse()
    {
        //Arrange
        IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenKeyValuePairsIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();
        var other = new Grid<Garbage>(differentCells).ToDictionary();

        //Act
        var result = Instance == other!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();
        foreach (var ((x, y), _) in Instance)
            other[new Vector2<int>(x, y)] = Dummy.Create<Garbage>();

        //Act
        var result = Instance == other!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();

        //Act
        var result = Instance == other!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenBothAreNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != (IEnumerable<KeyValuePair<Vector2<int>, Garbage>>)null!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenGridIsNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenKeyValuePairsIsNull_ReturnTrue()
    {
        //Arrange
        IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenKeyValuePairsIsDifferent_ReturnTrue()
    {
        //Arrange
        var other = Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();
        var other = new Grid<Garbage>(differentCells).ToDictionary();

        //Act
        var result = Instance != other!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();
        foreach (var ((x, y), _) in Instance)
            other[new Vector2<int>(x, y)] = Dummy.Create<Garbage>();

        //Act
        var result = Instance != other!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithKeyValuePairs_WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToDictionary();

        //Act
        var result = Instance != other!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayIsNull_ReturnFalse()
    {
        //Arrange
        Garbage[,] other = null!;

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenBothAreEmpty_ReturnTrue()
    {
        //Arrange
        var other = new Garbage[0, 0];

        //Act
        //Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridIsEmptyButArrayIsNot_ReturnFalse()
    {
        //Arrange
        var other = Dummy.Create<Garbage[,]>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);

    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridContainsItemsButArrayIsEmpty_ReturnFalse()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var other = new Garbage[0, 0];

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridAndArrayContainNullValuesAtCorrespondingPositions_ReturnTrue()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = null;

        var other = Instance.To2dArray();

        // Act
        // Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayHasAnExtraRow_ReturnFalse()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        for (var x = 0; x < 3; x++)
            otherGrid[x, 3] = Dummy.Create<Garbage>();

        var other = otherGrid.To2dArray();

        // Act
        // Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridHasAnExtraRow_ReturnFalse()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        otherGrid.RemoveAt(0, 2);
        otherGrid.RemoveAt(1, 2);
        otherGrid.RemoveAt(2, 2);

        var other = otherGrid.To2dArray();

        // Act
        // Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayHasAnExtraColumn_ReturnFalse()
    {
        //Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        for (var y = 0; y < 3; y++)
            otherGrid[3, y] = Dummy.Create<Garbage>();

        var other = otherGrid.To2dArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridHasAnExtraColumn_ReturnFalse()
    {
        //Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        otherGrid.RemoveAt(2, 0);
        otherGrid.RemoveAt(2, 1);
        otherGrid.RemoveAt(2, 2);

        var other = otherGrid.To2dArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }


    [TestMethod]
    public void EqualsWith2dArray_WhenGridHasMoreColumnsThanArray_ReturnFalse()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 3] = Dummy.Create<Garbage>();
        Instance[2, 4] = Dummy.Create<Garbage>();

        var other = new Garbage[2, 0];

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.Create<Garbage[,]>();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();
        var other = new Grid<Garbage>(differentCells).To2dArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.To2dArray();
        foreach (var ((x, y), _) in Instance)
            other[x, y] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.To2dArray();

        //Act
        //Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridHasANegativeX_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.To2dArray();

        Instance[-Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWith2dArray_WhenGridHasANegativeY_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.To2dArray();

        Instance[Dummy.Create<int>(), -Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsOperatorWith2dArray_WhenBothAreNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == (Garbage[,])null!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWith2dArray_WhenGridIsNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == Dummy.Create<Garbage[,]>();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWith2dArray_WhenArrayIsNull_ReturnFalse()
    {
        //Arrange
        Garbage[,] other = null!;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWith2dArray_WhenBothAreNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != (Garbage[,])null!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWith2dArray_WhenGridIsNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != Dummy.Create<Garbage[,]>();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWith2dArray_WhenArrayIsNull_ReturnTrue()
    {
        //Arrange
        Garbage[,] other = null!;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayIsNull_ReturnFalse()
    {
        //Arrange
        Garbage[][] other = null!;

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenBothAreEmpty_ReturnTrue()
    {
        //Arrange
        var other = Array.Empty<Garbage[]>();

        //Act
        //Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridIsEmptyButArrayIsNot_ReturnFalse()
    {
        //Arrange
        var other = Dummy.Create<Garbage[][]>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);

    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridContainsItemsButArrayIsEmpty_ReturnFalse()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var other = Array.Empty<Garbage[]>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridAndArrayContainNullValuesAtCorrespondingPositions_ReturnTrue()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = null;

        var other = Instance.ToJaggedArray();

        // Act
        // Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayHasAnExtraRow_ReturnFalse()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        for (var x = 0; x < 3; x++)
            otherGrid[x, 3] = Dummy.Create<Garbage>();

        var other = otherGrid.ToJaggedArray();

        // Act
        // Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridHasAnExtraRow_ReturnFalse()
    {
        // Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        otherGrid.RemoveAt(0, 2);
        otherGrid.RemoveAt(1, 2);
        otherGrid.RemoveAt(2, 2);

        var other = otherGrid.ToJaggedArray();

        // Act
        // Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayHasAnExtraColumn_ReturnFalse()
    {
        //Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        for (var y = 0; y < 3; y++)
            otherGrid[3, y] = Dummy.Create<Garbage>();

        var other = otherGrid.ToJaggedArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridHasAnExtraColumn_ReturnFalse()
    {
        //Arrange
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                Instance[x, y] = Dummy.Create<Garbage>();

        var otherGrid = Instance.ToGrid();
        otherGrid.RemoveAt(2, 0);
        otherGrid.RemoveAt(2, 1);
        otherGrid.RemoveAt(2, 2);

        var other = otherGrid.ToJaggedArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridHasMoreColumnsThanArray_ReturnFalse()
    {
        //Arrange
        Instance[0, 0] = Dummy.Create<Garbage>();
        Instance[1, 3] = Dummy.Create<Garbage>();
        Instance[2, 4] = Dummy.Create<Garbage>();

        var other = new Garbage[2][];

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayIsDifferent_ReturnFalse()
    {
        //Arrange
        var other = Dummy.Create<Garbage[][]>();

        var cells = Dummy.CreateMany<Cell<Garbage>>();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var differentCells = cells.Select(x => Dummy.Build<Cell<Garbage>>().With(y => y.Value, x.Value).Create()).ToList();
        var other = new Grid<Garbage>(differentCells).ToJaggedArray();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToJaggedArray();
        foreach (var ((x, y), _) in Instance)
            other[x][y] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToJaggedArray();

        //Act
        //Assert
        Ensure.Equality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridHasANegativeX_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToJaggedArray();

        Instance[-Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsWithJaggedArray_WhenGridHasANegativeY_ReturnFalse()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
        foreach (var (index, value) in cells)
            Instance[index] = value;

        var other = Instance.ToJaggedArray();

        Instance[Dummy.Create<int>(), -Dummy.Create<int>()] = Dummy.Create<Garbage>();

        //Act
        //Assert
        Ensure.Inequality(Instance, other);
    }

    [TestMethod]
    public void EqualsOperatorWithJaggedArray_WhenBothAreNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == (Garbage[][])null!;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void EqualsOperatorWithJaggedArray_WhenGridIsNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! == Dummy.Create<Garbage[][]>();

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualsOperatorWithJaggedArray_WhenArrayIsNull_ReturnFalse()
    {
        //Arrange
        Garbage[][] other = null!;

        //Act
        var result = Instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWithJaggedArray_WhenBothAreNull_ReturnFalse()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != (Garbage[][])null!;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void NotEqualsOperatorWithJaggedArray_WhenGridIsNull_ReturnTrue()
    {
        //Arrange

        //Act
        var result = (Grid<Garbage>)null! != Dummy.Create<Garbage[][]>();

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void NotEqualsOperatorWithJaggedArray_WhenArrayIsNull_ReturnTrue()
    {
        //Arrange
        Garbage[][] other = null!;

        //Act
        var result = Instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Enumerator_Always_Enumerates()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        var enumeratedItems = new List<Cell<Garbage>>();

        //Act
        foreach (var item in Instance)
            enumeratedItems.Add(item);

        //Assert
        enumeratedItems.Should().NotBeEmpty();
        enumeratedItems.Should().BeEquivalentTo(Instance);
        enumeratedItems.Should().HaveCount(Instance.Count);
    }

    [TestMethod]
    public void Enumerator_WhenUsingResetAfterCollectionChanged_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

        using var enumerator = Instance.GetEnumerator();

        //Act
        var action = () =>
        {
            while (enumerator.MoveNext())
            {
                Instance.Add(Dummy.Create<Cell<Garbage>>());
                enumerator.Reset();
            }
        };

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void HashCode_Always_ReturnInternalCollectionHashCode()
    {
        //Arrange

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(GetFieldValue<Dictionary<Vector2<int>, Garbage>>("_items")!.GetHashCode());
    }

    [TestMethod]
    public void Equality_Always_EnsureValueEquality() => Ensure.ValueEquality<Grid<Garbage>>(Dummy);

    [TestMethod]
    public void Serialization_WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        var json = JsonConvert.SerializeObject(Instance);

        //Act
        var result = JsonConvert.DeserializeObject<Grid<Garbage>>(json);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void Serialization_WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
    {
        //Arrange
        var items = Dummy.CreateMany<Cell<Garbage>>().ToList();
        Instance.Add(items);

        JsonSerializerOptions.WithGridConverters();

        var json = System.Text.Json.JsonSerializer.Serialize(Instance, JsonSerializerOptions);

        //Act
        var result = System.Text.Json.JsonSerializer.Deserialize<Grid<Garbage>>(json, JsonSerializerOptions);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }
}