namespace Collections.Caching.Tests;

[TestClass]
public class CachingDictionaryTests : ToolBX.Collections.UnitTesting.Tester<CachingDictionary<int, Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
        JsonSerializerOptions.WithCachingConverters();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZeroAndContainsItems_ClearAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = -Dummy.Create<int>();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZeroAndContainsItems_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = -Dummy.Create<int>();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new()
            {
                OldValues = items
            }
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZeroAndEmpty_DoNothing()
    {
        //Arrange

        //Act
        Instance.Limit = -Dummy.Create<int>();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZeroAndEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = -Dummy.Create<int>();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsZeroAndContainsItems_ClearAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 0;

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsLessThanZero_PreventAddingItemsAfter()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = -Dummy.Create<int>();

        //Assert
        Instance.Add(Dummy.CreateMany<KeyValuePair<int, Garbage>>());
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsZeroAndContainsItems_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 0;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new()
            {
                OldValues = items
            }
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_RemoveOlderHalf()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 5;

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[5].Key, items[5].Value },
            { items[6].Key, items[6].Value },
            { items[7].Key, items[7].Value },
            { items[8].Key, items[8].Value },
            { items[9].Key, items[9].Value },
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 5;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new()
            {
                OldValues = new List<KeyValuePair<int, Garbage>>
                {
                    items[0],
                    items[1],
                    items[2],
                    items[3],
                    items[4]
                }
            }
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToHalfCollection_PreventAddingMoreItemsPastLimit()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 5;

        //Assert
        var newItem = Dummy.Create<KeyValuePair<int, Garbage>>();
        Instance.Add(newItem);
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[6].Key, items[6].Value },
            { items[7].Key, items[7].Value },
            { items[8].Key, items[8].Value },
            { items[9].Key, items[9].Value },
            { newItem.Key, newItem.Value },
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToCollectionSize_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 10;

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToCollectionSize_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 10;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToCollectionSize_NextItemAddedRemovesOldest()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        var newItem = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        Instance.Limit = 10;

        //Assert
        Instance.Add(newItem);
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[1].Key, items[1].Value },
            { items[2].Key, items[2].Value },
            { items[3].Key, items[3].Value },
            { items[4].Key, items[4].Value },
            { items[5].Key, items[5].Value },
            { items[6].Key, items[6].Value },
            { items[7].Key, items[7].Value },
            { items[8].Key, items[8].Value },
            { items[9].Key, items[9].Value },
            { newItem.Key, newItem.Value },
        });
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToMoreThanCollectionSize_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 15;

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToMoreThanCollectionSize_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Limit = 15;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Limit_WhenLimitIsSetToMoreThanCollectionSize_NextItemIsAddedWithoutRemovingAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.Limit = 15;

        //Assert
        var newItem = Dummy.Create<KeyValuePair<int, Garbage>>();
        Instance.Add(newItem);
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[0].Key, items[0].Value },
            { items[1].Key, items[1].Value },
            { items[2].Key, items[2].Value },
            { items[3].Key, items[3].Value },
            { items[4].Key, items[4].Value },
            { items[5].Key, items[5].Value },
            { items[6].Key, items[6].Value },
            { items[7].Key, items[7].Value },
            { items[8].Key, items[8].Value },
            { items[9].Key, items[9].Value },
            { newItem.Key, newItem.Value },
        });
    }

    [TestMethod]
    public void TrimStartDownTo_WhenEmpty_DoNothing()
    {
        //Arrange

        //Act
        Instance.TrimStartDownTo(Dummy.Create<int>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(Dummy.Create<int>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItems_RemoveOldestItems()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.TrimStartDownTo(5);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[5].Key, items[5].Value },
            { items[6].Key, items[6].Value },
            { items[7].Key, items[7].Value },
            { items[8].Key, items[8].Value },
            { items[9].Key, items[9].Value },
        });
    }

    [TestMethod]
    public void TrimEndDownTo_WhenEmpty_DoNothing()
    {
        //Arrange

        //Act
        Instance.TrimEndDownTo(Dummy.Create<int>());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(Dummy.Create<int>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItems_RemoveNewestItems()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(10).ToList();
        Instance.Add(items);

        //Act
        Instance.TrimEndDownTo(5);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
        {
            { items[0].Key, items[0].Value },
            { items[1].Key, items[1].Value },
            { items[2].Key, items[2].Value },
            { items[3].Key, items[3].Value },
            { items[4].Key, items[4].Value },
        });
    }

    [TestMethod]
    public void Keys_WhenEmpty_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Instance.Keys;

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Keys_WhenContainsItems_ReturnAllKeys()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Keys;

        //Assert
        result.Should().BeEquivalentTo(items.Select(x => x.Key));
    }

    [TestMethod]
    public void Values_WhenEmpty_ReturnEmpty()
    {
        //Arrange

        //Act
        var result = Instance.Values;

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Values_WhenContainsItems_ReturnAllValues()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Values;

        //Assert
        result.Should().BeEquivalentTo(items.Select(x => x.Value));
    }

    [TestMethod]
    public void Indexer_WhenThereIsNothingWithKey_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var action = () => Instance[Dummy.Create<int>()];

        //Assert
        action.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void Indexer_WhenThereIsSomethingWithKey_ReturnValueAssociatedWithKey()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance[item.Key];

        //Assert
        result.Should().Be(item.Value);
    }

    [TestMethod]
    public void Indexer_WhenThereIsSomethingWithKey_SwapValues()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();
        var value = Dummy.Create<Garbage>();

        //Act
        Instance[item.Key] = value;

        //Assert
        Instance[item.Key].Should().Be(value);
    }

    [TestMethod]
    public void Indexer_WhenThereIsSomethingWithKey_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();
        var value = Dummy.Create<Garbage>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = value;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new()
                {
                    OldValues = new List<KeyValuePair<int, Garbage>>{ item },
                    NewValues = new List<KeyValuePair<int, Garbage>>{ new(item.Key, value) }
                }
            });
    }

    [TestMethod]
    public void Indexer_WhenPassingTheSameValueToTheSameKey_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Indexer_WhenKeyDoesNotExist_AddNewEntry()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
            {
                { items[0].Key, items[0].Value },
                { items[1].Key, items[1].Value },
                { items[2].Key, items[2].Value },
                { item.Key, item.Value }
            });
    }

    [TestMethod]
    public void Indexer_WhenKeyDoesNotExist_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new() { NewValues = new List<KeyValuePair<int, Garbage>> { item } }
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
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>(5).ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Count;

        //Assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void IsReadOnly_Always_ReturnFalse()
    {
        //Arrange

        //Act
        var result = Instance.IsReadOnly;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void TryGetValue_WhenKeyIsNotInCollection_ReturnFailure()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.TryGetValue(Dummy.Create<int>());

        //Assert
        result.Should().Be(Result<Garbage>.Failure());
    }

    [TestMethod]
    public void TryGetValue_WhenKeyHasNullValueInCollection_ReturnSuccessWithNull()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var key = Dummy.Create<int>();
        Instance[key] = null!;

        //Act
        var result = Instance.TryGetValue(key);

        //Assert
        result.Should().Be(Result<Garbage>.Success(null!));
    }

    [TestMethod]
    public void TryGetValue_WhenKeyHasValue_ReturnSuccess()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.TryGetValue(item.Key);

        //Assert
        result.Should().Be(Result<Garbage>.Success(item.Value));
    }

    [TestMethod]
    public void AddKeyValue_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var action = () => Instance.Add(item.Key, item.Value);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddKeyValue_WhenKeyDoesNotAlreadyExist_Add()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        Instance.Add(item.Key, item.Value);

        //Assert
        Instance[item.Key].Should().Be(item.Value);
    }

    [TestMethod]
    public void AddKeyValue_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Add(item.Key, item.Value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new()
            {
                NewValues = new List<KeyValuePair<int, Garbage>>{ item }
            }
        });
    }

    [TestMethod]
    public void AddKeyValuePair_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var action = () => Instance.Add(item);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddKeyValuePair_WhenKeyDoesNotAlreadyExist_Add()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        Instance.Add(item);

        //Assert
        Instance[item.Key].Should().Be(item.Value);
    }

    [TestMethod]
    public void AddKeyValuePair_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Add(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new()
            {
                NewValues = new List<KeyValuePair<int, Garbage>>{ item }
            }
        });
    }

    [TestMethod]
    public void AddParams_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item1 = items.GetRandom();
        var item2 = items.GetRandom();

        //Act
        var action = () => Instance.Add(item1, item2);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddParams_WhenKeyDoesNotAlreadyExist_Add()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item1 = Dummy.Create<KeyValuePair<int, Garbage>>();
        var item2 = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        Instance.Add(item1, item2);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
            {
                { items[0].Key, items[0].Value },
                { items[1].Key, items[1].Value },
                { items[2].Key, items[2].Value },
                { item1.Key, item1.Value },
                { item2.Key, item2.Value },
            });
    }

    [TestMethod]
    public void AddParams_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item1 = Dummy.Create<KeyValuePair<int, Garbage>>();
        var item2 = Dummy.Create<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Add(item1, item2);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new()
                {
                    NewValues = new List<KeyValuePair<int, Garbage>>{ item1, item2 }
                }
            });
    }

    [TestMethod]
    public void AddParams_WhenItemsContainsDuplicateKeys_Throw()
    {
        //Arrange
        var key = Dummy.Create<int>();
        var items = new KeyValuePair<int, Garbage>[]
        {
            new(key, Dummy.Create<Garbage>()),
            new(key, Dummy.Create<Garbage>()),
            new(key, Dummy.Create<Garbage>())
        };

        //Act
        var action = () => Instance.Add(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<int, Garbage>> items = null!;

        //Act
        var action = () => Instance.Add(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void AddEnumerable_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var addedItems = new List<KeyValuePair<int, Garbage>>
            {
                items.GetRandom(),
                items.GetRandom(),
            };

        //Act
        var action = () => Instance.Add(addedItems);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddEnumerable_WhenKeyDoesNotAlreadyExist_Add()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var addedItems = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();

        //Act
        Instance.Add(addedItems);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Garbage>
            {
                { items[0].Key, items[0].Value },
                { items[1].Key, items[1].Value },
                { items[2].Key, items[2].Value },
                { addedItems[0].Key, addedItems[0].Value },
                { addedItems[1].Key, addedItems[1].Value },
                { addedItems[2].Key, addedItems[2].Value },
            });
    }

    [TestMethod]
    public void AddEnumerable_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var addedItems = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Add(addedItems);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new()
                {
                    NewValues = addedItems
                }
            });
    }

    [TestMethod]
    public void AddEnumerable_WhenItemsContainsDuplicateKeys_Throw()
    {
        //Arrange
        var key = Dummy.Create<int>();
        var items = new List<KeyValuePair<int, Garbage>>
        {
            {new(key, Dummy.Create<Garbage>())},
            {new(key, Dummy.Create<Garbage>())},
            {new(key, Dummy.Create<Garbage>())},
        };

        //Act
        var action = () => Instance.Add(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Clear_WhenIsAlreadyEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_RemoveEverything()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        Instance.Clear();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = items }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePair_WhenKeyValuePairIsNotInCollection_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        var action = () => Instance.Remove(item);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveKeyValuePair_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(item);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void RemoveKeyValuePair_WhenKeyValuePairIsInCollection_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Remove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Garbage>> { item } }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePair_WhenKeyValuePairIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        var action = () => Instance.TryRemove(item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveKeyValuePair_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(item);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveKeyValuePair_WhenKeyValuePairIsInCollection_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Garbage>> { item } }
        });
    }

    [TestMethod]
    public void RemoveKey_WhenKeyValuePairIsNotInCollection_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var key = Dummy.Create<int>();

        //Act
        var action = () => Instance.Remove(key);

        //Assert
        action.Should().Throw<InvalidOperationException>()/*.WithMessage(string.Format(Exceptions.RemoveInexistantKey, key))*/;
    }

    [TestMethod]
    public void RemoveKey_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(item.Key);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void RemoveKey_WhenKeyValuePairIsInCollection_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Remove(item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Garbage>> { item } }
        });
    }

    [TestMethod]
    public void TryRemoveKey_WhenKeyValuePairIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var key = Dummy.Create<int>();

        //Act
        var action = () => Instance.TryRemove(key);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveKey_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(item.Key);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveKey_WhenKeyValuePairIsInCollection_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Garbage>> { item } }
        });
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<KeyValuePair<int, Garbage>, bool> match = null!;

        //Act
        var action = () => Instance.Remove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasNoMatch_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Remove(x => x.Key < 0 && x.Value == null!);

        //Assert
        action.Should().Throw<InvalidOperationException>()/*.WithMessage(Exceptions.RemoveWithNonInexistantPredicate)*/;
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        //Act
        Instance.Remove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<KeyValuePair<int, Garbage>, bool> match = null!;

        //Act
        var action = () => Instance.TryRemove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasNoMatch_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.TryRemove(x => x.Key < 0 && x.Value == null!);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void RemoveFuncKey_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<int, bool> match = null!;

        //Act
        var action = () => Instance.Remove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasNoMatch_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Remove(x => x < 0);

        //Assert
        action.Should().Throw<InvalidOperationException>()/*.WithMessage(Exceptions.RemoveWithNonInexistantPredicate)*/;
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x == item.Key);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x == item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.Remove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<int, bool> match = null!;

        //Act
        var action = () => Instance.TryRemove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasNoMatch_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.TryRemove(x => x < 0);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x == item.Key);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x == item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void RemoveFuncValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<Garbage, bool> match = null!;

        //Act
        var action = () => Instance.Remove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasNoMatch_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.Remove((Garbage x) => x == null!);

        //Assert
        action.Should().Throw<InvalidOperationException>()/*.WithMessage(Exceptions.RemoveWithNonInexistantPredicate)*/;
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Description == item.Value.Description);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        //Act
        Instance.Remove(x => x.Description == "Seb");

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => x.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<Garbage, bool> match = null!;

        //Act
        var action = () => Instance.TryRemove(match);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasNoMatch_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.TryRemove((Garbage x) => x == null!);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Description == item.Value.Description);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Garbage>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(x => x.Description == "Seb");

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Garbage>>
            {
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
                new(Dummy.Create<int>(), Dummy.Build<Garbage>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => x.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void Contains_WhenItemIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WhenItemIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ContainsKey_WhenKeyIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<KeyValuePair<int, Garbage>>();

        //Act
        var result = Instance.ContainsKey(item.Key);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsKey_WhenKeyIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.ContainsKey(item.Key);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Copy_Always_ReturnCopy()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEquivalentTo(Instance);
        result.Should().NotBeSameAs(Instance);
    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsNotInDictionary_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsNotInDictionary_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<int>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsNotInDictionary_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        var result = ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<int>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsInDictionary_RemoveItemWithKey()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(toRemove.Key);

        //Assert
        Instance.Should().BeEquivalentTo(items.Where(x => x.Key != toRemove.Key));

    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsInDictionary_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        //Act
        var result = ((IDictionary<int, Garbage>)Instance).Remove(toRemove.Key);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void RemoveKeyFromDictionaryInterface_WhenKeyIsInDictionary_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(toRemove.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new(){OldValues = new List<KeyValuePair<int, Garbage>>{toRemove}}
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenDoesNotContainItem_DoNotModifyCollection()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<KeyValuePair<int, Garbage>>());

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenDoesNotContainItem_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<KeyValuePair<int, Garbage>>());

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenDoesNotContainItem_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(Dummy.Create<KeyValuePair<int, Garbage>>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenContainsItem_RemoveFromCollection()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(items.Where(x => x.Key != toRemove.Key));
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenContainsItem_RemoveTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        //Act
        var result = ((IDictionary<int, Garbage>)Instance).Remove(toRemove);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromCollection_WhenContainsItem_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Garbage>)Instance).Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new(){OldValues = new List<KeyValuePair<int, Garbage>>{toRemove}}
        });
    }

    [TestMethod]
    public void TryRemoveParamsKeys_WhenKeysNull_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        int[] keys = null!;

        //Act
        var action = () => Instance.TryRemove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void TryRemoveParamsKeys_WhenKeysEmpty_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = Array.Empty<int>();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveParamsKeys_WhenKeysEmpty_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = Array.Empty<int>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveParamsKeys_WhenKeysNotEmpty_RemoveKeys()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveParamsKeys_WhenKeysNotEmpty_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveEnumerableKeys_WhenKeysNull_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        List<int> keys = null!;

        //Act
        var action = () => Instance.TryRemove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void TryRemoveEnumerableKeys_WhenKeysEmpty_DoNothing()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = new List<int>();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveEnumerableKeys_WhenKeysEmpty_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = new List<int>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveEnumerableKeys_WhenKeysNotEmpty_RemoveKeys()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveEnumerableKeys_WhenKeysNotEmpty_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveParamsKeys_WhenKeysNull_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        int[] keys = null!;

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void RemoveParamsKeys_WhenKeysEmpty_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = Array.Empty<int>();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveParamsKeys_WhenKeysNotEmpty_RemoveKeys()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        //Act
        Instance.Remove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveParamsKeys_WhenKeysNotEmpty_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveEnumerableKeys_WhenKeysNull_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        List<int> keys = null!;

        //Act
        var action = () => Instance.TryRemove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void RemoveEnumerableKeys_WhenKeysEmpty_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var keys = new List<int>();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveEnumerableKeys_WhenKeysNotEmpty_RemoveKeys()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        //Act
        Instance.Remove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveEnumerableKeys_WhenKeysNotEmpty_Trigger()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveSingleKeyValuePair_WhenItemIsNotInCollection_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var action = () => Instance.Remove(Dummy.Create<KeyValuePair<int, Garbage>>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveSingleKeyValuePair_WhenItemIsInCollection_Remove()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.Remove(item);

        //Assert
        Instance.Should().BeEquivalentTo(items.Where(x => !x.Equals(item)));
    }

    [TestMethod]
    public void RemoveSingleKeyValuePair_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Garbage>> { item } }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenIsNull_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        KeyValuePair<int, Garbage>[] items = null!;

        //Act
        var action = () => Instance.TryRemove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenIsEmpty_DoNothing()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = Array.Empty<KeyValuePair<int, Garbage>>();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = Array.Empty<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenIsNotEmpty_RemoveFromCollection()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToArray();
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToArray();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenIsNull_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        IEnumerable<KeyValuePair<int, Garbage>> items = null!;

        //Act
        var action = () => Instance.TryRemove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenIsEmpty_DoNothing()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = new List<KeyValuePair<int, Garbage>>();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = new List<KeyValuePair<int, Garbage>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenIsNotEmpty_RemoveFromCollection()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenIsNull_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        KeyValuePair<int, Garbage>[] items = null!;

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenIsEmpty_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = Array.Empty<KeyValuePair<int, Garbage>>();

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentException>("items should not be empty");
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenIsNotEmpty_RemoveFromCollection()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToArray();
        Instance.Add(toRemove);

        //Act
        Instance.Remove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToArray();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenIsNull_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        IEnumerable<KeyValuePair<int, Garbage>> items = null!;

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenIsEmpty_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var items = new List<KeyValuePair<int, Garbage>>();

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentException>("items should not be empty");
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenIsNotEmpty_RemoveFromCollection()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.Remove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenIsNotEmpty_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(content);

        var toRemove = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Garbage>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void CopyTo_Always_CopyToArray()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var array = new KeyValuePair<int, Garbage>[items.Count];

        //Act
        ((ICollection<KeyValuePair<int, Garbage>>)Instance).CopyTo(array, 0);

        //Assert
        array.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void ToString_WhenEmpty_ReturnTypeAndZero()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be("Empty CachingDictionary<Int32, Dummy>");
    }

    [TestMethod]
    public void ToString_WhenNotEmpty_ReturnTypeAndCount()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"CachingDictionary<Int32, Dummy> with {items.Count} items");
    }

    [TestMethod]
    public void GetHashCode_Always_ReturnFromInternalCollection()
    {
        //Arrange
        var items = Dummy.CreateMany<KeyValuePair<int, Garbage>>().ToList();
        Instance.Add(items);

        var internalCollection = GetFieldValue<CachingList<KeyValuePair<int, Garbage>>>("_items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(internalCollection.GetHashCode());
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<CachingDictionary<int, Garbage>>(Dummy, JsonSerializerOptions);
}