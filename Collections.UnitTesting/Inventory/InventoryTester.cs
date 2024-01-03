using Exceptions = ToolBX.Collections.Inventory.Resources.Exceptions;

namespace ToolBX.Collections.UnitTesting.Inventory;

public abstract class InventoryTester<TInventory> : Tester<TInventory> where TInventory : Inventory<DummyItem>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Fixture.WithCollectionCustomizations();
    }

    [TestMethod]
    public void AddItem_WhenItemIsNull_AddStackOfNull()
    {
        //Arrange
        DummyItem item = null!;
        var quantity = Fixture.Create<int>();

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().Contain(new Entry<DummyItem>(item, quantity));
    }

    [TestMethod]
    public void AddItem_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        const int quantity = 0;

        //Act
        var action = () => Instance.Add(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void AddItem_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Add(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsLesserThanStackSize_AddOneStackOfItemOfSpecifiedQuantity()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<DummyItem>>
            {
                new(item, quantity)
            });
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsLesserThanStackSize_TriggerEvent()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        var eventsArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

        //Act
        Instance.Add(item, quantity);

        //Assert
        eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>>
                    {
                        new(item, quantity)
                    }
                }
            });
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.Add(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddPredicate_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Add(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void AddPredicate_WhenQuantityIsZero_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        const int quantity = 0;

        //Act
        var action = () => Instance.Add(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void AddPredicate_WhenNoExistingEntryCorrespondsToPredicate_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = Fixture.Create<int>();

        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        //Act
        var action = () => Instance.Add(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseThereIsNoMatch, predicate));
    }

    [TestMethod]
    public void AddPredicate_WhenOneEntryCorrespondsToPredicateAndQuantityIsNotBusted_AddToExistingEntry()
    {
        //Arrange
        var quantity = Fixture.Create<int>();

        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var existingEntryIndex = entries.GetRandomIndex();
        var existingEntry = entries[existingEntryIndex];
        var previousQuantity = existingEntry.Quantity;

        Func<DummyItem, bool> predicate = x => x.Id == existingEntry.Item.Id;

        var expected = entries.ToList();
        expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = previousQuantity + quantity };

        //Act
        Instance.Add(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddPredicate_Always_TriggerEvent()
    {
        //Arrange
        var quantity = Fixture.Create<int>();

        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var existingEntryIndex = entries.GetRandomIndex();
        var existingEntry = entries[existingEntryIndex];
        var previousQuantity = existingEntry.Quantity;

        Func<DummyItem, bool> predicate = x => x.Id == existingEntry.Item.Id;

        var expected = entries.ToList();
        expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = previousQuantity + quantity };

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>>
                    {
                        new(existingEntry.Item, quantity)
                    }

                }
            });
    }

    [TestMethod]
    public void RemoveItem_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = 0;

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void RemoveItem_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void RemoveItem_WhenThereIsNoOccurenceOfItemInCollection_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, item));
    }

    [TestMethod]
    public void RemoveItem_WhenThereIsOneOccurenceOfItemInCollection_SubtractQuantityFromOccurence()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 9;

        Instance.Add(item, 15);

        //Act
        Instance.Remove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<DummyItem>>
            {
                new(item, 6)
            }));
    }

    [TestMethod]
    public void RemoveItem_WhenThereIsOneOccurenceOfItemInCollectionAndQuantityRemovedIsEqualToOwned_RemoveEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 15;

        Instance.Add(item, 15);

        //Act
        Instance.Remove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void RemoveItem_WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 16;

        Instance.Add(item, 15);

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item, 16, 15));
    }

    [TestMethod]
    public void TryRemoveItem_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = 0;

        //Act
        var action = () => Instance.TryRemove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void TryRemoveItem_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.TryRemove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsNoOccurenceOfItemInCollection_DoNotModify()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        Instance.TryRemove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsNoOccurenceOfItemInCollection_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(item, quantity);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsNoOccurenceOfItemInCollection_ReturnResultWithZeroItemsRemoved()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        var result = Instance.TryRemove(item, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(0, quantity));
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsOneOccurenceOfItemInCollection_SubtractQuantityFromOccurence()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 9;

        Instance.Add(item, 15);

        //Act
        Instance.TryRemove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<DummyItem>>
            {
                new(item, 6)
            }));
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsOneOccurenceOfItemInCollectionAndQuantityRemovedIsEqualToOwned_RemoveEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 15;

        Instance.Add(item, 15);

        //Act
        Instance.TryRemove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_RemoveAllEntries()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 16;

        Instance.Add(item, 15);

        //Act
        Instance.TryRemove(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_ReturnResultOfOneItemNotRemoved()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();
        var quantity = 16;

        Instance.Add(item, 15);

        //Act
        var result = Instance.TryRemove(item, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(15, 1));
    }

    [TestMethod]
    public void RemovePredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemovePredicate_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = 0;

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void RemovePredicate_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsNoOccurence_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseThereIsNoMatch, predicate));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceOfOneStack_RemoveFromThatStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<DummyItem>>
            {
                new(item.Item, 179)
            }));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceOfOneStack_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>{new(item.Item, 21)}
                }
            });
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 221;

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item.Item, quantity, 200));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceOfOneStackAndAllItemsAreRemoved_RemoveEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 200;

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void RemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacks_RemoveFrommAllThoseStacks()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 99;

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void RemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacks_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 35;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = itemsWithSameLevel.Select(x => new Entry<DummyItem>(x, 35)).ToList()
                }
            });
    }

    [TestMethod]
    public void RemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 100;

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, itemsWithSameLevel.First(), quantity, 99));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsNoMatch_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        //Act
        var action = () => Instance.Remove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_Throw()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 5).CreateMany().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        //Act
        var action = () => Instance.Remove(x => x.Id == entries.First().Item.Id, 14);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_RemoveNumberOfItemFromEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var entry = entries.GetRandom();
        var quantity = entry.Quantity;

        //Act
        Instance.Remove(x => x.Id == entry.Item.Id, quantity);

        //Assert
        Instance.QuantityOf(entry.Item).Should().Be(0);
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var entry = entries.GetRandom();
        var quantity = entry.Quantity;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(x => x.Id == entry.Item.Id, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        entry
                    }
                }
            });
    }

    [TestMethod]
    public void RemovePredicate_WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        //Act
        Instance.Remove(x => x.Id == id, 15);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = 10 }));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_TriggerChange()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(x => x.Id == id, 15);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.TryRemove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryRemovePredicate_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = 0;

        //Act
        var action = () => Instance.TryRemove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.TryRemove(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsNoOccurence_DoNotModify()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = Fixture.Create<int>();

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsNoOccurence_ReturnZeroModified()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = Fixture.Create<int>();

        //Act
        var result = Instance.TryRemove(predicate, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(0, quantity));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsNoOccurence_DoNotTriggerChanges()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == Fixture.Create<int>();
        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStack_RemoveFromThatStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<DummyItem>>
            {
                new(item.Item, 179)
            }));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStack_ReturnAllRemoved()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        //Act
        var result = Instance.TryRemove(predicate, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(21, 0));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStack_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>{new(item.Item, 21)}
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_RemoveTheStackEntirely()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 221;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_ReturnRemovedAndNotRemoved()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 221;

        //Act
        var result = Instance.TryRemove(predicate, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(200, 21));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceOfOneStackAndAllItemsAreRemoved_RemoveEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<DummyItem>(Fixture.Create<DummyItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<DummyItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 200;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacks_RemoveFromAllThoseStacks()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 99;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacks_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 35;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = itemsWithSameLevel.Select(x => new Entry<DummyItem>(x, 35)).ToList()
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_RemoveThoseStacks()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 100;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_ReturnAmountRemoved()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, 99);

        Func<DummyItem, bool> predicate = x => x.Level == level;
        var quantity = 100;

        //Act
        var result = Instance.TryRemove(predicate, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(297, 3));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsNoMatch_DoNotModify()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var copy = Instance.Select(x => x with { }).ToList();

        //Act
        Instance.TryRemove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsNoMatch_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_SetToZero()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 5).CreateMany().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var item = entries.GetRandom().Item;

        //Act
        Instance.TryRemove(x => x.Id == item.Id, 14);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_ReturnRemovedAndNotRemoved()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 5).CreateMany().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var item = entries.GetRandom().Item;

        //Act
        var result = Instance.TryRemove(x => x.Id == item.Id, 14);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(5, 9));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 5).CreateMany().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var item = entries.GetRandom().Item;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(x => x.Id == item.Id, 14);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(item, 5)
                    }
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_RemoveNumberOfItemFromEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var entry = entries.GetRandom();
        var quantity = entry.Quantity;

        //Act
        Instance.TryRemove(x => x.Id == entry.Item.Id, quantity);

        //Assert
        Instance.QuantityOf(entry.Item).Should().Be(0);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var entry = entries.GetRandom();
        var quantity = entry.Quantity;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(x => x.Id == entry.Item.Id, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        entry
                    }
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        //Act
        Instance.TryRemove(x => x.Id == id, 15);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = 10 }));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_TriggerChange()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(x => x.Id == id, 15);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
    }

    [TestMethod]
    public void RemoveEntry_WhenItemIsNull_Throw()
    {
        //Arrange
        Entry<DummyItem> item = null!;

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Remove(item);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(item));
    }

    [TestMethod]
    public void RemoveEntry_WhenItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();

        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Remove(item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void RemoveEntry_WhenItemIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();

        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        //Act
        var result = ((ICollection<Entry<DummyItem>>)Instance).Remove(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveEntry_WhenItemIsInCollection_ReturnTrue()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();
        Instance.Add(item.Item, item.Quantity);

        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        //Act
        var result = ((ICollection<Entry<DummyItem>>)Instance).Remove(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void RemoveEntry_WhenItemIsInCollection_RemoveFromCollection()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();
        Instance.Add(item.Item, item.Quantity);

        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Remove(item);

        //Assert
        Instance.Should().NotContain(item);
    }

    //TODO Test RemoveAt
    [TestMethod]
    public void RemoveAt_WhenIndexIsNegative_Throw()
    {
        //Arrange
        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        var index = -Fixture.Create<int>();
        var count = Fixture.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName(nameof(index));
    }

    [TestMethod]
    public void RemoveAt_WhenIndexIsHigherThanLastIndex_Throw()
    {
        //Arrange
        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        var index = Instance.LastIndex + 1;
        var count = Fixture.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName(nameof(index));
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    public void RemoveAt_WhenCountIsZeroOrNegative_Throw(int count)
    {
        //Arrange
        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        var index = Instance.GetRandomIndex();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentException>().WithParameterName(nameof(count));
    }

    [TestMethod]
    public void RemoveAt_WhenIndexPlusCountIsOutsideRangeOfInventory_Throw()
    {
        //Arrange
        foreach (var (dummy, quantity) in Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Random.Shared.Next(1, 5)).CreateMany())
            Instance.Add(dummy, quantity);

        var index = Instance.GetRandomIndex();
        var count = Instance.Count() + 1;

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Collections.ObservableList.Resources.Exceptions.CannotRemoveItemBecauseRangeFallsOutsideBoundaries, "ObservableList<Entry<DummyItem>>", 0, Instance.LastIndex, index, count) + " (Parameter 'count')").WithParameterName(nameof(count));
    }

    [TestMethod]
    public void RemoveAt_WhenIndexAndCountAreWithinBoundaries_RemoveItemsAtIndexPlusCount()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);
        var index = Instance.GetRandomIndex();
        var count = Math.Clamp(Instance.LastIndex - index, 1, int.MaxValue);

        //Act
        Instance.RemoveAt(index, count);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Take(index).Concat(entries.Skip(index + count)));
    }

    [TestMethod]
    public void RemoveAt_WhenIndexAndCountAreWithinBoundaries_TriggerEvent()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);
        var index = Instance.GetRandomIndex();
        var count = Math.Clamp(Instance.LastIndex - index, 1, int.MaxValue);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.RemoveAt(index, count);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
        {
                new()
                {
                    OldValues = entries.Skip(index).Take(count).ToList()
                }
            });
    }

    [TestMethod]
    public void ClearItem_WhenItemIsNullAndThereIsNoOccurenceOfNullInStock_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear((DummyItem)null!);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearItem_WhenThereIsNoOccurenceOfItemInStock_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(Fixture.Create<DummyItem>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearItem_WhenItemIsNullAndThereIsAnOccurenceOfNullInStock_RemoveEntireStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        Instance.Add((DummyItem)null!, Fixture.Create<int>());

        //Act
        Instance.Clear((DummyItem)null!);

        //Assert
        Instance.QuantityOf((DummyItem)null!).Should().Be(0);
    }

    [TestMethod]
    public void ClearItem_WhenItemIsNullAndThereIsAnOccurenceOfNullInStock_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var oldQuantity = Fixture.Create<int>();

        Instance.Add((DummyItem)null!, oldQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear((DummyItem)null!);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>()
                    {
                        new(null!, oldQuantity)
                    }
                }
            });
    }

    [TestMethod]
    public void ClearItem_WhenThereIsAnOccurenceOfItem_RemoveEntireStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, Fixture.Create<int>());

        //Act
        Instance.Clear(item);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void ClearItem_WhenThereIsAnOccurenceOfItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        var oldQuantity = Fixture.Create<int>();
        Instance.Add(item, oldQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>()
                    {
                        new(item, oldQuantity)
                    }
                }
            });
    }

    [TestMethod]
    public void ClearItem_WhenThereAreMultipleOccurencesOfItem_RemoveThemAll()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, Fixture.Create<int>());
        Instance.Add(item, Fixture.Create<int>());
        Instance.Add(item, Fixture.Create<int>());

        //Act
        Instance.Clear(item);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void ClearPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;

        //Act
        var action = () => Instance.Clear(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ClearPredicate_WhenInventoryIsEmpty_DoNotModify()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();

        //Act
        Instance.Clear(predicate);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearPredicate_WhenInventoryIsEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(predicate);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearPredicate_WhenThereIsNoMatchForPredicate_DoNotModify()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var copy = Instance.Select(x => x with { }).ToList();

        //Act
        Instance.Clear(x => x.Id == Fixture.Create<int>());

        //Assert
        Instance.Should().BeEquivalentTo(copy);
    }

    [TestMethod]
    public void ClearPredicate_WhenThereIsNoMatchForPredicate_DoNotTriggerEvent()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(x => x.Id == Fixture.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearPredicate_WhenThereIsOneMatchForPredicate_RemoveThatStackOfItems()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = entries.GetRandom().Item;

        //Act
        Instance.Clear(x => x.Id == item.Id);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Where(x => x.Item != item));
    }

    [TestMethod]
    public void ClearPredicate_WhenThereIsOneMatchForPredicate_TriggerEvent()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var entry = entries.GetRandom();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(x => x.Id == entry.Item.Id);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        entry
                    }
                }
            });
    }

    [TestMethod]
    public void ClearPredicate_WhenThereAreMultipleMatches_RemoveThoseStacks()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        Instance.Clear(x => x.Id > 0);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void ClearPredicate_WhenThereAreMultipleMatches_TriggerEvent()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear(x => x.Id > 0);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = entries
                }
            });
    }

    [TestMethod]
    public void Clear_WhenInventoryIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear();

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenInventoryIsNotEmpty_RemoveEverything()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        Instance.Clear();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenInventoryIsNotEmpty_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear();

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
        {
            new()
            {
                OldValues = entries
            }
        });
    }

    [TestMethod]
    public void QuantityOfItem_WhenItemIsNullAndNoNullInStock_ReturnZero()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        DummyItem item = null!;

        //Act
        var result = Instance.QuantityOf(item);

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void QuantityOfItem_WhenThereIsNoEntryWithItemInStock_ReturnZero()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();

        //Act
        var result = Instance.QuantityOf(item);

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void QuantityOfItem_WhenItemIsNullAndThereIsAnEntryWithNull_ReturnQuantityOfNull()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var expectedQuantity = Fixture.Create<int>();

        Instance.Add((DummyItem)null!, expectedQuantity);

        //Act
        var result = Instance.QuantityOf((DummyItem)null!);

        //Assert
        result.Should().Be(expectedQuantity);
    }

    [TestMethod]
    public void QuantityOfItem_WhenThereIsEntryWithItemInStock_ReturnItsQuantity()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var entry = Fixture.Create<Entry<DummyItem>>();
        Instance.Add(entry.Item, entry.Quantity);

        //Act
        var result = Instance.QuantityOf(entry.Item);

        //Assert
        result.Should().Be(entry.Quantity);
    }

    [TestMethod]
    public void QuantityOfItem_WhenThereIsMoreThanOneEntryOfItem_CompileSumOfAllEntriesQuantity()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var entry = Fixture.Create<Entry<DummyItem>>();
        Instance.Add(entry.Item, entry.Quantity);

        var quantity2 = Fixture.Create<int>();
        Instance.Add(entry.Item, quantity2);

        var quantity3 = Fixture.Create<int>();
        Instance.Add(entry.Item, quantity3);

        //Act
        var result = Instance.QuantityOf(entry.Item);

        //Assert
        result.Should().Be(entry.Quantity + quantity2 + quantity3);
    }

    [TestMethod]
    public void QuantityOfPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;

        //Act
        var action = () => Instance.QuantityOf(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void QuantityOfPredicate_WhenIsEmpty_ReturnZero()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();

        //Act
        var result = Instance.QuantityOf(predicate);

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void QuantityOfPredicate_WhenThereIsNoMatch_ReturnZero()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.QuantityOf(x => x.Id == Fixture.Create<int>());

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void QuantityOfPredicate_WhenThereIsOneMatch_ReturnSizeOfThatStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.QuantityOf(x => x == entries.First().Item);

        //Assert
        result.Should().Be(entries.First().Quantity);
    }

    [TestMethod]
    public void QuantityOfPredicate_WhenThereAreMultipleMatches_ReturnSumOfAllMatches()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.QuantityOf(x => x.Id > 0);

        //Assert
        result.Should().Be(entries.Sum(x => x.Quantity));
    }

    [TestMethod]
    public void SearchItem_WhenThereIsNoOccurence_ReturnEmpty()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Fixture.Create<DummyItem>();

        //Act
        var result = Instance.Search(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void SearchItem_WhenThereIsAnOccurence_ReturnOccurence()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var randomIndex = entries.GetRandomIndex();
        var random = entries[randomIndex];

        //Act
        var result = Instance.Search(random.Item);

        //Assert
        result.Should().BeEquivalentTo(new List<IndexedEntry<DummyItem>>
        {
            new(random.Item, random.Quantity, randomIndex)
        });
    }

    [TestMethod]
    public void SearchPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;

        //Act
        var action = () => Instance.Search(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SearchPredicate_WhenThereIsNoOccurence_ReturnEmpty()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        //Act
        var result = Instance.Search(x => x.Id == Fixture.Create<int>());

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void SearchPredicate_WhenThereIsOneOccurence_ReturnOnlyOccurence()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var itemsWithSameLevel = Fixture.Build<DummyItem>().With(x => x.Level, level).CreateMany().ToList();
        foreach (var item in itemsWithSameLevel)
            Instance.Add(item, Fixture.Create<int>());

        var randomIndex = entries.GetRandomIndex();
        var random = entries[randomIndex];

        //Act
        var result = Instance.Search(x => x.Id == random.Item.Id);

        //Assert
        result.Should().BeEquivalentTo(new List<IndexedEntry<DummyItem>>
            {
                new(random.Item, random.Quantity, randomIndex)
            });
    }

    [TestMethod]
    public void SearchPredicate_WhenThereAreManyOccurences_ReturnAllOccurences()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var level = Fixture.Create<int>();
        var item3 = Fixture.Build<Entry<DummyItem>>().With(x => x.Item, Fixture.Build<DummyItem>().With(y => y.Level, level).Create).Create();
        Instance.Add(item3.Item, item3.Quantity);
        var item4 = Fixture.Build<Entry<DummyItem>>().With(x => x.Item, Fixture.Build<DummyItem>().With(y => y.Level, level).Create).Create();
        Instance.Add(item4.Item, item4.Quantity);
        var item5 = Fixture.Build<Entry<DummyItem>>().With(x => x.Item, Fixture.Build<DummyItem>().With(y => y.Level, level).Create).Create();
        Instance.Add(item5.Item, item5.Quantity);

        //Act
        var result = Instance.Search(x => x.Level == level);

        //Assert
        result.Should().BeEquivalentTo(new List<IndexedEntry<DummyItem>>
            {
                new(item3.Item, item3.Quantity, 3),
                new(item4.Item, item4.Quantity, 4),
                new(item5.Item, item5.Quantity, 5),
            });
    }

    [TestMethod]
    public void Swap_WhenCurrentIsNegative_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = -Fixture.Create<int>();
        var destination = entries.GetRandomIndex();

        //Act
        var action = () => Instance.Swap(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenDestinationIsNegative_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = entries.GetRandomIndex();
        var destination = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Swap(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenCurrentIsOutsideRange_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = Instance.LastIndex + 1;
        var destination = entries.GetRandomIndex();

        //Act
        var action = () => Instance.Swap(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenDestinationIsOutsideRange_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = entries.GetRandomIndex();
        var destination = Instance.LastIndex + 1;

        //Act
        var action = () => Instance.Swap(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreEqual_DoNotModifyCollection()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = entries.GetRandomIndex();
        var destination = current;

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreWithinRange_Swap()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var current = 2;
        var destination = 1;

        //Act
        Instance.Swap(current, destination);

        //Assert
        Instance.Should().ContainInOrder(new List<Entry<DummyItem>>
            {
                entries[0],
                entries[2],
                entries[1],
            });
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnMessageThatItIsEmpty()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"Empty {Instance.GetType().GetHumanReadableName()}");
    }

    [TestMethod]
    public void ToString_WhenItContainsItems_ReturnTotalStackCount()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"{Instance.GetType().GetHumanReadableName()} with {Instance.StackCount} stacks of items");
    }

    [TestMethod]
    public void LastIndex_WhenIsEmpty_ReturnMinusOne()
    {
        //Arrange

        //Act
        var result = Instance.LastIndex;

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void LastIndex_WhenIsNotEmpty_ReturnLastIndex()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>(3).ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.LastIndex;

        //Assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void StackSize_WhenNewValueIsSet_ReturnThatNewValue()
    {
        //Arrange
        var value = Fixture.Create<int>();

        //Act
        Instance.StackSize = value;

        //Assert
        Instance.StackSize.Should().Be(value);
    }

    [TestMethod]
    public void StackSize_WhenValueIsZero_Throw()
    {
        //Arrange
        var value = 0;

        //Act
        var action = () => Instance.StackSize = value;

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, Instance.GetType().GetHumanReadableName(), value));
    }

    [TestMethod]
    public void StackSize_WhenValueIsNegative_Throw()
    {
        //Arrange
        var value = -Fixture.Create<int>();

        //Act
        var action = () => Instance.StackSize = value;

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, Instance.GetType().GetHumanReadableName(), value));
    }

    [TestMethod]
    public void StackSize_WhenValueIsGreaterThanAllStacksInCollections_DoNotChangeQuantities()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        //Act
        Instance.StackSize = 100;

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void StackSize_WhenValueIsGreaterThanAllStacksInCollections_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.StackSize = 100;

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void StackSize_WhenValueIsEqualToAllStacksInCollections_DoNotChangeQuantities()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 99).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        //Act
        Instance.StackSize = 99;

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void StackSize_WhenValueIsEqualToAllStacksInCollections_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 99).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.StackSize = 99;

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void StackSize_WhenValueIsLessThanStacksInCollection_RemoveExcessQuantities()
    {
        //Arrange
        Instance.Add(Fixture.Create<DummyItem>(), 58);
        Instance.Add(Fixture.Create<DummyItem>(), 95);
        Instance.Add(Fixture.Create<DummyItem>(), 72);

        //Act
        Instance.StackSize = 50;

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<DummyItem>>
            {
                new(Instance[0].Item, 50),
                new(Instance[1].Item, 50),
                new(Instance[2].Item, 50),
            });
    }

    [TestMethod]
    public void StackSize_WhenValueIsLessThanStacksInCollection_TriggerChangeOnce()
    {
        //Arrange
        Instance.Add(Fixture.Create<DummyItem>(), 58);
        Instance.Add(Fixture.Create<DummyItem>(), 95);
        Instance.Add(Fixture.Create<DummyItem>(), 72);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.StackSize = 50;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(Instance[0].Item, 8),
                        new(Instance[1].Item, 45),
                        new(Instance[2].Item, 22),
                    }
                }
            });
    }

    [TestMethod]
    public void Indexer_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var index = -Fixture.Create<int>();

        //Act
        var action = () => Instance[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Indexer_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = Instance.LastIndex + 1;

        //Act
        var action = () => Instance[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Indexer_WhenIndexIsZero_ReturnFirstEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = 0;

        //Act
        var result = Instance[index];

        //Assert
        result.Should().BeEquivalentTo(entries.First());
    }

    [TestMethod]
    public void Indexer_WhenIndexIsEqualToLastIndex_ReturnLastEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = Instance.LastIndex;

        //Act
        var result = Instance[index];

        //Assert
        result.Should().BeEquivalentTo(entries.Last());
    }

    [TestMethod]
    public void TotalCount_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.TotalCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void TotalCount_WhenContainsOnlyOneEntry_ReturnQuantityOfThatEntry()
    {
        //Arrange
        var entry = Fixture.Create<Entry<DummyItem>>();
        Instance.Add(entry.Item, entry.Quantity);

        //Act
        var result = Instance.TotalCount;

        //Assert
        result.Should().Be(entry.Quantity);
    }

    [TestMethod]
    public void TotalCount_WhenContainsMultipleEntries_ReturnSumOfAllQuantities()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.TotalCount;

        //Assert
        result.Should().Be(entries.Sum(x => x.Quantity));
    }

    [TestMethod]
    public void StackCount_WhenIsEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = Instance.StackCount;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void StackCount_WhenIsNotEmpty_ReturnNumberOfUniqueEntries()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.StackCount;

        //Assert
        result.Should().Be(entries.Count);
    }

    [TestMethod]
    public void AddEntry_WhenEntryIsNull_Throw()
    {
        //Arrange
        Entry<DummyItem> item = null!;

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddEntry_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, 0).Create();

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item.Item, 0));
    }

    [TestMethod]
    public void AddEntry_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, -Fixture.Create<int>()).Create();

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item.Item, item.Quantity));
    }

    [TestMethod]
    public void AddEntry_WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
    {
        //Arrange
        var oldQuantity = Fixture.Create<int>();
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, oldQuantity).Create();
        Instance.Add(item.Item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item with { Quantity = newQuantity });

        //Assert
        Instance.QuantityOf(item.Item).Should().Be(oldQuantity + newQuantity);
    }

    [TestMethod]
    public void AddEntry_WhenThisItemIsAlreadyInStock_TriggerChange()
    {
        //Arrange
        var oldQuantity = Fixture.Create<int>();
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, oldQuantity).Create();
        Instance.Add(item.Item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item with { Quantity = newQuantity });

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>> { new(item.Item, newQuantity) }
                }
            });
    }

    [TestMethod]
    public void AddEntry_WhenItemIsNotCurrentlyInStock_AddNewEntry()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        Instance.QuantityOf(item.Item).Should().Be(item.Quantity);
    }

    [TestMethod]
    public void AddEntry_WhenItemIsNotCurrentlyInStock_TriggerChange()
    {
        //Arrange
        var item = Fixture.Create<Entry<DummyItem>>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>> { new(item.Item, item.Quantity) }
                }
            });
    }

    [TestMethod]
    public void AddEntry_WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;
        var oldQuantity = 44;
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, oldQuantity).Create();
        Instance.Add(item.Item, oldQuantity);

        var newQuantity = 55;

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item with { Quantity = newQuantity });

        //Assert
        Instance.QuantityOf(item.Item).Should().Be(oldQuantity + newQuantity);
    }



    [TestMethod]
    public void AddEntry_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Instance.StackSize).Create();

        //Act
        ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        Instance.QuantityOf(item.Item).Should().Be(item.Quantity);
    }



    [TestMethod]
    public void Add_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();

        //Act
        var action = () => Instance.Add(item, 0);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, 0));
    }

    [TestMethod]
    public void Add_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.Add(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
    }

    [TestMethod]
    public void Add_WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var oldQuantity = Fixture.Create<int>();
        Instance.Add(item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        //Act
        Instance.Add(item, newQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
    }

    [TestMethod]
    public void Add_WhenThisItemIsAlreadyInStock_TriggerChange()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var oldQuantity = Fixture.Create<int>();
        Instance.Add(item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(item, newQuantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>> { new(item, newQuantity) }
                }
            });
    }

    [TestMethod]
    public void Add_WhenItemIsNotCurrentlyInStock_AddNewEntry()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(quantity);
    }

    [TestMethod]
    public void Add_WhenItemIsNotCurrentlyInStock_TriggerChange()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(item, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>> { new(item, quantity) }
                }
            });
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasNoMatch_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var action = () => Instance.Add(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasOneMatch_AddToThatEntry()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = entries.GetRandomIndex();
        var random = entries[index].Item;
        var newQuantity = Fixture.Create<int>();

        var expectedEntries = entries.ToList();
        expectedEntries[index] = entries[index] with
        {
            Quantity = entries[index].Quantity + newQuantity
        };

        //Act
        Instance.Add(x => x.Id == random.Id, newQuantity);

        //Assert
        Instance.Should().BeEquivalentTo(expectedEntries);
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasOneMatch_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = entries.GetRandomIndex();
        var random = entries[index].Item;
        var newQuantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(x => x.Id == random.Id, newQuantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>>
                    {
                        new(random, newQuantity)
                    }
                }
            });
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasMultipleMatches_AddToAllThoseEntries()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).CreateMany().ToList();

        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var newQuantity = Fixture.Create<int>();

        //Act
        Instance.Add(x => x.Id == id, newQuantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = x.Quantity + newQuantity }));

    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasMultipleMatches_TriggerChange()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).CreateMany().ToList();

        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var newQuantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(x => x.Id == id, newQuantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = newQuantity }).ToList()
                }
            });
    }

    [TestMethod]
    public void Remove_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        Instance.Add(item);

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Remove_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = 0;

        Instance.Add(item);

        //Act
        var action = () => Instance.Remove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Remove_WhenNullAndNoNullInStock_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var action = () => Instance.Remove((DummyItem)null!, Fixture.Create<int>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Remove_WhenItemIsNotInStock_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var action = () => Instance.Remove(Fixture.Create<DummyItem>(), Fixture.Create<int>());

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Remove_WhenNullIsInStockButNotEnoughQuantity_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        Instance.Add((DummyItem)null!);

        //Act
        var action = () => Instance.Remove((DummyItem)null!, 2);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Remove_WhenItemIsInStockButNotEnoughQuantity_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item);

        //Act
        var action = () => Instance.Remove(item, 2);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Remove_WhenQuantityIsEqualToWhatIsInStockForItem_RemoveItemEntirely()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        var itemQuantity = Fixture.Create<int>();
        Instance.Add(item, itemQuantity);

        //Act
        Instance.Remove(item, itemQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void Remove_WhenQuantityIsEqualToWhatIsInStockForItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        var itemQuantity = Fixture.Create<int>();
        Instance.Add(item, itemQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(item, itemQuantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(item, itemQuantity)
                    }
                }
            });
    }

    [TestMethod]
    public void Remove_WhenQuantityIsLessThanWhatIsInStockForItem_SubstractQuantityForItem()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        //Act
        Instance.Remove(item, 8);

        //Assert
        Instance.QuantityOf(item).Should().Be(4);
    }

    [TestMethod]
    public void Remove_WhenQuantityIsLessThanWhatIsInStockForItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(item, 8);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(item, 8)
                    }
                }
            });
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = -Fixture.Create<int>();

        Instance.Add(item);

        //Act
        var action = () => Instance.TryRemove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = 0;

        Instance.Add(item);

        //Act
        var action = () => Instance.TryRemove(item, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryRemove_WhenNullAndNoNullInStock_DoNotThrow()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var action = () => Instance.TryRemove((DummyItem)null!, Fixture.Create<int>());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemove_WhenNullAndNoNullInStock_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove((DummyItem)null!, Fixture.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemove_WhenItemIsNotInStock_DoNotThrow()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var action = () => Instance.TryRemove(Fixture.Create<DummyItem>(), Fixture.Create<int>());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemove_WhenItemIsNotInStock_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(Fixture.Create<DummyItem>(), Fixture.Create<int>());

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemove_WhenNullIsInStockButNotEnoughQuantity_ClearEntireStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        Instance.Add((DummyItem)null!);

        //Act
        Instance.TryRemove((DummyItem)null!, 2);

        //Assert
        Instance.QuantityOf((DummyItem)null!).Should().Be(0);
    }

    [TestMethod]
    public void TryRemove_WhenItemIsInStockButNotEnoughQuantity_ClearEntireStack()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item);

        //Act
        Instance.TryRemove(item, 2);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsEqualToWhatIsInStockForItem_RemoveItemEntirely()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        var itemQuantity = Fixture.Create<int>();
        Instance.Add(item, itemQuantity);

        //Act
        Instance.TryRemove(item, itemQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(0);
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsEqualToWhatIsInStockForItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        var itemQuantity = Fixture.Create<int>();
        Instance.Add(item, itemQuantity);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(item, itemQuantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(item, itemQuantity)
                    }
                }
            });
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsLessThanWhatIsInStockForItem_SubstractQuantityForItem()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        //Act
        Instance.TryRemove(item, 8);

        //Assert
        Instance.QuantityOf(item).Should().Be(4);
    }

    [TestMethod]
    public void TryRemove_WhenQuantityIsLessThanWhatIsInStockForItem_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(item, 8);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    OldValues = new List<Entry<DummyItem>>
                    {
                        new(item, 8)
                    }
                }
            });
    }

    [TestMethod]
    public void TryRemove_WhenRemovingAllItemsInStack_ReturnAllItemsRemoved()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        //Act
        var result = Instance.TryRemove(item, 12);

        //Assert
        result.Should().Be(new TryRemoveResult(12, 0));
    }

    [TestMethod]
    public void TryRemove_WhenRemovingNoItemInStack_ReturnNoItemsRemoved()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();

        //Act
        var result = Instance.TryRemove(item, 12);

        //Assert
        result.Should().Be(new TryRemoveResult(0, 12));
    }

    [TestMethod]
    public void TryRemove_WhenRemovingSomeItemsInStack_ReturnRemovedAndNotRemoved()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        Instance.Add(item, 12);

        //Act
        var result = Instance.TryRemove(item, 28);

        //Assert
        result.Should().Be(new TryRemoveResult(12, 16));
    }

    [TestMethod]
    public void IndexesOf_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;

        //Act
        var action = () => Instance.IndexesOf(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void IndexesOf_WhenInventoryIsEmpty_ReturnEmpty()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();

        //Act
        var result = Instance.IndexesOf(predicate);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOf_WhenInventoryIsNotEmptyButThereIsZeroMatch_ReturnEmpty()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.IndexesOf(x => x.Id == Fixture.Create<int>());

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOf_WhenThereIsOnlyOneMatchAndItsTheLastIndex_ReturnLastIndex()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.IndexesOf(x => x == entries.Last().Item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { entries.Count - 1 });
    }

    [TestMethod]
    public void IndexesOf_WhenThereIsOnlyOneMatchAndItsTheFirstIndex_ReturnFirstIndex()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.IndexesOf(x => x == entries.First().Item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 0 });
    }

    [TestMethod]
    public void IndexesOf_WhenThereAreMultipleMatches_ReturnAll()
    {
        //Arrange
        var level = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>().With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Level, level).Create()).CreateMany(3).ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var otherEntries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.IndexesOf(x => x.Level == level);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 0, 1, 2 });
    }

    [TestMethod]
    public void EqualityOperator_WhenFirstIsNullAndSecondIsNot_ReturnFalse()
    {
        //Arrange
        InventoryTable<DummyItem> instance = null!;
        var other = Fixture.Create<TInventory>();

        //Act
        var result = instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualityOperator_WhenFirstIsNotNullAndSecondIsNull_ReturnFalse()
    {
        //Arrange
        var instance = Fixture.Create<TInventory>();
        InventoryTable<DummyItem> other = null!;

        //Act
        var result = instance == other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void EqualityOperator_WhenBothAreNull_ReturnTrue()
    {
        //Arrange
        TInventory instance = null!;
        TInventory other = null!;

        //Act
        var result = instance == other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void InequalityOperator_WhenFirstIsNullAndSecondIsNot_ReturnTrue()
    {
        //Arrange
        InventoryTable<DummyItem> instance = null!;
        var other = Fixture.Create<TInventory>();

        //Act
        var result = instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void InequalityOperator_WhenFirstIsNotNullAndSecondIsNull_ReturnTrue()
    {
        //Arrange
        var instance = Fixture.Create<TInventory>();
        InventoryTable<DummyItem> other = null!;

        //Act
        var result = instance != other;

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void InequalityOperator_WhenBothAreNull_ReturnFalse()
    {
        //Arrange
        TInventory instance = null!;
        TInventory other = null!;

        //Act
        var result = instance != other;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void GetHashCode_Always_ReturnInternalCollectionHashCode()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);

        var expected = GetFieldValue<ObservableList<Entry<DummyItem>>>("Items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(expected.GetHashCode());
    }

    [TestMethod]
    public void Enumerator_Always_CorrectlyEnumeratesEveryItem()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);

        var enumeratedItems = new List<Entry<DummyItem>>();

        //Act
        foreach (var item in Instance)
            enumeratedItems.Add(item);

        //Assert
        enumeratedItems.Should().NotBeEmpty();
        enumeratedItems.Should().BeEquivalentTo(Instance);
        enumeratedItems.Should().HaveCount(Instance.Count());
    }

    [TestMethod]
    public void Enumerator_WhenCollectionIsModifiedDuringEnumeration_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);

        //Act
        var action = () =>
        {
            foreach (var item in Instance)
                Instance.Remove(item.Item, item.Quantity);
        };

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Enumerator_WhenUsingResetAfterCollectionChanged_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);

        using var enumerator = Instance.GetEnumerator();
        Instance.RemoveAt(Instance.GetRandomIndex());

        //Act
        var action = () => enumerator.Reset();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Enumerator_WhenUsingResetWhileCollectionIsStillUnchanged_SetCurrentToDefault()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);
        using var enumerator = Instance.GetEnumerator();

        //Act
        enumerator.Reset();

        //Assert
        enumerator.Current.Should().BeNull();
    }

    [TestMethod]
    public void InterfaceEnumerator_WhenUsingResetAfterCollectionChanged_Throw()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);

        using var enumerator = ((IEnumerable)Instance).GetEnumerator() as IEnumerator<Entry<DummyItem>>;
        Instance.RemoveAt(Instance.GetRandomIndex());

        //Act
        var action = () => enumerator.Reset();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void InterfaceEnumerator_WhenUsingResetWhileCollectionIsStillUnchanged_SetCurrentToDefault()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (item, quantity) in entries)
            Instance.Add(item, quantity);
        using var enumerator = ((IEnumerable)Instance).GetEnumerator() as IEnumerator<Entry<DummyItem>>;

        //Act
        enumerator.Reset();

        //Assert
        enumerator.Current.Should().BeNull();
    }

    [TestMethod]
    public void IsReadOnly_Always_ReturnFalse()
    {
        //Arrange

        //Act
        var result = ((ICollection<Entry<DummyItem>>)Instance).IsReadOnly;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Always_EnsureValueEquality() => Ensure.ValueEquality<TInventory>(Fixture, new JsonSerializerOptions().WithInventoryConverters());

    [TestMethod]
    public void Always_IsJsonSerializable() => Ensure.IsJsonSerializable<TInventory>(Fixture, new JsonSerializerOptions().WithInventoryConverters());

}