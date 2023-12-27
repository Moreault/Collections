namespace Collections.Caching.Tests;

[TestClass]
public class CachingStackTests
{
    [TestClass]
    public class Limit : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenLimitIsLessThanZero_RemoveAllItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var value = -Fixture.Create<int>();

            //Act
            Instance.Limit = value;

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsLessThanZero_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var value = -Fixture.Create<int>();

            //Act
            Instance.Limit = value;

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void WhenLimitIsLessThanZeroButStackWasEmpty_DoNotTrigger()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Limit = value;

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsZero_RemoveAllItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.Limit = 0;

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsZero_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Limit = 0;

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void WhenLimitIsZeroButStackWasEmpty_DoNotTrigger()
        {
            //Arrange
            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Limit = 0;

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsLessThanZero_PreventAddingItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var value = -Fixture.Create<int>();

            //Act
            Instance.Limit = value;

            //Assert
            Instance.Push(Fixture.Create<Dummy>());
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsZero_PreventAddingItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.Limit = 0;

            //Assert
            Instance.Push(Fixture.Create<Dummy>());
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsEqualToSize_DoNotRemoveItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(6).ToList();
            Instance.Push(items);

            //Act
            Instance.Limit = 6;

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenLimitIsEqualToSize_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(6).ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Limit = 6;

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsHalfSize_RemoveHalfItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(10).ToList();
            Instance.Push(items);

            //Act
            Instance.Limit = 5;

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[5],
                items[6],
                items[7],
                items[8],
                items[9],
            });
        }

        [TestMethod]
        public void WhenLimitIsHalfSize_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(10).ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Limit = 5;

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy>
                    {
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                    }
                }
            });
        }

        [TestMethod]
        public void WhenLimitIsHalfSizeAndAddMoreItems_RemoveItemsAsNewOnesAreAdded()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(10).ToList();
            Instance.Push(items);

            //Act
            Instance.Limit = 5;

            //Assert
            var newItems = Fixture.CreateMany<Dummy>(3).ToList();
            Instance.Push(newItems);
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                newItems[0],
                newItems[1],
                newItems[2],
                items[8],
                items[9],
            });
        }
    }

    [TestClass]
    public class TrimTopDownTo : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenMaxSizeIsNegative_Throw()
        {
            //Arrange
            var maxSize = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TrimTopDownTo(maxSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
        {
            //Arrange
            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimTopDownTo(0);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToZero_RemoveEverything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimTopDownTo(0);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToZero_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimTopDownTo(0);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimTopDownTo(items.Count + 1);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimTopDownTo(items.Count + 1);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimTopDownTo(items.Count);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimTopDownTo(items.Count);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToHalf_RemoveTopHalfOfItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(12).ToList();
            Instance.Push(items);

            //Act
            Instance.TrimTopDownTo(6);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
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
        public void WhenContainsItemsAndTrimToHalf_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(12).ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimTopDownTo(6);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy>
                    {
                        items[6],
                        items[7],
                        items[8],
                        items[9],
                        items[10],
                        items[11],
                    }
                }
            });
        }
    }

    [TestClass]
    public class TrimBottomDownTo : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenMaxSizeIsNegative_Throw()
        {
            //Arrange
            var maxSize = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TrimBottomDownTo(maxSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
        {
            //Arrange
            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimBottomDownTo(0);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToZero_RemoveEverything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimBottomDownTo(0);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToZero_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimBottomDownTo(0);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimBottomDownTo(items.Count + 1);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimBottomDownTo(items.Count + 1);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TrimBottomDownTo(items.Count);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimBottomDownTo(items.Count);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsAndTrimToHalf_RemoveTopHalfOfItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(12).ToList();
            Instance.Push(items);

            //Act
            Instance.TrimBottomDownTo(6);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
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
        public void WhenContainsItemsAndTrimToHalf_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(12).ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TrimBottomDownTo(6);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy>
                    {
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                        items[5],
                    }
                }
            });
        }
    }

    [TestClass]
    public class Count : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenContainsItems_ReturnNumberOfItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(items.Count);
        }
    }

    [TestClass]
    public class Clear : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_DoNotTriggerEvent()
        {
            //Arrange
            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItems_RemoveAll()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.Clear();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItems_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }
    }

    [TestClass]
    public class Contains : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNotInCollection_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.Contains(item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsInCollection_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var item = items.GetRandom();

            //Act
            var result = Instance.Contains(item);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Peek : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Peek();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_ReturnThatItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            var result = Instance.Peek();

            //Assert
            result.Should().Be(item);
        }

        [TestMethod]
        public void WhenContainsItems_ReturnLastItemAdded()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            var result = Instance.Peek();

            //Assert
            result.Should().Be(items.Last());
        }
    }

    [TestClass]
    public class TryPeek : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnFailure()
        {
            //Arrange

            //Act
            var result = Instance.TryPeek();

            //Assert
            result.Should().Be(Result<Dummy>.Failure());
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_ReturnThatItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            var result = Instance.TryPeek();

            //Assert
            result.Should().Be(Result<Dummy>.Success(item));
        }

        [TestMethod]
        public void WhenContainsItems_ReturnLastItemAdded()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);
            //Act
            var result = Instance.TryPeek();

            //Assert
            result.Should().Be(Result<Dummy>.Success(items.Last()));
        }
    }

    [TestClass]
    public class Pop : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Pop();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_ReturnThatItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            var result = Instance.Pop();

            //Assert
            result.Should().Be(item);
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_RemoveItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            Instance.Pop();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Pop();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy> { item }
                }
            });
        }

        [TestMethod]
        public void WhenContainsItems_ReturnLastItemAdded()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);
            //Act
            var result = Instance.Pop();

            //Assert
            result.Should().Be(items.Last());
        }

        [TestMethod]
        public void WhenContainsItems_RemoveItem()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.Pop();

            //Assert
            Instance.Should().NotContain(items.Last());
        }

        [TestMethod]
        public void WhenContainsItems_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Pop();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy> { items.Last() }
                }
            });
        }
    }

    [TestClass]
    public class TryPop : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnFailure()
        {
            //Arrange

            //Act
            var result = Instance.TryPop();

            //Assert
            result.Should().Be(Result<Dummy>.Failure());
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_ReturnThatItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            var result = Instance.TryPop();

            //Assert
            result.Should().Be(Result<Dummy>.Success(item));
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_RemoveItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            //Act
            Instance.TryPop();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsOnlyOneItem_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            Instance.Push(item);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryPop();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy> { item }
                }
            });
        }

        [TestMethod]
        public void WhenContainsItems_ReturnLastItemAdded()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);
            //Act
            var result = Instance.TryPop();

            //Assert
            result.Should().Be(Result<Dummy>.Success(items.Last()));
        }

        [TestMethod]
        public void WhenContainsItems_RemoveItem()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            //Act
            Instance.TryPop();

            //Assert
            Instance.Should().NotContain(items.Last());
        }

        [TestMethod]
        public void WhenContainsItems_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryPop();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy> { items.Last() }
                }
            });
        }
    }

    [TestClass]
    public class Push_Params : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenItemsIsEmpty_DoNotAddNewElements()
        {
            //Arrange

            //Act
            Instance.Push();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemsIsEmpty_DoNotTrigger()
        {
            //Arrange

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Push();

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsNotEmpty_PushAllOfThemToTopOfStack()
        {
            //Arrange
            var firstItems = Fixture.CreateMany<Dummy>().ToArray();
            Instance.Push(firstItems);

            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            Instance.Push(items);

            //Assert
            Instance.Should().ContainInOrder(firstItems.Concat(items).Reverse());
        }

        [TestMethod]
        public void WhenIsNotEmpty_TriggerEvent()
        {
            //Arrange
            var firstItems = Fixture.CreateMany<Dummy>().ToArray();
            Instance.Push(firstItems);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            Instance.Push(items);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = items
                }
            });
        }
    }

    [TestClass]
    public class Push_Enumerable : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenItemsIsNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> items = null!;

            //Act
            var action = () => Instance.Push(items);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenItemsIsEmpty_DoNotAddNewElements()
        {
            //Arrange
            var items = new List<Dummy>();

            //Act
            Instance.Push(items);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemsIsEmpty_DoNotTrigger()
        {
            //Arrange
            var items = new List<Dummy>();

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Push(items);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenIsNotEmpty_PushAllOfThemToTopOfStack()
        {
            //Arrange
            var firstItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(firstItems);

            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            Instance.Push(items);

            //Assert
            Instance.Should().ContainInOrder(firstItems.Concat(items).Reverse());
        }

        [TestMethod]
        public void WhenIsNotEmpty_TriggerEvent()
        {
            //Arrange
            var firstItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(firstItems);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            Instance.Push(items);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    NewValues = items
                }
            });
        }
    }

    [TestClass]
    public class EqualsMethod : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void OtherIsNull_ReturnFalse()
        {
            //Arrange
            CachingStack<Dummy> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Equals(Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferentInstanceButContainsSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var other = new CachingStack<Dummy>();
            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            otherItems.Reverse();
            other.Push(otherItems);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentInstanceButContainsSameItemsInSameOrder_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Push(items);

            var other = new CachingStack<Dummy>();
            other.Push(items);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<CachingStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("Empty CachingStack<Dummy>");
        }

        [TestMethod]
        public void WhenContainsItems_ReturnNumberOfItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(3).ToList();
            Instance.Push(items);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("CachingStack<Dummy> with 3 items");
        }
    }
}