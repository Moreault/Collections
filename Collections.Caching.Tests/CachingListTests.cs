namespace Collections.Caching.Tests;

[TestClass]
public class CachingListTests : ObservableListTester<CachingList<Garbage>, Garbage>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        JsonSerializerOptions.WithCachingConverters();
    }

    [TestMethod]
    public void Limit_WhenUnset_ReturnIntMaxValueByDefault()
    {
        //Arrange

        //Act
        var result = Instance.Limit;

        //Assert
        result.Should().Be(int.MaxValue);
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZero_ClearCollection()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<Garbage>());

        //Act
        Instance.Limit = 0;

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZero_PreventAnyNewItemFromBeingAdded()
    {
        //Arrange

        //Act
        Instance.Limit = 0;

        //Assert
        Instance.Add(Dummy.CreateMany<Garbage>());
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZeroAndCollectionIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZeroAndCollectionIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var eventArgs = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_CutFirstHalfOfCollectionOut()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(14).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 7;

        //Assert
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                items[7],
                items[8],
                items[9],
                items[10],
                items[11],
                items[12],
                items[13],
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_TrimDownTheBeginingOfCollectionToThatSizeAutomaticallyWhenAddingNewItems()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(14).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 7;

        //Assert
        var newItems = Dummy.CreateMany<Garbage>(3).ToList();
        Instance.Add(newItems);
        Instance.Should().BeEquivalentTo(new List<Garbage>
            {
                items[10],
                items[11],
                items[12],
                items[13],
                newItems[0],
                newItems[1],
                newItems[2]
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>(14).ToList();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<Garbage>>();
        Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

        //Act
        Instance.Limit = 7;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Garbage>>
            {
                new()
                {
                    OldValues = new List<Garbage>
                    {
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                        items[5],
                        items[6],
                    }
                }
            });
    }

    [TestMethod]
    public void Constructor_WhenUsingParams_ReturnNewCachingList()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var result = new CachingList<Garbage>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }
}