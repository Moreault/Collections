namespace Collections.ObservableDictionary.Tests;

[TestClass]
public class ObservableDictionaryTests
{
    [TestClass]
    public class Keys : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenEmpty_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.Keys;

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItems_ReturnAllKeys()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Keys;

            //Assert
            result.Should().BeEquivalentTo(items.Select(x => x.Key));
        }
    }

    [TestClass]
    public class Values : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenEmpty_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.Values;

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItems_ReturnAllValues()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Values;

            //Assert
            result.Should().BeEquivalentTo(items.Select(x => x.Value));
        }
    }

    [TestClass]
    public class Indexer : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenThereIsNothingWithKey_Throw()
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
        public void WhenThereIsSomethingWithKey_ReturnValueAssociatedWithKey()
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
        public void WhenThereIsSomethingWithKey_SwapValues()
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
        public void WhenThereIsSomethingWithKey_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();
            var value = Fixture.Create<Dummy>();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
        public void WhenPassingTheSameValueToTheSameKey_DoNotTrigger()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance[item.Key] = item.Value;

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenKeyDoesNotExist_AddNewEntry()
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
        public void WhenKeyDoesNotExist_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<KeyValuePair<int, Dummy>>();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance[item.Key] = item.Value;

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { NewValues = new List<KeyValuePair<int, Dummy>> { item } }
            });
        }
    }

    [TestClass]
    public class Count : Tester<ObservableDictionary<int, Dummy>>
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
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>(5).ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Count;

            //Assert
            result.Should().Be(5);
        }
    }

    [TestClass]
    public class IsReadOnly : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void Always_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.IsReadOnly;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class TryGetValue : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyIsNotInCollection_ReturnFailure()
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
        public void WhenKeyHasNullValueInCollection_ReturnSuccessWithNull()
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
        public void WhenKeyHasValue_ReturnSuccess()
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
    }

    [TestClass]
    public class Add_Key_Value : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyAlreadyExists_Throw()
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
        public void WhenKeyDoesNotAlreadyExist_Add()
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
        public void WhenKeyDoesNotAlreadyExist_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<KeyValuePair<int, Dummy>>();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
    }

    [TestClass]
    public class Add_KeyValuePair : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyAlreadyExists_Throw()
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
        public void WhenKeyDoesNotAlreadyExist_Add()
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
        public void WhenKeyDoesNotAlreadyExist_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<KeyValuePair<int, Dummy>>();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
    }

    [TestClass]
    public class Add_Params : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyAlreadyExists_Throw()
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
        public void WhenKeyDoesNotAlreadyExist_Add()
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
        public void WhenKeyDoesNotAlreadyExist_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item1 = Fixture.Create<KeyValuePair<int, Dummy>>();
            var item2 = Fixture.Create<KeyValuePair<int, Dummy>>();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
    }

    [TestClass]
    public class Add_Enumerable : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyAlreadyExists_Throw()
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
        public void WhenKeyDoesNotAlreadyExist_Add()
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
        public void WhenKeyDoesNotAlreadyExist_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var addedItems = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
    }

    [TestClass]
    public class Clear : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenIsAlreadyEmpty_DoNotTrigger()
        {
            //Arrange
            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItems_RemoveEverything()
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
        public void WhenContainsItems_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Clear();

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { OldValues = items }
            });
        }
    }

    [TestClass]
    public class Remove_KeyValuePair : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyValuePairIsNotInCollection_Throw()
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
        public void WhenKeyValuePairIsInCollection_RemoveIt()
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
        public void WhenKeyValuePairIsInCollection_Trigger()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Remove(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
            });
        }
    }

    [TestClass]
    public class TryRemove_KeyValuePair : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyValuePairIsNotInCollection_DoNotThrow()
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
        public void WhenKeyValuePairIsInCollection_RemoveIt()
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
        public void WhenKeyValuePairIsInCollection_Trigger()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryRemove(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
            });
        }
    }

    [TestClass]
    public class Remove_Key : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyValuePairIsNotInCollection_Throw()
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
        public void WhenKeyValuePairIsInCollection_RemoveIt()
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
        public void WhenKeyValuePairIsInCollection_Trigger()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Remove(item.Key);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
            });
        }
    }

    [TestClass]
    public class TryRemove_Key : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyValuePairIsNotInCollection_DoNotThrow()
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
        public void WhenKeyValuePairIsInCollection_RemoveIt()
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
        public void WhenKeyValuePairIsInCollection_Trigger()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryRemove(item.Key);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new() { OldValues = new List<KeyValuePair<int, Dummy>> { item } }
            });
        }
    }

    [TestClass]
    public class Remove_Func_KeyValue : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<KeyValuePair<int, Dummy>, bool> match = null!;

            //Act
            var action = () => Instance.Remove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Remove(x => x.Key < 0 && x.Value == null!);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
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
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
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
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
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
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Remove(x => x.Key > 0 && x.Value.Description == "Seb");

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class TryRemove_Func_KeyValue : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<KeyValuePair<int, Dummy>, bool> match = null!;

            //Act
            var action = () => Instance.TryRemove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryRemove(x => x.Key < 0 && x.Value == null!);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
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
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

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
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
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
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
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
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryRemove(x => x.Key > 0 && x.Value.Description == "Seb");

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class Remove_Func_Key : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<int, bool> match = null!;

            //Act
            var action = () => Instance.Remove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Remove(x => x.Key < 0);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            //Act
            Instance.Remove(x => x.Key == item.Key);

            //Assert
            Instance.Should().NotContain(item);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var item = items.GetRandom();

            //Act
            Instance.Remove(x => x.Key == item.Key);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(toRemove);

            //Act
            Instance.Remove(x => toRemove.Select(y => y.Key).Contains(x.Key));

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(toRemove);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Remove(x => toRemove.Select(y => y.Key).Contains(x.Key));

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class TryRemove_Func_Key : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<int, bool> match = null!;

            //Act
            var action = () => Instance.TryRemove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryRemove(x => x.Key < 0);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            //Act
            Instance.TryRemove(x => x.Key == item.Key);

            //Assert
            Instance.Should().NotContain(item);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var item = items.GetRandom();

            //Act
            Instance.TryRemove(x => x.Key == item.Key);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(toRemove);

            //Act
            Instance.TryRemove(x => toRemove.Select(y => y.Key).Contains(x.Key));

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var toRemove = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(toRemove);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryRemove(x => toRemove.Select(y => y.Key).Contains(x.Key));

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class Remove_Func_Value : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> match = null!;

            //Act
            var action = () => Instance.Remove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.Remove(x => x.Value == null!);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(Exceptions.RemoveWithNonInexistantPredicate);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var item = items.GetRandom();

            //Act
            Instance.Remove(x => x.Value.Description == item.Value.Description);

            //Assert
            Instance.Should().NotContain(item);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var item = items.GetRandom();

            //Act
            Instance.Remove(x => x.Value.Description == item.Value.Description);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
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
            Instance.Remove(x => x.Value.Description == "Seb");

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
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
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.Remove(x => x.Value.Description == "Seb");

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class TryRemove_Func_Value : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenMatchIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> match = null!;

            //Act
            var action = () => Instance.TryRemove(match);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(match));
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.TryRemove(x => x.Value == null!);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenPredicateHasExactlyOneMatch_RemoveThatOneMatch()
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
        public void WhenPredicateHasExactlyOneMatch_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            var item = items.GetRandom();

            //Act
            Instance.TryRemove(x => x.Value.Description == item.Value.Description);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = new List<KeyValuePair<int, Dummy>>{item}}
            });
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_RemoveThatOneMatch()
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
            Instance.TryRemove(x => x.Value.Description == "Seb");

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenPredicateHasExactlyMultipleMatches_TriggerEvent()
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
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.TryRemove(x => x.Value.Description == "Seb");

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<KeyValuePair<int, Dummy>>>
            {
                new(){OldValues = toRemove}
            });
        }
    }

    [TestClass]
    public class Contains : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNotInCollection_ReturnFalse()
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
        public void WhenItemIsInCollection_ReturnTrue()
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
    }

    [TestClass]
    public class ContainsKey : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenKeyIsNotInCollection_ReturnFalse()
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
        public void WhenKeyIsInCollection_ReturnTrue()
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
    }

    [TestClass]
    public class Copy : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void Always_ReturnCopy()
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
    }

    [TestClass]
    public class EqualsMethod : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            ObservableDictionary<int, Dummy> other = null!;

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsTheSameReference_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Equals(Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherContainsOneDifferentItem_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var other = Instance.Copy();
            other.Add(Fixture.Create<KeyValuePair<int, Dummy>>());

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentReferenceButContainsSameItems_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            var other = Instance.Copy();

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<ObservableDictionary<int, Dummy>>
    {
        [TestMethod]
        public void WhenEmpty_ReturnTypeAndZero()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("Empty ObservableDictionary<Int32, Dummy>");
        }

        [TestMethod]
        public void WhenNotEmpty_ReturnTypeAndCount()
        {
            //Arrange
            var items = Fixture.CreateMany<KeyValuePair<int, Dummy>>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be($"ObservableDictionary<Int32, Dummy> with {items.Count} items");
        }
    }
}