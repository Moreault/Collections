using ToolBX.Dummies;

namespace Collections.Grid.Tests;

[TestClass]
public sealed class GridOfCharTests : Tester<Grid<char>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenGridIsEmpty_DoNotModifyGrid()
    {
        //Arrange

        //Act
        Instance.TranslateAll(Dummy.Create<int>(), Dummy.Create<int>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateAllWithXY_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void TranslateAllWithXY_WhenZeroIndex_DoNotModifyGrid()
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
    public void TranslateAllWithXY_WhenZeroIndex_DoNotTriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingToTheLeft_Move()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingToTheLeft_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingUp_Move()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingUp_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingToTheRight_Move()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingToTheRight_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingDown_Move()
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
    public void TranslateAllWithXY_WhenTryingToTranslateEverythingDown_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToMoveUpLeft_Move()
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
    public void TranslateAllWithXY_WhenTryingToMoveUpLeft_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToMoveUpRight_Move()
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
    public void TranslateAllWithXY_WhenTryingToMoveUpRight_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToMoveDownLeft_Move()
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
    public void TranslateAllWithXY_WhenTryingToMoveDownLeft_TriggerChange()
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
    public void TranslateAllWithXY_WhenTryingToMoveDownRight_Move()
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
    public void TranslateAllWithXY_WhenTryingToMoveDownRight_TriggerChange()
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

    [TestMethod]
    public void TranslateAllWithCoordinates_WhenGridIsEmpty_DoNotModifyGrid()
    {
        //Arrange

        //Act
        Instance.TranslateAll(Dummy.Create<Vector2<int>>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TranslateAllWithCoordinates_WhenGridIsEmpty_DoNotTriggerChange()
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
    public void TranslateAllWithCoordinates_WhenZeroIndex_DoNotModifyGrid()
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
    public void TranslateAllWithCoordinates_WhenZeroIndex_DoNotTriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingToTheLeft_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingToTheLeft_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingUp_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingUp_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingToTheRight_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingToTheRight_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingDown_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToTranslateEverythingDown_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveUpLeft_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveUpLeft_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveUpRight_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveUpRight_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveDownLeft_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveDownLeft_TriggerChange()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveDownRight_Move()
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
    public void TranslateAllWithCoordinates_WhenTryingToMoveDownRight_TriggerChange()
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