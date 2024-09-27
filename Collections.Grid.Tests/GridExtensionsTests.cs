using Collections.Grid.Tests.Customizations;
using ToolBX.Dummies;

namespace Collections.Grid.Tests;

[TestClass]
public class GridExtensionsTests : Tester
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void ToGridCells_WhenCellsAreNull_Throw()
    {
        //Arrange
        IEnumerable<Cell<int>> cells = null!;

        //Act
        var action = () => cells.ToGrid();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToGridCells_WhenCellsAreEmpty_ReturnEmptyGrid()
    {
        //Arrange
        var cells = Array.Empty<Cell<int>>();

        //Act
        var result = cells.ToGrid();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToGridCells_WhenCellsAreNotEmpty_ReturnGrid()
    {
        //Arrange
        var cells = Dummy.CreateMany<Cell<int>>().ToList();

        //Act
        var result = cells.ToGrid();

        //Assert
        result.Should().BeEquivalentTo(cells);
    }

    [TestMethod]
    public void ToGridKeyValuePairs_WhenCellsAreNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<Vector2<int>, string>> keyValuePairs = null!;

        //Act
        var action = () => keyValuePairs.ToGrid();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToGridKeyValuePairs_WhenCellsAreEmpty_ReturnEmptyGrid()
    {
        //Arrange
        var dictionary = new Dictionary<Vector2<int>, string>();

        //Act
        var result = dictionary.ToGrid();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToGridKeyValuePairs_WhenCellsAreNotEmpty_ReturnGrid()
    {
        //Arrange
        var keyValuePairs = Dummy.CreateMany<KeyValuePair<Vector2<int>, string>>().ToList();

        //Act
        var result = keyValuePairs.ToGrid();

        //Assert
        result.Should().BeEquivalentTo(keyValuePairs.Select(x => new Cell<string>(x.Key, x.Value)));
    }

    [TestMethod]
    public void ToGrid2dArray_WhenArrayIsNull_Throw()
    {
        //Arrange
        char[,] collection = null!;

        //Act
        var action = () => collection.ToGrid();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToGrid2dArray_WhenArrayIsEmpty_ReturnEmptyGrid()
    {
        //Arrange
        var collection = new char[0, 0];

        //Act
        var result = collection.ToGrid();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToGrid2dArray_WhenArrayIsNotEmpty_ReturnGrid()
    {
        //Arrange
        var collection = Dummy.Create<char[,]>();

        //Act
        var result = collection.ToGrid();

        //Assert
        var expected = new Grid<char>();
        for (var x = 0; x < collection.GetLength(0); x++)
        {
            for (var y = 0; y < collection.GetLength(1); y++)
            {
                expected[x, y] = collection[x, y];
            }

        }
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ToGridJaggedArray_WhenArrayIsNull_Throw()
    {
        //Arrange
        float[][] collection = null!;

        //Act
        var action = () => collection.ToGrid();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToGridJaggedArray_WhenArrayIsEmpty_ReturnEmptyGrid()
    {
        //Arrange
        var collection = Array.Empty<float[]>();

        //Act
        var result = collection.ToGrid();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToGridJaggedArray_WhenArrayIsNotEmpty_ReturnGrid()
    {
        //Arrange
        var collection = Dummy.Create<float[][]>();

        //Act
        var result = collection.ToGrid();

        //Assert
        var expected = new Grid<float>();
        for (var x = 0; x < collection.Length; x++)
        {
            for (var y = 0; y < collection[x].Length; y++)
            {
                expected[x, y] = collection[x][y];
            }

        }
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void ToGridColumnCount_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<int> collection = null!;
        var columnCount = Dummy.Create<int>();

        //Act
        var action = () => collection.ToGrid(columnCount);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToGridColumnCount_WhenColumnCountIsZero_Throw()
    {
        //Arrange
        var collection = Dummy.CreateMany<int>().ToList();
        var columnCount = 0;

        //Act
        var action = () => collection.ToGrid(columnCount);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotConvertToGridFromColumnCount, columnCount));
    }

    [TestMethod]
    public void ToGridColumnCount_WhenColumnCountIsNegative_Throw()
    {
        //Arrange
        var collection = Dummy.CreateMany<int>().ToList();
        var columnCount = -Dummy.Create<int>();

        //Act
        var action = () => collection.ToGrid(columnCount);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotConvertToGridFromColumnCount, columnCount));
    }

    [TestMethod]
    public void ToGridColumnCount_Always_ConvertToGrid()
    {
        //Arrange
        var collection = Dummy.CreateMany<int>(9).ToList();
        var maxX = 2;

        var expected = new Grid<int>
        {
            [0, 0] = collection[0],
            [1, 0] = collection[1],
            [2, 0] = collection[2],
            [0, 1] = collection[3],
            [1, 1] = collection[4],
            [2, 1] = collection[5],
            [0, 2] = collection[6],
            [1, 2] = collection[7],
            [2, 2] = collection[8]
        };

        //Act
        var result = collection.ToGrid(maxX);

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void To2dArray_WhenGridIsNull_Throw()
    {
        //Arrange
        IGrid<int> grid = null!;

        //Act
        var action = () => grid.To2dArray();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void To2dArray_WhenGridIsEmpty_ReturnEmptyArray()
    {
        //Arrange
        var grid = new Grid<int>();

        //Act
        var result = grid.To2dArray();

        //Assert
        result.Should().BeEquivalentTo(new int[0, 0]);
    }

    [TestMethod]
    public void To2dArray_WhenGridIsNotEmpty_ReturnArray()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var grid = Dummy.CreateMany<Cell<int>>().ToGrid();

        //Act
        var result = grid.To2dArray();

        //Assert
        foreach (var ((x, y), value) in grid)
        {
            result[x, y].Should().Be(value);
        }
    }

    [TestMethod]
    public void ToJaggedArray_WhenGridIsNull_Throw()
    {
        //Arrange
        IGrid<int> grid = null!;

        //Act
        var action = () => grid.ToJaggedArray();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToJaggedArray_WhenGridIsEmpty_ReturnEmptyArray()
    {
        //Arrange
        var grid = new Grid<int>();

        //Act
        var result = grid.ToJaggedArray();

        //Assert
        result.Should().BeEquivalentTo(Array.Empty<int[]>());
    }

    [TestMethod]
    public void ToJaggedArray_WhenGridIsNotEmpty_ReturnArray()
    {
        //Arrange
        Dummy.Customize(new PositiveIndexCellCustomization());
        var grid = Dummy.CreateMany<Cell<int>>().ToGrid();

        //Act
        var result = grid.ToJaggedArray();

        //Assert
        foreach (var ((x, y), value) in grid)
        {
            result[x][y].Should().Be(value);
        }
    }

    [TestMethod]
    public void ToDictionary_WhenGridIsNull_Throw()
    {
        //Arrange
        IGrid<Garbage> grid = null!;

        //Act
        var action = () => grid.ToDictionary();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToDictionary_WhenGridIsEmpty_ReturnEmptyDictionary()
    {
        //Arrange
        var grid = new Grid<Garbage>();

        //Act
        var result = grid.ToDictionary();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToDictionary_WhenGridIsNotEmpty_ReturnDictionary()
    {
        //Arrange
        var grid = Dummy.CreateMany<Cell<int>>().ToGrid();

        //Act
        var result = grid.ToDictionary();

        //Assert
        result.Should().BeEquivalentTo(grid.ToDictionary(x => x.Index, x => x.Value));
    }

    [TestMethod]
    public void SequenceEqual_WhenFirstIsNull_Throw()
    {
        //Arrange
        IGrid<Garbage> first = null!;
        var second = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        //Act
        var action = () => first.SequenceEqual(second);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(first));
    }

    [TestMethod]
    public void SequenceEqual_WhenSecondIsNull_Throw()
    {
        //Arrange
        var first = Dummy.CreateMany<Cell<Garbage>>().ToGrid();
        IGrid<Garbage> second = null!;

        //Act
        var action = () => first.SequenceEqual(second);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(second));
    }

    [TestMethod]
    public void SequenceEqual_WhenFirstIsEmptyAndSecondIsNot_ReturnFalse()
    {
        //Arrange
        var first = new Grid<Garbage>();
        var second = Dummy.CreateMany<Cell<Garbage>>().ToGrid();

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void SequenceEqual_WhenSecondIsEmptyAndFirstIsNot_ReturnFalse()
    {
        //Arrange
        var first = Dummy.CreateMany<Cell<Garbage>>().ToGrid();
        var second = new Grid<Garbage>();

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void SequenceEqual_WhenFirstAndSecondDoNotHaveTheSameNumberOfItems_ReturnFalse()
    {
        //Arrange
        var first = Dummy.CreateMany<Cell<Garbage>>().ToGrid();
        var second = first.Concat(new List<Cell<Garbage>> { Dummy.Create<Cell<Garbage>>() }).ToGrid();

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeFalse();
    }


    [TestMethod]
    public void SequenceEqual_WhenBothGridsAreSameReference_ReturnTrue()
    {
        //Arrange
        var first = Dummy.CreateMany<Cell<Garbage>>().ToGrid();
        var second = first;

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void SequenceEqual_WhenBothGridsAreEqual_ReturnTrue()
    {
        //Arrange
        var first = Dummy.CreateMany<Cell<Garbage>>().ToGrid();
        var second = first.ToGrid();

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void SequenceEqual_WhenBothGridsAreEqualButSecondIsScrambled_ReturnTrue()
    {
        //Arrange
        var first = new Grid<Garbage>
            {
                { 0, 0, Dummy.Create<Garbage>() },
                { 1, 0, Dummy.Create<Garbage>() },
                { 2, 0, Dummy.Create<Garbage>() },
            };
        var second = new Grid<Garbage>
            {
                { 1, 0, first[1, 0] },
                { 0, 0, first[0, 0] },
                { 2, 0, first[2, 0] },
            };

        //Act
        var result = first.SequenceEqual(second);

        //Assert
        result.Should().BeTrue();
    }
}