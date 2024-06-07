namespace Collections.Caching.Tests;

[TestClass]
public class CachingStackTests : Tester<CachingStack<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
        JsonSerializerOptions.WithCachingConverters();
    }

    [TestMethod]
    public void IsSynchronized_Always_ReturnsFalse()
    {
        //Arrange

        //Act
        var result = ((ICollection)Instance).IsSynchronized;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void SyncRoot_Always_ReturnSelf()
    {
        //Arrange
        //Act
        var result = ((ICollection)Instance).SyncRoot;

        //Assert
        result.Should().Be(Instance);
    }


    [TestMethod]
    public void Limit_WhenLimitIsLessThanZero_RemoveAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var value = -Dummy.Create<int>();

        //Act
        Instance.Limit = value;

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var value = -Dummy.Create<int>();

        //Act
        Instance.Limit = value;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZeroButStackWasEmpty_DoNotTrigger()
    {
        //Arrange
        var value = -Dummy.Create<int>();

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = value;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsZero_RemoveAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.Limit = 0;

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsZeroButStackWasEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZero_PreventAddingItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var value = -Dummy.Create<int>();

        //Act
        Instance.Limit = value;

        //Assert
        Instance.Push(Dummy.Create<Garbage>());
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsZero_PreventAddingItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.Limit = 0;

        //Assert
        Instance.Push(Dummy.Create<Garbage>());
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsEqualToSize_DoNotRemoveItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(6).ToList();
        Instance.Push(items);

        //Act
        Instance.Limit = 6;

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Limit_WhenLimitIsEqualToSize_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(6).ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 6;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsHalfSize_RemoveHalfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(10).ToList();
        Instance.Push(items);

        //Act
        Instance.Limit = 5;

        //Assert
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                items[5],
                items[6],
                items[7],
                items[8],
                items[9],
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsHalfSize_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(10).ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 5;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                    ]
                }
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsHalfSizeAndAddMoreItems_RemoveItemsAsNewOnesAreAdded()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(10).ToList();
        Instance.Push(items);

        //Act
        Instance.Limit = 5;

        //Assert
        var newItems = Dummy.CreateMany<Garbage>(3).ToList();
        Instance.Push(newItems);
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                newItems[0],
                newItems[1],
                newItems[2],
                items[8],
                items[9],
            });
    }

    [TestMethod]
    public void TrimTopDownTo_WhenMaxSizeIsNegative_Throw()
    {
        //Arrange
        var maxSize = -Dummy.Create<int>();

        //Act
        var action = () => Instance.TrimTopDownTo(maxSize);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TrimTopDownTo_WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimTopDownTo(0);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsAndTrimToZero_RemoveEverything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimTopDownTo(0);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsAndTrimToZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimTopDownTo(0);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimTopDownTo(items.Count + 1);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimTopDownTo(items.Count + 1);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimTopDownTo(items.Count);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimTopDownTo(items.Count);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsAndTrimToHalf_RemoveTopHalfOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(12).ToList();
        Instance.Push(items);

        //Act
        Instance.TrimTopDownTo(6);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                items[0],
                items[1],
                items[2],
                items[3],
                items[4],
                items[5],
            });
    }

    [TestMethod]
    public void TrimTopDownTo_WhenContainsItemsAndTrimToHalf_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(12).ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimTopDownTo(6);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        items[6],
                        items[7],
                        items[8],
                        items[9],
                        items[10],
                        items[11],
                    ]
                }
            });
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenMaxSizeIsNegative_Throw()
    {
        //Arrange
        var maxSize = -Dummy.Create<int>();

        //Act
        var action = () => Instance.TrimBottomDownTo(maxSize);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimBottomDownTo(0);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsAndTrimToZero_RemoveEverything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimBottomDownTo(0);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsAndTrimToZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimBottomDownTo(0);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimBottomDownTo(items.Count + 1);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimBottomDownTo(items.Count + 1);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TrimBottomDownTo(items.Count);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimBottomDownTo(items.Count);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsAndTrimToHalf_RemoveTopHalfOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(12).ToList();
        Instance.Push(items);

        //Act
        Instance.TrimBottomDownTo(6);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                items[6],
                items[7],
                items[8],
                items[9],
                items[10],
                items[11],
            });
    }

    [TestMethod]
    public void TrimBottomDownTo_WhenContainsItemsAndTrimToHalf_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(12).ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimBottomDownTo(6);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues =
                    [
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                        items[5],
                    ]
                }
            });
    }

    [TestMethod]
    public void Count_WhenEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void Count_WhenContainsItems_ReturnNumberOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(items.Count);
    }

    [TestMethod]
    public void CountFromReadOnlyCollection_WhenEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = ((IReadOnlyCollection<Garbage>)Instance).Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void CountFromReadOnlyCollection_WhenContainsItems_ReturnNumberOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        var result = ((IReadOnlyCollection<Garbage>)Instance).Count;

        //Assert
        result.Should().Be(items.Count);
    }

    [TestMethod]
    public void Clear_WhenIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_RemoveAll()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.Clear();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
        {
            new() { OldValues = items }
        });
    }

    [TestMethod]
    public void Contains_WhenItemIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var item = Dummy.Create<Garbage>();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WhenItemIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Peek_WhenIsEmpty_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Peek();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Peek_WhenContainsOnlyOneItem_ReturnThatItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        var result = Instance.Peek();

        //Assert
        result.Should().Be(item);
    }

    [TestMethod]
    public void Peek_WhenContainsItems_ReturnLastItemAdded()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        var result = Instance.Peek();

        //Assert
        result.Should().Be(items.Last());
    }

    [TestMethod]
    public void TryPeek_WhenIsEmpty_ReturnFailure()
    {
        //Arrange

        //Act
        var result = Instance.TryPeek();

        //Assert
        result.Should().Be(Result<Garbage>.Failure());
    }

    [TestMethod]
    public void TryPeek_WhenContainsOnlyOneItem_ReturnThatItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        var result = Instance.TryPeek();

        //Assert
        result.Should().Be(Result<Garbage>.Success(item));
    }

    [TestMethod]
    public void TryPeek_WhenContainsItems_ReturnLastItemAdded()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);
        //Act
        var result = Instance.TryPeek();

        //Assert
        result.Should().Be(Result<Garbage>.Success(items.Last()));
    }

    [TestMethod]
    public void Pop_WhenIsEmpty_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Pop();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Pop_WhenContainsOnlyOneItem_ReturnThatItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        var result = Instance.Pop();

        //Assert
        result.Should().Be(item);
    }

    [TestMethod]
    public void Pop_WhenContainsOnlyOneItem_RemoveItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        Instance.Pop();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Pop_WhenContainsOnlyOneItem_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Pop();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void Pop_WhenContainsItems_ReturnLastItemAdded()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);
        //Act
        var result = Instance.Pop();

        //Assert
        result.Should().Be(items.Last());
    }

    [TestMethod]
    public void Pop_WhenContainsItems_RemoveItem()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.Pop();

        //Assert
        Instance.Should().NotContain(items.Last());
    }

    [TestMethod]
    public void Pop_WhenContainsItems_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Pop();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items.Last()]
                }
            });
    }

    [TestMethod]
    public void TryPop_WhenIsEmpty_ReturnFailure()
    {
        //Arrange

        //Act
        var result = Instance.TryPop();

        //Assert
        result.Should().Be(Result<Garbage>.Failure());
    }

    [TestMethod]
    public void TryPop_WhenContainsOnlyOneItem_ReturnThatItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        var result = Instance.TryPop();

        //Assert
        result.Should().Be(Result<Garbage>.Success(item));
    }

    [TestMethod]
    public void TryPop_WhenContainsOnlyOneItem_RemoveItem()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        //Act
        Instance.TryPop();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TryPop_WhenContainsOnlyOneItem_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<Garbage>();
        Instance.Push(item);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryPop();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [item]
                }
            });
    }

    [TestMethod]
    public void TryPop_WhenContainsItems_ReturnLastItemAdded()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);
        //Act
        var result = Instance.TryPop();

        //Assert
        result.Should().Be(Result<Garbage>.Success(items.Last()));
    }

    [TestMethod]
    public void TryPop_WhenContainsItems_RemoveItem()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        Instance.TryPop();

        //Assert
        Instance.Should().NotContain(items.Last());
    }

    [TestMethod]
    public void TryPop_WhenContainsItems_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryPop();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = [items.Last()]
                }
            });
    }

    [TestMethod]
    public void PushParams_WhenItemsIsEmpty_DoNotAddNewElements()
    {
        //Arrange

        //Act
        Instance.Push();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void PushParams_WhenItemsIsEmpty_DoNotTrigger()
    {
        //Arrange

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Push();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void PushParams_WhenIsNotEmpty_PushAllOfThemToTopOfStack()
    {
        //Arrange
        var firstItems = Dummy.CreateMany<Garbage>().ToArray();
        Instance.Push(firstItems);

        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        Instance.Push(items);

        //Assert
        Instance.Should().ContainInOrder(firstItems.Concat(items).Reverse());
    }

    [TestMethod]
    public void PushParams_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var firstItems = Dummy.CreateMany<Garbage>().ToArray();
        Instance.Push(firstItems);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        Instance.Push(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
        {
            new()
            {
                NewValues = items
            }
        });
    }

    [TestMethod]
    public void PushEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> items = null!;

        //Act
        var action = () => Instance.Push(items);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void PushEnumerable_WhenItemsIsEmpty_DoNotAddNewElements()
    {
        //Arrange
        var items = new List<Garbage>();

        //Act
        Instance.Push(items);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void PushEnumerable_WhenItemsIsEmpty_DoNotTrigger()
    {
        //Arrange
        var items = new List<Garbage>();

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Push(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void PushEnumerable_WhenIsNotEmpty_PushAllOfThemToTopOfStack()
    {
        //Arrange
        var firstItems = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(firstItems);

        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        Instance.Push(items);

        //Assert
        Instance.Should().ContainInOrder(firstItems.Concat(items).Reverse());
    }

    [TestMethod]
    public void PushEnumerable_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var firstItems = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(firstItems);

        var triggers = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        Instance.Push(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    NewValues = items
                }
            });
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnEmptyMessage()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be("Empty CachingStack<Garbage>");
    }

    [TestMethod]
    public void ToString_WhenContainsItems_ReturnNumberOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(3).ToList();
        Instance.Push(items);

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be("CachingStack<Garbage> with 3 items");
    }

    [TestMethod]
    public void CopyTo_Always_CopyToArray()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);
        var array = new Garbage[items.Count];

        //Act
        ((ICollection)Instance).CopyTo(array, 0);

        //Assert
        array.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void GetHashCode_Always_ReturnFromInternalCollection()
    {
        //Arrange
        var internalCollection = GetFieldValue<ObservableList<Garbage>>("_items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(internalCollection.GetHashCode());
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<CachingStack<Garbage>>(Dummy, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<CachingStack<Garbage>>(Dummy, JsonSerializerOptions);
}