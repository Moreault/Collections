using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolBX.Collections.Common;
using ToolBX.Collections.Inventory;
using ToolBX.Collections.Inventory.Resources;
using ToolBX.Eloquentest;
using ToolBX.Eloquentest.Extensions;

namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryTester
{
    [TestClass]
    public class LastIndex : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnMinusOne()
        {
            //Arrange

            //Act
            var result = Instance.LastIndex;

            //Assert
            result.Should().Be(-1);
        }

        [TestMethod]
        public void WhenIsNotEmpty_ReturnLastIndex()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.LastIndex;

            //Assert
            result.Should().Be(2);
        }
    }

    [TestClass]
    public class Indexer : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();

            //Act
            var action = () => Instance[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = Instance.LastIndex + 1;

            //Act
            var action = () => Instance[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsZero_ReturnFirstEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = 0;

            //Act
            var result = Instance[index];

            //Assert
            result.Should().BeEquivalentTo(entries.First());
        }

        [TestMethod]
        public void WhenIndexIsEqualToLastIndex_ReturnLastEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = Instance.LastIndex;

            //Act
            var result = Instance[index];

            //Assert
            result.Should().BeEquivalentTo(entries.Last());
        }
    }

    [TestClass]
    public class StackSize : Tester
    {
        [TestMethod]
        public void Always_ReturnValueInstancedWith()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();

            //Act
            var instance = new Inventory<Dummy>(stackSize);

            //Assert
            instance.StackSize.Should().Be(stackSize);
        }
    }

    [TestClass]
    public class TotalCount : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.TotalCount;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenContainsOnlyOneEntry_ReturnQuantityOfThatEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entry = Fixture.Create<InventoryEntry<Dummy>>();
            Instance.Add(entry.Item, entry.Quantity);

            //Act
            var result = Instance.TotalCount;

            //Assert
            result.Should().Be(entry.Quantity);
        }

        [TestMethod]
        public void WhenContainsMultipleEntries_ReturnSumOfAllQuantities()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.TotalCount;

            //Assert
            result.Should().Be(entries.Sum(x => x.Quantity));
        }
    }

    [TestClass]
    public class StackCount : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.StackCount;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenIsNotEmpty_ReturnNumberOfUniqueEntries()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.StackCount;

            //Assert
            result.Should().Be(entries.Count);
        }
    }

    [TestClass]
    public class Constructor_Default : Tester
    {
        [TestMethod]
        public void WhenStackSizeIsNegative_Throw()
        {
            //Arrange
            var stackSize = -Fixture.Create<int>();

            //Act
            var action = () => new Inventory<Dummy>(stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange

            //Act
            var action = () => new Inventory<Dummy>(0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsPositive_SetStackSize()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();

            //Act
            var result = new Inventory<Dummy>(stackSize);

            //Assert
            result.StackSize.Should().Be(stackSize);
        }
    }

    [TestClass]
    public class Constructor_EnumerableOfT : Tester
    {
        [TestMethod]
        public void WhenStackSizeIsNegative_Throw()
        {
            //Arrange
            var stackSize = -Fixture.Create<int>();
            var collection = Fixture.CreateMany<Dummy>();

            //Act
            var action = () => new Inventory<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange
            var collection = Fixture.CreateMany<Dummy>();

            //Act
            var action = () => new Inventory<Dummy>(collection, 0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            IEnumerable<Dummy> collection = null!;

            //Act
            var action = () => new Inventory<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithStackSize()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = new Inventory<Dummy>(collection, stackSize);

            //Assert
            result.StackSize.Should().Be(stackSize);
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithCollectionWithOneElementInEach()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = new Inventory<Dummy>(collection, stackSize);

            //Assert
            result.Should().BeEquivalentTo(collection.Select(x => new InventoryEntry<Dummy>(x)));
        }
    }

    [TestClass]
    public class Constructor_EnumerableOfEntries : Tester
    {
        [TestMethod]
        public void WhenStackSizeIsNegative_Throw()
        {
            //Arrange
            var stackSize = -Fixture.Create<int>();
            var collection = Fixture.CreateMany<InventoryEntry<Dummy>>();

            //Act
            var action = () => new Inventory<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange
            var collection = Fixture.CreateMany<InventoryEntry<Dummy>>();

            //Act
            var action = () => new Inventory<Dummy>(collection, 0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            IEnumerable<InventoryEntry<Dummy>> collection = null!;

            //Act
            var action = () => new Inventory<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithStackSize()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

            //Act
            var result = new Inventory<Dummy>(collection, stackSize);

            //Assert
            result.StackSize.Should().Be(stackSize);
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithCollectionWithOneElementInEach()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

            //Act
            var result = new Inventory<Dummy>(collection, stackSize);

            //Assert
            result.Should().BeEquivalentTo(collection);
        }

        [TestMethod]
        public void WhenAtLeastOneEntryHasQuantityHigherThanStackSize_Throw()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, stackSize + 1).CreateMany().ToList();

            //Act
            var action = () => new Inventory<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }

    [TestClass]
    public class Add : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();

            //Act
            var action = () => Instance.Add(item, 0);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, 0));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var item = Fixture.Create<Dummy>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item, quantity));
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
        {
            //Arrange
            ConstructWith(int.MaxValue);
            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.Add(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            //Act
            Instance.Add(item, newQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.Add(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(item, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>> { new(item, newQuantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_AddNewEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>> { new(item, quantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = 44;
            Instance.Add(item, oldQuantity);

            var newQuantity = 55;

            //Act
            Instance.Add(item, newQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_Throw()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = 50;
            Instance.Add(item, oldQuantity);

            var newQuantity = 50;

            //Act
            var action = () => Instance.Add(item, newQuantity);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var quantity = Instance.StackSize;

            //Act
            Instance.Add(item, quantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_Throw()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var quantity = Instance.StackSize + 1;

            //Act
            var action = () => Instance.Add(item, quantity);

            //Assert
            action.Should().Throw<Exception>();
        }

    }

    [TestClass]
    public class TryAdd : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.TryAdd(Fixture.Create<Dummy>(), 0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.TryAdd(Fixture.Create<Dummy>(), -Fixture.Create<int>());

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
        {
            //Arrange
            ConstructWith(int.MaxValue);
            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.TryAdd(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            //Act
            Instance.TryAdd(item, newQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.TryAdd(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(item, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>> { new(item, newQuantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_AddNewEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            Instance.TryAdd(item, quantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>> { new(item, quantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = 44;
            Instance.Add(item, oldQuantity);

            var newQuantity = 55;

            //Act
            Instance.TryAdd(item, newQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_SetMaximumSizeToStack()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = 50;
            Instance.Add(item, oldQuantity);

            var newQuantity = 50;

            //Act
            Instance.TryAdd(item, newQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(99);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var quantity = Instance.StackSize;

            //Act
            Instance.TryAdd(item, quantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_SetMaximumSizeToStack()
        {
            //Arrange
            ConstructWith(99);

            var item = Fixture.Create<Dummy>();
            var quantity = Instance.StackSize + 1;

            //Act
            Instance.TryAdd(item, quantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(99);
        }

        [TestMethod]
        public void WhenAllItemsAreAdded_ReturnAllItemsAdded()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            //Act
            var result = Instance.TryAdd(item, quantity);

            //Assert
            result.Should().Be(new TryAddResult(quantity, 0));
        }

        [TestMethod]
        public void WhenNoItemsAreAdded_ReturnNoItemsAdded()
        {
            //Arrange
            ConstructWith(10);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();
            Instance.Add(item, 10);

            //Act
            var result = Instance.TryAdd(item, quantity);

            //Assert
            result.Should().Be(new TryAddResult(0, quantity));
        }

        [TestMethod]
        public void WhenSomeItemsAreAdded_ReturnNumberOfItemsAddedOutOfTotal()
        {
            //Arrange
            ConstructWith(10);

            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();
            Instance.Add(item, 5);

            //Act
            var result = Instance.TryAdd(item, 10);

            //Assert
            result.Should().Be(new TryAddResult(5, 5));
        }
    }

    [TestClass]
    public class Add_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = 0;

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Add(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPredicateHasNoMatch_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var action = () => Instance.Add(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenPredicateHasOneMatch_AddToThatEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
        public void WhenPredicateHasOneMatch_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = entries.GetRandomIndex();
            var random = entries[index].Item;
            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(x => x.Id == random.Id, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>>
                    {
                        new(random, newQuantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenPredicateHasMultipleMatches_AddToAllThoseEntries()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var newQuantity = Fixture.Create<int>();

            //Act
            Instance.Add(x => x.Id == id, newQuantity);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = x.Quantity + newQuantity }));

        }

        [TestMethod]
        public void WhenPredicateHasMultipleMatches_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(x => x.Id == id, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = newQuantity }).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenPredicateHasMultipleMatchesAndOneOfThoseWillBustStackSizeWithNewQuantity_Throw()
        {
            //Arrange
            ConstructWith(50);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create())
                .With(x => x.Quantity, 20).CreateMany().ToList();

            var bustingEntry = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, Fixture.Build<Dummy>().With(y => y.Id, id).Create())
                .With(x => x.Quantity, 49).Create();

            entries.Add(bustingEntry);

            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var newQuantity = 20;

            //Act
            var action = () => Instance.Add(x => x.Id == id, newQuantity);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }

    [TestClass]
    public class TryAdd_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.TryAdd(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TryAdd(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = 0;

            //Act
            var action = () => Instance.TryAdd(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenThereIsZeroPredicateMatch_DoNotModifyStock()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var copy = Instance.Select(x => x with { }).ToList();

            //Act
            Instance.TryAdd(x => x.Id == Fixture.Create<int>(), quantity);

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenThereIsZeroPredicateMatch_DoNotTriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == Fixture.Create<int>(), quantity);

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsZeroPredicateMatch_ReturnZeroItemsAdded()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            //Act
            var result = Instance.TryAdd(x => x.Id == Fixture.Create<int>(), quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryAddResult(0, quantity));
        }

        [TestMethod]
        public void WhenThereIsOneMatch_AddToThatItemOnly()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var entry = entries.GetRandom();
            var id = entry.Item.Id;
            var newQuantity = entry.Quantity + quantity;

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            Instance.Single(x => x.Item == entry.Item).Quantity.Should().Be(newQuantity);
        }

        [TestMethod]
        public void WhenThereIsOneMatch_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var entry = entries.GetRandom();
            var id = entry.Item.Id;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<InventoryEntry<Dummy>>
                    {
                        new(entry.Item, quantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneMatch_ReturnItemsAdded()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var entry = entries.GetRandom();
            var id = entry.Item.Id;

            //Act
            var result = Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryAddResult(quantity, 0));
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_AddToAllThoseItems()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var copy = Instance.Select(x => x with { }).ToList();

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(copy.Select(x => x with { Quantity = x.Quantity + quantity }));
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = quantity }).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_ReturnAllItemsAdded()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            //Act
            var result = Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryAddResult(quantity * entries.Count, 0));
        }

        [TestMethod]
        public void WhenItemsWouldNormallyBustStackLimit_SetStacksToMaximum()
        {
            //Arrange
            ConstructWith(50);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = 20;

            var copy = Instance.Select(x => x with { }).ToList();

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            Instance.Should().BeEquivalentTo(copy.Select(x => x with { Quantity = 50 }));
        }

        [TestMethod]
        public void WhenItemsWouldNormallyBustStackLimit_TriggerChange()
        {
            //Arrange
            ConstructWith(50);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = 20;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = 10 }).ToList()
                }
            });
        }

        [TestMethod]
        public void WhenItemsWouldNormallyBustStackLimit_ReturnItemsAddedAndNotAdded()
        {
            //Arrange
            ConstructWith(50);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = 20;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            var result = Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryAddResult(30, 30));
        }
    }

    [TestClass]
    public class Remove : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = -Fixture.Create<int>();

            Instance.Add(item);

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = 0;

            Instance.Add(item);

            //Act
            var action = () => Instance.Remove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenNullAndNoNullInStock_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var action = () => Instance.Remove((Dummy)null!, Fixture.Create<int>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenItemIsNotInStock_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var action = () => Instance.Remove(Fixture.Create<Dummy>(), Fixture.Create<int>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenNullIsInStockButNotEnoughQuantity_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            Instance.Add((Dummy)null!);

            //Act
            var action = () => Instance.Remove((Dummy)null!, 2);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenItemIsInStockButNotEnoughQuantity_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item);

            //Act
            var action = () => Instance.Remove(item, 2);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenQuantityIsEqualToWhatIsInStockForItem_RemoveItemEntirely()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            //Act
            Instance.Remove(item, itemQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenQuantityIsEqualToWhatIsInStockForItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(item, itemQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        new(item, itemQuantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenQuantityIsLessThanWhatIsInStockForItem_SubstractQuantityForItem()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            //Act
            Instance.Remove(item, 8);

            //Assert
            Instance.QuantityOf(item).Should().Be(4);
        }

        [TestMethod]
        public void WhenQuantityIsLessThanWhatIsInStockForItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(item, 8);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        new(item, 8)
                    }
                }
            });
        }
    }

    [TestClass]
    public class TryRemove : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = -Fixture.Create<int>();

            Instance.Add(item);

            //Act
            var action = () => Instance.TryRemove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            var quantity = 0;

            Instance.Add(item);

            //Act
            var action = () => Instance.TryRemove(item, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenNullAndNoNullInStock_DoNotThrow()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var action = () => Instance.TryRemove((Dummy)null!, Fixture.Create<int>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenNullAndNoNullInStock_DoNotTriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove((Dummy)null!, Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemIsNotInStock_DoNotThrow()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var action = () => Instance.TryRemove(Fixture.Create<Dummy>(), Fixture.Create<int>());

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenItemIsNotInStock_DoNotTriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(Fixture.Create<Dummy>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenNullIsInStockButNotEnoughQuantity_ClearEntireStack()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            Instance.Add((Dummy)null!);

            //Act
            Instance.TryRemove((Dummy)null!, 2);

            //Assert
            Instance.QuantityOf((Dummy)null!).Should().Be(0);
        }

        [TestMethod]
        public void WhenItemIsInStockButNotEnoughQuantity_ClearEntireStack()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item);

            //Act
            Instance.TryRemove(item, 2);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenQuantityIsEqualToWhatIsInStockForItem_RemoveItemEntirely()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            //Act
            Instance.TryRemove(item, itemQuantity);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenQuantityIsEqualToWhatIsInStockForItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, itemQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        new(item, itemQuantity)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenQuantityIsLessThanWhatIsInStockForItem_SubstractQuantityForItem()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            //Act
            Instance.TryRemove(item, 8);

            //Assert
            Instance.QuantityOf(item).Should().Be(4);
        }

        [TestMethod]
        public void WhenQuantityIsLessThanWhatIsInStockForItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, 8);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        new(item, 8)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenRemovingAllItemsInStack_ReturnAllItemsRemoved()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            //Act
            var result = Instance.TryRemove(item, 12);

            //Assert
            result.Should().Be(new TryRemoveResult(12, 0));
        }

        [TestMethod]
        public void WhenRemovingNoItemInStack_ReturnNoItemsRemoved()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();

            //Act
            var result = Instance.TryRemove(item, 12);

            //Assert
            result.Should().Be(new TryRemoveResult(0, 12));
        }

        [TestMethod]
        public void WhenRemovingSomeItemsInStack_ReturnRemovedAndNotRemoved()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            //Act
            var result = Instance.TryRemove(item, 28);

            //Assert
            result.Should().Be(new TryRemoveResult(12, 16));
        }
    }

    [TestClass]
    public class Remove_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = 0;

            //Act
            var action = () => Instance.Remove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenThereIsNoMatch_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            //Act
            var action = () => Instance.Remove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            //Act
            var action = () => Instance.Remove(x => x.Id == entries.First().Item.Id, 14);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_RemoveNumberOfItemFromEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
        public void WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var entry = entries.GetRandom();
            var quantity = entry.Quantity;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(x => x.Id == entry.Item.Id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        entry
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            //Act
            Instance.Remove(x => x.Id == id, 15);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = 10 }));
        }

        [TestMethod]
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(x => x.Id == id, 15);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class TryRemove_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;
            var quantity = Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = -Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();
            var quantity = 0;

            //Act
            var action = () => Instance.TryRemove(predicate, quantity);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenThereIsNoMatch_DoNotModify()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var copy = Instance.Select(x => x with { }).ToList();

            //Act
            Instance.TryRemove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            Instance.Should().BeEquivalentTo(copy);
        }

        [TestMethod]
        public void WhenThereIsNoMatch_DoNotTriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == Fixture.Create<int>(), Fixture.Create<int>());

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_SetToZero()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var item = entries.GetRandom().Item;

            //Act
            Instance.TryRemove(x => x.Id == item.Id, 14);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_ReturnRemovedAndNotRemoved()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var item = entries.GetRandom().Item;

            //Act
            var result = Instance.TryRemove(x => x.Id == item.Id, 14);

            //Assert
            result.Should().BeEquivalentTo(new TryRemoveResult(5, 9));
        }

        [TestMethod]
        public void WhenThereIsAMatchButRemovalWouldBustTheLowerQuantityLimitOfAtLeastOneItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var item = entries.GetRandom().Item;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == item.Id, 14);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        new(item, 5)
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_RemoveNumberOfItemFromEntry()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
        public void WhenThereIsOneMatchAndRemovalDoesNotBustLowerQuantityLimit_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var entry = entries.GetRandom();
            var quantity = entry.Quantity;

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == entry.Item.Id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
                    {
                        entry
                    }
                }
            });
        }

        [TestMethod]
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            //Act
            Instance.TryRemove(x => x.Id == id, 15);

            //Assert
            Instance.Should().BeEquivalentTo(entries.Select(x => x with { Quantity = 10 }));
        }

        [TestMethod]
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == id, 15);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class Clear_Item : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNullAndThereIsNoOccurenceOfNullInStock_DoNotTriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var oldQuantity = Fixture.Create<int>();

            Instance.Add((Dummy)null!, oldQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear((Dummy)null!);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>()
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add((Dummy)null!, Fixture.Create<int>());

            //Act
            Instance.Clear(item);

            //Assert
            Instance.QuantityOf(item).Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsAnOccurenceOfItem_TriggerChange()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.Add(item, oldQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>()
                    {
                        new(item, oldQuantity)
                    }
                }
            });
        }
    }

    [TestClass]
    public class Clear_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.Clear(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotModify()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();

            //Act
            Instance.Clear(predicate);

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotTriggerEvent()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var entry = entries.GetRandom();

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(x => x.Id == entry.Item.Id);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<InventoryEntry<Dummy>>
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear(x => x.Id > 0);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = entries
                }
            });
        }
    }

    [TestClass]
    public class Clear_All : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenInventoryIsEmpty_DoNotTriggerChange()
        {
            //Arrange
            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear();

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<InventoryEntry<Dummy>>>
            {
                new()
                {
                    OldValues = entries
                }
            });
        }
    }

    [TestClass]
    public class QuantityOf : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenItemIsNullAndNoNullInStock_ReturnZero()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var entry = Fixture.Create<InventoryEntry<Dummy>>();
            Instance.Add(entry.Item, entry.Quantity);

            //Act
            var result = Instance.QuantityOf(entry.Item);

            //Assert
            result.Should().Be(entry.Quantity);
        }
    }

    [TestClass]
    public class QuantityOf_Predicate : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.QuantityOf(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenIsEmpty_ReturnZero()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();

            //Act
            var result = Instance.QuantityOf(predicate);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenThereIsNoMatch_ReturnZero()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.QuantityOf(x => x.Id > 0);

            //Assert
            result.Should().Be(entries.Sum(x => x.Quantity));
        }
    }

    [TestClass]
    public class IndexOf : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenCollectionIsEmpty_ReturnMinusOne()
        {
            //Arrange

            //Act
            var result = Instance.IndexOf(Fixture.Create<Dummy>());

            //Assert
            result.Should().Be(-1);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_ReturnMinusOne()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexOf(Fixture.Create<Dummy>());

            //Assert
            result.Should().Be(-1);
        }

        [TestMethod]
        public void WhenItemIsInCollection_ReturnItsIndex()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = entries.GetRandomIndex();

            //Act
            var result = Instance.IndexOf(Instance[index].Item);

            //Assert
            result.Should().Be(index);
        }
    }

    [TestClass]
    public class IndexesOf : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.IndexesOf(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_ReturnEmpty()
        {
            //Arrange
            var predicate = Fixture.Create<Predicate<Dummy>>();

            //Act
            var result = Instance.IndexesOf(predicate);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsNotEmptyButThereIsZeroMatch_ReturnEmpty()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexesOf(x => x.Id == Fixture.Create<int>());

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOnlyOneMatchAndItsTheLastIndex_ReturnLastIndex()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexesOf(x => x == entries.Last().Item);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { entries.Count - 1 });
        }

        [TestMethod]
        public void WhenThereIsOnlyOneMatchAndItsTheFirstIndex_ReturnFirstIndex()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexesOf(x => x == entries.First().Item);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { 0 });
        }

        [TestMethod]
        public void WhenThereAreMultipleMatches_ReturnAll()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var level = Fixture.Create<int>();

            var entries = Fixture.Build<InventoryEntry<Dummy>>().With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Level, level).Create()).CreateMany(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var otherEntries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexesOf(x => x.Level == level);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { 0, 1, 2 });
        }
    }

    [TestClass]
    public class Swap : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenCurrentIsNegative_Throw()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
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
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var current = 2;
            var destination = 1;

            //Act
            Instance.Swap(current, destination);

            //Assert
            Instance.Should().ContainInOrder(new List<InventoryEntry<Dummy>>
            {
                entries[0],
                entries[2],
                entries[1],
            });
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Empty Inventory<Dummy>");
        }

        [TestMethod]
        public void WhenContainsItems_ReturnAmount()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Inventory<Dummy> with 3 unique items");
        }
    }

    [TestClass]
    public class Equals_Interface : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((IInventory<Dummy>)null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((IInventory<Dummy>)Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameStackSizeButDifferentItems_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            IInventory<Dummy> other = Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            IInventory<Dummy> other = entries.ToInventory(int.MaxValue - 1);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            IInventory<Dummy> other = entries.ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_ConcreteType : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((Inventory<Dummy>)null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals(Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameStackSizeButDifferentItems_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = entries.ToInventory(int.MaxValue - 1);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = entries.ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_Object : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((object)null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((object)Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameStackSizeButDifferentItems_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = entries.ToInventory(int.MaxValue - 1);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = entries.ToInventory(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_Operator : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenFirstIsNullAndSecondIsNot_ReturnFalse()
        {
            //Arrange
            Inventory<Dummy> instance = null!;
            var other = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenFirstIsNotNullAndSecondIsNull_ReturnFalse()
        {
            //Arrange
            var instance = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);
            Inventory<Dummy> other = null!;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            Inventory<Dummy> instance = null!;
            Inventory<Dummy> other = null!;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance == Instance;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameStackSizeButDifferentItems_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)entries.ToInventory(int.MaxValue - 1);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)entries.ToInventory(int.MaxValue);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class NotEquals_Operator : Tester<Inventory<Dummy>>
    {
        [TestMethod]
        public void WhenFirstIsNullAndSecondIsNot_ReturnFalse()
        {
            //Arrange
            Inventory<Dummy> instance = null!;
            var other = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenFirstIsNotNullAndSecondIsNull_ReturnFalse()
        {
            //Arrange
            var instance = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);
            Inventory<Dummy> other = null!;

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            Inventory<Dummy> instance = null!;
            Inventory<Dummy> other = null!;

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance != Instance;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameStackSizeButDifferentItems_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)Fixture.CreateMany<InventoryEntry<Dummy>>().ToInventory(int.MaxValue);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)entries.ToInventory(int.MaxValue - 1);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<InventoryEntry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (Inventory<Dummy>)entries.ToInventory(int.MaxValue);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeFalse();
        }
    }
}