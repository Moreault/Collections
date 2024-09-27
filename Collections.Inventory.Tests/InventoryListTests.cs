namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryListTests : InventoryTester<InventoryList<GarbageItem>>
{
    [TestMethod]
    public void ParameterlessConstructor_Always_CreateEmptyWithMaxStackSize()
    {
        //Arrange

        //Act
        var result = new InventoryList<GarbageItem>();

        //Assert
        result.Should().BeEmpty();
        result.StackSize.Should().Be(int.MaxValue);
    }

    [TestMethod]
    public void StackSizeConstructor_Always_CreateEmptyWithSpecifiedStackSize()
    {
        //Arrange
        var stackSize = Dummy.Create<int>();

        //Act
        var result = new InventoryList<GarbageItem>(stackSize);

        //Assert
        result.Should().BeEmpty();
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void ItemTypeConstructor_Always_CreateWithEntriesOfItemsWithQuantityOfOnePlusStackSize()
    {
        //Arrange
        var items = Dummy.CreateMany<GarbageItem>().ToList();
        var stackSize = Dummy.Create<int>();

        //Act
        var result = new InventoryList<GarbageItem>(items, stackSize);

        //Assert
        result.Should().BeEquivalentTo(items.Select(x => new Entry<GarbageItem>(x, 1)));
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void EntriesConstructor_Always_CreateWithEntriesAndStackSize()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        var stackSize = entries.Max(x => x.Quantity) + Dummy.Create<int>();

        //Act
        var result = new InventoryList<GarbageItem>(entries, stackSize);

        //Assert
        result.Should().BeEquivalentTo(entries);
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsEqualToStackSize_AddOneStackOfItemOfStackSize()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;
        var item = Dummy.Create<GarbageItem>();

        //Act
        Instance.Add(item, stackSize);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
        {
            new(item, stackSize)
        });
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsEqualToStackSize_Trigger()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;
        var item = Dummy.Create<GarbageItem>();

        var eventsArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

        //Act
        Instance.Add(item, stackSize);

        //Assert
        eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
        {
            new()
            {
                NewValues =
                [
                    new(item, stackSize)
                ]
            }
        });
    }

    [TestMethod]
    public void
        AddPredicate_WhenOneEntryCorrespondsToPredicateButBustsStackSize_AddNewEntryOfThisTypeToEndOfCollectionWithRemainder()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        const int quantity = 25;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, 85).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var existingEntryIndex = entries.GetRandomIndex();
        var existingEntry = entries[existingEntryIndex];

        Func<GarbageItem, bool> predicate = x => x.Id == existingEntry.Item.Id;

        var expected = entries.ToList();
        expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = 99 };
        expected.Add(new Entry<GarbageItem>(existingEntry.Item, 11));

        //Act
        Instance.Add(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddPredicate_WhenMultipleEntriesCorrespondToPredicateAndQuantityIsNotBustedForAny_AddToExistingEntries()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        const int quantity = 25;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();

        var level = Dummy.Create<int>();
        var entriesWithSameLevel = Dummy
            .Build<GarbageItem>().With(x => x.Level, level)
            .CreateMany()
            .Select(x => new Entry<GarbageItem>(x, Dummy.Number.Between(1, 25).Create())).ToList();

        entries.AddRange(entriesWithSameLevel);

        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Level == level;

        var expected = entries.ToList();
        for (var i = 0; i < expected.Count; i++)
        {
            if (expected[i].Item.Level == level)
                expected[i] = entries[i] with { Quantity = expected[i].Quantity + quantity };
        }

        //Act
        Instance.Add(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void
        AddPredicate_WhenMultipleEntriesCorrespondToPredicateAndQuantityIsBusted_AddNewEntriesForBustedTypesToTheEndOfCollectionWithTheirRespectiveRemainders()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        const int quantity = 25;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();

        var level = Dummy.Create<int>();
        var entriesWithSameLevel = Dummy
            .Build<GarbageItem>().With(x => x.Level, level)
            .CreateMany()
            .Select(x => new Entry<GarbageItem>(x, 80)).ToList();

        entries.AddRange(entriesWithSameLevel);

        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Level == level;

        var expected = entries.ToList();
        for (var i = 0; i < expected.Count; i++)
        {
            if (expected[i].Item.Level == level)
                expected[i] = expected[i] with { Quantity = 99 };
        }

        expected.AddRange(entriesWithSameLevel.Select(x => x with { Quantity = 6 }));

        //Act
        Instance.Add(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanStackSize_AddTwoStacksOfItems()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;
        var item = Dummy.Create<GarbageItem>();
        const int quantity = 145;

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
        {
            new(item, stackSize),
            new(item, 46)
        });
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanStackSize_Trigger()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;
        var item = Dummy.Create<GarbageItem>();
        const int quantity = 145;

        var eventsArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

        //Act
        Instance.Add(item, quantity);

        //Assert
        eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
        {
            new()
            {
                NewValues =
                [
                    new(item, 145)
                ]
            }
        });
    }

    [TestMethod]
    public void AddItem_WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanTwoStackSizes_AddThreeStacksOfItem()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;
        var item = Dummy.Create<GarbageItem>();
        const int quantity = 281;

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
        {
            new(item, stackSize),
            new(item, stackSize),
            new(item, 83),
        });
    }

    [TestMethod]
    public void AddItem_WhenAlreadyContainsAFullStackOfThisItem_AddNewStackWithSpecifiedQuantityAtTheEnd()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, stackSize).Create()).CreateMany()
            .ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        const int quantity = 65;
        Instance.Add(item, stackSize);

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
        {
            new(item, stackSize),
            new(item, quantity),
        }));
    }

    [TestMethod]
    public void AddItem_WhenAlreadyContainsAStackOfThisItem_AddToExistingStack()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, stackSize).Create()).CreateMany()
            .ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        const int quantity = 65;
        Instance.Add(item, 25);

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
        {
            new(item, 90),
        }));
    }

    [TestMethod]
    public void AddItem_WhenAlreadyContainsAStackOfThisItemThatIsAlmostFull_FullOutExistingStackAndCreateNewEntryForTheRemainder()
    {
        //Arrange
        const int stackSize = 99;
        Instance.StackSize = stackSize;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, stackSize).Create()).CreateMany()
            .ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        const int quantity = 65;
        Instance.Add(item, 80);

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
        {
            new(item, stackSize),
            new(item, 46),
        }));
    }

    [TestMethod]
    public void InsertFirst_WhenIsEmpty_InsertAsFirstItem()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>> { new(item, quantity) });
    }

    [TestMethod]
    public void InsertFirst_WhenIsEmpty_TriggerChange()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
        {
            new()
            {
                NewValues =
                [
                    new(item, quantity)
                ]
            }
        });
    }

    [TestMethod]
    public void InsertFirst_WhenIsEmptyAndQuantityBustsStackSize_InsertAdditionalEntriesAfterFirst()
    {
        //Arrange
        Instance.StackSize = 99;
        var item = Dummy.Create<GarbageItem>();
        var quantity = 300;

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
        {
            new(item, 99),
            new(item, 99),
            new(item, 99),
            new(item, 3),
        });
    }

    [TestMethod]
    public void InsertFirst_WhenContainsItems_InsertBeforeEveryOther()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        Instance.Should().ContainInOrder(new List<Entry<GarbageItem>> { new(item, quantity) }.Concat(entries));
    }

    [TestMethod]
    public void InsertFirst_WhenContainsItems_TriggerChange()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        var quantity = Dummy.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
        {
            new()
            {
                NewValues =
                [
                    new(item, quantity)
                ]
            }
        });
    }

    [TestMethod]
    public void InsertFirst_WhenContainsItemsAndQuantityBustsStackSize_InsertMultipleEntriesBeforeEveryOther()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        var quantity = 300;

        //Act
        Instance.InsertFirst(item, quantity);

        //Assert
        Instance.Should().ContainInOrder(new List<Entry<GarbageItem>>
        {
            new(item, 99),
            new(item, 99),
            new(item, 99),
            new(item, 3),
        }.Concat(entries));
    }

    [TestClass]
    public class Insert : Tester<InventoryList<GarbageItem>>
    {
        [TestMethod]
        public void Insert_WhenIsEmptyAndInsertAtIndexZero_Add()
        {
            //Arrange
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
            {
                new(item, quantity),
            });
        }

        [TestMethod]
        public void Insert_WhenIsEmptyAndInsertAtIndexZeroAndQuantityBustsStackSize_AddMultipleEntries()
        {
            //Arrange
            Instance.StackSize = 50;

            var item = Dummy.Create<GarbageItem>();
            var quantity = 121;

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
            {
                new(item, 50),
                new(item, 50),
                new(item, 21),
            });
        }

        [TestMethod]
        public void Insert_WhenIsEmptyAndInsertAtIndexZero_TriggerChange()
        {
            //Arrange
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item, quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void Insert_WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Dummy.Create<int>();
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            var action = () => Instance.Insert(index, item, quantity);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Insert_WhenIndexIsZero_InsertBeforeEverythingElse()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 0;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<GarbageItem>> { new(item, quantity) }.Concat(entries));
        }

        [TestMethod]
        public void Insert_WhenIndexIsZero_TriggerChange()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 0;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item, quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void Insert_WhenIndexIsLastIndexPlusOne_InsertAfterEverythingElse()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = Instance.LastIndex + 1;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<GarbageItem>> { new(item, quantity) }));
        }

        [TestMethod]
        public void Insert_WhenIndexIsLastIndex_TriggerChange()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = Instance.LastIndex + 1;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item, quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void Insert_WhenIndexIsBetweenTwoOtherEntries_SqueezeItIn()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<GarbageItem>>
            {
                entries[0],
                new(item, quantity),
                entries[1],
                entries[2]
            });
        }

        [TestMethod]
        public void Insert_WhenIndexIsBetweenTwoOtherEntries_TriggerChange()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item, quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void Insert_WhenIndexIsBetweenTwoOtherEntriesAndQuantityBustsStackSize_SqueezeMultipleEntriesBetween()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany()
                .ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Dummy.Create<GarbageItem>();
            var quantity = 221;

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<GarbageItem>>
            {
                entries[0],
                new(item, 99),
                new(item, 99),
                new(item, 23),
                entries[1],
                entries[2]
            });
        }

        [TestMethod]
        public void InsertLast_WhenIsEmpty_InsertAsFirstItem()
        {
            //Arrange
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>> { new(item, quantity) });
        }

        [TestMethod]
        public void InsertLast_WhenIsEmptyAndQuantityBustsStackSize_InsertMultipleEntries()
        {
            //Arrange
            Instance.StackSize = 99;

            var item = Dummy.Create<GarbageItem>();
            var quantity = 250;

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<GarbageItem>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 52),
            });
        }

        [TestMethod]
        public void InsertLast_WhenIsEmpty_TriggerChange()
        {
            //Arrange
            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item ,quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void InsertLast_WhenContainsItems_InsertAfterEveryOther()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<GarbageItem>> { new(item, quantity) }));
        }

        [TestMethod]
        public void InsertLast_WhenContainsItems_TriggerChange()
        {
            //Arrange
            var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Dummy.Create<GarbageItem>();
            var quantity = Dummy.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    NewValues =
                    [
                        new(item ,quantity)
                    ]
                }
            });
        }

        [TestMethod]
        public void InsertLast_WhenContainsItemsAndQuantityBustsStackSize_InsertMultipleEntriesAtTheEnd()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Dummy.Create<GarbageItem>();
            var quantity = 250;

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 52),
            }));
        }
    }


    [TestMethod]
    public void RemoveItem_WhenThereAreMultipleOccurences_RemoveTotalQuantityFromCollection()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 12);
        Instance.Add(item, 78);
        Instance.Add(item, 57);

        //Act
        Instance.Remove(item, 99);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item, 48)
            }));
    }

    [TestMethod]
    public void RemoveItem_WhenThereAreMultipleOccurencesOfFullStacksAndRemovingOne_RemoveOneStackAtTheEnd()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 99);
        Instance.Add(item, 78);
        Instance.Add(item, 99);

        //Act
        Instance.Remove(item, 99);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item, 99),
                new(item, 78),
            }));
    }

    [TestMethod]
    public void RemoveItem_Always_TriggerChange()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 12);
        Instance.Add(item, 78);
        Instance.Add(item, 57);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(item, 99);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    OldValues =
                    [
                        new(item, 99)
                    ]
                }
            });
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereAreMultipleOccurences_RemoveTotalQuantityFromCollection()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 12);
        Instance.Add(item, 78);
        Instance.Add(item, 57);

        //Act
        Instance.TryRemove(item, 99);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item, 48)
            }));
    }

    [TestMethod]
    public void TryRemoveItem_WhenThereAreMultipleOccurencesOfFullStacksAndRemovingOne_RemoveOneStackAtTheEnd()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 99);
        Instance.Add(item, 78);
        Instance.Add(item, 99);

        //Act
        Instance.TryRemove(item, 99);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item, 99),
                new(item, 78),
            }));
    }

    [TestMethod]
    public void TryRemoveItem_Always_TriggerChange()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, 12);
        Instance.Add(item, 78);
        Instance.Add(item, 57);

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(item, 99);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    OldValues =
                    [
                        new(item, 99)
                    ]
                }
            });
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_RemoveFromThoseStacks()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item.Item, 99),
                new(item.Item, 80),
            }));
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_TriggerChange()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Remove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    OldValues = [new(item.Item, quantity)]
                }
            });
    }

    [TestMethod]
    public void RemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_Throw()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 99);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 100;

        //Act
        var action = () => Instance.Remove(predicate, quantity);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item.Item, quantity, 99));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_RemoveFromThoseStacks()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<GarbageItem>>
            {
                new(item.Item, 99),
                new(item.Item, 80),
            }));
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_TriggerChange()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 200);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 21;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<GarbageItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<GarbageItem>>>
            {
                new()
                {
                    OldValues = [new(item.Item, quantity)]
                }
            });
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_RemoveThoseStacks()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 99);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 100;

        //Act
        Instance.TryRemove(predicate, quantity);

        //Assert
        Instance.Should().BeEquivalentTo(entries);
    }

    [TestMethod]
    public void TryRemovePredicate_WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_ReturnAmountRemoved()
    {
        //Arrange
        Instance.StackSize = 99;

        var entries = Dummy.Build<Entry<GarbageItem>>().With(x => x.Quantity, Dummy.Number.Between(1, 99).Create()).CreateMany().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = new Entry<GarbageItem>(Dummy.Create<GarbageItem>(), 99);
        Instance.Add(item.Item, item.Quantity);

        Func<GarbageItem, bool> predicate = x => x.Id == item.Item.Id;
        var quantity = 100;

        //Act
        var result = Instance.TryRemove(predicate, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryRemoveResult(99, 1));
    }

    [TestMethod]
    public void IndexesOfItem_WhenIsEmpty_ReturnEmptyList()
    {
        //Arrange
        var item = Dummy.Create<GarbageItem>();

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenContainsItemsButNotTheOneSought_ReturnEmpty()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenContainsOneSoughtItem_ReturnThatItemIndex()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Add(item, Dummy.Create<int>());

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 3 });
    }

    [TestMethod]
    public void IndexesOfItem_WhenContainsMultipleEntriesOfThisItem_ReturnAllIndexes()
    {
        //Arrange
        var entries = Dummy.CreateMany<Entry<GarbageItem>>(9).ToList();
        foreach (var entry in entries)
            Instance.Add(entry.Item, entry.Quantity);

        var item = Dummy.Create<GarbageItem>();
        Instance.Insert(2, item, Dummy.Create<int>());
        Instance.Insert(4, item, Dummy.Create<int>());
        Instance.Insert(7, item, Dummy.Create<int>());

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 2, 4, 7 });
    }
}