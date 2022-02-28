using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolBX.Collections.Inventory;
using ToolBX.Collections.Inventory.Resources;
using ToolBX.Eloquentest;

namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryExtensionsTester
{
    [TestClass]
    public class ToInventory_EnumerableOfDummy : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IEnumerable<string> collection = null!;

            //Act
            var action = () => collection.ToInventory();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsEmpty_ReturnEmptyInventory()
        {
            //Arrange
            var collection = new List<string>();

            //Act
            var result = collection.ToInventory();

            //Assert
            result.Should().BeOfType<Inventory<string>>();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenCollectionHasNoDuplicates_ReturnInventoryWithQuantityOfOneForEach()
        {
            //Arrange
            var collection = Fixture.CreateMany<string>().ToList();

            //Act
            var result = collection.ToInventory();

            //Assert
            result.Should().BeEquivalentTo(collection.Select(x => new InventoryEntry<string>(x)));
        }

        [TestMethod]
        public void WhenCollectionHasDuplicates_ReturnInventoryWithUniqueEntriesAndQuantityOfDuplicates()
        {
            //Arrange
            var collection = new List<string>
            {
                "abc",
                "def",
                "hgi",
                "def",
                "xyz",
                "hgi",
                "abc",
                "abc",
                "def"
            };

            //Act
            var result = collection.ToInventory();

            //Assert
            result.Should().BeEquivalentTo(new Inventory<string>
            {
                { "abc", 3 },
                { "def", 3 },
                { "hgi", 2 },
                { "xyz" }
            });
        }

        [TestMethod]
        public void WhenThereAreDuplicatesAndNumberOfDuplicatesBustsStackLimit_Throw()
        {
            //Arrange
            var collection = new List<string>
            {
                "abc",
                "def",
                "hgi",
                "def",
                "xyz",
                "hgi",
                "abc",
                "abc",
                "def"
            };

            //Act
            var action = () => collection.ToInventory(2);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }

    [TestClass]
    public class ToInventory_EnumerableOfEntries : Tester
    {
        [TestMethod]
        public void WhenEntriesNull_Throw()
        {
            //Arrange
            IEnumerable<InventoryEntry<string>> collection = null;

            //Act
            var action = () => collection.ToInventory(int.MaxValue);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenStackSizeIsNegative_Throw()
        {
            //Arrange
            var collection = Fixture.Create<List<InventoryEntry<string>>>();
            var stackSize = -Fixture.Create<int>();

            //Act
            var action = () => collection.ToInventory(stackSize);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, "Inventory<String>", stackSize));
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange
            var collection = Fixture.Create<List<InventoryEntry<string>>>();

            //Act
            var action = () => collection.ToInventory(0);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, "Inventory<String>", 0));
        }

        [TestMethod]
        public void WhenEntriesEmpty_ReturnEmptyInventory()
        {
            //Arrange
            var collection = Array.Empty<InventoryEntry<string>>();

            //Act
            var result = collection.ToInventory();

            //Assert
            result.Should().BeEquivalentTo(new Inventory<string>());
        }

        [TestMethod]
        public void WhenThereAreNoDuplicateEntries_ReturnEquivalentInventory()
        {
            //Arrange
            var collection = Fixture.Create<List<InventoryEntry<string>>>();

            //Act
            var result = collection.ToInventory(int.MaxValue);

            //Assert
            result.Should().BeEquivalentTo(collection);
        }

        [TestMethod]
        public void WhenThereAreDuplicateEntries_MergeDuplicates()
        {
            //Arrange
            var collection = new List<InventoryEntry<string>>
            {
                new("abc", 8),
                new("def", 81),
                new("xyz", 14),
                new("abc", 2),
                new("def", 4),
            };

            //Act
            var result = collection.ToInventory(int.MaxValue);

            //Assert
            result.Should().BeEquivalentTo(new Inventory<string>
            {
                { "abc", 10 },
                { "def", 85 },
                { "xyz", 14 }
            });
        }

        [TestMethod]
        public void WhenThereAreDuplicateEntriesAndAddingQuantitiesTogetherBustsStackLimit_Throw()
        {
            //Arrange
            var collection = new List<InventoryEntry<string>>
            {
                new("abc", 8),
                new("def", 8),
                new("xyz", 8),
                new("abc", 2),
                new("def"),
            };

            //Act
            var action = () => collection.ToInventory(9);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }
}