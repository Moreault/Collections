namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryListTests
{
    [TestClass]
    public class Add_Item : Tester<InventoryList<string>>
    {
        [TestMethod]
        public void WhenItemIsNull_AddStackOfNull()
        {
            //Arrange
            string item = null!;
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().Contain(new Entry<string>(item, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var item = Fixture.Create<string>();
            const int quantity = 0;

            //Act
            var action = () => Instance.Add(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsLesserThanStackSize_AddOneStackOfItemOfSpecifiedQuantity()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<string>>
            {
                new(item, quantity)
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsLesserThanStackSize_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var quantity = Fixture.Create<int>();

            var eventsArgs = new List<CollectionChangeEventArgs<Entry<string>>>();
            Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

            //Act
            Instance.Add(item, quantity);

            //Assert
            eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<string>>>
            {
                new()
                {
                    NewValues = new List<Entry<string>>
                    {
                        new(item, quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsEqualToStackSize_AddOneStackOfItemOfStackSize()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(item, stackSize);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<string>>
            {
                new(item, stackSize)
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsEqualToStackSize_Trigger()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;
            var item = Fixture.Create<string>();

            var eventsArgs = new List<CollectionChangeEventArgs<Entry<string>>>();
            Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

            //Act
            Instance.Add(item, stackSize);

            //Assert
            eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<string>>>
            {
                new()
                {
                    NewValues = new List<Entry<string>>
                    {
                        new(item, stackSize)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanStackSize_AddTwoStacksOfItems()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;
            var item = Fixture.Create<string>();
            const int quantity = 145;

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<string>>
            {
                new(item, stackSize),
                new(item, 46)
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanStackSize_Trigger()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;
            var item = Fixture.Create<string>();
            const int quantity = 145;

            var eventsArgs = new List<CollectionChangeEventArgs<Entry<string>>>();
            Instance.CollectionChanged += (sender, args) => eventsArgs.Add(args);

            //Act
            Instance.Add(item, quantity);

            //Assert
            eventsArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<string>>>
            {
                new()
                {
                    NewValues = new List<Entry<string>>
                    {
                        new(item, 145)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenDoesNotContainAnyEntryOfThisItemAndQuantityIsGreaterThanTwoStackSizes_AddThreeStacksOfItem()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;
            var item = Fixture.Create<string>();
            const int quantity = 281;

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<string>>
            {
                new(item, stackSize),
                new(item, stackSize),
                new(item, 83),
            });
        }

        [TestMethod]
        public void WhenAlreadyContainsAFullStackOfThisItem_AddNewStackWithSpecifiedQuantityAtTheEnd()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            var entries = Fixture.Build<Entry<string>>().With(x => x.Quantity, Fixture.CreateBetween(1, stackSize)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<string>();
            const int quantity = 65;
            Instance.Add(item, stackSize);

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<string>>
            {
                new(item, stackSize),
                new(item, quantity),
            }));
        }

        [TestMethod]
        public void WhenAlreadyContainsAStackOfThisItem_AddToExistingStack()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            var entries = Fixture.Build<Entry<string>>().With(x => x.Quantity, Fixture.CreateBetween(1, stackSize)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<string>();
            const int quantity = 65;
            Instance.Add(item, 25);

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<string>>
            {
                new(item, 90),
            }));
        }

        [TestMethod]
        public void WhenAlreadyContainsAStackOfThisItemThatIsAlmostFull_FullOutExistingStackAndCreateNewEntryForTheRemainder()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            var entries = Fixture.Build<Entry<string>>().With(x => x.Quantity, Fixture.CreateBetween(1, stackSize)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<string>();
            const int quantity = 65;
            Instance.Add(item, 80);

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<string>>
            {
                new(item, stackSize),
                new(item, 46),
            }));
        }
    }

    [TestClass]
    public class Add_Predicate : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            const int quantity = 0;

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenNoExistingEntryCorrespondsToPredicate_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = Fixture.Create<int>();

            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemUsingPredicateBecauseThereIsNoMatch, predicate));
        }

        [TestMethod]
        public void WhenOneEntryCorrespondsToPredicateAndQuantityIsNotBusted_AddToExistingEntry()
        {
            //Arrange
            var quantity = Fixture.Create<int>();

            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var existingEntryIndex = entries.GetRandomIndex();
            var existingEntry = entries[existingEntryIndex];
            var previousQuantity = existingEntry.Quantity;

            Func<Dummy, bool> predicate = x => x.Id == existingEntry.Item.Id;

            var expected = entries.ToList();
            expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = previousQuantity + quantity };

            //Act
            Instance.Add(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void WhenOneEntryCorrespondsToPredicateButBustsStackSize_AddNewEntryOfThisTypeToEndOfCollectionWithRemainder()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            const int quantity = 25;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 85).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var existingEntryIndex = entries.GetRandomIndex();
            var existingEntry = entries[existingEntryIndex];

            Func<Dummy, bool> predicate = x => x.Id == existingEntry.Item.Id;

            var expected = entries.ToList();
            expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = 99 };
            expected.Add(new Entry<Dummy>(existingEntry.Item, 11));

            //Act
            Instance.Add(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void WhenMultipleEntriesCorrespondToPredicateAndQuantityIsNotBustedForAny_AddToExistingEntries()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            const int quantity = 25;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();

            var level = Fixture.Create<int>();
            var entriesWithSameLevel = Fixture
                .Build<Dummy>().With(x => x.Level, level)
                .CreateMany()
                .Select(x => new Entry<Dummy>(x, Fixture.CreateBetween(1, 25))).ToList();

            entries.AddRange(entriesWithSameLevel);

            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Level == level;

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
        public void WhenMultipleEntriesCorrespondToPredicateAndQuantityIsBusted_AddNewEntriesForBustedTypesToTheEndOfCollectionWithTheirRespectiveRemainders()
        {
            //Arrange
            const int stackSize = 99;
            Instance.StackSize = stackSize;

            const int quantity = 25;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();

            var level = Fixture.Create<int>();
            var entriesWithSameLevel = Fixture
                .Build<Dummy>().With(x => x.Level, level)
                .CreateMany()
                .Select(x => new Entry<Dummy>(x, 80)).ToList();

            entries.AddRange(entriesWithSameLevel);

            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Level == level;

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
        public void Always_TriggerEvent()
        {
            //Arrange
            var quantity = Fixture.Create<int>();

            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var existingEntryIndex = entries.GetRandomIndex();
            var existingEntry = entries[existingEntryIndex];
            var previousQuantity = existingEntry.Quantity;

            Func<Dummy, bool> predicate = x => x.Id == existingEntry.Item.Id;

            var expected = entries.ToList();
            expected[existingEntryIndex] = expected[existingEntryIndex] with { Quantity = previousQuantity + quantity };

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(existingEntry.Item, quantity)
                    }

                }
            });
        }
    }

    [TestClass]
    public class InsertFirst : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_InsertAsFirstItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>> { new(item, quantity) });
        }

        [TestMethod]
        public void WhenIsEmpty_TriggerChange()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenIsEmptyAndQuantityBustsStackSize_InsertAdditionalEntriesAfterFirst()
        {
            //Arrange
            Instance.StackSize = 99;
            var item = Fixture.Create<Dummy>();
            var quantity = 300;

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 99),
                new(item, 3),
            });
        }

        [TestMethod]
        public void WhenContainsItems_InsertBeforeEveryOther()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>> { new(item, quantity) }.Concat(entries));
        }

        [TestMethod]
        public void WhenContainsItems_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenContainsItemsAndQuantityBustsStackSize_InsertMultipleEntriesBeforeEveryOther()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 300;

            //Act
            Instance.InsertFirst(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 99),
                new(item, 3),
            }.Concat(entries));
        }
    }

    [TestClass]
    public class Insert : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmptyAndInsertAtIndexZero_Add()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>>
            {
                new(item, quantity),
            });
        }

        [TestMethod]
        public void WhenIsEmptyAndInsertAtIndexZeroAndQuantityBustsStackSize_AddMultipleEntries()
        {
            //Arrange
            Instance.StackSize = 50;

            var item = Fixture.Create<Dummy>();
            var quantity = 121;

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>>
            {
                new(item, 50),
                new(item, 50),
                new(item, 21),
            });
        }

        [TestMethod]
        public void WhenIsEmptyAndInsertAtIndexZero_TriggerChange()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(0, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Insert(index, item, quantity);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsZero_InsertBeforeEverythingElse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 0;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>> { new(item, quantity) }.Concat(entries));
        }

        [TestMethod]
        public void WhenIndexIsZero_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 0;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenIndexIsLastIndexPlusOne_InsertAfterEverythingElse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = Instance.LastIndex + 1;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<Dummy>> { new(item, quantity) }));
        }

        [TestMethod]
        public void WhenIndexIsLastIndex_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = Instance.LastIndex + 1;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenIndexIsBetweenTwoOtherEntries_SqueezeItIn()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>>
            {
                entries[0],
                new(item, quantity),
                entries[1],
                entries[2]
            });
        }

        [TestMethod]
        public void WhenIndexIsBetweenTwoOtherEntries_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenIndexIsBetweenTwoOtherEntriesAndQuantityBustsStackSize_SqueezeMultipleEntriesBetween()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var index = 1;
            var item = Fixture.Create<Dummy>();
            var quantity = 221;

            //Act
            Instance.Insert(index, item, quantity);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>>
            {
                entries[0],
                new(item, 99),
                new(item, 99),
                new(item, 23),
                entries[1],
                entries[2]
            });
        }

    }

    [TestClass]
    public class InsertLast : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_InsertAsFirstItem()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>> { new(item, quantity) });
        }

        [TestMethod]
        public void WhenIsEmptyAndQuantityBustsStackSize_InsertMultipleEntries()
        {
            //Arrange
            Instance.StackSize = 99;

            var item = Fixture.Create<Dummy>();
            var quantity = 250;

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 52),
            });
        }

        [TestMethod]
        public void WhenIsEmpty_TriggerChange()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenContainsItems_InsertAfterEveryOther()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<Dummy>> { new(item, quantity) }));
        }

        [TestMethod]
        public void WhenContainsItems_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
                    {
                        new(item ,quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenContainsItemsAndQuantityBustsStackSize_InsertMultipleEntriesAtTheEnd()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 250;

            //Act
            Instance.InsertLast(item, quantity);

            //Assert
            Instance.Should().ContainInOrder(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 99),
                new(item, 52),
            }));
        }
    }

    [TestClass]
    public class Remove_Item : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = 0;

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInCollection_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseItIsNotInCollection, item));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollection_SubtractQuantityFromOccurence()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 9;

            Instance.Add(item, 15);

            //Act
            Instance.Remove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 6)
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollectionAndQuantityRemovedIsEqualToOwned_RemoveEntry()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 15;

            Instance.Add(item, 15);

            //Act
            Instance.Remove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 16;

            Instance.Add(item, 15);

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item, 16, 15));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurences_RemoveTotalQuantityFromCollection()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);
            Instance.Add(item, 78);
            Instance.Add(item, 57);

            //Act
            Instance.Remove(item, 99);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 48)
            }));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesOfFullStacksAndRemovingOne_RemoveOneStackAtTheEnd()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 99);
            Instance.Add(item, 78);
            Instance.Add(item, 99);

            //Act
            Instance.Remove(item, 99);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 78),
            }));
        }

        [TestMethod]
        public void Always_TriggerChange()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);
            Instance.Add(item, 78);
            Instance.Add(item, 57);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(item, 99);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
                    {
                        new(item, 99)
                    }
                }
            });
        }
    }

    [TestClass]
    public class TryRemove_Item : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = 0;

            //Act
            var action = () => Instance.TryRemove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInCollection_DoNotModify()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.TryRemove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInCollection_DoNotTriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, quantity);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInCollection_ReturnResultWithZeroItemsRemoved()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            var result = Instance.TryRemove(item, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(0, quantity));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollection_SubtractQuantityFromOccurence()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 9;

            Instance.Add(item, 15);

            //Act
            Instance.TryRemove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 6)
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollectionAndQuantityRemovedIsEqualToOwned_RemoveEntry()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 15;

            Instance.Add(item, 15);

            //Act
            Instance.TryRemove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_RemoveAllEntries()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 16;

            Instance.Add(item, 15);

            //Act
            Instance.TryRemove(item, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfItemInCollectionAndQuantityBustsRemaining_ReturnResultOfOneItemNotRemoved()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            var quantity = 16;

            Instance.Add(item, 15);

            //Act
            var result = Instance.TryRemove(item, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(15, 1));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurences_RemoveTotalQuantityFromCollection()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);
            Instance.Add(item, 78);
            Instance.Add(item, 57);

            //Act
            Instance.TryRemove(item, 99);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 48)
            }));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesOfFullStacksAndRemovingOne_RemoveOneStackAtTheEnd()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 99);
            Instance.Add(item, 78);
            Instance.Add(item, 99);

            //Act
            Instance.TryRemove(item, 99);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item, 99),
                new(item, 78),
            }));
        }

        [TestMethod]
        public void Always_TriggerChange()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);
            Instance.Add(item, 78);
            Instance.Add(item, 57);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, 99);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
                    {
                        new(item, 99)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Remove_Predicate : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
            var quantity = 0;

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenThereIsNoOccurence_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseThereIsNoMatch, predicate));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStack_RemoveFromThatStack()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item.Item, 179)
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStack_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>{new(item.Item, 21)}
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 221;

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item.Item, quantity, 200));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStackAndAllItemsAreRemoved_RemoveEntry()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 200;

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_RemoveFromThoseStacks()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item.Item, 99),
                new(item.Item, 80),
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_TriggerChange()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>{new(item.Item, quantity)}
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_Throw()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 99);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 100;

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, item.Item, quantity, 99));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacks_RemoveFrommAllThoseStacks()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 99;

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacks_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 35;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = itemsWithSameLevel.Select(x => new Entry<Dummy>(x, 35)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 100;

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemBecauseQuantityIsGreaterThanStock, itemsWithSameLevel.First(), quantity, 99));
        }
    }

    [TestClass]
    public class TryRemove_Predicate : Tester<InventoryList<Dummy>>
    {
        //TODO Test
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
            var quantity = 0;

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotRemoveItemUsingPredicateBecauseQuantityMustBeGreaterThanZero, quantity));
        }

        [TestMethod]
        public void WhenThereIsNoOccurence_DoNotModify()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsNoOccurence_ReturnZeroModified()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = Fixture.Create<int>();

            //Act
            var result = Instance.TryRemove(predicate, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(0, quantity));
        }

        [TestMethod]
        public void WhenThereIsNoOccurence_DoNotTriggerChanges()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == Fixture.Create<int>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStack_RemoveFromThatStack()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item.Item, 179)
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStack_ReturnAllRemoved()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            //Act
            var result = Instance.TryRemove(predicate, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(21, 0));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStack_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>{new(item.Item, 21)}
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_RemoveTheStackEntirely()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 221;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStackButRemovesMoreThatQuantity_ReturnRemovedAndNotRemoved()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 221;

            //Act
            var result = Instance.TryRemove(predicate, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(200, 21));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceOfOneStackAndAllItemsAreRemoved_RemoveEntry()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 200;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_RemoveFromThoseStacks()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Concat(new List<Entry<Dummy>>
            {
                new(item.Item, 99),
                new(item.Item, 80),
            }));
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacks_TriggerChange()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 200);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 21;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>{new(item.Item, quantity)}
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_RemoveThoseStacks()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 99);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 100;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereIsOneOccurenceThatCorrespondToMultipleStacksButTakesMoreThanAvailable_ReturnAmountRemoved()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = new Entry<Dummy>(Fixture.Create<Dummy>(), 99);
            Instance.Add(item.Item, item.Quantity);

            Func<Dummy, bool> predicate = x => x.Id == item.Item.Id;
            var quantity = 100;

            //Act
            var result = Instance.TryRemove(predicate, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(99, 1));
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacks_RemoveFromAllThoseStacks()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 99;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacks_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 35;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = itemsWithSameLevel.Select(x => new Entry<Dummy>(x, 35)).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_RemoveThoseStacks()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 100;

            //Act
            Instance.TryRemove(predicate, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesInMultipleStacksButTakesMoreThanAvailable_ReturnAmountRemoved()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, 99);

            Func<Dummy, bool> predicate = x => x.Level == level;
            var quantity = 100;

            //Act
            var result = Instance.TryRemove(predicate, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(297, 3));
        }
    }

    [TestClass]
    public class Clear_Item : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNullAndThereIsNoOccurenceOfNullInStock_DoNotTriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear((Dummy)null!);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNoOccurenceOfItemInStock_DoNotTriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(Fixture.Create<Dummy>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemIsNullAndThereIsAnOccurenceOfNullInStock_RemoveEntireStack()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            Instance.Add((Dummy)null!, Fixture.Create<int>());

            //Act
            Instance.Clear((Dummy)null!);

            //Assert
            Instance.QuantityOf((Dummy)null!).Should().Be(0);
        }

        [TestMethod]
        public void WhenItemIsNullAndThereIsAnOccurenceOfNullInStock_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var oldQuantity = Fixture.Create<int>();

            Instance.Add((Dummy)null!, oldQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear((Dummy)null!);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>()
                    {
                        new(null!, oldQuantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsAnOccurenceOfItem_RemoveEntireStack()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, Fixture.Create<int>());

            //Act
            Instance.Clear(item);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsAnOccurenceOfItem_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.Add(item, oldQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>()
                    {
                        new(item, oldQuantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurencesOfItem_RemoveThemAll()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, Fixture.Create<int>());
            Instance.Add(item, Fixture.Create<int>());
            Instance.Add(item, Fixture.Create<int>());

            //Act
            Instance.Clear(item);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }
    }

    [TestClass]
    public class Clear_Predicate : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.Clear(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotModify()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();

            //Act
            Instance.Clear(predicate);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotTriggerEvent()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(predicate);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsNoMatchForPredicate_DoNotModify()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var copy = Instance.Select(x => x with { }).ToList();

            //Act
            Instance.Clear(x => x.Id == Fixture.Create<int>());

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenThereIsNoMatchForPredicate_DoNotTriggerEvent()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(x => x.Id == Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneMatchForPredicate_RemoveThatStackOfItems()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = entries.GetRandom().Item;

            //Act
            Instance.Clear(x => x.Id == item.Id);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Where(x => x.Item != item));
        }

        [TestMethod]
        public void WhenThereIsOneMatchForPredicate_TriggerEvent()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var entry = entries.GetRandom();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(x => x.Id == entry.Item.Id);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
                    {
                        entry
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_RemoveThoseStacks()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            Instance.Clear(x => x.Id > 0);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_TriggerEvent()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(x => x.Id > 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = entries
                }
            });
        }
    }

    [TestClass]
    public class Clear : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear();

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsNotEmpty_RemoveEverything()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            Instance.Clear();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsNotEmpty_TriggerChange()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear();

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = entries
                }
            });
        }
    }

    [TestClass]
    public class QuantityOf_Item : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNullAndNoNullInStock_ReturnZero()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            Dummy item = null!;

            //Act
            var result = Instance.QuantityOf(item);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsNoEntryWithItemInStock_ReturnZero()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.QuantityOf(item);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenItemIsNullAndThereIsAnEntryWithNull_ReturnQuantityOfNull()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var expectedQuantity = Fixture.Create<int>();

            Instance.Add((Dummy)null!, expectedQuantity);

            //Act
            var result = Instance.QuantityOf((Dummy)null!);

            //Assert
            result.Should().Be(expectedQuantity);
        }

        [TestMethod]
        public void WhenThereIsEntryWithItemInStock_ReturnItsQuantity()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var entry = Fixture.Create<Entry<Dummy>>();
            Instance.Add(entry.Item, entry.Quantity);

            //Act
            var result = Instance.QuantityOf(entry.Item);

            //Assert
            result.Should().Be(entry.Quantity);
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneEntryOfItem_CompileSumOfAllEntriesQuantity()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var entry = Fixture.Create<Entry<Dummy>>();
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
    }

    [TestClass]
    public class QuantityOf_Predicate : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.QuantityOf(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();

            //Act
            var result = Instance.QuantityOf(predicate);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsNoMatch_ReturnZero()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.QuantityOf(x => x.Id == Fixture.Create<int>());

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsOneMatch_ReturnSizeOfThatStack()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.QuantityOf(x => x == entries.First().Item);

            //Assert
            result.Should().Be(entries.First().Quantity);
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_ReturnSumOfAllMatches()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.QuantityOf(x => x.Id > 0);

            //Assert
            result.Should().Be(entries.Sum(x => x.Quantity));
        }
    }

    [TestClass]
    public class IndexesOf_Item : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyList()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsItemsButNotTheOneSought_ReturnEmpty()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenContainsOneSoughtItem_ReturnThatItemIndex()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, Fixture.Create<int>());

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { 3 });
        }

        [TestMethod]
        public void WhenContainsMultipleEntriesOfThisItem_ReturnAllIndexes()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>(9).ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Insert(2, item, Fixture.Create<int>());
            Instance.Insert(4, item, Fixture.Create<int>());
            Instance.Insert(7, item, Fixture.Create<int>());

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { 2, 4, 7 });
        }

    }

    [TestClass]
    public class IndexesOf_Predicate : Tester<InventoryList<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class Search_Item : Tester<InventoryList<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class Search_Predicate : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.Search(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereIsNoOccurence_ReturnEmpty()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            //Act
            var result = Instance.Search(x => x.Id == Fixture.Create<int>());

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneOccurence_ReturnOnlyOccurence()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var itemsWithSameLevel = Fixture.Build<Dummy>().With(x => x.Level, level).CreateMany().ToList();
            foreach (var item in itemsWithSameLevel)
                Instance.Add(item, Fixture.Create<int>());

            var randomIndex = entries.GetRandomIndex();
            var random = entries[randomIndex];

            //Act
            var result = Instance.Search(x => x.Id == random.Item.Id);

            //Assert
            result.Should().BeEquivalentTo(new List<IndexedEntry<Dummy>>
            {
                new(random.Item, random.Quantity, randomIndex)
            });
        }

        [TestMethod]
        public void WhenThereAreManyOccurences_ReturnAllOccurences()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var level = Fixture.Create<int>();
            var item3 = Fixture.Build<Entry<Dummy>>().With(x => x.Item, Fixture.Build<Dummy>().With(y => y.Level, level).Create).Create();
            Instance.Add(item3.Item, item3.Quantity);
            var item4 = Fixture.Build<Entry<Dummy>>().With(x => x.Item, Fixture.Build<Dummy>().With(y => y.Level, level).Create).Create();
            Instance.Add(item4.Item, item4.Quantity);
            var item5 = Fixture.Build<Entry<Dummy>>().With(x => x.Item, Fixture.Build<Dummy>().With(y => y.Level, level).Create).Create();
            Instance.Add(item5.Item, item5.Quantity);

            //Act
            var result = Instance.Search(x => x.Level == level);

            //Assert
            result.Should().BeEquivalentTo(new List<IndexedEntry<Dummy>>
            {
                new(item3.Item, item3.Quantity, 3),
                new(item4.Item, item4.Quantity, 4),
                new(item5.Item, item5.Quantity, 5),
            });
        }
    }

    [TestClass]
    public class Swap : Tester<InventoryList<Dummy>>
    {
        [TestMethod]
        public void WhenCurrentIsNegative_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void WhenDestinationIsNegative_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void WhenCurrentIsOutsideRange_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void WhenDestinationIsOutsideRange_Throw()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void WhenCurrentAndDestinationAreEqual_DoNotModifyCollection()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void WhenCurrentAndDestinationAreWithinRange_Swap()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var current = 2;
            var destination = 1;

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().ContainInOrder(new List<Entry<Dummy>>
            {
                entries[0],
                entries[2],
                entries[1],
            });
        }
    }

    [TestClass]
    public class RemoveAt : Tester<InventoryList<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class ToStringMethod : Tester<InventoryList<string>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnMessageThatItIsEmpty()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("Empty InventoryList<String>");
        }

        [TestMethod]
        public void WhenItContainsItems_ReturnTotalStackCount()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<string>>().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be($"InventoryList<String> with {Instance.StackCount} stacks of items");
        }
    }

    [TestClass]
    public class Equals_Object : Tester<InventoryList<string>>
    {
        //TODO Test
    }

    [TestClass]
    public class Equals_Inventory : Tester<InventoryList<string>>
    {
        //TODO Test
    }

    [TestClass]
    public class Equals_Operator : Tester<InventoryList<string>>
    {
        //TODO Test
    }

    [TestClass]
    public class NotEquals_Operator : Tester<InventoryList<string>>
    {
        //TODO Test
    }

    [TestClass]
    public class Serialization : Tester<InventoryList<Dummy>>
    {
        //TODO Test
    }
}