namespace Collections.Grid.Tests;

[TestClass]
public class GridTests
{
    [TestClass]
    public class ColumnCount : Tester<Grid<string>>
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
            Instance[0, Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenHasOneItemAtNegativeColumnIndex_ReturnDifferenceBetweenThatAndZero()
        {
            //Arrange
            Instance[-3, Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(4);
        }

        [TestMethod]
        public void WhenHasOneItemAtColumnIndexGreaterThanZero_ReturnNumberOfColumns()
        {
            //Arrange
            Instance[5, Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenHasOneItemInNegativeColumnIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
        {
            //Arrange
            Instance[-3, Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[5, Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(9);
        }

        [TestMethod]
        public void WhenHasABunchOfColumns_ReturnDifferenceBetweenMinimumAndMaximum()
        {
            //Arrange
            Instance[-3, Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[-5, Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[5, Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[7, Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(13);
        }
    }

    [TestClass]
    public class RowCount : Tester<Grid<string>>
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
            Instance[Fixture.Create<int>(), 0] = Fixture.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenHasOneItemAtNegativeRowIndex_ReturnDifferenceBetweenThatAndZero()
        {
            //Arrange
            Instance[Fixture.Create<int>(), -3] = Fixture.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(4);
        }

        [TestMethod]
        public void WhenHasOneItemAtRowIndexGreaterThanZero_ReturnNumberOfRows()
        {
            //Arrange
            Instance[Fixture.Create<int>(), 5] = Fixture.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenHasOneItemInNegativeRowIndexAndOneAtPositiveGreaterThanZero_ReturnDifference()
        {
            //Arrange
            Instance[Fixture.Create<int>(), -3] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), 5] = Fixture.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(9);
        }

        [TestMethod]
        public void WhenHasABunchOfRows_ReturnDifferenceBetweenMinimumAndMaximum()
        {
            //Arrange
            Instance[Fixture.Create<int>(), -3] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), -5] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), 5] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), 7] = Fixture.Create<string>();

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(13);
        }
    }

    [TestClass]
    public class FirstColumn : Tester<Grid<Dummy>>
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
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[0, Fixture.Create<int>()] = item;

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenContainsSomethingInTheNegatives_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), Fixture.Create<int>()] = item;

            var negativeIndex = -Fixture.Create<int>();
            Instance[negativeIndex, Fixture.Create<int>()] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(negativeIndex);
        }

        [TestMethod]
        public void WhenContainsSomethingGreaterThanZero_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.CreateBetween(10, 30), Fixture.Create<int>()] = item;

            Instance[5, Fixture.Create<int>()] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(5);
        }
    }

    [TestClass]
    public class LastColumn : Tester<Grid<Dummy>>
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
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[0, Fixture.Create<int>()] = item;

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[-20, Fixture.Create<int>()] = item;

            var negativeIndex = -5;
            Instance[negativeIndex, Fixture.Create<int>()] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(negativeIndex);
        }

        [TestMethod]
        public void WhenHighestIsSomethingGreaterThanZero_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.CreateBetween(0, 20), Fixture.Create<int>()] = item;

            Instance[35, Fixture.Create<int>()] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(35);
        }
    }

    [TestClass]
    public class FirstRow : Tester<Grid<Dummy>>
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
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), 0] = item;

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenContainsSomethingInTheNegatives_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), Fixture.Create<int>()] = item;

            var negativeIndex = -Fixture.Create<int>();
            Instance[Fixture.Create<int>(), negativeIndex] = Fixture.Create<Dummy>();

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
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.CreateBetween(10, 30), Fixture.Create<int>()] = item;

            Instance[Fixture.Create<int>(), 5] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(5);
        }
    }

    [TestClass]
    public class LastRow : Tester<Grid<Dummy>>
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
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), 0] = item;

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenHighestIsSomethingInTheNegatives_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), -15] = item;

            var negativeIndex = -2;
            Instance[Fixture.Create<int>(), negativeIndex] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(negativeIndex);
        }

        [TestMethod]
        public void WhenHighestIsSomethingGreaterThanZero_ReturnThatIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>();
            foreach (var item in items)
                Instance[Fixture.Create<int>(), Fixture.CreateBetween(10, 30)] = item;

            Instance[Fixture.Create<int>(), 35] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(35);
        }
    }

    [TestClass]
    public class Count : Tester<Grid<string>>
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
            Instance[Fixture.Create<int>(), Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenThereAreTwoItemsAtOppositeSidesOfTheGrid_ReturnTwo()
        {
            //Arrange
            Instance[-10, -10] = Fixture.Create<string>();
            Instance[10, 10] = Fixture.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(2);
        }

        [TestMethod]
        public void WhenThereIsABunchOfItems_ReturnExactNumberOfItemsRegardlessOfColumnAndRowCount()
        {
            //Arrange
            Instance[Fixture.Create<int>(), Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), Fixture.Create<int>()] = Fixture.Create<string>();
            Instance[Fixture.Create<int>(), Fixture.Create<int>()] = Fixture.Create<string>();

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(3);
        }
    }

    [TestClass]
    public class Indexer_XY : Tester<Grid<string>>
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
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var value = Fixture.Create<string>();
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
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var value = Fixture.Create<string>();

            //Act
            Instance[x, y] = value;

            //Assert
            Instance[x, y].Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var value = Fixture.Create<string>();

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
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var oldValue = Fixture.Create<string>();
            Instance[x, y] = oldValue;
            var newValue = Fixture.Create<string>();

            //Act
            Instance[x, y] = newValue;

            //Assert
            Instance[x, y].Should().Be(newValue);

        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var oldValue = Fixture.Create<string>();
            Instance[x, y] = oldValue;
            var newValue = Fixture.Create<string>();

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
    public class Indexer_Coordinates : Tester<Grid<string>>
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
            var coordinates = Fixture.Create<Vector2<int>>();
            var value = Fixture.Create<string>();
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
            var coordinates = Fixture.Create<Vector2<int>>();
            var value = Fixture.Create<string>();

            //Act
            Instance[coordinates] = value;

            //Assert
            Instance[coordinates].Should().Be(value);
        }

        [TestMethod]
        public void WhenThereIsNothingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var coordinates = Fixture.Create<Vector2<int>>();
            var value = Fixture.Create<string>();

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
            var coordinates = Fixture.Create<Vector2<int>>();
            var oldValue = Fixture.Create<string>();
            Instance[coordinates] = oldValue;
            var newValue = Fixture.Create<string>();

            //Act
            Instance[coordinates] = newValue;

            //Assert
            Instance[coordinates].Should().Be(newValue);

        }

        [TestMethod]
        public void WhenThereIsSomethingAtGivenIndex_TriggerOnChange()
        {
            //Arrange
            var coordinates = Fixture.Create<Vector2<int>>();
            var oldValue = Fixture.Create<string>();
            Instance[coordinates] = oldValue;
            var newValue = Fixture.Create<string>();

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
    public class IndexesOf_Item : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmpty()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreNoOccurences_ReturnEmpty()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var item = Fixture.Create<string>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOnlyOneOccurence_ReturnListWithOnlyThatOneItem()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var index = Fixture.Create<Vector2<int>>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var indexes = Fixture.CreateMany<Vector2<int>>().ToList();
            var item = Fixture.Create<string>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
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
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = null;

            var cells = Fixture.CreateMany<Cell<string>>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance.Add(cell.Index, cell.Value);

            var indexes = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexes)
                Instance.Add(index, null);

            //Act
            var result = Instance.IndexesOf((string)null!);

            //Assert
            result.Should().BeEquivalentTo(indexes);
        }
    }

    [TestClass]
    public class IndexesOf_Predicate : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<string>>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
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
            var cells = Fixture.CreateMany<Cell<string>>();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var sameValue = Fixture.Create<string>();
            var sameCells = Fixture.Build<Cell<string>>().With(x => x.Value, sameValue).CreateMany().ToList();
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
            var indexesOfNulls = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexesOfNulls)
                Instance[index] = null;

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            var indexesOfNulls = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var index in indexesOfNulls)
                Instance[index] = null;

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
    public class Boundaries : Tester<Grid<Dummy>>
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
            Instance[3, 5] = Fixture.Create<Dummy>();
            Instance[-4, 9] = Fixture.Create<Dummy>();
            Instance[2, -14] = Fixture.Create<Dummy>();

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
    public class Constructor_Cells : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> cells = null!;

            //Act
            var action = () => new Grid<Dummy>(cells);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var cells = Array.Empty<Cell<Dummy>>();

            //Act
            var result = new Grid<Dummy>(cells);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            var result = new Grid<Dummy>(cells);

            //Assert
            result.Should().BeEquivalentTo(cells);
        }
    }

    [TestClass]
    public class Constructor_KeyValuePairs : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Dummy>> pairs = null!;

            //Act
            var action = () => new Grid<Dummy>(pairs);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var pairs = Array.Empty<KeyValuePair<Vector2<int>, Dummy>>();

            //Act
            var result = new Grid<Dummy>(pairs);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var pairs = Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            //Act
            var result = new Grid<Dummy>(pairs);

            //Assert
            result.Should().BeEquivalentTo(pairs.Select(x => new Cell<Dummy>(x.Key, x.Value)));
        }
    }

    [TestClass]
    public class Constructor_2dArray : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            Dummy[,] array = null!;

            //Act
            var action = () => new Grid<Dummy>(array);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var array = new Dummy[0, 0];

            //Act
            var result = new Grid<Dummy>(array);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var array = Fixture.Create<Dummy[,]>();

            //Act
            var result = new Grid<Dummy>(array);

            //Assert
            result.Should().BeEquivalentTo(array.ToGrid());
        }
    }

    [TestClass]
    public class Constructor_JaggedArray : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            Dummy[][] array = null!;

            //Act
            var action = () => new Grid<Dummy>(array);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmptyGrid()
        {
            //Arrange
            var array = Array.Empty<Dummy[]>();

            //Act
            var result = new Grid<Dummy>(array);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionIsNotEmpty_InstantiateWithContents()
        {
            //Arrange
            var array = Fixture.Create<Dummy[][]>();

            //Act
            var result = new Grid<Dummy>(array);

            //Assert
            result.Should().BeEquivalentTo(array.ToGrid());
        }
    }

    [TestClass]
    public class Add_XY : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Add(index.X, index.Y, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_DoNotTriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_Throw()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, null);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }


    }

    [TestClass]
    public class Add_Coordinates : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Add(index, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNullButInNewCell_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.Add(index, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_Throw()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.Add(index, null);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    [TestClass]
    public class Add_ValueType : Tester<Grid<bool>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<bool>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<bool>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<bool>();

            //Act
            Instance.Add(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<bool>();

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
            var index = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Add(index.X, index.Y, default);

            //Assert
            Instance[index].Should().Be(default);
        }

        [TestMethod]
        public void WhenItemIsDefaultValue_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<bool>();
            Instance.Add(index.X, index.Y, Fixture.Create<bool>());

            //Act
            var action = () => Instance.Add(index.X, index.Y, item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenAddingNegativeIndexWithDefaultValue_UpdateFirstRow()
        {
            //Arrange
            var index = -Fixture.Create<Vector2<int>>();

            //Act
            Instance.Add(index, default);

            //Assert
            Instance.FirstRow.Should().Be(index.Y);
        }

        [TestMethod]
        public void WhenAddingNegativeIndexWithDefaultValue_UpdateFirstColumn()
        {
            //Arrange
            var index = -Fixture.Create<Vector2<int>>();

            //Act
            Instance.Add(index, default);

            //Assert
            Instance.FirstColumn.Should().Be(index.X);
        }
    }

    [TestClass]
    public class TryAdd_XY : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            //Act
            Instance.TryAdd(index.X, index.Y, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();

            //Act
            Instance.TryAdd(index.X, index.Y, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            var originalItem = Fixture.Create<string>();
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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index.X, index.Y, item);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotThrow()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index.X, index.Y, null);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotReplace()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var originalItem = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index.X, index.Y, null);

            //Assert
            Instance[index].Should().Be(originalItem);
        }
    }

    [TestClass]
    public class TryAdd_Coordinates : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_AddAtIndex()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

            //Act
            Instance.TryAdd(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsNegative_TriggerEvent()
        {
            //Arrange
            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            //Act
            Instance.TryAdd(index, item);

            //Assert
            Instance[index].Should().Be(item);
        }

        [TestMethod]
        public void WhenIndexIsPositive_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

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
            var index = Fixture.Create<Vector2<int>>();

            //Act
            Instance.TryAdd(index, null);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenItemIsNull_TriggerEvent()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();

            var originalItem = Fixture.Create<string>();
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
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index, item);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotThrow()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index.X, index.Y, Fixture.Create<string>());

            //Act
            var action = () => Instance.TryAdd(index, null);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingNullAtExistingIndex_DoNotReplace()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var originalItem = Fixture.Create<string>();
            Instance.Add(index.X, index.Y, originalItem);

            //Act
            Instance.TryAdd(index, null);

            //Assert
            Instance[index].Should().Be(originalItem);
        }
    }

    [TestClass]
    public class Add_Cells_Params : Tester<Grid<Dummy>>
    {
        //TODO Test
        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
        {
            //Arrange
            var cells = Array.Empty<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToArray();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToArray();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToArray();

            var triggered = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggered.Add(args);

            //Act
            Instance.Add(cells);

            //Assert
            triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new(){NewValues = cells}
            });
        }
    }

    [TestClass]
    public class Add_Cells_Enumerable : Tester<Grid<Dummy>>
    {
        //TODO Test
        [TestMethod]
        public void WhenCellsIsNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> cells = null!;

            //Act
            var action = () => Instance.Add(cells);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
        }

        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
        {
            //Arrange
            var cells = new List<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var triggered = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggered.Add(args);

            //Act
            Instance.Add(cells);

            //Assert
            triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new(){NewValues = cells}
            });
        }
    }

    [TestClass]
    public class TryAdd_Cells_Params : Tester<Grid<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class TryAdd_Cells_Enumerable : Tester<Grid<Dummy>>
    {
        //TODO Test
        [TestMethod]
        public void WhenCellsIsNull_DoNotThrow()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> cells = null!;

            //Act
            var action = () => Instance.TryAdd(cells);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenCellsIsEmpty_DoNotModify()
        {
            //Arrange
            var cells = new List<Cell<Dummy>>();
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
            var alreadyThereCells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(alreadyThereCells);

            var newCells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var alreadyThereCells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(alreadyThereCells);

            var newCells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            Instance.TryAdd(cells);

            //Assert
            Instance.Should().Contain(cells);
        }

        [TestMethod]
        public void WhenAddingToNewCoordinates_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var triggered = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggered.Add(args);

            //Act
            Instance.TryAdd(cells);

            //Assert
            triggered.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new(){NewValues = cells}
            });
        }
    }

    [TestClass]
    public class RemoveAt_XY : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index.X, index.Y);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = Fixture.Create<string>();

            //Act
            Instance.RemoveAt(index.X, index.Y);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
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
    public class TryRemoveAt_XY : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_DoNotThrow()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = Fixture.Create<string>();

            //Act
            Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
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
    public class RemoveAt_Coordinates : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = Fixture.Create<string>();

            //Act
            Instance.RemoveAt(index);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
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
    public class TryRemoveAt_Coordinates : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenThereIsNoItemAtIndex_Throw()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_RemoveItem()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = Fixture.Create<string>();

            //Act
            Instance.TryRemoveAt(index);

            //Assert
            Instance[index].Should().BeNull();
        }

        [TestMethod]
        public void WhenThereIsItemAtIndex_TriggerChange()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<string>();
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
    public class RemoveAll_Item : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenItemIsNull_RemoveAllKeysThatPointToNullReferences()
        {
            //Arrange
            var keys = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Fixture.CreateMany<Cell<string>>().ToList();
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
            var keys = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Fixture.CreateMany<Cell<string>>().ToList();
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
            var keys = Fixture.CreateMany<Vector2<int>>().ToList();
            foreach (var key in keys)
                Instance[key] = null;

            var nonNullEntries = Fixture.CreateMany<Cell<string>>().ToList();
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
            var entries = Fixture.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var copy = Instance.Copy();

            var item = Fixture.Create<string>();

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInGrid_DoNotTriggerEvent()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var eventArgs = new List<GridChangedEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            var item = Fixture.Create<string>();

            //Act
            Instance.RemoveAll(item);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreOccurencesOfItemInGrid_RemoveAllThoseOccurences()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var item = Fixture.Create<string>();
            var keys = Fixture.CreateMany<Vector2<int>>();
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
            var entries = Fixture.CreateMany<Cell<string>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var item = Fixture.Create<string>();
            var keys = Fixture.CreateMany<Vector2<int>>().ToList();
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
    public class RemoveAll_Predicate : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<Dummy?, bool> match = null!;

            //Act
            var action = () => Instance.RemoveAll(match);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenNoItemsMatch_DoNotModifyGrid()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var value = Fixture.Create<string>();
            var dummies = Fixture.Build<Dummy>().With(x => x.Value, value).CreateMany();
            var items = dummies.Select(x => new Cell<Dummy>(Fixture.Create<Vector2<int>>(), x)).ToList();
            foreach (var item in items)
                Instance[item.Index] = item.Value;

            //Act
            Instance.RemoveAll(x => x.Value == value);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>(entries));
        }

        [TestMethod]
        public void WhenItemsMatch_TriggerEvent()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var value = Fixture.Create<string>();
            var dummies = Fixture.Build<Dummy>().With(x => x.Value, value).CreateMany();
            var items = dummies.Select(x => new Cell<Dummy>(Fixture.Create<Vector2<int>>(), x)).ToList();
            foreach (var item in items)
                Instance[item.Index] = item.Value;

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.RemoveAll(x => x.Value == value);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class Contains_Coordinates_Item : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsItemAtIndex_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<Dummy>();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<Dummy>();
            Instance[index] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Contains(index, item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index, null);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Contains_XY_Item : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsItemAtIndex_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<Dummy>();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
            var item = Fixture.Create<Dummy>();
            Instance[index] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Contains(index.X, index.Y, item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsSomewhereInGridButNotAtIndex_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();
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
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance[entry.Index] = entry.Value;

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index.X, index.Y, null);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtIndexAndItemIsNull_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Contains_Item : Tester<Grid<Dummy>>
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
            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Contains(item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsAtLeastOneNullValueSomewhereAndSeekingNull_ReturnTrue()
        {
            //Arrange
            var indexes = Fixture.CreateMany<Vector2<int>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            //Act
            var result = Instance.Contains(cells.GetRandom().Value);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_XY : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnFalse()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
        {
            //Arrange
            var cell = Fixture.Create<Cell<Dummy>>();
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
            var cell = Fixture.Create<Cell<Dummy>>();
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
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = null;

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_Coordinates : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnFalse()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereWasSomethingThereButItWasRemoved_ReturnFalse()
        {
            //Arrange
            var cell = Fixture.Create<Cell<Dummy>>();
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
            var cell = Fixture.Create<Cell<Dummy>>();
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
            var index = Fixture.Create<Vector2<int>>();
            Instance[index] = null;

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class FloodFill_XY_NewValue : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
    public class FloodFill_Coordinates_NewValue : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
    public class FloodFill_XY_NewValue_Boundaries : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
    public class FloodFill_Coordinates_NewValue_Boundaries : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
        {
            //Arrange
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            var boundaries = Fixture.Create<Boundaries<int>>();
            var index = new Vector2<int>(boundaries.Left - 1, boundaries.Bottom + 1);
            var newValue = Fixture.Create<string>();

            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
    public class FloodClear_XY : Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Fixture.Create<int>(), Fixture.Create<int>());

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
            Instance.FloodClear(Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
    public class FloodClear_Coordinates : Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Fixture.Create<Vector2<int>>());

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
            Instance.FloodClear(Fixture.Create<Vector2<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
    public class FloodClear_XY_Boundaries : Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Fixture.Create<int>(), Fixture.Create<int>(), Fixture.Create<Boundaries<int>>());

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
            Instance.FloodClear(Fixture.Create<int>(), Fixture.Create<int>(), Fixture.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
    public class FloodClear_Coordinates_Boundaries : Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange

            //Act
            Instance.FloodClear(Fixture.Create<Vector2<int>>(), Fixture.Create<Boundaries<int>>());

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
            Instance.FloodClear(Fixture.Create<Vector2<int>>(), Fixture.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToClearOutsideBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
    public class Resize : Tester<Grid<int>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.Resize(Fixture.Create<Boundaries<int>>());

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
            Instance.Resize(Fixture.Create<Boundaries<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTryingToResizeUsingSameSize_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
            var cells = Fixture.CreateMany<Cell<int>>();
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
    public class TranslateAll_XY : Tester<Grid<char>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.TranslateAll(Fixture.Create<int>(), Fixture.Create<int>());

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
            Instance.TranslateAll(Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotModifyGrid()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>();
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
            var cells = Fixture.CreateMany<Cell<char>>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = 0;
            var y = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
            var y = -Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
            var y = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = -Fixture.Create<int>();
            var y = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
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
    public class TranslateAll_Coordinates : Tester<Grid<char>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotModifyGrid()
        {
            //Arrange

            //Act
            Instance.TranslateAll(Fixture.Create<Vector2<int>>());

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
            Instance.TranslateAll(Fixture.Create<Vector2<int>>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenZeroIndex_DoNotModifyGrid()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>();
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
            var cells = Fixture.CreateMany<Cell<char>>();
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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), 0);

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheLeft_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), 0);

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, -Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingUp_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, -Fixture.Create<int>());

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), 0);

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingToTheRight_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), 0);

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToTranslateEverythingDown_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(0, Fixture.Create<int>());

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpLeft_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), -Fixture.Create<int>());

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), -Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveUpRight_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), -Fixture.Create<int>());

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownLeft_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(-Fixture.Create<int>(), Fixture.Create<int>());

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
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), Fixture.Create<int>());

            //Act
            Instance.TranslateAll(index);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<char>(cells.Select(x => new Cell<char>(x.Index + index, x.Value))));
        }

        [TestMethod]
        public void WhenTryingToMoveDownRight_TriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<char>>().ToList();
            foreach (var cell in cells)
                Instance[cell.Index] = cell.Value;

            var index = new Vector2<int>(Fixture.Create<int>(), Fixture.Create<int>());

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
    public class Translate_Range_XY : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var copy = Instance.Copy();

            var range = new Rectangle<int>(Fixture.Create<Vector2<int>>(), 0, 0);

            //Act
            Instance.Translate(range, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var range = new Rectangle<int>(Fixture.Create<Vector2<int>>(), 0, 0);

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(range, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange
            var range = Fixture.Create<Rectangle<int>>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var range = Fixture.Create<Rectangle<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(range, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
        {
            //Arrange
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 1, 0);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 1, 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Dummy>>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 3, 0);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), 3, 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Dummy>>
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
    public class Translate_Range_Coordinates : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var copy = Instance.Copy();

            var range = new Rectangle<int>(Fixture.Create<Vector2<int>>(), 0, 0);
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var range = new Rectangle<int>(Fixture.Create<Vector2<int>>(), 0, 0);
            var translation = Fixture.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            var range = Fixture.Create<Rectangle<int>>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var range = Fixture.Create<Rectangle<int>>();
            var translation = Fixture.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(1, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(1, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Dummy>>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(3, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(0, 0, 1, 3), new Vector2<int>(3, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Dummy>>
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
    public class Translate_Boundaries_XY : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var copy = Instance.Copy();

            var boundaries = new Boundaries<int>(0, 0, 0, 0);

            //Act
            Instance.Translate(boundaries, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var boundaries = new Boundaries<int>(0, 0, 0, 0);

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(boundaries, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotModify()
        {
            //Arrange
            var boundaries = Fixture.Create<Boundaries<int>>();

            //Act
            Instance.Translate(boundaries, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Fixture.Create<Boundaries<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(boundaries, Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenMovingRectangleOverAnotherAreaOfTheGrid_SquashExistingItemsWithMovedOnes()
        {
            //Arrange
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 1, 0);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 1, 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Dummy>>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 3, 0);

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, 3, 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Dummy>>
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
    public class Translate_Boundaries_Coordinates : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenRectangleHasZeroBoundaries_DoNotModify()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var copy = Instance.Copy();

            var boundaries = new Boundaries<int>(0, 0, 0, 0);
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenRectangleHasZeroSize_DoNotTriggerChange()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var boundaries = new Boundaries<int>(0, 0, 0, 0);
            var translation = Fixture.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Fixture.Create<Vector2<int>>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(1, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(1, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(0, 0, copy[0, 0]),
                        new(0, 1, copy[0, 1]),
                        new(0, 2, copy[0, 2]),
                        new(1, 0, copy[1, 0]),
                        new(1, 1, copy[1, 1]),
                        new(1, 2, copy[1, 2]),
                    },
                    NewValues = new List<Cell<Dummy>>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(3, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var copy = Instance.Copy();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Translate(new Boundaries<int> { Right = 1, Bottom = 3 }, new Vector2<int>(3, 0));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(3, 0, copy[0, 0]),
                        new(3, 1, copy[0, 1]),
                        new(3, 2, copy[0, 2]),
                    },
                    OldValues = new List<Cell<Dummy>>
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
    public class Copy : Tester<Grid<Dummy>>
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Copy_Boundaries : Tester<Grid<string>>
    {
        [TestMethod]
        public void WhenBoundariesAreEqualToGrid_ReturnExactCopyOfGrid()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<string>>().ToList();
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
            Instance[0, 0] = Fixture.Create<string>();
            Instance[1, 4] = Fixture.Create<string>();
            Instance[2, 3] = Fixture.Create<string>();
            Instance[3, 2] = Fixture.Create<string>();

            //Act
            var result = Instance.Copy(new Boundaries<int> { Top = -14, Left = -8, Right = -4, Bottom = -7 });

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreInsideGrid_ReturnThatPartOfGrid()
        {
            //Arrange
            Instance[0, 0] = Fixture.Create<string>();
            Instance[1, 4] = Fixture.Create<string>();
            Instance[2, 3] = Fixture.Create<string>();
            Instance[3, 2] = Fixture.Create<string>();

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
            var result = Instance.Copy(Fixture.Create<Boundaries<int>>());

            //Assert
            result.Should().BeEmpty();
        }
    }

    [TestClass]
    public class Swap : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenCurrentAndDestinationAreEqual_DoNotModifyGrid()
        {
            //Arrange
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var firstItem = Instance[2, 0];
            var secondItem = Instance[1, 2];

            //Act
            Instance.Swap(new Vector2<int>(2, 0), new Vector2<int>(1, 2));

            //Assert
            Instance.Should().BeEquivalentTo(new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();
            Instance[0, 1] = Fixture.Create<Dummy>();
            Instance[1, 1] = Fixture.Create<Dummy>();
            Instance[2, 1] = Fixture.Create<Dummy>();
            Instance[0, 2] = Fixture.Create<Dummy>();
            Instance[1, 2] = Fixture.Create<Dummy>();
            Instance[2, 2] = Fixture.Create<Dummy>();

            var firstItem = Instance[2, 0];
            var secondItem = Instance[1, 2];

            var eventArgs = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Swap(new Vector2<int>(2, 0), new Vector2<int>(1, 2));

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>> { new(2,0, secondItem), new(1, 2, firstItem) },
                    OldValues = new List<Cell<Dummy>> { new(2,0, firstItem), new(1, 2, secondItem) },
                }
            });
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<Grid<string>>
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
            var cells = Fixture.CreateMany<Cell<string>>(5).ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Grid<String> with 5 items");
        }
    }

    [TestClass]
    public class Equals_Object : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsSimilarGrid_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilar2dArray_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.Create<Dummy[,]>();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilarJaggedArray_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.Create<Dummy[][]>();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilarCells_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSimilarKeyValuePairs_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.Create<Dictionary<Vector2<int>, Dummy>>();

            //Act
            var result = Instance.Equals((object)other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentUnsupportedType_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Fixture.Create<int>();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Equals_Grid : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenOtherGridIsNull_ReturnFalse()
        {
            //Arrange
            Grid<Dummy> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToGrid();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToGrid();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();

            var other = new Grid<Dummy>
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
            Instance[0, 0] = Fixture.Create<Dummy>();
            Instance[1, 0] = Fixture.Create<Dummy>();
            Instance[2, 0] = Fixture.Create<Dummy>();

            var other = new List<Cell<Dummy>>
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
    public class EqualsOperator_Grid : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == (Grid<Dummy>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToGrid();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToGrid();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class NotEqualsOperator_Grid : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != (Grid<Dummy>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridIsDifferent_ReturnTrue()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToGrid();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToGrid();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToGrid();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherGridAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Equals_Cells : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenCellsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();

            //Act
            var result = Instance.Equals(differentCells);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToList();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class EqualsOperator_Cells : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == (IEnumerable<Cell<Dummy>>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();

            //Act
            var result = Instance == differentCells;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToList();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class NotEqualsOperator_Cells : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != (IEnumerable<Cell<Dummy>>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsIsDifferent_ReturnTrue()
        {
            //Arrange
            var other = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();

            //Act
            var result = Instance != differentCells;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.Select(x => new Cell<Dummy>(x.Index, Fixture.Create<Dummy>())).ToList();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenCellsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Equals_KeyValuePairs : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Dummy>> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToDictionary();

            //Act
            var result = Instance.Equals(other!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToDictionary();
            foreach (var ((x, y), _) in Instance)
                other[new Vector2<int>(x, y)] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Equals(other!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class EqualsOperator_KeyValuePairs : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == (IEnumerable<KeyValuePair<Vector2<int>, Dummy>>)null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnFalse()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Dummy>> other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToDictionary();

            //Act
            var result = Instance == other!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToDictionary();
            foreach (var ((x, y), _) in Instance)
                other[new Vector2<int>(x, y)] = Fixture.Create<Dummy>();

            //Act
            var result = Instance == other!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class NotEqualsOperator_KeyValuePairs : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != (IEnumerable<KeyValuePair<Vector2<int>, Dummy>>)null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsNull_ReturnTrue()
        {
            //Arrange
            IEnumerable<KeyValuePair<Vector2<int>, Dummy>> other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsIsDifferent_ReturnTrue()
        {
            //Arrange
            var other = Fixture.CreateMany<KeyValuePair<Vector2<int>, Dummy>>().ToList();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
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
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToDictionary();

            //Act
            var result = Instance != other!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToDictionary();
            foreach (var ((x, y), _) in Instance)
                other[new Vector2<int>(x, y)] = Fixture.Create<Dummy>();

            //Act
            var result = Instance != other!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeyValuePairsAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
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
    public class Equals_2dArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Dummy[,] other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.Create<Dummy[,]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).To2dArray();

            //Act
            var result = Instance.Equals(other!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();
            foreach (var ((x, y), _) in Instance)
                other[x, y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Equals(other!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();

            //Act
            var result = Instance.Equals(other!);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class EqualsOperator_2dArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == (Dummy[,])null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == Fixture.Create<Dummy[,]>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Dummy[,] other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.Create<Dummy[,]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).To2dArray();

            //Act
            var result = Instance == other!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();
            foreach (var ((x, y), _) in Instance)
                other[x, y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance == other!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();

            //Act
            var result = Instance == other!;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class NotEqualsOperator_2dArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != (Dummy[,])null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != Fixture.Create<Dummy[,]>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnTrue()
        {
            //Arrange
            Dummy[,] other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnTrue()
        {
            //Arrange
            var other = Fixture.Create<Dummy[,]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).To2dArray();

            //Act
            var result = Instance != other!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();
            foreach (var ((x, y), _) in Instance)
                other[x, y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance != other!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.To2dArray();

            //Act
            var result = Instance != other!;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Equals_JaggedArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Dummy[][] other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.Create<Dummy[][]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToJaggedArray();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();
            foreach (var ((x, y), _) in Instance)
                other[x][y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class EqualsOperator_JaggedArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == (Dummy[][])null!;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! == Fixture.Create<Dummy[][]>();

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnFalse()
        {
            //Arrange
            Dummy[][] other = null!;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnFalse()
        {
            //Arrange
            var other = Fixture.Create<Dummy[][]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToJaggedArray();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();
            foreach (var ((x, y), _) in Instance)
                other[x][y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class NotEqualsOperator_JaggedArray : Tester<Grid<Dummy>>
    {
        [TestMethod]
        public void WhenBothAreNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != (Dummy[][])null!;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenGridIsNull_ReturnTrue()
        {
            //Arrange

            //Act
            var result = (Grid<Dummy>)null! != Fixture.Create<Dummy[][]>();

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsNull_ReturnTrue()
        {
            //Arrange
            Dummy[][] other = null!;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayIsDifferent_ReturnTrue()
        {
            //Arrange
            var other = Fixture.Create<Dummy[][]>();

            var cells = Fixture.CreateMany<Cell<Dummy>>();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtDifferentIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var differentCells = cells.Select(x => Fixture.Build<Cell<Dummy>>().With(y => y.Value, x.Value).Create()).ToList();
            var other = new Grid<Dummy>(differentCells).ToJaggedArray();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveDifferentItemsAtSameIndexes_ReturnTrue()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();
            foreach (var ((x, y), _) in Instance)
                other[x][y] = Fixture.Create<Dummy>();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenArrayAndGridHaveSameItemsAtSameIndexes_ReturnFalse()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();
            foreach (var (index, value) in cells)
                Instance[index] = value;

            var other = Instance.ToJaggedArray();

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Serialization : Tester<Grid<Dummy>>
    {
        //TODO Test
        [TestMethod]
        public void WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var json = JsonConvert.SerializeObject(Instance);

            //Act
            var result = JsonConvert.DeserializeObject<Grid<Dummy>>(json);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var options = new JsonSerializerOptions().WithGridConverters();

            var json = System.Text.Json.JsonSerializer.Serialize(Instance, options);

            //Act
            var result = System.Text.Json.JsonSerializer.Deserialize<Grid<Dummy>>(json, options);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }
    }

}