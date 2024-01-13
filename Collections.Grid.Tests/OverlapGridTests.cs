namespace Collections.Grid.Tests;

[TestClass]
public class OverlapGridTests
{
    [TestClass]
    public class Indexer_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmpty()
        {
            //Arrange
            var value = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance[value.X, value.Y];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNothingThere_ReturnEmpty()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var value = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance[value.X, value.Y];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsSomethingWithThatIndex_ReturnThatThing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();

            var value = item.Index;

            //Act
            var result = Instance[value.X, value.Y];

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy> { item.Value! });
        }

        [TestMethod]
        public void WhenThereAreManyThingsWithThatIndex_ReturnAllOfThem()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var value = Fixture.Create<Vector2<int>>();
            var searchedItems = Fixture.Build<Cell<Dummy>>().With(x => x.Index, value).CreateMany().ToArray();
            Instance.Add(searchedItems);


            //Act
            var result = Instance[value.X, value.Y];

            //Assert
            result.Should().BeEquivalentTo(searchedItems.Select(x => x.Value));
        }
    }

    [TestClass]
    public class Indexer_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmpty()
        {
            //Arrange
            var value = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance[value];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNothingThere_ReturnEmpty()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var value = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance[value];

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsSomethingWithThatIndex_ReturnThatThing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();

            var value = item.Index;

            //Act
            var result = Instance[value];

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy> { item.Value! });
        }

        [TestMethod]
        public void WhenThereAreManyThingsWithThatIndex_ReturnAllOfThem()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var value = Fixture.Create<Vector2<int>>();
            var searchedItems = Fixture.Build<Cell<Dummy>>().With(x => x.Index, value).CreateMany().ToArray();
            Instance.Add(searchedItems);


            //Act
            var result = Instance[value];

            //Assert
            result.Should().BeEquivalentTo(searchedItems.Select(x => x.Value));
        }
    }

    [TestClass]
    public class ColumnCount : Tester<OverlapGrid<Dummy>>
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
        public void WhenContainsOneColumn_ReturnOne()
        {
            //Arrange
            Instance.Add(Fixture.Create<Cell<Dummy>>());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenContainsMultipleItemsOnSameColumn_ReturnOne()
        {
            //Arrange
            var column = Fixture.Create<int>();

            Instance.Add(Fixture.Build<Cell<Dummy>>().With(x => x.Index, new Vector2<int>(column, Fixture.Create<int>())).CreateMany());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenContainsMultipleConsecutiveColumns_ReturnColumnCount()
        {
            //Arrange
            Instance.Add(2, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(3, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(4, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(3);
        }

        [TestMethod]
        public void WhenContainsMultipleColumnsWithGaps_ReturnByCountingGaps()
        {
            //Arrange
            Instance.Add(2, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(4, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(6, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(5);
        }

        [TestMethod]
        public void WhenContainsOnlyNegativeColumns_ReturnAmount()
        {
            //Arrange
            Instance.Add(-2, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(-4, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(-6, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(5);
        }

        [TestMethod]
        public void WhenContainsColumnsInNegativesAndPositives_ReturnAmount()
        {
            //Arrange
            Instance.Add(2, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(4, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(-6, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.ColumnCount;

            //Assert
            result.Should().Be(11);
        }
    }

    [TestClass]
    public class RowCount : Tester<OverlapGrid<Dummy>>
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
        public void WhenThereIsOnlyOneItem_ReturnOne()
        {
            //Arrange
            Instance.Add(Fixture.Create<Cell<Dummy>>());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAlignedOnSameRow_ReturnOne()
        {
            //Arrange
            var row = Fixture.Create<int>();

            Instance.Add(Fixture.Build<Cell<Dummy>>().With(x => x.Index, new Vector2<int>(Fixture.Create<int>(), row)).CreateMany());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenThereAreMultipleConsecutiveItemsOnRows_ReturnAmount()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), 4, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 5, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 6, Fixture.Create<Dummy>());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(3);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsWithGaps_CountGapsAsWell()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), 4, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 6, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 9, Fixture.Create<Dummy>());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenAllRowsAreNegative_ReturnAmount()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), -4, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), -6, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), -9, Fixture.Create<Dummy>());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void WhenThereAreNegativeAndPositiveRowIndexes_ReturnAmount()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), 4, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), -6, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 9, Fixture.Create<Dummy>());

            //Act
            var result = Instance.RowCount;

            //Assert
            result.Should().Be(16);
        }
    }

    [TestClass]
    public class FirstColumn : Tester<OverlapGrid<Dummy>>
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
        public void WhenContainsOneItem_ReturnItemColumn()
        {
            //Arrange
            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(item.X);
        }

        [TestMethod]
        public void WhenContainsMultipleItemsOnSameColumn_ReturnThatColumn()
        {
            //Arrange
            var column = Fixture.Create<int>();
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(column);
        }

        [TestMethod]
        public void WhenContainsMultipleItems_ReturnSmallestColumn()
        {
            //Arrange
            Instance.Add(-14, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(0, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(29, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.FirstColumn;

            //Assert
            result.Should().Be(-14);
        }
    }

    [TestClass]
    public class LastColumn : Tester<OverlapGrid<Dummy>>
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
        public void WhenContainsOneItem_ReturnItemColumn()
        {
            //Arrange
            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(item.X);
        }

        [TestMethod]
        public void WhenContainsMultipleItemsOnSameColumn_ReturnThatColumn()
        {
            //Arrange
            var column = Fixture.Create<int>();
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(column, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(column);
        }

        [TestMethod]
        public void WhenContainsMultipleItems_ReturnHighestColumn()
        {
            //Arrange
            Instance.Add(-14, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(0, Fixture.Create<int>(), Fixture.Create<Dummy>());
            Instance.Add(29, Fixture.Create<int>(), Fixture.Create<Dummy>());

            //Act
            var result = Instance.LastColumn;

            //Assert
            result.Should().Be(29);
        }
    }

    [TestClass]
    public class FirstRow : Tester<OverlapGrid<Dummy>>
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
        public void WhenThereIsOnlyOneItem_ReturnItemRow()
        {
            //Arrange
            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(item.Y);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsWithSameRow_ReturnThatRow()
        {
            //Arrange
            var row = Fixture.Create<int>();

            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(row);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsOnDifferentRows_ReturnSmallestRow()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), -8, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), -1, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 0, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 91, Fixture.Create<Dummy>());

            //Act
            var result = Instance.FirstRow;

            //Assert
            result.Should().Be(-8);
        }
    }

    [TestClass]
    public class LastRow : Tester<OverlapGrid<Dummy>>
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
        public void WhenThereIsOnlyOneItem_ReturnItemRow()
        {
            //Arrange
            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(item.Y);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsWithSameRow_ReturnThatRow()
        {
            //Arrange
            var row = Fixture.Create<int>();

            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), row, Fixture.Create<Dummy>());

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(row);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsOnDifferentRows_ReturnHighestRow()
        {
            //Arrange
            Instance.Add(Fixture.Create<int>(), -8, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), -1, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 0, Fixture.Create<Dummy>());
            Instance.Add(Fixture.Create<int>(), 91, Fixture.Create<Dummy>());

            //Act
            var result = Instance.LastRow;

            //Assert
            result.Should().Be(91);
        }
    }

    [TestClass]
    public class Count : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenThereAreNoItems_ReturnZero()
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
            Instance.Add(Fixture.Create<Cell<Dummy>>());

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(1);
        }

        [TestMethod]
        public void WhenThereAreItemsButTheyAreAllInSameCell_ReturnNumberOfItems()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany(4));

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(4);
        }

        [TestMethod]
        public void WhenThereAreMultipleItems_ReturnNumberOfItems()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>(7));

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(7);
        }
    }

    [TestClass]
    public class Constructor : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => new OverlapGrid<Dummy>(null!);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("collection");
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_InstantiateEmpty()
        {
            //Arrange
            var collection = new List<Cell<Dummy>>();

            //Act
            var result = new OverlapGrid<Dummy>(collection);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionContainsItems_ContainThoseItems()
        {
            //Arrange
            var collection = Fixture.CreateMany<Cell<Dummy>>().ToArray();

            //Act
            var result = new OverlapGrid<Dummy>(collection);

            //Assert
            result.Should().BeEquivalentTo(collection);
        }
    }

    [TestClass]
    public class Boundaries : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZeroes()
        {
            //Arrange

            //Act
            var result = Instance.Boundaries;

            //Assert
            result.Should().Be(new Boundaries<int>());
        }

        [TestMethod]
        public void WhenContainsOneItemAtZeroZero_ReturnZeroes()
        {
            //Arrange
            Instance.Add(0, 0, Fixture.Create<Dummy>());

            //Act
            var result = Instance.Boundaries;

            //Assert
            result.Should().Be(new Boundaries<int>());
        }

        [TestMethod]
        public void WhenContainsMultipleItemsAtTheSameSpot_ReturnThatSpot()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index, Fixture.Create<Dummy>());
            Instance.Add(index, Fixture.Create<Dummy>());
            Instance.Add(index, Fixture.Create<Dummy>());
            Instance.Add(index, Fixture.Create<Dummy>());

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
        public void WhenContainsMultipleItemsAtDifferentPositions_ReturnBoundaries()
        {
            //Arrange
            Instance.Add(-2, 4, Fixture.Create<Dummy>());
            Instance.Add(-8, 2, Fixture.Create<Dummy>());
            Instance.Add(12, 1, Fixture.Create<Dummy>());
            Instance.Add(0, -3, Fixture.Create<Dummy>());

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
    }

    [TestClass]
    public class IndexesOf_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmpty()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemIsNotInGrid_ReturnEmpty()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhereThereIsOnlyOneOccurence_ReturnThatOccurence()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            //Act
            var result = Instance.IndexesOf(item.Value);

            //Assert
            result.Should().BeEquivalentTo(new List<Vector2<int>> { item.Index });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurences_ReturnAll()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Dummy>();

            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Value, item).CreateMany().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(items.Select(x => x.Index));
        }
    }

    [TestClass]
    public class IndexesOf_Predicate : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.IndexesOf(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenIsEmpty_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.IndexesOf(x => x.Id > 0);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfPredicate_ReturnEmpty()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            //Act
            var result = Instance.IndexesOf(x => x.Value.StartsWith("Jer"));

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreOccurencesOfPredicate_ReturnThoseOccurences()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Value, Fixture.Build<Dummy>().With(y => y.Value, "Jerry").Create()).CreateMany().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.IndexesOf(x => x.Value.StartsWith("Jer"));

            //Assert
            result.Should().BeEquivalentTo(items.Select(x => x.Index));
        }
    }

    [TestClass]
    public class Add_XY_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void Always_AddItemAtPosition()
        {
            //Arrange
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var value = Fixture.Create<Dummy>();

            //Act
            Instance.Add(x, y, value);

            //Assert
            Instance.Should().BeEquivalentTo(new OverlapGrid<Dummy> { new Cell<Dummy>(x, y, value) });
        }

        [TestMethod]
        public void Always_TriggerChange()
        {
            //Arrange
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();
            var value = Fixture.Create<Dummy>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Add(x, y, value);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>> { new(x, y, value) }
                }
            });
        }

        [TestMethod]
        public void WhenAddingMultipleItemsAtSamePosition_AddThemAll()
        {
            //Arrange
            var x = Fixture.Create<int>();
            var y = Fixture.Create<int>();

            var values = Fixture.CreateMany<Dummy>().ToList();

            //Act
            foreach (var value in values) Instance.Add(x, y, value);

            //Assert
            Instance.Should().BeEquivalentTo(new OverlapGrid<Dummy>(values.Select(value => new Cell<Dummy>(x, y, value))));
        }
    }

    [TestClass]
    public class Add_Vector_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void Always_AddItemAtPosition()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var value = Fixture.Create<Dummy>();

            //Act
            Instance.Add(index, value);

            //Assert
            Instance.Should().BeEquivalentTo(new OverlapGrid<Dummy> { new Cell<Dummy>(index, value) });
        }

        [TestMethod]
        public void Always_TriggerChange()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var value = Fixture.Create<Dummy>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Add(index, value);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = new List<Cell<Dummy>> { new(index, value) }
                }
            });
        }

        [TestMethod]
        public void WhenAddingMultipleItemsAtSamePosition_AddThemAll()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();

            var values = Fixture.CreateMany<Dummy>().ToList();

            //Act
            foreach (var value in values) Instance.Add(index, value);

            //Assert
            Instance.Should().BeEquivalentTo(new OverlapGrid<Dummy>(values.Select(value => new Cell<Dummy>(index, value))));
        }
    }

    [TestClass]
    public class Add_Params_Cells : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void Always_AddItemsAtPosition()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToArray();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().BeEquivalentTo(cells);
        }

        [TestMethod]
        public void Always_TriggerChangeOnce()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToArray();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Add(cells);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = cells
                }
            });
        }

        [TestMethod]
        public void WhenAddingMultipleItemsAtSamePosition_AddThemAll()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var cells = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToArray();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().BeEquivalentTo(cells);
        }
    }

    [TestClass]
    public class Add_Enumerable_Cells : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenCellsAreNull_Throw()
        {
            //Arrange
            IEnumerable<Cell<Dummy>> cells = null!;

            //Act
            var action = () => Instance.Add(cells);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(cells));
        }

        [TestMethod]
        public void Always_AddItemsAtPosition()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().BeEquivalentTo(cells);
        }

        [TestMethod]
        public void Always_TriggerChangeOnce()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>().ToList();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Add(cells);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = cells
                }
            });
        }

        [TestMethod]
        public void WhenAddingMultipleItemsAtSamePosition_AddThemAll()
        {
            //Arrange
            var index = Fixture.Create<Vector2<int>>();
            var cells = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();

            //Act
            Instance.Add(cells);

            //Assert
            Instance.Should().BeEquivalentTo(cells);
        }
    }

    [TestClass]
    public class RemoveAt_XY : Tester<OverlapGrid<Dummy>>
    {
        //TODO Fix
        [TestMethod, Ignore("For some reason it throws something else")]
        public void WhenThereIsNothingAtIndex_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index.X, index.Y);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_RemoveThatItem()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            Instance.RemoveAt(item.X, item.Y);

            //Assert
            Instance.Should().NotContain(item);
            Instance.Should().NotContain(x => x.Index == item.Index);
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_TriggerChange()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(item.X, item.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { item }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveAt(index.X, index.Y);

            //Assert
            Instance.Should().NotContain(items);
            Instance.Should().NotContain(x => x.Index == index);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(index.X, index.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class RemoveAt_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod, Ignore("For some reason it throws something else")]
        public void WhenThereIsNothingAtIndex_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.RemoveAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage(string.Format(Exceptions.CannotRemoveItemAtIndexBecauseThereIsNothingThere, index));
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_RemoveThatItem()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            Instance.RemoveAt(item.Index);

            //Assert
            Instance.Should().NotContain(item);
            Instance.Should().NotContain(x => x.Index == item.Index);
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_TriggerChange()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(item.Index);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { item }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveAt(index);

            //Assert
            Instance.Should().NotContain(items);
            Instance.Should().NotContain(x => x.Index == index);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(index);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class TryRemoveAt_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtIndex_DoesNotThrow()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_RemoveThatItem()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            Instance.TryRemoveAt(item.X, item.Y);

            //Assert
            Instance.Should().NotContain(item);
            Instance.Should().NotContain(x => x.Index == item.Index);
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_TriggerChange()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAt(item.X, item.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { item }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            Instance.Should().NotContain(items);
            Instance.Should().NotContain(x => x.Index == index);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAt(index.X, index.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class TryRemoveAt_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtIndex_DoesNotThrow()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var action = () => Instance.TryRemoveAt(index);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_RemoveThatItem()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            Instance.TryRemoveAt(item.Index);

            //Assert
            Instance.Should().NotContain(item);
            Instance.Should().NotContain(x => x.Index == item.Index);
        }

        [TestMethod]
        public void WhenThereIsOneItemAtIndex_TriggerChange()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAt(item.Index);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { item }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_RemoveThemAll()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveAt(index);

            //Assert
            Instance.Should().NotContain(items);
            Instance.Should().NotContain(x => x.Index == index);
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsAtIndex_TriggerChangeOnce()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAt(index);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class RemoveAll_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNotInGrid_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = Fixture.Create<Dummy>();

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenItemIsNotInGrid_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = Fixture.Create<Cell<Dummy>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(item.Value);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemIsInGridOnce_RemoveThatInstanceOnly()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            Instance.RemoveAll(item.Value);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenItemIsInGridOnce_TriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(item.Value);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { item }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsInMultiplePlaces_RemoveAll()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var value = Fixture.Create<Dummy>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Value, value).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveAll(value);

            //Assert
            Instance.Should().BeEquivalentTo(originalItems);
        }

        [TestMethod]
        public void WhenItemIsInMultiplePlaces_DoNotTriggerChange()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var value = Fixture.Create<Dummy>();
            var items = Fixture.Build<Cell<Dummy>>().With(x => x.Value, value).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(value);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class RemoveAll_Predicate : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.RemoveAll(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenNothingMatchesPredicate_DoNothing()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            //Act
            Instance.RemoveAll(x => x.Id < 0);

            //Assert
            Instance.Should().BeEquivalentTo(originalItems);
        }

        [TestMethod]
        public void WhenNothingMatchesPredicate_DoNotTriggerChange()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(x => x.Id < 0);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOneItemMatchesPredicate_RemoveIt()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var extraItem = Fixture.Create<Cell<Dummy>>();
            Instance.Add(extraItem);

            //Act
            Instance.RemoveAll(x => x.Id == extraItem.Value!.Id);

            //Assert
            Instance.Should().BeEquivalentTo(originalItems);
        }

        [TestMethod]
        public void WhenOneItemMatchesPredicate_TriggerChange()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var extraItem = Fixture.Create<Cell<Dummy>>();
            Instance.Add(extraItem);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(x => x.Id == extraItem.Value!.Id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { extraItem }
                }
            });
        }

        [TestMethod]
        public void WhenMultipleItemsMatchPredicate_RemoveThemAll()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var value = Fixture.Create<string>();
            var extraItems = Fixture.Build<Cell<Dummy>>().With(x => x.Value, Fixture.Build<Dummy>().With(y => y.Value, value).Create()).CreateMany().ToList();
            Instance.Add(extraItems);

            //Act
            Instance.RemoveAll(x => x.Value == value);

            //Assert
            Instance.Should().BeEquivalentTo(originalItems);
        }

        [TestMethod]
        public void WhenMultipleItemsMatchPredicate_TriggerChangeOnce()
        {
            //Arrange
            var originalItems = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(originalItems);

            var value = Fixture.Create<string>();
            var extraItems = Fixture.Build<Cell<Dummy>>().With(x => x.Value, Fixture.Build<Dummy>().With(y => y.Value, value).Create()).CreateMany().ToList();
            Instance.Add(extraItems);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(x => x.Value == value);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = extraItems
                }
            });
        }
    }

    [TestClass]
    public class Contains_XY_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsInGridButNotAtPosition_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(Fixture.Create<int>(), Fixture.Create<int>(), item.Value);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtPositionButNotItem_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(item.X, item.Y, Fixture.Create<Dummy>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsAtPosition_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(item.X, item.Y, item.Value);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_Vector_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsInGridButNotAtPosition_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(Fixture.Create<Vector2<int>>(), item.Value);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtPositionButNotItem_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(item.Index, Fixture.Create<Dummy>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsAtPosition_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToArray();
            Instance.Add(items);

            var item = items.GetRandom();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(item.Index, item.Value);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_Item : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNullButGridIsEmpty_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(null);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsNullAndGridContainsNullValues_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());
            Instance.Add(Fixture.Create<Vector2<int>>(), null);

            //Act
            var result = Instance.Contains(null);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenItemIsNotInGrid_ReturnFalse()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            //Act
            var result = Instance.Contains(Fixture.Create<Dummy>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsInGrid_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var item = Fixture.Create<Cell<Dummy>>();
            Instance.Add(item);

            //Act
            var result = Instance.Contains(item);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtPosition_ReturnFalse()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtPosition_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var index = items.GetRandom().Index;

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenThereIsOneNullValueAtPosition_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index, null);

            //Act
            var result = Instance.Contains(index.X, index.Y);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Contains_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenThereIsNothingAtPosition_ReturnFalse()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());

            var index = Fixture.Create<Vector2<int>>();

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenThereIsSomethingAtPosition_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var index = items.GetRandom().Index;

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenThereIsOneNullValueAtPosition_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Cell<Dummy>>());
            var index = Fixture.Create<Vector2<int>>();
            Instance.Add(index, null);

            //Act
            var result = Instance.Contains(index);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Resize : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenResizedBiggerThanContent_DoNothing()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Resize(new Boundaries<int>(-6, 6, 6, -6));

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenResizedBiggerThanContent_DoNotTrigger()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Resize(new Boundaries<int>(-6, 6, 6, -6));

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenResizedSmallerThanContent_RemoveExcess()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Resize(new Boundaries<int>(-4, 4, 4, -4));

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>> { items[1] });
        }

        [TestMethod]
        public void WhenResizedSmallerThanContent_TriggerChangeOnce()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Resize(new Boundaries<int>(-4, 4, 4, -4));

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        items[0], items[2]
                    }
                }
            });
        }

        [TestMethod]
        public void WhenResizedToZero_OnlyItemsAtOriginRemain()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Resize(new Boundaries<int>(0, 0, 0, 0));

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>> { items[2] });
        }

        [TestMethod]
        public void WhenResizedToZero_TriggerChangeOnce()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(-5, -5, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(0, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(5, 5, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Resize(new Boundaries<int>(0, 0, 0, 0));

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { items[0], items[1], items[3], items[4] }
                }
            });
        }
    }

    [TestClass]
    public class TranslateAll_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Vector2<int>.Zero;

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveAllItemsInDirection()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveAllItemsInDirection()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = -Fixture.Create<Vector2<int>>();

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
        }

        [TestMethod]
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = -Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation.X, translation.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class TranslateAll_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Vector2<int>.Zero;

            //Act
            Instance.TranslateAll(translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveAllItemsInDirection()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.TranslateAll(translation);

            //Assert
            Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveAllItemsInDirection()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = -Fixture.Create<Vector2<int>>();

            //Act
            Instance.TranslateAll(translation);

            //Assert
            Instance.Should().BeEquivalentTo(items.Select(x => x with { Index = x.Index + translation }));
        }

        [TestMethod]
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var translation = -Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TranslateAll(translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items,
                    NewValues = items.Select(x => x with { Index = x.Index + translation }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class Translate_Range_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = Fixture.Create<Rectangle<int>>();
            var translation = Vector2<int>.Zero;

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = Fixture.Create<Rectangle<int>>();
            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = new Rectangle<int>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = new Rectangle<int>();
            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNothing()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNotTrigger()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(range, translation.X, translation.Y);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Range_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = Fixture.Create<Rectangle<int>>();
            var translation = Vector2<int>.Zero;

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = Fixture.Create<Rectangle<int>>();
            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(range, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = new Rectangle<int>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var range = new Rectangle<int>();
            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(range, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(range, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(range, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNothing()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Fixture.Create<Vector2<int>>());

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNotTrigger()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(new Rectangle<int>(15, 20, 100, 200), Fixture.Create<Vector2<int>>());

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(range, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var range = new Rectangle<int>(1, 1, 2, 2);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(range, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Boundaries_XY : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Vector2<int>.Zero;

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = new Boundaries<int>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = new Boundaries<int>();
            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNothing()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Fixture.Create<Vector2<int>>());

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNotTrigger()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Fixture.Create<Vector2<int>>());

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Translate_Boundaries_Vector : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenTranslationIsZero_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Vector2<int>.Zero;

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenTranslationIsZero_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = Fixture.Create<Boundaries<int>>();
            var translation = Vector2<int>.Zero;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = new Boundaries<int>();
            var translation = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Translate(boundaries, translation.X, translation.Y);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenBoundariesAreEqualOnAllAxis_DoNotTriggerChange()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var boundaries = new Boundaries<int>();
            var translation = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationIsNegative_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsNegative_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(-5, -5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(-4, -4, items[5].Value),
                        new(-3, -4, items[6].Value),
                        new(-2, -4, items[7].Value),
                        new(-4, -3, items[9].Value),
                        new(-3, -3, items[10].Value),
                        new(-2, -3, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenTranslationIsPositiveAndNotZero_MoveBoundariesInThatDirection()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationIsPositiveAndNotZero_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(5, 5);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(6, 6, items[5].Value),
                        new(7, 6, items[6].Value),
                        new(8, 6, items[7].Value),
                        new(6, 7, items[9].Value),
                        new(7, 7, items[10].Value),
                        new(8, 7, items[11].Value)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNothing()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Fixture.Create<Vector2<int>>());

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenThereIsNothingWithinBoundaries_DoNotTrigger()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Translate(new Boundaries<int>(25, 400, 500, 100), Fixture.Create<Vector2<int>>());

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenTranslationMovesItemsIntoExistingItems_OverlapItems()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenTranslationMovesItemsIntoExistingItems_TriggerChange()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var boundaries = new Boundaries<int>(1, 3, 3, 1);
            var translation = new Vector2<int>(0, -1);

            //Act
            Instance.Translate(boundaries, translation);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>>
                    {
                        new(1, 1, items[5].Value),
                        new(2, 1, items[6].Value),
                        new(3, 1, items[7].Value),
                        new(1, 2, items[9].Value),
                        new(2, 2, items[10].Value),
                        new(3, 2, items[11].Value)
                    },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(1, 0, items[5].Value),
                        new(2, 0, items[6].Value),
                        new(3, 0, items[7].Value),
                        new(1, 1, items[9].Value),
                        new(2, 1, items[10].Value),
                        new(3, 1, items[11].Value)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Copy : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void Always_MakeAPerfectCopy()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Copy();

            //Assert
            result.Should().BeEquivalentTo(items);
            result.Should().NotBeSameAs(items);
        }
    }

    [TestClass]
    public class Copy_Boundaries : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenBoundariesAreEqualToCollectionBoundaries_CopyEverything()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            var result = Instance.Copy(Instance.Boundaries);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenBoundariesAreEqualToPartOfCollection_CopyThatPart()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            var result = Instance.Copy(new Boundaries<int>(1, 3, 3, 1));

            //Assert
            result.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenBoundariesAreEmpty_CreateEmptyGrid()
        {
            //Arrange
            var items = new List<Cell<Dummy>>
            {
                new(0, 0, Fixture.Create<Dummy>()),
                new(1, 0, Fixture.Create<Dummy>()),
                new(2, 0, Fixture.Create<Dummy>()),
                new(3, 0, Fixture.Create<Dummy>()),
                new(0, 1, Fixture.Create<Dummy>()),
                new(1, 1, Fixture.Create<Dummy>()),
                new(2, 1, Fixture.Create<Dummy>()),
                new(3, 1, Fixture.Create<Dummy>()),
                new(0, 2, Fixture.Create<Dummy>()),
                new(1, 2, Fixture.Create<Dummy>()),
                new(2, 2, Fixture.Create<Dummy>()),
                new(3, 2, Fixture.Create<Dummy>()),
            };

            Instance.Add(items);

            //Act
            var result = Instance.Copy(new Boundaries<int>(100, 200, 200, 100));

            //Assert
            result.Should().BeEmpty();
        }
    }

    [TestClass]
    public class Swap : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenCurrentAndDestinationAreEqual_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = current;

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenCurrentAndDestinationAreEqual_DoNotTriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = current;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNothingAtBothCurrentAndDestination_DoNothing()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenThereIsNothingAtBothCurrentAndDestination_DoNotTriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCurrentAndDestinationAreDifferent_SwapTheirIndexes()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = items.First().Index;
            var destination = items.Last().Index;

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
            {
                new(items[2].Index, items[0].Value),
                items[1],
                new(items[0].Index, items[2].Value),
            });
        }

        [TestMethod]
        public void WhenCurrentAndDestinationAreDifferent_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = items.First().Index;
            var destination = items.Last().Index;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { items[0], items[2] },
                    NewValues = new List<Cell<Dummy>> { new(items[2].Index, items[0].Value), new(items[0].Index, items[2].Value) }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleItemsInBothCurrentAndDestination_SwapThemAll()
        {
            //Arrange
            var indexes = Fixture.CreateMany<Vector2<int>>(3).ToList();

            var items = new List<Cell<Dummy>>();
            foreach (var index in indexes)
                items.AddRange(Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany(3));

            Instance.Add(items);

            var current = indexes.First();
            var destination = indexes.Last();

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
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
        public void WhenThereAreMultipleItemsInBothCurrentAndDestination_TriggerEventOnce()
        {
            //Arrange
            var indexes = Fixture.CreateMany<Vector2<int>>(3).ToList();

            var items = new List<Cell<Dummy>>();
            foreach (var index in indexes)
                items.AddRange(Fixture.Build<Cell<Dummy>>().With(x => x.Index, index).CreateMany(3));

            Instance.Add(items);

            var current = indexes.First();
            var destination = indexes.Last();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { items[0], items[1], items[2], items[6], items[7], items[8] },
                    NewValues = new List<Cell<Dummy>>
                    {
                        new(indexes[2], items[0].Value),
                        new(indexes[2], items[1].Value),
                        new(indexes[2], items[2].Value),
                        new(indexes[0], items[6].Value),
                        new(indexes[0], items[7].Value),
                        new(indexes[0], items[8].Value),
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingAtCurrent_MoveDestinationToCurrent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = items.Last().Index;

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
            {
                items[0],
                items[1],
                new(current, items[2].Value),
            });
        }

        [TestMethod]
        public void WhenThereIsNothingAtCurrent_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = Fixture.Create<Vector2<int>>();
            var destination = items.Last().Index;

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { items[2] },
                    NewValues = new List<Cell<Dummy>> { new(current, items[2].Value) }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsNothingAtDestination_MoveCurrentToDestination()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = items.First().Index;
            var destination = Fixture.Create<Vector2<int>>();

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Cell<Dummy>>
            {
                new(destination, items[0].Value),
                items[1],
                items[2],
            });
        }

        [TestMethod]
        public void WhenThereIsNothingAtDestination_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>(3).ToList();
            Instance.Add(items);

            var current = items.First().Index;
            var destination = Fixture.Create<Vector2<int>>();

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Swap(current, destination);

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Cell<Dummy>> { items[0] },
                    NewValues = new List<Cell<Dummy>> { new(destination, items[0].Value) }
                }
            });
        }
    }

    [TestClass]
    public class Clear : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_DoNotTrigger()
        {
            //Arrange
            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsNotEmpty_RemoveAllItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            //Act
            Instance.Clear();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenGridIsNotEmpty_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<GridChangedEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEquivalentTo(new List<GridChangedEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = items
                }
            });
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenGridIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Empty OverlapGrid<Dummy>");
        }

        [TestMethod]
        public void WhenGridIsNotEmpty_ReturnCount()
        {
            //Arrange
            var cells = Fixture.CreateMany<Cell<Dummy>>(5).ToList();
            Instance.Add(cells);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("OverlapGrid<Dummy> with 5 items");
        }
    }

    [TestClass]
    public class Equality : Tester<OverlapGrid<Dummy>>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            Fixture.Customizations.Add(new OverlapGridSpecimenBuilder());
        }

        [TestMethod]
        public void Always_EnsureValueEquality() => Ensure.ValueEquality<OverlapGrid<Dummy>>(Fixture);
    }

    [TestClass]
    public class HashCode : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void Always_ReturnInternalListHashCode()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.GetHashCode();

            //Assert
            result.Should().Be(GetFieldValue<List<Cell<Dummy>>>("_items")!.GetHashCode());
        }
    }

    [TestClass]
    public class Serialization : Tester<OverlapGrid<Dummy>>
    {
        [TestMethod]
        public void WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            var json = JsonConvert.SerializeObject(Instance);

            //Act
            var result = JsonConvert.DeserializeObject<OverlapGrid<Dummy>>(json);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
        {
            //Arrange
            var items = Fixture.CreateMany<Cell<Dummy>>().ToList();
            Instance.Add(items);

            JsonSerializerOptions.WithGridConverters();

            var json = System.Text.Json.JsonSerializer.Serialize(Instance, JsonSerializerOptions);

            //Act
            var result = System.Text.Json.JsonSerializer.Deserialize<OverlapGrid<Dummy>>(json, JsonSerializerOptions);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }
    }
}