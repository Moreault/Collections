using ToolBX.Dummies;

namespace Collections.Grid.Tests;

[TestClass]
public sealed class GridOfIntTests : Tester<Grid<int>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void FloodClearXY_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange

        //Act
        Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodClearXY_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void FloodClearXY_WhenTryingToClearOutsideBoundaries_DoNotModify()
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
    public void FloodClearXY_WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodClearXY_WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
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
    public void FloodClearXY_WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(1, 1, 2)
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearXY_WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
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
    public void FloodClearXY_WhenItemIsSurroundedBySameNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithCoordinates_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange

        //Act
        Instance.FloodClear(Dummy.Create<Vector2<int>>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodClearWithCoordinates_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void FloodClearWithCoordinates_WhenTryingToClearOutsideBoundaries_DoNotModify()
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
    public void FloodClearWithCoordinates_WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodClearWithCoordinates_WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
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
    public void FloodClearWithCoordinates_WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(1, 1, 2)
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithCoordinates_WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
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
    public void FloodClearWithCoordinates_WhenItemIsSurroundedBySameNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithXYAndBoundaries_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange

        //Act
        Instance.FloodClear(Dummy.Create<int>(), Dummy.Create<int>(), Dummy.Create<Boundaries<int>>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodClearWithXYAndBoundaries_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void FloodClearWithXYAndBoundaries_WhenTryingToClearOutsideBoundaries_DoNotModify()
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
    public void FloodClearWithXYAndBoundaries_WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodClearWithXYAndBoundaries_WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
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
    public void FloodClearWithXYAndBoundaries_WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(1, 1, 2)
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithXYAndBoundaries_WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
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
    public void FloodClearWithXYAndBoundaries_WhenItemIsSurroundedBySameNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithXYAndBoundaries_WhenItemIsSurroundedBySameNeighbors_ConstrainDeletionToBoundaries()
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

    [TestMethod]
    public void FloodClearWithCoordinatesAndBoundaries_WhenGridIsEmpty_DoNotModify()
    {
        //Arrange

        //Act
        Instance.FloodClear(Dummy.Create<Vector2<int>>(), Dummy.Create<Boundaries<int>>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void FloodClearWithCoordinatesAndBoundaries_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void FloodClearWithCoordinatesAndBoundaries_WhenTryingToClearOutsideBoundaries_DoNotModify()
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
    public void FloodClearWithCoordinatesAndBoundaries_WhenTryingToClearOutsideBoundaries_DoNotTriggerChange()
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
    public void FloodClearWithCoordinatesAndBoundaries_WhenOneItemIsSurroundedByDifferentNeighbors_OnlyClearThatItem()
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
    public void FloodClearWithCoordinatesAndBoundaries_WhenOneItemIsSurroundedByDifferentNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(1, 1, 2)
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithCoordinatesAndBoundaries_WhenItemIsSurroundedBySameNeighbors_ClearAllTheOnesItTouches()
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
    public void FloodClearWithCoordinatesAndBoundaries_WhenItemIsSurroundedBySameNeighbors_TriggerChange()
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
                    OldValues =
                    [
                        new(0, 0, 1),
                        new(1, 0, 1),
                        new(2, 0, 1),
                        new(0, 1, 1),
                        new(2, 1, 1),
                        new(0, 2, 1),
                        new(1, 2, 1),
                        new(2, 2, 1),
                    ]
                }
            });
    }

    [TestMethod]
    public void FloodClearWithCoordinatesAndBoundaries_WhenItemIsSurroundedBySameNeighbors_ConstrainDeletionToBoundaries()
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

    [TestMethod]
    public void Resize_WhenGridIsEmpty_DoNotModifyGrid()
    {
        //Arrange

        //Act
        Instance.Resize(Dummy.Create<Boundaries<int>>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Resize_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void Resize_WhenTryingToResizeUsingSameSize_DoNotModify()
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
    public void Resize_WhenTryingToResizeUsingSameSize_DoNotTriggerChange()
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
    public void Resize_WhenTryingToResizeUsingLargerBoundaries_DoNotModify()
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
    public void Resize_WhenTryingToResizeUsingLargerBoundaries_DoNotTriggerChange()
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
    public void Resize_WhenResizingWithSmallerBoundaries_RemoveExcessItems()
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
    public void Resize_WhenResizingWithSmallerBoundaries_TriggerChange()
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
                    OldValues =
                    [
                        new(-1,0,1),
                        new(-1,1,4),
                        new(-1,2,7),
                        new(0,2,8),
                        new(1,2,9),
                    ]
                }
            });
    }

    [TestMethod]
    public void Resize_WhenResizingWithZeroBoundaries_OnlyContainsItemAtOrigin()
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