using Newtonsoft.Json.Linq;

namespace Collections.ObservableDictionary.Tests;

[TestClass]
public class ObservableDictionaryTests : Tester<ObservableDictionary<int, Dummy>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Fixture.WithCollectionCustomizations();
        JsonSerializerOptions.WithObservableDictionaryConverters();
    }

    [TestMethod]
    public void ConstructorWithCapacity_Always_ReturnNewInstance()
    {
        //Arrange
        var capacity = Fixture.Create<int>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(capacity);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorWithEqualityComparer_Always_ReturnWithComparer()
    {
        //Arrange
        var comparer = Fixture.Create<EqualityComparer<int>>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(comparer);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorWithCapacityAndComparer_Always_ReturnNew()
    {
        //Arrange
        var capacity = Fixture.Create<int>();
        var comparer = Fixture.Create<EqualityComparer<int>>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(capacity, comparer);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConstructorWithDictionary_Always_ReturnEquivalent()
    {
        //Arrange
        var dictionary = Fixture.Create<Dictionary<int, Dummy>>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(dictionary);

        //Assert
        result.Should().BeEquivalentTo(dictionary);
    }

    [TestMethod]
    public void ConstructorWithDictionaryAndComparer_Always_ReturnEquivalent()
    {
        //Arrange
        var dictionary = Fixture.Create<Dictionary<int, Dummy>>();
        var comparer = Fixture.Create<EqualityComparer<int>>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(dictionary, comparer);

        //Assert
        result.Should().BeEquivalentTo(dictionary);
    }

    [TestMethod]
    public void ConstructorWithEnumerable_Always_ReturnEquivalent()
    {
        //Arrange
        var collection = Fixture.Create<IEnumerable<KeyValuePair<int, Dummy>>>().ToList();

        //Act
        var result = new ObservableDictionary<int, Dummy>(collection);

        //Assert
        result.Should().BeEquivalentTo(collection);
    }

    [TestMethod]
    public void ConstructorWithEnumerableAndComparer_Always_ReturnEquivalent()
    {
        //Arrange
        var collection = Fixture.Create<IEnumerable<KeyValuePair<int, Dummy>>>().ToList();
        var comparer = Fixture.Create<EqualityComparer<int>>();

        //Act
        var result = new ObservableDictionary<int, Dummy>(collection, comparer);

        //Assert
        result.Should().BeEquivalentTo(collection);
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        //Act
        var action = () => Instance[Fixture.Create<int>()];

        //Assert
        action.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void Indexer_WhenThereIsSomethingWithKey_ReturnValueAssociatedWithKey()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();
        var value = Fixture.Create<Dummy>();

        //Act
        Instance[item.Key] = value;

        //Assert
        Instance[item.Key].Should().Be(value);
    }

    [TestMethod]
    public void Indexer_WhenThereIsSomethingWithKey_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();
        var value = Fixture.Create<Dummy>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = value;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new()
                {
                    OldValues = new List<KeyValuePair<int, Dummy>>{ item },
                    NewValues = new List<KeyValuePair<int, Dummy>>{ new(item.Key, value) }
                }
            });
    }

    [TestMethod]
    public void Indexer_WhenPassingTheSameValueToTheSameKey_DoNotTrigger()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Indexer_WhenKeyDoesNotExist_AddNewEntry()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Dummy>
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance[item.Key] = item.Value;

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { NewValues = new List<KeyValuePair<int, Dummy>> { item } }
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>(5).ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.TryGetValue(Fixture.Create<int>());

        //Assert
        result.Should().Be(Result<Dummy>.Failure());
    }

    [TestMethod]
    public void TryGetValue_WhenKeyHasNullValueInCollection_ReturnSuccessWithNull()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var key = Fixture.Create<int>();
        Instance[key] = null!;

        //Act
        var result = Instance.TryGetValue(key);

        //Assert
        result.Should().Be(Result<Dummy>.Success(null!));
    }

    [TestMethod]
    public void TryGetValue_WhenKeyHasValue_ReturnSuccess()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        var result = Instance.TryGetValue(item.Key);

        //Assert
        result.Should().Be(Result<Dummy>.Success(item.Value));
    }

    [TestMethod]
    public void AddKeyValue_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        Instance.Add(item.Key, item.Value);

        //Assert
        Instance[item.Key].Should().Be(item.Value);
    }

    [TestMethod]
    public void AddKeyValue_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(item.Key, item.Value);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new()
            {
                NewValues = new List<KeyValuePair<int, Dummy>>{ item }
            }
        });
    }

    [TestMethod]
    public void AddKeyValuePair_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        Instance.Add(item);

        //Assert
        Instance[item.Key].Should().Be(item.Value);
    }

    [TestMethod]
    public void AddKeyValuePair_WhenKeyDoesNotAlreadyExist_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new()
            {
                NewValues = new List<KeyValuePair<int, Dummy>>{ item }
            }
        });
    }

    [TestMethod]
    public void AddParams_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item1 = Fixture.Create<KeyValuePair<int, Dummy>>();
        var item2 = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        Instance.Add(item1, item2);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Dummy>
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item1 = Fixture.Create<KeyValuePair<int, Dummy>>();
        var item2 = Fixture.Create<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(item1, item2);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new()
                {
                    NewValues = new List<KeyValuePair<int, Dummy>>{ item1, item2 }
                }
            });
    }

    [TestMethod]
    public void AddEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<int, Dummy>> items = null!;

        //Act
        var action = () => Instance.Add(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void AddEnumerable_WhenItemIsEmpty_DoNotModify()
    {
        //Arrange
        var items = new List<KeyValuePair<int, Dummy>>();

        //Act
        Instance.Add(items);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void AddEnumerable_WhenItemIsEmpty_DoNotTrigger()
    {
        //Arrange
        var items = new List<KeyValuePair<int, Dummy>>();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void AddEnumerable_WhenKeyAlreadyExists_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var addedItems = new List<KeyValuePair<int, Dummy>>
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var addedItems = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

        //Act
        Instance.Add(addedItems);

        //Assert
        Instance.Should().BeEquivalentTo(new Dictionary<int, Dummy>
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var addedItems = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Add(addedItems);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new()
                {
                    NewValues = addedItems
                }
            });
    }

    [TestMethod]
    public void Clear_WhenIsAlreadyEmpty_DoNotTrigger()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_RemoveEverything()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Clear();

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = items }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePair_WhenKeyValuePairIsNotInCollection_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        var action = () => Instance.Remove(item);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.RemoveInexistantKeyValue, item.Key, item.Value));
    }

    [TestMethod]
    public void RemoveKeyValuePair_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePair_WhenKeyValuePairIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        var action = () => Instance.TryRemove(item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveKeyValuePair_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
        });
    }

    [TestMethod]
    public void RemoveKey_WhenKeyValuePairIsNotInCollection_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var key = Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(key);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.RemoveInexistantKey, key));
    }

    [TestMethod]
    public void RemoveKey_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
        });
    }

    [TestMethod]
    public void TryRemoveKey_WhenKeyValuePairIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var key = Fixture.Create<int>();

        //Act
        var action = () => Instance.TryRemove(key);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveKey_WhenKeyValuePairIsInCollection_RemoveIt()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
        });
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<KeyValuePair<int, Dummy>, bool> match = null!;

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
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<KeyValuePair<int, Dummy>, bool> match = null!;

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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Key == item.Key && x.Value.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKeyValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => x.Key > 0 && x.Value.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
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
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x == item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x == item.Key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncKey_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => toRemove.Select(y => y.Key).Contains(x));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void RemoveFuncValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<Dummy, bool> match = null!;

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
        var action = () => Instance.Remove((Dummy x) => x == null!);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.Remove(x => x.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void RemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(x => x.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenMatchIsNull_Throw()
    {
        //Arrange
        Func<Dummy, bool> match = null!;

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
        var action = () => Instance.TryRemove((Dummy x) => x == null!);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Value.Description == item.Value.Description);

        //Assert
        Instance.Should().NotContain(item);
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyOneMatch_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var item = items.GetRandom();

        //Act
        Instance.TryRemove(x => x.Description == item.Value.Description);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFuncValue_WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = new List<KeyValuePair<int, Dummy>>
            {
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
                new(Fixture.Create<int>(), Fixture.Build<Dummy>().With(x => x.Description, "Seb").Create()),
            };
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(x => x.Description == "Seb");

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
    }

    [TestMethod]
    public void Contains_WhenItemIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        var result = Instance.Contains(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WhenItemIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        var result = Instance.ContainsKey(item.Key);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ContainsKey_WhenKeyIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
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
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEquivalentTo(Instance);
        result.Should().NotBeSameAs(Instance);
    }

    [TestMethod]
    public void RemoveKeysParams_WhenKeysIsNull_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        int[] keys = null!;

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void RemoveKeysParams_WhenKeysIsEmpty_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var keys = Array.Empty<int>();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName("items");
    }

    [TestMethod]
    public void RemoveKeysParams_WhenKeysAreNotInDictionary_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var keys = Fixture.CreateMany<int>().ToArray();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveAtLeastOneInexistantKey);
    }

    [TestMethod]
    public void RemoveKeysParams_WhenAllKeysAreInDictionary_RemoveAllCorrespondingObjects()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        //Act
        Instance.Remove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeysParams_WhenAllKeysAreInDictionary_TriggerEvent()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeysEnumerable_WhenKeysIsNull_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        IEnumerable<int> keys = null!;

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void RemoveKeysEnumerable_WhenKeysIsEmpty_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var keys = new List<int>();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName("items");
    }

    [TestMethod]
    public void RemoveKeysEnumerable_WhenKeysAreNotInDictionary_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var keys = Fixture.CreateMany<int>().ToList();

        //Act
        var action = () => Instance.Remove(keys);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveAtLeastOneInexistantKey);
    }

    [TestMethod]
    public void RemoveKeysEnumerable_WhenAllKeysAreInDictionary_RemoveAllCorrespondingObjects()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        //Act
        Instance.Remove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeysEnumerable_WhenAllKeysAreInDictionary_TriggerEvent()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenItemsIsNull_Throw()
    {
        //Arrange
        KeyValuePair<int, Dummy>[] items = null!;

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenItemsIsEmpty_Throw()
    {
        //Arrange
        var items = Array.Empty<KeyValuePair<int, Dummy>>();

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenAtLeastOneItemIsNotInCollection_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();

        //Act
        var action = () => Instance.Remove(toRemove.Concat(items).ToArray());

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveAtLeastOneInexistantItem);
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenAllItemsAreInCollection_RemoveThem()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();
        Instance.Add(toRemove);

        //Act
        Instance.Remove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveKeyValuePairParams_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<int, Dummy>> items = null!;

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenItemsIsEmpty_Throw()
    {
        //Arrange
        var items = new List<KeyValuePair<int, Dummy>>();

        //Act
        var action = () => Instance.Remove(items);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenAtLeastOneItemIsNotInCollection_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

        //Act
        var action = () => Instance.Remove(toRemove.Concat(items));

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveAtLeastOneInexistantItem);
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenAllItemsAreInCollection_RemoveThem()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.Remove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void RemoveKeyValuePairEnumerable_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenKeyIsNull_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        int[] keys = null!;

        //Act
        var action = () => Instance.TryRemove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenKeysIsEmpty_DoNotModify()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Array.Empty<int>();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenKeysIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Array.Empty<int>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenKeysAreNotInCollection_DoNotModify()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Fixture.CreateMany<int>().ToArray();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenKeysAreNotInCollection_DoNotTriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Fixture.CreateMany<int>().ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenSomeKeysAreInCollectionsButOthersAreNot_RemoveThoseThatAreInCollection()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).Concat(Fixture.CreateMany<int>()).ToArray();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenSomeKeysAreInCollectionsButOthersAreNot_TriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).Concat(Fixture.CreateMany<int>()).ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenAllKeysAreInCollection_RemoveThemAll()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysParams_WhenAllKeysAreInCollection_TriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenKeyIsNull_Throw()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        IEnumerable<int> keys = null!;

        //Act
        var action = () => Instance.TryRemove(keys);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keys));
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenKeysIsEmpty_DoNotModify()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = new List<int>();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenKeysIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = new List<int>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenKeysAreNotInCollection_DoNotModify()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Fixture.CreateMany<int>().ToList();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenKeysAreNotInCollection_DoNotTriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var keys = Fixture.CreateMany<int>().ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenSomeKeysAreInCollectionsButOthersAreNot_RemoveThoseThatAreInCollection()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).Concat(Fixture.CreateMany<int>()).ToList();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenSomeKeysAreInCollectionsButOthersAreNot_TriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).Concat(Fixture.CreateMany<int>()).ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenAllKeysAreInCollection_RemoveThemAll()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        //Act
        Instance.TryRemove(keys);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TryRemoveKeysEnumerable_WhenAllKeysAreInCollection_TriggerChange()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var keys = toRemove.Select(x => x.Key).ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(keys);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenItemsIsNull_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        KeyValuePair<int, Dummy>[] items = null!;

        //Act
        var action = () => Instance.TryRemove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenItemsIsEmpty_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Array.Empty<KeyValuePair<int, Dummy>>();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenItemsIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Array.Empty<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenItemsIsNotInCollection_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenItemsIsNotInCollection_DoNotTriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenSomeItemsAreNotInCollection_RemoveThoseThatAre()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.TryRemove(toRemove);

        var items = toRemove.Concat(Fixture.CreateMany<KeyValuePair<int, Dummy>>()).ToArray();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenSomeItemsAreNotInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenAllItemsAreInCollection_RemoveThem()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairParams_WhenAllItemsAreInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToArray();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        IEnumerable<KeyValuePair<int, Dummy>> items = null!;

        //Act
        var action = () => Instance.TryRemove(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenItemsIsEmpty_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = new List<KeyValuePair<int, Dummy>>();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenItemsIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = new List<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenItemsIsNotInCollection_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenItemsIsNotInCollection_DoNotTriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(items);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenSomeItemsAreNotInCollection_RemoveThoseThatAre()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.TryRemove(toRemove);

        var items = toRemove.Concat(Fixture.CreateMany<KeyValuePair<int, Dummy>>()).ToList();

        //Act
        Instance.TryRemove(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenSomeItemsAreNotInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenAllItemsAreInCollection_RemoveThem()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveKeyValuePairEnumerable_WhenAllItemsAreInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(toRemove);

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = toRemove }
        });
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsNotInCollection_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = Fixture.Create<int>();

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsNotInCollection_DoNotTrigger()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = Fixture.Create<int>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = Fixture.Create<int>();

        //Act
        var result = ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsInCollection_RemoveAssociatedItem()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = content.GetRandom().Key;

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        Instance.Should().BeEquivalentTo(content.Where(x => x.Key != key));
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = content.GetRandom().Key;

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { content.Single(x => x.Key == key) } }
        });
    }

    [TestMethod]
    public void RemoveKeyFromDictionary_WhenKeyIsInCollection_ReturnTrue()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var key = content.GetRandom().Key;

        //Act
        var result = ((IDictionary<int, Dummy>)Instance).Remove(key);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsNotInCollection_DoNotModify()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsNotInCollection_DoNotTrigger()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = Fixture.Create<KeyValuePair<int, Dummy>>();

        //Act
        var result = ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsInCollection_RemoveAssociatedItem()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = content.GetRandom();

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        Instance.Should().BeEquivalentTo(content.Where(x => !x.Equals(item)));
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsInCollection_TriggerChange()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = content.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
        {
            new() { OldValues = new List<KeyValuePair<int, Dummy>> { content.Single(x => x.Equals(item)) } }
        });
    }

    [TestMethod]
    public void RemoveKeyValuePairFromDictionary_WhenKeyIsInCollection_ReturnTrue()
    {
        //Arrange
        var content = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(content);

        var item = content.GetRandom();

        //Act
        var result = ((IDictionary<int, Dummy>)Instance).Remove(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ToString_WhenEmpty_ReturnTypeAndZero()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be("Empty ObservableDictionary<Int32, Dummy>");
    }

    [TestMethod]
    public void ToString_WhenNotEmpty_ReturnTypeAndCount()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"ObservableDictionary<Int32, Dummy> with {items.Count} items");
    }

    [TestMethod]
    public void CopyTo_Always_CopyToArray()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var array = new KeyValuePair<int, Dummy>[items.Count];

        //Act
        ((ICollection<KeyValuePair<int, Dummy>>)Instance).CopyTo(array, 0);

        //Assert
        array.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void GetHashCode_Always_ReturnFromInternalCollection()
    {
        //Arrange
        var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
        Instance.Add(items);

        var internalCollection = GetFieldValue<IDictionary<int, Dummy>>("_items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(internalCollection.GetHashCode());
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<ObservableDictionary<int, Dummy>>(Fixture, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_EnumeratesAllItems() => Ensure.EnumeratesAllItems<ObservableDictionary<int, Dummy>, KeyValuePair<int, Dummy>>(Fixture);
}