namespace Collections.Grid.Tests;

[TestClass]
public class GridTests
{
    [TestClass]
    public class ColumnCount : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenHasOnlyOneItemAtColumnZero_ReturnOne()
        {
            //Arrange
            Instance[0, Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenHasOneItemAtNegativeColumnIndex_ReturnDifferenceBetweenThatAndZero()
        {
            //Arrange
            Instance[-3, Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(4);
        }

        [TestMethod]
        public void WhenHasOneItemAtColumnIndexGreaterThanZero_ReturnNumberOfColumns()
        {
            //Arrange
            Instance[5, Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenHasOneItemInNegativeColumnIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
        {
            //Arrange
            Instance[-3, Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[5, Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(9);
        }

        [TestMethod]
        public void WhenHasABunchOfColumns_ReturnDifferenceBetweenMinimumAndMaximum()
        {
            //Arrange
            Instance[-3, Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[-5, Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[5, Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[7, Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(13);
        }
    }

    [TestClass]
    public class RowCount : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
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
            Instance[Dummy.Create<int>(), 0] = Dummy.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenHasOneItemAtNegativeRowIndex_ReturnDifferenceBetweenThatAndZero()
        {
            //Arrange
            Instance[Dummy.Create<int>(), -3] = Dummy.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(4);
        }

        [TestMethod]
        public void WhenHasOneItemAtRowIndexGreaterThanZero_ReturnNumberOfRows()
        {
            //Arrange
            Instance[Dummy.Create<int>(), 5] = Dummy.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenHasOneItemInNegativeRowIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
        {
            //Arrange
            Instance[Dummy.Create<int>(), -3] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), 5] = Dummy.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(9);
        }

        [TestMethod]
        public void WhenHasABunchOfRows_ReturnDifferenceBetweenMinimumAndMaximum()
        {
            //Arrange
            Instance[Dummy.Create<int>(), -3] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), -5] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), 5] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), 7] = Dummy.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(13);
        }
    }

    [TestClass]
    public class FirstColumn : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenOnlyContainsItemsAtColumnZero_ReturnZero()
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
        public void WhenContainsSomethingInTheNegatives_ReturnThatIndex()
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
        public void WhenContainsSomethingGreaterThanZero_ReturnThatIndex()
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
    }

    [TestClass]
    public class LastColumn : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenOnlyContainsItemsAtColumnZero_ReturnZero()
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
        public void WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
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
    }

