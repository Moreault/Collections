namespace Collections.Caching.Tests;

[TestClass]
public class CachingListTests : Tester<CachingList<Dummy>>
{
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
        Instance.Add(Fixture.CreateMany<Dummy>());

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
        Instance.Add(Fixture.CreateMany<Dummy>());
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZeroAndCollectionIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<Dummy>().ToList();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
        Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToZeroAndCollectionIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
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
        var items = Fixture.CreateMany<Dummy>(14).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 7;

        //Assert
        Instance.Should().BeEquivalentTo(new List<Dummy>
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
        var items = Fixture.CreateMany<Dummy>(14).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 7;

        //Assert
        var newItems = Fixture.CreateMany<Dummy>(3).ToList();
        Instance.Add(newItems);
        Instance.Should().BeEquivalentTo(new List<Dummy>
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
        var items = Fixture.CreateMany<Dummy>(14).ToList();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
        Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

        //Act
        Instance.Limit = 7;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
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
                        items[6],
                    }
                }
            });
    }

    [TestMethod]
    public void Constructor_WhenUsingParams_ReturnNewCachingList()
    {
        //Arrange
        var items = Fixture.CreateMany<Dummy>().ToArray();

        //Act
        var result = new CachingList<Dummy>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }
}