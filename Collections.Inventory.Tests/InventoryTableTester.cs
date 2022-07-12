namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryTableTester
{
    [TestClass]
    public class LastIndex : Tester<InventoryTable<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.LastIndex;

            //Assert
            result.Should().Be(2);
        }
    }

    [TestClass]
    public class StackSize : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenNewValueIsSet_ReturnThatNewValue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            Instance.StackSize = value;

            //Assert
            Instance.StackSize.Should().Be(value);
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            var value = 0;

            //Act
            var action = () => Instance.StackSize = value;

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, Instance.GetType().GetHumanReadableName(), value));
        }

        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => Instance.StackSize = value;

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotInstantiateBecauseStackSizeMustBeGreaterThanZero, Instance.GetType().GetHumanReadableName(), value));
        }

        [TestMethod]
        public void WhenValueIsGreaterThanAllStacksInCollections_DoNotChangeQuantities()
        {
            //Arrange
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            //Act
            Instance.StackSize = 100;

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenValueIsGreaterThanAllStacksInCollections_DoNotTriggerChange()
        {
            //Arrange
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.StackSize = 100;

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenValueIsEqualToAllStacksInCollections_DoNotChangeQuantities()
        {
            //Arrange
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 99).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            //Act
            Instance.StackSize = 99;

            //Assert
            Instance.Should().BeEquivalentTo(entries);
        }

        [TestMethod]
        public void WhenValueIsEqualToAllStacksInCollections_DoNotTriggerChange()
        {
            //Arrange
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 99).CreateMany().ToList();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.StackSize = 99;

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenValueIsLessThanStacksInCollection_RemoveExcessQuantities()
        {
            //Arrange
            Instance.Add(Fixture.Create<Dummy>(), 58);
            Instance.Add(Fixture.Create<Dummy>(), 95);
            Instance.Add(Fixture.Create<Dummy>(), 72);

            //Act
            Instance.StackSize = 50;

            //Assert
            Instance.Should().BeEquivalentTo(new List<Entry<Dummy>>
            {
                new(Instance[0].Item, 50),
                new(Instance[1].Item, 50),
                new(Instance[2].Item, 50),
            });
        }

        [TestMethod]
        public void WhenValueIsLessThanStacksInCollection_TriggerChangeOnce()
        {
            //Arrange
            Instance.Add(Fixture.Create<Dummy>(), 58);
            Instance.Add(Fixture.Create<Dummy>(), 95);
            Instance.Add(Fixture.Create<Dummy>(), 72);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.StackSize = 50;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
                    {
                        new(Instance[0].Item, 8),
                        new(Instance[1].Item, 45),
                        new(Instance[2].Item, 22),
                    }
                }
            });
        }
    }

    [TestClass]
    public class Indexer : Tester<InventoryTable<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
    public class TotalCount : Tester<InventoryTable<Dummy>>
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
            var entry = Fixture.Create<Entry<Dummy>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.TotalCount;

            //Assert
            result.Should().Be(entries.Sum(x => x.Quantity));
        }
    }

    [TestClass]
    public class StackCount : Tester<InventoryTable<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
        public void Always_InitializeStackSizeWithDefaultValue()
        {
            //Arrange

            //Act
            var result = new InventoryTable<Dummy>();

            //Assert
            result.StackSize.Should().Be(DefaultValues.StackSize);
        }
    }

    [TestClass]
    public class Constructor_StackSize : Tester
    {
        [TestMethod]
        public void WhenStackSizeIsNegative_Throw()
        {
            //Arrange
            var stackSize = -Fixture.Create<int>();

            //Act
            var action = () => new InventoryTable<Dummy>(stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange

            //Act
            var action = () => new InventoryTable<Dummy>(0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsPositive_SetStackSize()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();

            //Act
            var result = new InventoryTable<Dummy>(stackSize);

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
            var action = () => new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange
            var collection = Fixture.CreateMany<Dummy>();

            //Act
            var action = () => new InventoryTable<Dummy>(collection, 0);

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
            var action = () => new InventoryTable<Dummy>(collection, stackSize);

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
            var result = new InventoryTable<Dummy>(collection, stackSize);

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
            var result = new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            result.Should().BeEquivalentTo(collection.Select(x => new Entry<Dummy>(x)));
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
            var collection = Fixture.CreateMany<Entry<Dummy>>();

            //Act
            var action = () => new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStackSizeIsZero_Throw()
        {
            //Arrange
            var collection = Fixture.CreateMany<Entry<Dummy>>();

            //Act
            var action = () => new InventoryTable<Dummy>(collection, 0);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            IEnumerable<Entry<Dummy>> collection = null!;

            //Act
            var action = () => new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithStackSize()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

            //Act
            var result = new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            result.StackSize.Should().Be(stackSize);
        }

        [TestMethod]
        public void WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithCollectionWithOneElementInEach()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

            //Act
            var result = new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            result.Should().BeEquivalentTo(collection);
        }

        [TestMethod]
        public void WhenAtLeastOneEntryHasQuantityHigherThanStackSize_Throw()
        {
            //Arrange
            var stackSize = Fixture.Create<int>();
            var collection = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, stackSize + 1).CreateMany().ToList();

            //Act
            var action = () => new InventoryTable<Dummy>(collection, stackSize);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }

    [TestClass]
    public class Add_Entry : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenEntryIsNull_Throw()
        {
            //Arrange
            Entry<Dummy> item = null!;

            //Act
            var action = () => ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenQuantityIsZero_Throw()
        {
            //Arrange
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 0).Create();

            //Act
            var action = () => ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item.Item, 0));
        }

        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, -Fixture.Create<int>()).Create();

            //Act
            var action = () => ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddItemBecauseQuantityMustBeGreaterThanZero, item.Item, item.Quantity));
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
        {
            //Arrange
            var oldQuantity = Fixture.Create<int>();
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, oldQuantity).Create();
            Instance.Add(item.Item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item with { Quantity = newQuantity });

            //Assert
            Instance.QuantityOf(item.Item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenThisItemIsAlreadyInStock_TriggerChange()
        {
            //Arrange
            var oldQuantity = Fixture.Create<int>();
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, oldQuantity).Create();
            Instance.Add(item.Item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item with { Quantity = newQuantity });

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item.Item, newQuantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_AddNewEntry()
        {
            //Arrange
            var item = Fixture.Create<Entry<Dummy>>();

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            Instance.QuantityOf(item.Item).Should().Be(item.Quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_TriggerChange()
        {
            //Arrange
            var item = Fixture.Create<Entry<Dummy>>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item.Item, item.Quantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
        {
            //Arrange
            Instance.StackSize = 99;
            var oldQuantity = 44;
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, oldQuantity).Create();
            Instance.Add(item.Item, oldQuantity);

            var newQuantity = 55;

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item with { Quantity = newQuantity });

            //Assert
            Instance.QuantityOf(item.Item).Should().Be(oldQuantity + newQuantity);
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_Throw()
        {
            //Arrange
            Instance.StackSize = 99;
            var oldQuantity = 50;
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, oldQuantity).Create();
            Instance.Add(item.Item, oldQuantity);

            var newQuantity = 50;

            //Act
            var action = () => ((ICollection<Entry<Dummy>>)Instance).Add(item with { Quantity = newQuantity });

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
        {
            //Arrange
            Instance.StackSize = 99;
            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Instance.StackSize).Create();

            //Act
            ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            Instance.QuantityOf(item.Item).Should().Be(item.Quantity);
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_Throw()
        {
            //Arrange
            Instance.StackSize = 99;

            var item = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Instance.StackSize + 1).Create();

            //Act
            var action = () => ((ICollection<Entry<Dummy>>)Instance).Add(item);

            //Assert
            action.Should().Throw<InventoryStackFullException>();
        }
    }

    [TestClass]
    public class Add : Tester<InventoryTable<Dummy>>
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
            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.Add(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(item, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item, newQuantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_AddNewEntry()
        {
            //Arrange
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
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item, quantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
        {
            //Arrange
            Instance.StackSize = 99;
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
            Instance.StackSize = 99;

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
            Instance.StackSize = 99;

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
            Instance.StackSize = 99;

            var item = Fixture.Create<Dummy>();
            var quantity = Instance.StackSize + 1;

            //Act
            var action = () => Instance.Add(item, quantity);

            //Assert
            action.Should().Throw<Exception>();
        }
    }

    [TestClass]
    public class TryAdd : Tester<InventoryTable<Dummy>>
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
            var item = Fixture.Create<Dummy>();
            var oldQuantity = Fixture.Create<int>();
            Instance.TryAdd(item, oldQuantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(item, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item, newQuantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotCurrentlyInStock_AddNewEntry()
        {
            //Arrange
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
            var item = Fixture.Create<Dummy>();
            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(item, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>> { new(item, quantity) }
                }
            });
        }

        [TestMethod]
        public void WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
        {
            //Arrange
            Instance.StackSize = 99;

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
            Instance.StackSize = 99;

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
            Instance.StackSize = 99;

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
            Instance.StackSize = 99;

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
            Instance.StackSize = 10;

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
            Instance.StackSize = 10;

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
    public class Add_Predicate : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            Func<Dummy, bool> predicate = null!;
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var index = entries.GetRandomIndex();
            var random = entries[index].Item;
            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(x => x.Id == random.Id, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var newQuantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(x => x.Id == id, newQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
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
            Instance.StackSize = 50;

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create())
                .With(x => x.Quantity, 20).CreateMany().ToList();

            var bustingEntry = Fixture.Build<Entry<Dummy>>()
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
    public class TryAdd_Predicate : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;
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
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var entry = entries.GetRandom();
            var id = entry.Item.Id;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    NewValues = new List<Entry<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = Fixture.Create<int>();

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            Instance.StackSize = 50;

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            Instance.StackSize = 50;

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = 20;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
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
            Instance.StackSize = 50;

            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var quantity = 20;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            var result = Instance.TryAdd(x => x.Id == id, quantity);

            //Assert
            result.Should().BeEquivalentTo(new TryAddResult(30, 30));
        }
    }

    [TestClass]
    public class Contains_Entry : Tester<InventoryTable<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class CopyTo : Tester<InventoryTable<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class Remove_Entry : Tester<InventoryTable<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class Remove : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(item, itemQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(item, 8);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
                    {
                        new(item, 8)
                    }
                }
            });
        }
    }

    [TestClass]
    public class TryRemove : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            var itemQuantity = Fixture.Create<int>();
            Instance.Add(item, itemQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, itemQuantity);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(item, 8);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
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
            var item = Fixture.Create<Dummy>();
            Instance.Add(item, 12);

            //Act
            var result = Instance.TryRemove(item, 28);

            //Assert
            result.Should().Be(new TryRemoveResult(12, 16));
        }
    }

    [TestClass]
    public class Remove_Predicate : Tester<InventoryTable<Dummy>>
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
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var entry = entries.GetRandom();
            var quantity = entry.Quantity;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(x => x.Id == entry.Item.Id, quantity);

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
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Remove(x => x.Id == id, 15);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class TryRemove_Predicate : Tester<InventoryTable<Dummy>>
    {
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
        public void WhenQuantityIsNegative_Throw()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var predicate = Fixture.Create<Func<Dummy, bool>>();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
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
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
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
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
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
            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, 5).CreateMany().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var item = entries.GetRandom().Item;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == item.Id, 14);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = new List<Entry<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var entry = entries.GetRandom();
            var quantity = entry.Quantity;

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == entry.Item.Id, quantity);

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
        public void WhenThereAreMultipleMatchesAndNoneBustTheLowerQuantityLimit_RemoveNumberFromThoseEntries()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
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
            var id = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>()
                .With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Id, id).Create()).With(x => x.Quantity, 25).CreateMany().ToList();

            foreach (var (dummy, entryQuantity) in entries)
                Instance.Add(dummy, entryQuantity);

            var eventArgs = new List<CollectionChangeEventArgs<Entry<Dummy>>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.TryRemove(x => x.Id == id, 15);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<Dummy>>>
            {
                new()
                {
                    OldValues = entries.Select(x => x with { Quantity = 15 }).ToList()
                }
            });
        }
    }

    [TestClass]
    public class Clear_Item : Tester<InventoryTable<Dummy>>
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
    }

    [TestClass]
    public class Clear_Predicate : Tester<InventoryTable<Dummy>>
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
    public class Clear_All : Tester<InventoryTable<Dummy>>
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
    public class QuantityOf : Tester<InventoryTable<Dummy>>
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
    }

    [TestClass]
    public class QuantityOf_Predicate : Tester<InventoryTable<Dummy>>
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
    public class IndexOf : Tester<InventoryTable<Dummy>>
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
    public class IndexesOf : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => Instance.IndexesOf(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenInventoryIsEmpty_ReturnEmpty()
        {
            //Arrange
            var predicate = Fixture.Create<Func<Dummy, bool>>();

            //Act
            var result = Instance.IndexesOf(predicate);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenInventoryIsNotEmptyButThereIsZeroMatch_ReturnEmpty()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var level = Fixture.Create<int>();

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Item, () => Fixture.Build<Dummy>().With(y => y.Level, level).Create()).CreateMany(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var otherEntries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.IndexesOf(x => x.Level == level);

            //Assert
            result.Should().BeEquivalentTo(new List<int> { 0, 1, 2 });
        }
    }

    [TestClass]
    public class Swap : Tester<InventoryTable<Dummy>>
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
    public class RemoveAt : Tester<InventoryTable<Dummy>>
    {
        //TODO Test
    }

    [TestClass]
    public class ToStringMethod : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("Empty InventoryTable<Dummy>");
        }

        [TestMethod]
        public void WhenContainsItems_ReturnAmount()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>(3).ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().BeEquivalentTo("InventoryTable<Dummy> with 3 stacks of items");
        }
    }

    [TestClass]
    public class Equals_ConcreteType : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            //Act
            var result = Instance.Equals((InventoryTable<Dummy>)null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = entries.ToInventoryTable(int.MaxValue - 1);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = entries.ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_Object : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = entries.ToInventoryTable(int.MaxValue - 1);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            object other = entries.ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Equals_Operator : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenFirstIsNullAndSecondIsNot_ReturnFalse()
        {
            //Arrange
            InventoryTable<Dummy> instance = null!;
            var other = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenFirstIsNotNullAndSecondIsNull_ReturnFalse()
        {
            //Arrange
            var instance = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);
            InventoryTable<Dummy> other = null!;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            InventoryTable<Dummy> instance = null!;
            InventoryTable<Dummy> other = null!;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)entries.ToInventoryTable(int.MaxValue - 1);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)entries.ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance == other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class NotEquals_Operator : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        public void WhenFirstIsNullAndSecondIsNot_ReturnFalse()
        {
            //Arrange
            InventoryTable<Dummy> instance = null!;
            var other = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenFirstIsNotNullAndSecondIsNull_ReturnFalse()
        {
            //Arrange
            var instance = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);
            InventoryTable<Dummy> other = null!;

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            InventoryTable<Dummy> instance = null!;
            InventoryTable<Dummy> other = null!;

            //Act
            var result = instance != other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
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
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)Fixture.CreateMany<Entry<Dummy>>().ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherHasSameItemsButDifferentStackSize_ReturnFalse()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)entries.ToInventoryTable(int.MaxValue - 1);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenStackSizesAndItemsAreTheSame_ReturnTrue()
        {
            //Arrange
            var entries = Fixture.CreateMany<Entry<Dummy>>().ToList();
            foreach (var (dummy, quantity) in entries)
                Instance.Add(dummy, quantity);

            var other = (InventoryTable<Dummy>)entries.ToInventoryTable(int.MaxValue);

            //Act
            var result = Instance != other;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Serialization : Tester<InventoryTable<Dummy>>
    {
        [TestMethod]
        [Ignore("XML not yet supported")]
        public void WhenSerializingXml_DeserializeEquivalentObject()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<Entry<Dummy>>();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var serializer = new XmlSerializer(typeof(InventoryTable<Dummy>));
            var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter);
            serializer.Serialize(xmlWriter, Instance);
            var xml = stringWriter.ToString();

            //Act
            var stringReader = new StringReader(xml);
            var result = (InventoryTable<Dummy>)serializer.Deserialize(stringReader)!;

            //Assert
            Instance.Should().BeEquivalentTo(result);
        }

        //TODO Support XML
        [TestMethod]
        [Ignore("XML not yet supported")]
        public void WhenSerializingXml_DeserializeWithCorrectStackSize()
        {
            //Arrange
            Instance.StackSize = 99;

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var serializer = new XmlSerializer(typeof(InventoryTable<Dummy>));
            var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter);
            serializer.Serialize(xmlWriter, Instance);
            var xml = stringWriter.ToString();

            //Act
            var stringReader = new StringReader(xml);
            var result = (InventoryTable<Dummy>)serializer.Deserialize(stringReader)!;

            //Assert
            result.StackSize.Should().Be(99);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingNewtonsoft_DeserializeEquivalentObject()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<Entry<Dummy>>();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var json = JsonConvert.SerializeObject(Instance);

            //Act
            var result = JsonConvert.DeserializeObject<InventoryTable<Dummy>>(json);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingNewtonsoft_DeserializeWithCorrectStackSize()
        {
            //Arrange
            ConstructWith(99);

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var json = JsonConvert.SerializeObject(Instance);

            //Act
            var result = JsonConvert.DeserializeObject<InventoryTable<Dummy>>(json);

            //Assert
            result.StackSize.Should().Be(Instance.StackSize);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingSystemText_DeserializeEquivalentObject()
        {
            //Arrange
            ConstructWith(int.MaxValue);

            var entries = Fixture.CreateMany<Entry<Dummy>>();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var json = System.Text.Json.JsonSerializer.Serialize(Instance);

            //Act
            var result = System.Text.Json.JsonSerializer.Deserialize<InventoryTable<Dummy>>(json);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenSerializingJsonUsingSystemText_DeserializeWithCorrectStackSize()
        {
            //Arrange
            ConstructWith(99);

            var entries = Fixture.Build<Entry<Dummy>>().With(x => x.Quantity, Fixture.CreateBetween(1, 99)).CreateMany();
            foreach (var entry in entries)
                Instance.Add(entry.Item, entry.Quantity);

            var json = System.Text.Json.JsonSerializer.Serialize(Instance);

            //Act
            var result = System.Text.Json.JsonSerializer.Deserialize<InventoryTable<Dummy>>(json);

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }
    }
}