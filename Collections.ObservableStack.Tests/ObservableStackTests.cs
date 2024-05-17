using System.Collections;
using ToolBX.Collections.ObservableStack.Json;
using ToolBX.Dummies;
using ToolBX.Eloquentest.Dummies;

namespace Collections.ObservableStack.Tests;

[TestClass]
public class ObservableStackTests : ToolBX.Collections.UnitTesting.Tester<ObservableStack<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        JsonSerializerOptions.WithObservableStackConverters();
    }

    [TestMethod]
    public void ParameterlessConstructor_Always_ReturnEmptyStack()
    {
        //Arrange

        //Act
        var result = new ObservableStack<Garbage>();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void EnumerableConstructor_WhenCollectionIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> items = null!;

        //Act
        var action = () => new ObservableStack<Garbage>(items);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void EnumerableConstructor_WhenCollectionIsNotNull_InitializeWithItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = new ObservableStack<Garbage>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
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
    public void CountFromObservableStackInterface_WhenEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = ((IObservableStack<Garbage>)Instance).Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void CountFromObservableStackInterface_WhenContainsItems_ReturnNumberOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Push(items);

        //Act
        var result = ((IObservableStack<Garbage>)Instance).Count;


        //Assert
        result.Should().Be(items.Count);
    }

    [TestMethod]
    public void CountFromReadOnlyCollectionInterface_WhenEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = ((IReadOnlyCollection<Garbage>)Instance).Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void CountFromReadOnlyCollectionInterface_WhenContainsItems_ReturnNumberOfItems()
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
    public void IsSynchronized_Always_ReturnValueFromInternalCollection()
    {
        //Arrange
        var internalCollection = (ICollection)GetFieldValue<Stack<Garbage>>("_items")!;

        //Act
        var result = ((ICollection)Instance).IsSynchronized;

        //Assert
        result.Should().Be(internalCollection.IsSynchronized);
    }

    [TestMethod]
    public void SyncRoot_Always_ReturnValueFromInternalCollection()
    {
        //Arrange
        var internalCollection = (ICollection)GetFieldValue<Stack<Garbage>>("_items")!;

        //Act
        var result = ((ICollection)Instance).SyncRoot;

        //Assert
        result.Should().Be(internalCollection.SyncRoot);
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
                    OldValues = new List<Garbage> { item }
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
                    OldValues = new List<Garbage> { items.Last() }
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
                    OldValues = new List<Garbage> { item }
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
                    OldValues = new List<Garbage> { items.Last() }
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
        result.Should().Be("Empty ObservableStack<Dummy>");
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
        result.Should().Be("ObservableStack<Dummy> with 3 items");
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
        var internalCollection = GetFieldValue<Stack<Garbage>>("_items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(internalCollection.GetHashCode());
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<ObservableStack<Garbage>>(Dummy, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<ObservableStack<Garbage>>(Dummy, JsonSerializerOptions);
}