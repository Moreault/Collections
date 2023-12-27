namespace Collections.ObservableStack.Tests;

[TestClass]
public class ObservableStackTests
{
    [TestClass]
    public class Count : Tester<ObservableStack<Dummy>>
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
    public class Clear : Tester<ObservableStack<Dummy>>
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
    public class Contains : Tester<ObservableStack<Dummy>>
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
    public class Peek : Tester<ObservableStack<Dummy>>
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
    public class TryPeek : Tester<ObservableStack<Dummy>>
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
    public class Pop : Tester<ObservableStack<Dummy>>
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
    public class TryPop : Tester<ObservableStack<Dummy>>
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
    public class Push_Params : Tester<ObservableStack<Dummy>>
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
    public class Push_Enumerable : Tester<ObservableStack<Dummy>>
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
    public class EqualsMethod : Tester<ObservableStack<Dummy>>
    {
        [TestMethod]
        public void OtherIsNull_ReturnFalse()
        {
            //Arrange
            ObservableStack<Dummy> other = null!;

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

            var other = new ObservableStack<Dummy>();
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

            var other = new ObservableStack<Dummy>();
            other.Push(items);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<ObservableStack<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("Empty ObservableStack<Dummy>");
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
            result.Should().Be("ObservableStack<Dummy> with 3 items");
        }
    }
}