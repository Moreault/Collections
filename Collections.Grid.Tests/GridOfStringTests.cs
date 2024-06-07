namespace Collections.Grid.Tests;

[TestClass]
public sealed class GridOfStringTests : Tester<Grid<string>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void FloodFillXYNewValue_WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
    {
        //Arrange
        var newValue = Dummy.Create<string>();

        var cells = Dummy.CreateMany<Cell<string>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;

        var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);

        var copy = Instance.Copy();

        //Act
        Instance.FloodFill(index.X, index.Y, newValue);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void FloodFillXYNewValue_WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
    {
        //Arrange
        var newValue = Dummy.Create<string>();

        var cells = Dummy.CreateMany<Cell<string>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;
        var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);

        var eventArgs = new List<GridChangedEventArgs<string>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.FloodFill(index.X, index.Y, newValue);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodFillXYNewValue_WhenTryingToFillUsingSameValue_DoNotModifyGrid()
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
    public void FloodFillXYNewValue_WhenTryingToFillUsingSameValue_DoNotTriggerChange()
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
    public void FloodFillXYNewValue_WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
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
    public void FloodFillXYNewValue_WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
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
                    NewValues = [new(0,1, "C")],
                    OldValues = [new(0,1,"B")]
                }
            });
    }

    [TestMethod]
    public void FloodFillXYNewValue_WhenTryingToFillAnEmptyGrid_DoNothing()
    {
        //Arrange

        //Act
        Instance.FloodFill(1, 2, "F");

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodFillXYNewValue_WhenTryingToFillAnEmptyGrid_DoNotTriggerChangeEvent()
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
    public void FloodFillXYNewValue_WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
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
    public void FloodFillXYNewValue_WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
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
                    NewValues =
                    [
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
                    ],
                    OldValues =
                    [
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
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodFillCoordinatesNewValue_WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
    {
        //Arrange
        var newValue = Dummy.Create<string>();

        var cells = Dummy.CreateMany<Cell<string>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;
        var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);

        var copy = Instance.Copy();

        //Act
        Instance.FloodFill(index, newValue);

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void FloodFillCoordinatesNewValue_WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
    {
        //Arrange
        var newValue = Dummy.Create<string>();

        var cells = Dummy.CreateMany<Cell<string>>().ToList();
        foreach (var cell in cells)
            Instance[cell.Index] = cell.Value;
        var index = new Vector2<int>(Instance.Boundaries.Left - 1, Instance.Boundaries.Bottom + 1);

        var eventArgs = new List<GridChangedEventArgs<string>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.FloodFill(index, newValue);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodFillCoordinatesNewValue_WhenTryingToFillUsingSameValue_DoNotModifyGrid()
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
    public void FloodFillCoordinatesNewValue_WhenTryingToFillUsingSameValue_DoNotTriggerChange()
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
    public void FloodFillCoordinatesNewValue_WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
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
    public void FloodFillCoordinatesNewValue_WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
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
                    NewValues = [new(0,1, "C")],
                    OldValues = [new(0,1,"B")]
                }
            });
    }

    [TestMethod]
    public void FloodFillCoordinatesNewValue_WhenTryingToFillAnEmptyGrid_DoNothing()
    {
        //Arrange

        //Act
        Instance.FloodFill(new Vector2<int>(1, 2), "F");

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodFillCoordinatesNewValue_WhenTryingToFillAnEmptyGrid_DoNotTriggerChangeEvent()
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
    public void FloodFillCoordinatesNewValue_WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
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
    public void FloodFillCoordinatesNewValue_WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
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
                    NewValues =
                    [
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
                    ],
                    OldValues =
                    [
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
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillUsingSameValue_DoNotModifyGrid()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillUsingSameValue_DoNotTriggerChange()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
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
                    NewValues = [new(0,1, "C")],
                    OldValues = [new(0,1,"B")]
                }
            });
    }

    [TestMethod]
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillAnEmptyGridWithLargeBoundaries_FillEntireBoundaries()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillAnEmptyGridWithLargeBoundaries_TriggerChangeEventOnce()
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
                    NewValues =
                    [
                        new(0,0, "F"),
                        new(1,0, "F"),
                        new(2,0, "F"),
                        new(0,1, "F"),
                        new(1,1, "F"),
                        new(2,1, "F"),
                        new(0,2, "F"),
                        new(1,2, "F"),
                        new(2,2, "F"),
                    ],
                }
            });
    }

    [TestMethod]
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
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
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
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
                    NewValues =
                    [
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
                    ],
                    OldValues =
                    [
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
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodFillXYNewValueBoundaries_WhenTryingToFillAnAreaWithSimilarNeighbors_FillOnlyWithinBoundaries()
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

    [TestMethod]
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillOutsideBoundaries_DoNotModifyGrid()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillUsingSameValue_DoNotModifyGrid()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillUsingSameValue_DoNotTriggerChange()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFloodValueSurroundedByDifferentNeighbors_OnlyChangeThatOneOccurence()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFloodValueSurroundedByDifferentNeighbors_TriggerEvent()
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
                    NewValues = [new(0,1, "C")],
                    OldValues = [new(0,1,"B")]
                }
            });
    }

    [TestMethod]
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillAnEmptyGridWithLargeBoundaries_FillEntireBoundaries()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillAnEmptyGridWithLargeBoundaries_TriggerChangeEventOnce()
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
                    NewValues =
                    [
                        new(0,0, "F"),
                        new(1,0, "F"),
                        new(2,0, "F"),
                        new(0,1, "F"),
                        new(1,1, "F"),
                        new(2,1, "F"),
                        new(0,2, "F"),
                        new(1,2, "F"),
                        new(2,2, "F"),
                    ],
                }
            });
    }

    [TestMethod]
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillAreaWithSimilarNeighbors_ChangeAllNeighborsToNewValue()
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
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillAreaWithSimilarNeighbors_TriggerChangeEventOnce()
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
                    NewValues =
                    [
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
                    ],
                    OldValues =
                    [
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
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodFillWithCoordinatesAndNewValueAndBoundaries_WhenTryingToFillAnAreaWithSimilarNeighbors_FillOnlyWithinBoundaries()
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

    [TestMethod]
    public void CopyWithBoundaries_WhenBoundariesAreEqualToGrid_ReturnExactCopyOfGrid()
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
    public void CopyWithBoundaries_WhenBoundariesAreOutsideGrid_ReturnEmpty()
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
    public void CopyWithBoundaries_WhenBoundariesAreInsideGrid_ReturnThatPartOfGrid()
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
    public void CopyWithBoundaries_WhenGridIsEmpty_ReturnEmptyGrid()
    {
        //Arrange

        //Act
        var result = Instance.Copy(Dummy.Create<Boundaries<int>>());

        //Assert
        result.Should().BeEmpty();
    }
}