    [TestClass]
    public class FirstRow : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenOnlyContainsItemsAtRowZero_ReturnZero()
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
        public void WhenContainsSomethingInTheNegatives_ReturnThatIndex()
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
        public void WhenContainsSomethingGreaterThanZero_ReturnThatIndex()
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
    }

    [TestClass]
    public class LastRow : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenOnlyContainsItemsAtRowZero_ReturnZero()
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
        public void WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
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
        public void WhenHighestIsSomethingGreaterThanZero_ReturnThatIndex()
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
    }

    [TestClass]
    public class Count : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsOneItem_ReturnOne()
        {
            //Arrange
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenThereAreTwoItemsAtOppositeSidesOfTheGrid_ReturnTwo()
        {
            //Arrange
            Instance[-10, -10] = Dummy.Create<string>();
            Instance[10, 10] = Dummy.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(2);
        }

        [TestMethod]
        public void WhenThereIsABunchOfItems_ReturnExactNumberOfItemsRegardlessOfColumnAndRowCount()
        {
            //Arrange
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<string>();
            Instance[Dummy.Create<int>(), Dummy.Create<int>()] = Dummy.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(3);
        }
    }

    [TestClass]
    public class Indexer_XY : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtIndex_ReturnDefaultValue()
        {
            //Arrange

            //Act
            var result = Instance[2, 3];

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsValueAtIndex_ReturnValue()
        {
            //Arrange
            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var value = Dummy.Create<string>();
            Instance[x, y] = value;

            //Act
            var result = Instance[x, y];

            //Assert
            result.Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_AddValueAtIndex()
        {
            //Arrange
            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var value = Dummy.Create<string>();

            //Act
            Instance[x, y] = value;

            //Assert
            Instance[x, y].Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var value = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance[x, y] = value;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(x, y, value) } }
            });
        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_ReplaceExistingValueByNewValue()
        {
            //Arrange
            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var oldValue = Dummy.Create<string>();
            Instance[x, y] = oldValue;
            var newValue = Dummy.Create<string>();

            //Act
            Instance[x, y] = newValue;

            //Assert
            Instance[x, y].Should().Be(newValue);

        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var oldValue = Dummy.Create<string>();
            Instance[x, y] = oldValue;
            var newValue = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance[x, y] = newValue;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>> { new(x, y, newValue) },
                    OldValues = new List<Cell<string>> { new(x, y, oldValue) }
                }
            });
        }
    }

    [TestClass]
    public class Indexer_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtIndex_ReturnDefaultValue()
        {
            //Arrange

            //Act
            var result = Instance[new Vector2<int>(2, 3)];

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsValueAtIndex_ReturnValue()
        {
            //Arrange
            var coordinates = Dummy.Create<Vector2<int>>();
            var value = Dummy.Create<string>();
            Instance[coordinates] = value;

            //Act
            var result = Instance[coordinates];

            //Assert
            result.Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_AddValueAtIndex()
        {
            //Arrange
            var coordinates = Dummy.Create<Vector2<int>>();
            var value = Dummy.Create<string>();

            //Act
            Instance[coordinates] = value;

            //Assert
            Instance[coordinates].Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var coordinates = Dummy.Create<Vector2<int>>();
            var value = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance[coordinates] = value;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(coordinates, value) } }
            });
        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_ReplaceExistingValueByNewValue()
        {
            //Arrange
            var coordinates = Dummy.Create<Vector2<int>>();
            var oldValue = Dummy.Create<string>();
            Instance[coordinates] = oldValue;
            var newValue = Dummy.Create<string>();

            //Act
            Instance[coordinates] = newValue;

            //Assert
            Instance[coordinates].Should().Be(newValue);

        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var coordinates = Dummy.Create<Vector2<int>>();
            var oldValue = Dummy.Create<string>();
            Instance[coordinates] = oldValue;
            var newValue = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance[coordinates] = newValue;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>> { new(coordinates, newValue) },
                    OldValues = new List<Cell<string>> { new(coordinates, oldValue) }
                }
            });
        }
    }

    [TestClass]
    public class IndexesOf_Item : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmpty()
        {
            //Arrange
            var item = Dummy.Create<string>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreNoOccurences_ReturnEmpty()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var item = Dummy.Create<string>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOnlyOneOccurence_ReturnListWithOnlyThatOneItem()
        {
            //Arrange
            var item = Dummy.Create<string>();
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = item;

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(new List<Vector2<int>> { index });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurences_ReturnAllOccurences()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var indexes = Dummy.CreateMany<Vector2<int>>().ToList();
            var item = Dummy.Create<string>();
            foreach (var index in indexes)
                Instance.Add(index, item);

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(indexes);
        }

        [TestMethod]
        public void WhenSeekingNullAndThereAreNoOccurences_ReturnEmpty()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            //Act
            var result = Instance.IndexesOf((string)null!);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenSeekingNullAndThereIsOnlyOneOccurence_ReturnListWithOnlyThatOneItem()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = null;

            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            //Act
            var result = Instance.IndexesOf((string)null!);

            //Assert
            result.Should().BeEquivalentTo(new List<Vector2<int>> { index });
        }

        [TestMethod]
        public void WhenSeekingNullAndThereAreMultipleOccurences_ReturnAllOccurences()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var indexes = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexes)
                Instance.Add(index, null);

            //Act
            var result = Instance.IndexesOf((string)null!);

            //Assert
            result.Should().BeEquivalentTo(indexes);
        }
    }

    [TestClass]
    public class IndexesOf_Predicate : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            Func<string, bool> match = null!;

            //Act
            var action = () => Instance.IndexesOf(match!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.IndexesOf(x => x == "abc");

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenNothingMatches_ReturnEmpty()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            //Act
            var result = Instance.IndexesOf(x => x == "abc");

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneThingMatches_ReturnItsIndex()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var targetCell = cells.GetRandom();

            //Act
            var result = Instance.IndexesOf(x => x == targetCell.Value);

            //Assert
            result.Should().BeEquivalentTo(new List<Vector2<int>> { targetCell.Index });
        }

        [TestMethod]
        public void WhenMultipleThingsMatch_ReturnAllTheIndexes()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var sameValue = Dummy.Create<string>();
            var sameCells = Dummy.Build<Cell<string>>().With(x => x.Value, sameValue).CreateMany().ToList();
            foreach (var cell in sameCells)
                Instance[cell.Index] = cell.Value;

            //Act
            var result = Instance.IndexesOf(x => x == sameValue);

            //Assert
            result.Should().BeEquivalentTo(sameCells.Select(x => x.Index));
        }

        [TestMethod]
        public void WhenGridContainsNullValues_DoNotThrow()
        {
            //Arrange
            var indexesOfNulls = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexesOfNulls)
                Instance[index] = null;

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var targetCell = cells.GetRandom();

            //Act
            var result = Instance.IndexesOf(x => x == targetCell.Value);

            //Assert
            result.Should().BeEquivalentTo(new List<Vector2<int>> { targetCell.Index });
        }

        [TestMethod]
        public void WhenGridContainsNullValuesAndMatchIsNull_ReturnIndexesOfNullValues()
        {
            //Arrange
            var indexesOfNulls = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexesOfNulls)
                Instance[index] = null;

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var targetCell = cells.GetRandom();

            //Act
            var result = Instance.IndexesOf(x => x == null);

            //Assert
            result.Should().BeEquivalentTo(indexesOfNulls);
        }
    }

    [TestClass]
    public class Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_AllValuesAreZero()
        {
            //Arrange

            //Act
            var result = Instance.Boundaries;

            //Assert
            result.Should().BeEquivalentTo(new Boundaries<int>());
        }

        [TestMethod]
        public void WhenGridHasItems_ReturnBoundaries()
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
    }

    [TestClass]
    public class Constructor_Cells : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> cells = null!;

            //Act
            var action = () => new Grid<Garbage>(cells);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var cells = Array.Empty<Cell<Garbage>>();

            //Act
            var result = new Grid<Garbage>(cells);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Act
            var result = new Grid<Garbage>(cells);

            //Assert
            result.Should().BeEquivalentTo(cells);
        }
    }

    [TestClass]
    public class Constructor_KeyValuePairs : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Garbage>> pairs = null!;

            //Act
            var action = () => new Grid<Garbage>(pairs);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var pairs = Array.Empty<KeyValuePair<Vector2<int>, Garbage>>();

            //Act
            var result = new Grid<Garbage>(pairs);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var pairs = Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

            //Act
            var result = new Grid<Garbage>(pairs);

            //Assert
            result.Should().BeEquivalentTo(pairs.Select(x => new Cell<Garbage>(x.Key, x.Value)));
        }
    }

    [TestClass]
    public class Constructor_2dArray : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            Garbage[,] array = null!;

            //Act
            var action = () => new Grid<Garbage>(array);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var array = new Garbage[0, 0];

            //Act
            var result = new Grid<Garbage>(array);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var array = Dummy.Create<Garbage[,]>();

            //Act
            var result = new Grid<Garbage>(array);

            //Assert
            result.Should().BeEquivalentTo(array.ToGrid());
        }
    }

    [TestClass]
    public class Constructor_JaggedArray : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            Garbage[][] array = null!;

            //Act
            var action = () => new Grid<Garbage>(array);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var array = Array.Empty<Garbage[]>();

            //Act
            var result = new Grid<Garbage>(array);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var array = Dummy.Create<Garbage[][]>();

            //Act
            var result = new Grid<Garbage>(array);

            //Assert
            result.Should().BeEquivalentTo(array.ToGrid());
        }
    }

    [TestClass]
    public class Add_XY : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenIndexIsPositive_AddAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenItemIsNull_AddNullAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            Instance.Add(index.X, index.Y, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_DoNotTriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, null);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<string>>(),
                    NewValues = new List<Cell<string>> { new(index, null) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, null);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }


    }

    [TestClass]
    public class Add_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            //Act
            Instance.Add(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenIndexIsPositive_AddAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            //Act
            Instance.Add(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenItemIsNull_AddNullAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            Instance.Add(index, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNullButInNewCell_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index, null);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<string>>(),
                    NewValues = new List<Cell<string>> { new(index, null) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.Add(index, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.Add(index, null);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    [TestClass]
    public class Add_ValueType : ToolBX.Collections.UnitTesting.Tester<Grid<bool>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<bool>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<bool>();

            var eventArgs = new List<GridChangedEventArgs<bool>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<bool>>
            {
                new() { NewValues = new List<Cell<bool>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenIndexIsPositive_AddAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<bool>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<bool>();

            var eventArgs = new List<GridChangedEventArgs<bool>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<bool>>
            {
                new() { NewValues = new List<Cell<bool>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenItemIsDefaultValue_AddDefaultValueAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            Instance.Add(index.X, index.Y, default);

            //Assert
            Instance[index].Should().Be(default);
        }

        [TestMethod]
        public void WhenItemIsDefaultValue_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<bool>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(index.X, index.Y, default);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<bool>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<bool>>(),
                    NewValues = new List<Cell<bool>> { new(index, default) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<bool>();
            Instance.Add(index.X, index.Y, Dummy.Create<bool>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNegativeIndexWithDefaultValue_UpdateFirstRow()
        {
            //Arrange
            var index = -Dummy.Create<Vector2<int>>();

            //Act
            Instance.Add(index, default);

            //Assert
            Instance.FirstRow.Should().Be(index.Y);
        }

        [TestMethod]
        public void WhenAddingNegativeIndexWithDefaultValue_UpdateFirstColumn()
        {
            //Arrange
            var index = -Dummy.Create<Vector2<int>>();

            //Act
            Instance.Add(index, default);

            //Assert
            Instance.FirstColumn.Should().Be(index.X);
        }
    }

    [TestClass]
    public class TryAdd_XY : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenIndexIsPositive_AddAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenItemIsNull_AddNullAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            Instance.TryAdd(index.X, index.Y, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index.X, index.Y, null);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<string>>(),
                    NewValues = new List<Cell<string>> { new(index, null) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_DoNotReplace()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var originalItem = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(originalItem);
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_DoNotThrow()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index.X, index.Y, item);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotThrow()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index.X, index.Y, null);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotReplace()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var originalItem = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index.X, index.Y, null);

            //Assert
            Instance[index].Should().Be(originalItem);
        }
    }

    [TestClass]
    public class TryAdd_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            //Act
            Instance.TryAdd(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenIndexIsPositive_AddAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            //Act
            Instance.TryAdd(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index, item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new() { NewValues = new List<Cell<string>> { new(index, item) } }
            });
        }

        [TestMethod]
        public void WhenItemIsNull_AddNullAtIndex()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            Instance.TryAdd(index, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_TriggerEvent()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(index, null);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = Array.Empty<Cell<string>>(),
                    NewValues = new List<Cell<string>> { new(index, null) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_DoNotReplace()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();

            var originalItem = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index, item);

            //Assert
            Instance[index].Should().Be(originalItem);
        }

        [TestMethod]
        public void WhenThereIsAlreadySomethingAtIndex_DoNotThrow()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index, item);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotThrow()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Dummy.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index, null);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotReplace()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var originalItem = Dummy.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index, null);

            //Assert
            Instance[index].Should().Be(originalItem);
        }
    }

    [TestClass]
    public class Add_Cells_Params : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
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
        public void WhenAddingToNewCoordinates_Add()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
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
    }

    [TestClass]
    public class Add_Cells_Enumerable : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCellsIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> cells = null!;

            //Act
            var action = () => Instance.Add(cells);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
        }

        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_Throw()
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
        public void WhenAddingToNewCoordinates_Add()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
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
    }

    [TestClass]
    public class TryAdd_Cells_Params : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCellsIsNull_DoNotThrow()
        {
            //Arrange
            Cell<Garbage>[] cells = null!;

            //Act
            var action = () => Instance.TryAdd(cells);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_StillAddThoseThatAreNotAlreadyIn()
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
        public void WhenAddingToNewCoordinates_Add()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToArray();

            //Act
            Instance.TryAdd(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
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
    }

    [TestClass]
    public class TryAdd_Cells_Enumerable : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCellsIsNull_DoNotThrow()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> cells = null!;

            //Act
            var action = () => Instance.TryAdd(cells);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_DoNotThrow()
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
        public void WhenThereIsAlreadySomethingAtCoordinates_StillAddThoseThatAreNotAlreadyIn()
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
        public void WhenAddingToNewCoordinates_Add()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Act
            Instance.TryAdd(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
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
    }

    [TestClass]
    public class RemoveAt_XY : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index.X, index.Y);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = Dummy.Create<string>();

            //Act
            Instance.RemoveAt(index.X, index.Y);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance[index] = item;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.RemoveAt(index.X, index.Y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>> { new() { OldValues = new List<Cell<string>> { new(index, item) } } });
        }
    }

    [TestClass]
    public class TryRemoveAt_XY : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_DoNotThrow()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = Dummy.Create<string>();

            //Act
            Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance[index] = item;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>> { new() { OldValues = new List<Cell<string>> { new(index, item) } } });
        }
    }

    [TestClass]
    public class RemoveAt_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = Dummy.Create<string>();

            //Act
            Instance.RemoveAt(index);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance[index] = item;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.RemoveAt(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>> { new() { OldValues = new List<Cell<string>> { new(index, item) } } });
        }
    }

    [TestClass]
    public class TryRemoveAt_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = Dummy.Create<string>();

            //Act
            Instance.TryRemoveAt(index);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            var item = Dummy.Create<string>();
            Instance[index] = item;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemoveAt(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>> { new() { OldValues = new List<Cell<string>> { new(index, item) } } });
        }
    }

    [TestClass]
    public class RemoveAll_Item : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenItemIsNull_RemoveAllKeysThatPointToNullReferences()
        {
            //Arrange
            var keys = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in nonNullEntries)
                Instance[entry.Index] = entry.Value;

            //Act
            Instance.RemoveAll((string)null!);

            //Assert
            Instance.Should().NotContain(x => x.Value == null);
        }

        [TestMethod]
        public void WhenItemIsNull_DoNotRemoveNonNullReferences()
        {
            //Arrange
            var keys = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in nonNullEntries)
                Instance[entry.Index] = entry.Value;

            //Act
            Instance.RemoveAll((string)null!);

            //Assert
            Instance.Should().Contain(new Grid<string>(nonNullEntries));
        }

        [TestMethod]
        public void WhenItemIsNull_TriggerEvent()
        {
            //Arrange
            var keys = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in nonNullEntries)
                Instance[entry.Index] = entry.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.RemoveAll((string)null!);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = keys.Select(x => new Cell<string>(x, null)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInGrid_DoNotModifyCollection()
        {
            //Arrange
            var entries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var copy = Instance.Copy();

            var item = Dummy.Create<string>();

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInGrid_DoNotTriggerEvent()
        {
            //Arrange
            var entries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            var item = Dummy.Create<string>();

            //Act
            Instance.RemoveAll(item);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreOccurencesOfItemInGrid_RemoveAllThoseOccurences()
        {
            //Arrange
            var entries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var item = Dummy.Create<string>();
            var keys = Dummy.CreateMany<Vector2<int>>();
            foreach (var key in keys)
                Instance[key] = item;

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>(entries));
        }

        [TestMethod]
        public void WhenThereAreOccurencesOfItemInGrid_TriggerEvent()
        {
            //Arrange
            var entries = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var item = Dummy.Create<string>();
            var keys = Dummy.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = item;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.RemoveAll(item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    OldValues = keys.Select(x => new Cell<string>(x, item)).ToList()
                }
            });
        }
    }

    [TestClass]
    public class RemoveAll_Predicate : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<Garbage?, bool> match = null!;

            //Act
            var action = () => Instance.RemoveAll(match);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenNoItemsMatch_DoNotModifyGrid()
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
        public void WhenNoItemsMatch_DoNotTriggerEvent()
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
        public void WhenItemsMatch_RemoveThoseItems()
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
        public void WhenItemsMatch_TriggerEvent()
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
    }

    [TestClass]
    public class Contains_Coordinates_Item : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenThereIsItemAtIndex_ReturnTrue()
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
        public void WhenSomethingElseIsAtIndex_ReturnFalse()
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
        public void WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
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
        public void WhenThereIsNothingAtIndexAndItemIsNull_ReturnTrue()
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
        public void WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
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
    }

    [TestClass]
    public class Contains_XY_Item : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenThereIsItemAtIndex_ReturnTrue()
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
        public void WhenSomethingElseIsAtIndex_ReturnFalse()
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
        public void WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
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
        public void WhenThereIsNothingAtIndexAndItemIsNull_ReturnTrue()
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
        public void WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
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
    }

    [TestClass]
    public class Contains_Item : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenGridIsEmptyAndSeekingNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(null);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsEmptyAndSeekingSomething_ReturnFalse()
        {
            //Arrange
            var item = Dummy.Create<Garbage>();

            //Act
            var result = Instance.Contains(item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsAtLeastOneNullValueSomewhereAndSeekingNull_ReturnTrue()
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
        public void WhenThereAreNoNullValuesAndSeekingNull_ReturnFalse()
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
        public void WhenThereIsAtLeastOneOccurenceOfItemInGrid_ReturnTrue()
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
    }

    [TestClass]
    public class Contains_XY : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnFalse()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
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
        public void WhenThereIsSomethingAtIndex_ReturnTrue()
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
        public void WhenThereIsNullAtIndex_ReturnTrue()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = null;

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnFalse()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
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
        public void WhenThereIsSomethingAtIndex_ReturnTrue()
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
        public void WhenThereIsNullAtIndex_ReturnTrue()
        {
            //Arrange
            var index = Dummy.Create<Vector2<int>>();
            Instance[index] = null;

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class FloodFill_XY_NewValue : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(index.X, index.Y, newValue);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(index.X, index.Y, newValue);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotModifyGrid()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(4, 5, "A");

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotTriggerChange()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(4, 5, "A");

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            //Act
            Instance.FloodFill(0, 1, "C");

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { -1, 0, "A" },
                { 0, 0, "A" },
                { 1, 0, "A" },
                { -1, 1, "A" },
                { 0, 1, "C" },
                { 1, 1, "A" },
                { -1, 2, "A" },
                { 0, 2, "A" },
                { 1, 2, "A" },
            });
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(0, 1, "C");

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>{new(0,1, "C")},
                    OldValues = new List<Cell<string>>{new(0,1,"B")}
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGrid_DoNothing()
        {
            //Arrange

            //Act
            Instance.FloodFill(1, 2, "F");

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGrid_DoNotTriggerChangeEvent()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(1, 2, "F");

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(1, 2, "D");

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "D" },
                { 1, 0, "D" },
                { 2, 0, "D" },
                { 3, 0, "D" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "D" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "D" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "D" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(1, 2, "D");

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0, 0, "D"),
                        new(1, 0, "D"),
                        new(2, 0, "D"),
                        new(3, 0, "D"),
                        new(0, 1, "D"),
                        new(1, 1, "D"),
                        new(2, 1, "D"),
                        new(3, 1, "D"),
                        new(0, 2, "D"),
                        new(1, 2, "D"),
                        new(2, 2, "D"),
                        new(3, 2, "D"),
                        new(0, 3, "D"),
                        new(1, 3, "D"),
                        new(2, 3, "D"),
                        new(3, 3, "D"),
                    },
                    OldValues = new List<Cell<string>>
                    {
                        new(0, 0, "B"),
                        new(1, 0, "B"),
                        new(2, 0, "B"),
                        new(3, 0, "B"),
                        new(0, 1, "B"),
                        new(1, 1, "B"),
                        new(2, 1, "B"),
                        new(3, 1, "B"),
                        new(0, 2, "B"),
                        new(1, 2, "B"),
                        new(2, 2, "B"),
                        new(3, 2, "B"),
                        new(0, 3, "B"),
                        new(1, 3, "B"),
                        new(2, 3, "B"),
                        new(3, 3, "B"),
                    }
                }
            });
        }
    }

    [TestClass]
    public class FloodFill_Coordinates_NewValue : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(index, newValue);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(index, newValue);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotModifyGrid()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(new Vector2<int>(4, 5), "A");

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotTriggerChange()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(4, 5), "A");

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            //Act
            Instance.FloodFill(new Vector2<int>(0, 1), "C");

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { -1, 0, "A" },
                { 0, 0, "A" },
                { 1, 0, "A" },
                { -1, 1, "A" },
                { 0, 1, "C" },
                { 1, 1, "A" },
                { -1, 2, "A" },
                { 0, 2, "A" },
                { 1, 2, "A" },
            });
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(0, 1), "C");

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>{new(0,1, "C")},
                    OldValues = new List<Cell<string>>{new(0,1,"B")}
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGrid_DoNothing()
        {
            //Arrange

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "F");

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGrid_DoNotTriggerChangeEvent()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "F");

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(1, 2, "D");

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "D" },
                { 1, 0, "D" },
                { 2, 0, "D" },
                { 3, 0, "D" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "D" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "D" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "D" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "D");

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0, 0, "D"),
                        new(1, 0, "D"),
                        new(2, 0, "D"),
                        new(3, 0, "D"),
                        new(0, 1, "D"),
                        new(1, 1, "D"),
                        new(2, 1, "D"),
                        new(3, 1, "D"),
                        new(0, 2, "D"),
                        new(1, 2, "D"),
                        new(2, 2, "D"),
                        new(3, 2, "D"),
                        new(0, 3, "D"),
                        new(1, 3, "D"),
                        new(2, 3, "D"),
                        new(3, 3, "D"),
                    },
                    OldValues = new List<Cell<string>>
                    {
                        new(0, 0, "B"),
                        new(1, 0, "B"),
                        new(2, 0, "B"),
                        new(3, 0, "B"),
                        new(0, 1, "B"),
                        new(1, 1, "B"),
                        new(2, 1, "B"),
                        new(3, 1, "B"),
                        new(0, 2, "B"),
                        new(1, 2, "B"),
                        new(2, 2, "B"),
                        new(3, 2, "B"),
                        new(0, 3, "B"),
                        new(1, 3, "B"),
                        new(2, 3, "B"),
                        new(3, 3, "B"),
                    }
                }
            });
        }
    }

    [TestClass]
    public class FloodFill_XY_NewValue_Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(index.X, index.Y, newValue, boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(index.X, index.Y, newValue, boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotModifyGrid()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(4, 5, "A", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotTriggerChange()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(4, 5, "A", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            //Act
            Instance.FloodFill(0, 1, "C", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { -1, 0, "A" },
                { 0, 0, "A" },
                { 1, 0, "A" },
                { -1, 1, "A" },
                { 0, 1, "C" },
                { 1, 1, "A" },
                { -1, 2, "A" },
                { 0, 2, "A" },
                { 1, 2, "A" },
            });
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(0, 1, "C", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>{new(0,1, "C")},
                    OldValues = new List<Cell<string>>{new(0,1,"B")}
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGridWithLargeBoundaries_FillEntireBoundaries()
        {
            //Arrange

            //Act
            Instance.FloodFill(1, 2, "F", new Boundaries<int>(0, 2, 2, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                {0,0, "F"},
                {1,0, "F"},
                {2,0, "F"},
                {0,1, "F"},
                {1,1, "F"},
                {2,1, "F"},
                {0,2, "F"},
                {1,2, "F"},
                {2,2, "F"},
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGridWithLargeBoundaries_TriggerChangeEventOnce()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(1, 2, "F", new Boundaries<int>(0, 2, 2, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0,0, "F"),
                        new(1,0, "F"),
                        new(2,0, "F"),
                        new(0,1, "F"),
                        new(1,1, "F"),
                        new(2,1, "F"),
                        new(0,2, "F"),
                        new(1,2, "F"),
                        new(2,2, "F"),
                    },
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(1, 2, "D", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "D" },
                { 1, 0, "D" },
                { 2, 0, "D" },
                { 3, 0, "D" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "D" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "D" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "D" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(1, 2, "D", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0, 0, "D"),
                        new(1, 0, "D"),
                        new(2, 0, "D"),
                        new(3, 0, "D"),
                        new(0, 1, "D"),
                        new(1, 1, "D"),
                        new(2, 1, "D"),
                        new(3, 1, "D"),
                        new(0, 2, "D"),
                        new(1, 2, "D"),
                        new(2, 2, "D"),
                        new(3, 2, "D"),
                        new(0, 3, "D"),
                        new(1, 3, "D"),
                        new(2, 3, "D"),
                        new(3, 3, "D"),
                    },
                    OldValues = new List<Cell<string>>
                    {
                        new(0, 0, "B"),
                        new(1, 0, "B"),
                        new(2, 0, "B"),
                        new(3, 0, "B"),
                        new(0, 1, "B"),
                        new(1, 1, "B"),
                        new(2, 1, "B"),
                        new(3, 1, "B"),
                        new(0, 2, "B"),
                        new(1, 2, "B"),
                        new(2, 2, "B"),
                        new(3, 2, "B"),
                        new(0, 3, "B"),
                        new(1, 3, "B"),
                        new(2, 3, "B"),
                        new(3, 3, "B"),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnAreaWithSimilarNeighbors_FillOnlyWithinBoundaries()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(1, 2, "D", new Boundaries<int>(1, 2, 3, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "B" },
                { 1, 0, "B" },
                { 2, 0, "B" },
                { 3, 0, "B" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "B" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "B" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "B" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }
    }

    [TestClass]
    public class FloodFill_Coordinates_NewValue_Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(index, newValue, boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Dummy.Create<string>();

            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(index, newValue, boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotModifyGrid()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var copy = Instance.Copy();

            //Act
            Instance.FloodFill(new Vector2<int>(4, 5), "A", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToFillUsingSameValue_DoNotTriggerChange()
        {
            //Arrange
            Instance[4, 4] = "A";
            Instance[4, 5] = "A";
            Instance[4, 6] = "A";
            Instance[5, 4] = "A";
            Instance[6, 4] = "A";
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[4, 0] = "B";
            Instance[4, 1] = "B";
            Instance[4, 2] = "B";
            Instance[4, 3] = "B";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(4, 5), "A", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            //Act
            Instance.FloodFill(new Vector2<int>(0, 1), "C", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { -1, 0, "A" },
                { 0, 0, "A" },
                { 1, 0, "A" },
                { -1, 1, "A" },
                { 0, 1, "C" },
                { 1, 1, "A" },
                { -1, 2, "A" },
                { 0, 2, "A" },
                { 1, 2, "A" },
            });
        }

        [TestMethod]
        public void WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
        {
            //Arrange
            Instance[-1, 0] = "A";
            Instance[0, 0] = "A";
            Instance[1, 0] = "A";
            Instance[-1, 1] = "A";
            Instance[0, 1] = "B";
            Instance[1, 1] = "A";
            Instance[-1, 2] = "A";
            Instance[0, 2] = "A";
            Instance[1, 2] = "A";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(0, 1), "C", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>{new(0,1, "C")},
                    OldValues = new List<Cell<string>>{new(0,1,"B")}
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGridWithLargeBoundaries_FillEntireBoundaries()
        {
            //Arrange

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "F", new Boundaries<int>(0, 2, 2, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                {0,0, "F"},
                {1,0, "F"},
                {2,0, "F"},
                {0,1, "F"},
                {1,1, "F"},
                {2,1, "F"},
                {0,2, "F"},
                {1,2, "F"},
                {2,2, "F"},
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnEmptyGridWithLargeBoundaries_TriggerChangeEventOnce()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "F", new Boundaries<int>(0, 2, 2, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0,0, "F"),
                        new(1,0, "F"),
                        new(2,0, "F"),
                        new(0,1, "F"),
                        new(1,1, "F"),
                        new(2,1, "F"),
                        new(0,2, "F"),
                        new(1,2, "F"),
                        new(2,2, "F"),
                    },
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(1, 2, "D", Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "D" },
                { 1, 0, "D" },
                { 2, 0, "D" },
                { 3, 0, "D" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "D" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "D" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "D" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }

        [TestMethod]
        public void WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "D", Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<string>>
            {
                new()
                {
                    NewValues = new List<Cell<string>>
                    {
                        new(0, 0, "D"),
                        new(1, 0, "D"),
                        new(2, 0, "D"),
                        new(3, 0, "D"),
                        new(0, 1, "D"),
                        new(1, 1, "D"),
                        new(2, 1, "D"),
                        new(3, 1, "D"),
                        new(0, 2, "D"),
                        new(1, 2, "D"),
                        new(2, 2, "D"),
                        new(3, 2, "D"),
                        new(0, 3, "D"),
                        new(1, 3, "D"),
                        new(2, 3, "D"),
                        new(3, 3, "D"),
                    },
                    OldValues = new List<Cell<string>>
                    {
                        new(0, 0, "B"),
                        new(1, 0, "B"),
                        new(2, 0, "B"),
                        new(3, 0, "B"),
                        new(0, 1, "B"),
                        new(1, 1, "B"),
                        new(2, 1, "B"),
                        new(3, 1, "B"),
                        new(0, 2, "B"),
                        new(1, 2, "B"),
                        new(2, 2, "B"),
                        new(3, 2, "B"),
                        new(0, 3, "B"),
                        new(1, 3, "B"),
                        new(2, 3, "B"),
                        new(3, 3, "B"),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTryingToFillAnAreaWithSimilarNeighbors_FillOnlyWithinBoundaries()
        {
            //Arrange
            Instance[0, 0] = "B";
            Instance[1, 0] = "B";
            Instance[2, 0] = "B";
            Instance[3, 0] = "B";
            Instance[0, 1] = "B";
            Instance[1, 1] = "B";
            Instance[2, 1] = "B";
            Instance[3, 1] = "B";
            Instance[0, 2] = "B";
            Instance[1, 2] = "B";
            Instance[2, 2] = "B";
            Instance[3, 2] = "B";
            Instance[0, 3] = "B";
            Instance[1, 3] = "B";
            Instance[2, 3] = "B";
            Instance[3, 3] = "B";
            Instance[0, 4] = "C";
            Instance[1, 4] = "C";
            Instance[2, 4] = "C";
            Instance[3, 4] = "C";

            //Act
            Instance.FloodFill(new Vector2<int>(1, 2), "D", new Boundaries<int>(1, 2, 3, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, "B" },
                { 1, 0, "B" },
                { 2, 0, "B" },
                { 3, 0, "B" },
                { 0, 1, "D" },
                { 1, 1, "D" },
                { 2, 1, "D" },
                { 3, 1, "B" },
                { 0, 2, "D" },
                { 1, 2, "D" },
                { 2, 2, "D" },
                { 3, 2, "B" },
                { 0, 3, "D" },
                { 1, 3, "D" },
                { 2, 3, "D" },
                { 3, 3, "B" },
                { 0, 4, "C" },
                { 1, 4, "C" },
                { 2, 4, "C" },
                { 3, 4, "C" },
            });
        }
    }

    [TestClass]
    public class FloodClear_XY : ToolBX.Collections.UnitTesting.Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodClear(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(1, 1);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {0, 0, 1},
                {1, 0, 1},
                {2, 0, 1},
                {0, 1, 1},
                {2, 1, 1},
                {0, 2, 1},
                {1, 2, 1},
                {2, 2, 1},
            });
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(1, 1);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(1, 1, 2)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(0, 1);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {1, 1, 2}
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(1, 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    }
                }
            });
        }
    }

    [TestClass]
    public class FloodClear_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Dummy.Create<Vector2<int>>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Dummy.Create<Vector2<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            var index = new Vector2<int>(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Act
            Instance.FloodClear(index);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            var index = new Vector2<int>(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Act
            Instance.FloodClear(index);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(new Vector2<int>(1, 1));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {0, 0, 1},
                {1, 0, 1},
                {2, 0, 1},
                {0, 1, 1},
                {2, 1, 1},
                {0, 2, 1},
                {1, 2, 1},
                {2, 2, 1},
            });
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(new Vector2<int>(1, 1));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(1, 1, 2)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(new Vector2<int>(0, 1));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {1, 1, 2}
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(new Vector2<int>(1, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    }
                }
            });
        }
    }

    [TestClass]
    public class FloodClear_XY_Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>(), Dummy.Create<Boundaries<int>>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>(), Dummy.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.FloodClear(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1, Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1, Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(1, 1, Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {0, 0, 1},
                {1, 0, 1},
                {2, 0, 1},
                {0, 1, 1},
                {2, 1, 1},
                {0, 2, 1},
                {1, 2, 1},
                {2, 2, 1},
            });
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(1, 1, Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(1, 1, 2)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(0, 1, Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {1, 1, 2}
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(1, 0, Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ConstrainDeletionToBoundaries()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(0, 1, new Boundaries<int> { Bottom = 1, Right = 2 });

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                { 1, 1, 2 },
                { 0, 2, 1 },
                { 1, 2, 1 },
                { 2, 2, 1 },
            });
        }
    }

    [TestClass]
    public class FloodClear_Coordinates_Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Dummy.Create<Vector2<int>>(), Dummy.Create<Boundaries<int>>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(Dummy.Create<Vector2<int>>(), Dummy.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            var index = new Vector2<int>(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Act
            Instance.FloodClear(index, Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            var index = new Vector2<int>(Instance.Boundaries.Right + 1, Instance.Boundaries.Bottom + 1);

            //Act
            Instance.FloodClear(index, Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(new Vector2<int>(1, 1), Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {0, 0, 1},
                {1, 0, 1},
                {2, 0, 1},
                {0, 1, 1},
                {2, 1, 1},
                {0, 2, 1},
                {1, 2, 1},
                {2, 2, 1},
            });
        }

        [TestMethod]
        public void WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(new Vector2<int>(1, 1), Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(1, 1, 2)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(new Vector2<int>(0, 1), Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                {1, 1, 2}
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_TriggerChange()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.FloodClear(new Vector2<int>(1, 0), Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsSurroundedBySameNeighbors_ConstrainDeletionToBoundaries()
        {
            //Arrange
            Instance[0, 0] = 1;
            Instance[1, 0] = 1;
            Instance[2, 0] = 1;
            Instance[0, 1] = 1;
            Instance[1, 1] = 2;
            Instance[2, 1] = 1;
            Instance[0, 2] = 1;
            Instance[1, 2] = 1;
            Instance[2, 2] = 1;

            //Act
            Instance.FloodClear(new Vector2<int>(0, 1), new Boundaries<int> { Bottom = 1, Right = 2 });

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                { 1, 1, 2 },
                { 0, 2, 1 },
                { 1, 2, 1 },
                { 2, 2, 1 },
            });
        }
    }

    [TestClass]
    public class Resize : ToolBX.Collections.UnitTesting.Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.Resize(Dummy.Create<Boundaries<int>>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Resize(Dummy.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToResizeUsingSameSize_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.Resize(Instance.Boundaries);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToResizeUsingSameSize_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Resize(Instance.Boundaries);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToResizeUsingLargerBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.Resize(Instance.Boundaries with { Top = Instance.Boundaries.Top - 1, Left = Instance.Boundaries.Left - 1, Bottom = Instance.Boundaries.Bottom + 1, Right = Instance.Boundaries.Right + 1 });

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenTryingToResizeUsingLargerBoundaries_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<int>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Resize(Instance.Boundaries with { Top = Instance.Boundaries.Top - 1, Left = Instance.Boundaries.Left - 1, Bottom = Instance.Boundaries.Bottom + 1, Right = Instance.Boundaries.Right + 1 });

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenResizingWithSmallerBoundaries_RemoveExcessItems()
        {
            //Arrange
            Instance[-1, 0] = 1;
            Instance[0, 0] = 2;
            Instance[1, 0] = 3;
            Instance[-1, 1] = 4;
            Instance[0, 1] = 5;
            Instance[1, 1] = 6;
            Instance[-1, 2] = 7;
            Instance[0, 2] = 8;
            Instance[1, 2] = 9;

            //Act
            Instance.Resize(new Boundaries<int>
            {
                Bottom = 1,
                Right = 2,
                Left = 0
            });

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                { 0, 0, 2 },
                { 1, 0, 3 },
                { 0, 1, 5 },
                { 1, 1, 6 }
            });
        }

        [TestMethod]
        public void WhenResizingWithSmallerBoundaries_TriggerChange()
        {
            //Arrange
            Instance[-1, 0] = 1;
            Instance[0, 0] = 2;
            Instance[1, 0] = 3;
            Instance[-1, 1] = 4;
            Instance[0, 1] = 5;
            Instance[1, 1] = 6;
            Instance[-1, 2] = 7;
            Instance[0, 2] = 8;
            Instance[1, 2] = 9;

            var eventArgs = new List<GridChangedEventArgs<int>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Resize(new Boundaries<int>
            {
                Bottom = 1,
                Right = 2,
                Left = 0
            });

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<int>>
            {
                new()
                {
                    OldValues = new List<Cell<int>>
                    {
                        new(-1,0,1),
                        new(-1,1,4),
                        new(-1,2,7),
                        new(0,2,8),
                        new(1,2,9),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenResizingWithZeroBoundaries_OnlyContainsItemAtOrigin()
        {
            //Arrange
            Instance[-1, 0] = 1;
            Instance[0, 0] = 2;
            Instance[1, 0] = 3;
            Instance[-1, 1] = 4;
            Instance[0, 1] = 5;
            Instance[1, 1] = 6;
            Instance[-1, 2] = 7;
            Instance[0, 2] = 8;
            Instance[1, 2] = 9;

            //Act
            Instance.Resize(new Boundaries<int>(0, 0, 0, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<int>
            {
                { 0, 0, 2 }
            });
        }
    }

    [TestClass]
    public class TranslateAll_XY : ToolBX.Collections.UnitTesting.Tester<Grid<char>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.TranslateAll(Dummy.Create<int>(), Dummy.Create<int>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(Dummy.Create<int>(), Dummy.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotModifyGrid()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.TranslateAll(0, 0);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(0, 0);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = 0;
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = 0;
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingUp_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingUp_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = 0;
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = 0;
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingDown_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingDown_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveUpLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveUpRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = -Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveDownLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveDownRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Dummy.Create<int>();
            var y = Dummy.Create<int>();
            var index = new Vector2<int>(x, y);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(x, y);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }
    }

    [TestClass]
    public class TranslateAll_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<char>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.TranslateAll(Dummy.Create<Vector2<int>>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(Dummy.Create<Vector2<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotModifyGrid()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var copy = Instance.Copy();

            //Act
            Instance.TranslateAll(new Vector2<int>(0, 0));

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotTriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(new Vector2<int>(0, 0));

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), 0);

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), 0);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingUp_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, -Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingUp_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, -Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), 0);

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), 0);

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingDown_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingDown_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveUpLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), -Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveUpRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), -Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), -Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveDownLeft_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownLeft_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Dummy.Create<int>(), Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTryingToMoveDownRight_Move()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), Dummy.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownRight_TriggerChange()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Dummy.Create<int>(), Dummy.Create<int>());

            var eventArgs = new List<GridChangedEventArgs<char>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TranslateAll(index);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<char>>
            {
                new()
                {
                    OldValues = cells,
                    NewValues = cells.Select(x => new Cell<char>(x.Index + index, x.Value)).ToList()
                }
            });
        }
    }

    [TestClass]
    public class Translate_Range_XY : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotModify()
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
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
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
        public void WhenGridIsEmpty_DoNotModify()
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
        public void WhenGridIsEmpty_DoNotTriggerChange()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
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
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
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
        public void WhenMovingRectangleOverEmptyArea_TriggerChange()
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
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Range_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotModify()
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
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
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
        public void WhenGridIsEmpty_DoNotModify()
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
        public void WhenGridIsEmpty_DoNotTriggerChange()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
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
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
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
        public void WhenMovingRectangleOverEmptyArea_TriggerChange()
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
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Boundaries_XY : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroBoundaries_DoNotModify()
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
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
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
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange
            var boundaries = Dummy.Create<Boundaries<int>>();

            //Act
            Instance.Translate(boundaries, Dummy.Create<int>(), Dummy.Create<int>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
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
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
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
        public void WhenMovingRectangleOverEmptyArea_TriggerChange()
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
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Boundaries_Coordinates : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroBoundaries_DoNotModify()
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
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
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
        public void WhenGridIsEmpty_DoNotModify()
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
        public void WhenGridIsEmpty_DoNotTriggerChange()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
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
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_TriggerChange()
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
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(1, 0, copy[0, 0]),
                        new(1, 1, copy[0, 1]),
                        new(1, 2, copy[0, 2]),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenMovingRectangleOverEmptyArea_MoveThoseItemsToNewPositions()
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
        public void WhenMovingRectangleOverEmptyArea_TriggerChange()
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
                    NewValues = new List<Cell<Garbage>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Garbage>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                    }
                }
            });
        }
    }

    [TestClass]
    public class Copy : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmptyGrid()
        {
            //Arrange

            //Act
            var result = Instance.Copy();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsNotEmpty_ReturnExactCopy()
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
    }

    [TestClass]
    public class Copy_Boundaries : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenBoundariesAreEqualToGrid_ReturnExactCopyOfGrid()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            //Act
            var result = Instance.Copy(Instance.Boundaries);

            //Assert
            result.Should().BeEquivalentTo(Instance);
            result.Should().NotBeSameAs(Instance);
        }

        [TestMethod]
        public void WhenBoundariesAreOutsideGrid_ReturnEmpty()
        {
            //Arrange
            Instance[0, 0] = Dummy.Create<string>();
            Instance[1, 4] = Dummy.Create<string>();
            Instance[2, 3] = Dummy.Create<string>();
            Instance[3, 2] = Dummy.Create<string>();

            //Act
            var result = Instance.Copy(new Boundaries<int> { Top = -14, Left = -8, Right = -4, Bottom = -7 });

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreInsideGrid_ReturnThatPartOfGrid()
        {
            //Arrange
            Instance[0, 0] = Dummy.Create<string>();
            Instance[1, 4] = Dummy.Create<string>();
            Instance[2, 3] = Dummy.Create<string>();
            Instance[3, 2] = Dummy.Create<string>();

            //Act
            var result = Instance.Copy(new Boundaries<int> { Top = 0, Bottom = 2, Left = 0, Right = 3 });

            //Assert
            result.Should().BeEquivalentTo(new Grid<string>
            {
                { 0, 0, Instance[0, 0] },
                { 3, 2, Instance[3, 2] },
            });
        }

        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmptyGrid()
        {
            //Arrange

            //Act
            var result = Instance.Copy(Dummy.Create<Boundaries<int>>());

            //Assert
            result.Should().BeEmpty();
        }
    }

    [TestClass]
    public class Swap : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCurrentAndDestinationAreEqual_DoNotModifyGrid()
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
        public void WhenCurrentAndDestinationAreEqual_DoNotTriggerChange()
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
        public void WhenCurrentAndDestinationAreDifferent_Swap()
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
        public void WhenCurrentAndDestinationAreDifferent_TriggerChangeWithNewIndexes()
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
                    NewValues = new List<Cell<Garbage>> { new(2,0, secondItem), new(1, 2, firstItem) },
                    OldValues = new List<Cell<Garbage>> { new(2,0, firstItem), new(1, 2, secondItem) },
                }
            });
        }
    }

    [TestClass]
    public class ToStringMethod : ToolBX.Collections.UnitTesting.Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Empty Grid<String>");
        }

        [TestMethod]
        public void WhenGridIsNotEmpty_ReturnCount()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<string>>(5).ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Grid<String> with 5 items");
        }
    }

    [TestClass]
    public class Equals_Object : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenOtherIsSimilarGrid_ReturnTrue()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Copy();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferentGrid_ReturnFalse()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilar2dArray_ReturnTrue()
        {
            //Arrange
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
        public void WhenOtherIsDifferent2dArray_ReturnFalse()
        {
            //Arrange
            var cells = Dummy.Build<Cell<Garbage>>().With(x => x.Index, new Vector2<int>(Dummy.Number.Between(-5, 5).Create(), Dummy.Number.Between(-5, 5).Create())).CreateMany().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Dummy.Create<Garbage[,]>();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilarJaggedArray_ReturnTrue()
        {
            //Arrange
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
        public void WhenOtherIsDifferentJaggedArray_ReturnFalse()
        {
            //Arrange
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
        public void WhenOtherIsSimilarCells_ReturnTrue()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance.Equals((object)cells);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferentCells_ReturnFalse()
        {
            //Arrange
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
        public void WhenOtherIsSimilarKeyValuePairs_ReturnTrue()
        {
            //Arrange
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
        public void WhenOtherIsDifferentKeyValuePairs_ReturnFalse()
        {
            //Arrange
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
        public void WhenOtherIsDifferentUnsupportedType_ReturnFalse()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Dummy.Create<int>();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Equals_Grid : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenOtherGridIsNull_ReturnFalse()
        {
            //Arrange
            Grid<Garbage> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnFalse()
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
        public void WhenOtherGridIsSameReference_ReturnTrue()
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
        public void WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
        public void WhenOtherIsGridButOrderedDifferently_ReturnTrue()
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
        public void WhenOtherIsCellsButOrderedDifferently_ReturnTrue()
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
    }

    [TestClass]
    public class EqualsOperator_Grid : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == (Grid<Garbage>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == Dummy.CreateMany<Cell<Garbage>>().ToGrid();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnFalse()
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
        public void WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
    }

    [TestClass]
    public class NotEqualsOperator_Grid : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != (Grid<Garbage>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != Dummy.CreateMany<Cell<Garbage>>().ToGrid();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnTrue()
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
        public void WhenOtherGridAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
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
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
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
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
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
    }

    [TestClass]
    public class Equals_Cells : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenCellsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnFalse()
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
        public void WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
    }

    [TestClass]
    public class EqualsOperator_Cells : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == (IEnumerable<Cell<Garbage>>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnFalse()
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
        public void WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
    }

    [TestClass]
    public class NotEqualsOperator_Cells : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != (IEnumerable<Cell<Garbage>>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != Dummy.CreateMany<Cell<Garbage>>().ToList();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<Cell<Garbage>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnTrue()
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
        public void WhenCellsAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
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
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
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
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
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
    }

    [TestClass]
    public class Equals_KeyValuePairs : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
    }

    [TestClass]
    public class EqualsOperator_KeyValuePairs : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == (IEnumerable<KeyValuePair<Vector2<int>, Garbage>>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
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
    }

    [TestClass]
    public class NotEqualsOperator_KeyValuePairs : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != (IEnumerable<KeyValuePair<Vector2<int>, Garbage>>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != Dummy.CreateMany<KeyValuePair<Vector2<int>, Garbage>>().ToList();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Garbage>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnTrue()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
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
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
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
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
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
    }

    [TestClass]
    public class Equals_2dArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Garbage[,] other = null!;

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }

        [TestMethod]
        public void WhenBothAreEmpty_ReturnTrue()
        {
            //Arrange
            var other = new Garbage[0, 0];

            //Act
            //Assert
            Ensure.Equality(Instance, other);
        }

        [TestMethod]
        public void WhenGridIsEmptyButArrayIsNot_ReturnFalse()
        {
            //Arrange
            var other = Dummy.Create<Garbage[,]>();

            //Act
            //Assert
            Ensure.Inequality(Instance, other);

        }

        [TestMethod]
        public void WhenGridContainsItemsButArrayIsEmpty_ReturnFalse()
        {
            //Arrange
            Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

            var other = new Garbage[0, 0];

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }

        [TestMethod]
        public void WhenGridAndArrayContainNullValuesAtCorrespondingPositions_ReturnTrue()
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
        public void WhenArrayHasAnExtraRow_ReturnFalse()
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
        public void WhenGridHasAnExtraRow_ReturnFalse()
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
        public void WhenArrayHasAnExtraColumn_ReturnFalse()
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
        public void WhenGridHasAnExtraColumn_ReturnFalse()
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
        public void WhenGridHasMoreColumnsThanArray_ReturnFalse()
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
        public void WhenArrayIsDifferent_ReturnFalse()
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
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
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
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
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
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();

            //Act
            //Assert
            Ensure.Equality(Instance, other);
        }

        [TestMethod]
        public void WhenGridHasANegativeX_ReturnFalse()
        {
            //Arrange
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
        public void WhenGridHasANegativeY_ReturnFalse()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();

            Instance[Dummy.Create<int>(), -Dummy.Create<int>()] = Dummy.Create<Garbage>();

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }
    }

    [TestClass]
    public class EqualsOperator_2dArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == (Garbage[,])null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == Dummy.Create<Garbage[,]>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Garbage[,] other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_2dArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != (Garbage[,])null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != Dummy.Create<Garbage[,]>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnTrue()
        {
            //Arrange
            Garbage[,] other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_JaggedArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Garbage[][] other = null!;

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }

        [TestMethod]
        public void WhenBothAreEmpty_ReturnTrue()
        {
            //Arrange
            var other = Array.Empty<Garbage[]>();

            //Act
            //Assert
            Ensure.Equality(Instance, other);
        }

        [TestMethod]
        public void WhenGridIsEmptyButArrayIsNot_ReturnFalse()
        {
            //Arrange
            var other = Dummy.Create<Garbage[][]>();

            //Act
            //Assert
            Ensure.Inequality(Instance, other);

        }

        [TestMethod]
        public void WhenGridContainsItemsButArrayIsEmpty_ReturnFalse()
        {
            //Arrange
            Instance.Add(Dummy.CreateMany<Cell<Garbage>>());

            var other = Array.Empty<Garbage[]>();

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }

        [TestMethod]
        public void WhenGridAndArrayContainNullValuesAtCorrespondingPositions_ReturnTrue()
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
        public void WhenArrayHasAnExtraRow_ReturnFalse()
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
        public void WhenGridHasAnExtraRow_ReturnFalse()
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
        public void WhenArrayHasAnExtraColumn_ReturnFalse()
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
        public void WhenGridHasAnExtraColumn_ReturnFalse()
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
        public void WhenGridHasMoreColumnsThanArray_ReturnFalse()
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
        public void WhenArrayIsDifferent_ReturnFalse()
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
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
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
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
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
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();

            //Act
            //Assert
            Ensure.Equality(Instance, other);
        }

        [TestMethod]
        public void WhenGridHasANegativeX_ReturnFalse()
        {
            //Arrange
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
        public void WhenGridHasANegativeY_ReturnFalse()
        {
            //Arrange
            var cells = Dummy.CreateMany<Cell<Garbage>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();

            Instance[Dummy.Create<int>(), -Dummy.Create<int>()] = Dummy.Create<Garbage>();

            //Act
            //Assert
            Ensure.Inequality(Instance, other);
        }
    }

    [TestClass]
    public class EqualsOperator_JaggedArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == (Garbage[][])null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! == Dummy.Create<Garbage[][]>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Garbage[][] other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_JaggedArray : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != (Garbage[][])null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Garbage>)null! != Dummy.Create<Garbage[][]>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnTrue()
        {
            //Arrange
            Garbage[][] other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Enumerator : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void Always_Enumerates()
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
    }

    [TestClass]
    public class HashCode : Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void Always_ReturnInternalCollectionHashCode()
        {
            //Arrange

            //Act
            var result = Instance.GetHashCode();

            //Assert
            result.Should().Be(GetFieldValue<Dictionary<Vector2<int>, Garbage>>("_items")!.GetHashCode());
        }
    }

    [TestClass]
    public class Equality : ToolBX.Collections.UnitTesting.Tester
    {
        [TestMethod]
        public void Always_EnsureValueEquality() => Ensure.ValueEquality<Grid<Garbage>>(Dummy);
    }

    [TestClass]
    public class Serialization : ToolBX.Collections.UnitTesting.Tester<Grid<Garbage>>
    {
        [TestMethod]
        public void WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
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
        public void WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
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

}