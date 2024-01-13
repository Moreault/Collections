namespace Collections.Inventory.Tests;

[TestClass]
public class InventoryTableTests : InventoryTester<InventoryTable<DummyItem>>
{
    [TestMethod]
    public void ParameterlessConstructor_Always_InitializeStackSizeWithDefaultValue()
    {
        //Arrange

        //Act
        var result = new InventoryTable<DummyItem>();

        //Assert
        result.StackSize.Should().Be(DefaultValues.StackSize);
    }

    [TestMethod]
    public void StackSizeConstructor_WhenStackSizeIsNegative_Throw()
    {
        //Arrange
        var stackSize = -Fixture.Create<int>();

        //Act
        var action = () => new InventoryTable<DummyItem>(stackSize);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void StackSizeConstructor_WhenStackSizeIsZero_Throw()
    {
        //Arrange

        //Act
        var action = () => new InventoryTable<DummyItem>(0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void StackSizeConstructor_WhenStackSizeIsPositive_SetStackSize()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();

        //Act
        var result = new InventoryTable<DummyItem>(stackSize);

        //Assert
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void EnumerableConstructor_WhenStackSizeIsNegative_Throw()
    {
        //Arrange
        var stackSize = -Fixture.Create<int>();
        var collection = Fixture.CreateMany<DummyItem>();

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void EnumerableConstructor_WhenStackSizeIsZero_Throw()
    {
        //Arrange
        var collection = Fixture.CreateMany<DummyItem>();

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, 0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void EnumerableConstructor_WhenCollectionIsNull_Throw()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        IEnumerable<DummyItem> collection = null!;

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void EnumerableConstructor_WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithStackSize()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        var collection = Fixture.CreateMany<DummyItem>().ToList();

        //Act
        var result = new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void EnumerableConstructor_WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithCollectionWithOneElementInEach()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        var collection = Fixture.CreateMany<DummyItem>().ToList();

        //Act
        var result = new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection.Select(x => new Entry<DummyItem>(x)));
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenStackSizeIsNegative_Throw()
    {
        //Arrange
        var stackSize = -Fixture.Create<int>();
        var collection = Fixture.CreateMany<Entry<DummyItem>>();

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenStackSizeIsZero_Throw()
    {
        //Arrange
        var collection = Fixture.CreateMany<Entry<DummyItem>>();

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, 0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenCollectionIsNull_Throw()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        IEnumerable<Entry<DummyItem>> collection = null!;

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithStackSize()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        var collection = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

        //Act
        var result = new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        result.StackSize.Should().Be(stackSize);
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenStackSizeIsPositiveAndCollectionIsNotNull_InstantiateWithCollectionWithOneElementInEach()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        var collection = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, stackSize).CreateMany().ToList();

        //Act
        var result = new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        result.Should().BeEquivalentTo(collection);
    }

    [TestMethod]
    public void EnumerableOfEntriesConstructor_WhenAtLeastOneEntryHasQuantityHigherThanStackSize_Throw()
    {
        //Arrange
        var stackSize = Fixture.Create<int>();
        var collection = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, stackSize + 1).CreateMany().ToList();

        //Act
        var action = () => new InventoryTable<DummyItem>(collection, stackSize);

        //Assert
        action.Should().Throw<InventoryStackFullException>();
    }

    [TestMethod]
    public void Add_WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;
        var item = Fixture.Create<DummyItem>();
        var oldQuantity = 44;
        Instance.Add(item, oldQuantity);

        var newQuantity = 55;

        //Act
        Instance.Add(item, newQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
    }

    [TestMethod]
    public void Add_WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_Throw()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var oldQuantity = 50;
        Instance.Add(item, oldQuantity);

        var newQuantity = 50;

        //Act
        var action = () => Instance.Add(item, newQuantity);

        //Assert
        action.Should().Throw<InventoryStackFullException>();
    }

    [TestMethod]
    public void Add_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var quantity = Instance.StackSize;

        //Act
        Instance.Add(item, quantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(quantity);
    }

    [TestMethod]
    public void Add_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_Throw()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var quantity = Instance.StackSize + 1;

        //Act
        var action = () => Instance.Add(item, quantity);

        //Assert
        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void TryAdd_WhenQuantityIsZero_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.TryAdd(Fixture.Create<DummyItem>(), 0);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryAdd_WhenQuantityIsNegative_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.TryAdd(Fixture.Create<DummyItem>(), -Fixture.Create<int>());

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryAdd_WhenThisItemIsAlreadyInStock_AddNewQuantityToOldQuantity()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var oldQuantity = Fixture.Create<int>();
        Instance.TryAdd(item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        //Act
        Instance.TryAdd(item, newQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
    }

    [TestMethod]
    public void TryAdd_WhenThisItemIsAlreadyInStock_TriggerChange()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var oldQuantity = Fixture.Create<int>();
        Instance.TryAdd(item, oldQuantity);

        var newQuantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(item, newQuantity);

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
    public void TryAdd_WhenItemIsNotCurrentlyInStock_AddNewEntry()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        Instance.TryAdd(item, quantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(quantity);
    }

    [TestMethod]
    public void TryAdd_WhenItemIsNotCurrentlyInStock_TriggerChange()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(item, quantity);

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
    public void TryAdd_WhenItemIsAlreadyInStockAndNewQuantityIsEqualToStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var oldQuantity = 44;
        Instance.Add(item, oldQuantity);

        var newQuantity = 55;

        //Act
        Instance.TryAdd(item, newQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(oldQuantity + newQuantity);
    }

    [TestMethod]
    public void TryAdd_WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_SetMaximumSizeToStack()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var oldQuantity = 50;
        Instance.Add(item, oldQuantity);

        var newQuantity = 50;

        //Act
        Instance.TryAdd(item, newQuantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(99);
    }

    [TestMethod]
    public void TryAdd_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_DoNotThrow()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var quantity = Instance.StackSize;

        //Act
        Instance.TryAdd(item, quantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(quantity);
    }

    [TestMethod]
    public void TryAdd_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_SetMaximumSizeToStack()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Create<DummyItem>();
        var quantity = Instance.StackSize + 1;

        //Act
        Instance.TryAdd(item, quantity);

        //Assert
        Instance.QuantityOf(item).Should().Be(99);
    }

    [TestMethod]
    public void TryAdd_WhenAllItemsAreAdded_ReturnAllItemsAdded()
    {
        //Arrange
        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();

        //Act
        var result = Instance.TryAdd(item, quantity);

        //Assert
        result.Should().Be(new TryAddResult(quantity, 0));
    }

    [TestMethod]
    public void TryAdd_WhenNoItemsAreAdded_ReturnNoItemsAdded()
    {
        //Arrange
        Instance.StackSize = 10;

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        Instance.Add(item, 10);

        //Act
        var result = Instance.TryAdd(item, quantity);

        //Assert
        result.Should().Be(new TryAddResult(0, quantity));
    }

    [TestMethod]
    public void TryAdd_WhenSomeItemsAreAdded_ReturnNumberOfItemsAddedOutOfTotal()
    {
        //Arrange
        Instance.StackSize = 10;

        var item = Fixture.Create<DummyItem>();
        var quantity = Fixture.Create<int>();
        Instance.Add(item, 5);

        //Act
        var result = Instance.TryAdd(item, 10);

        //Assert
        result.Should().Be(new TryAddResult(5, 5));
    }

    [TestMethod]
    public void AddEntry_WhenItemIsNotCurrentlyInStockAndQuantityIsGreaterThanStackSize_Throw()
    {
        //Arrange
        Instance.StackSize = 99;

        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, Instance.StackSize + 1).Create();

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Add(item);

        //Assert
        action.Should().Throw<InventoryStackFullException>();
    }

    [TestMethod]
    public void AddEntry_WhenItemIsAlreadyInStockAndNewQuantityIsGreaterThanStackSize_Throw()
    {
        //Arrange
        Instance.StackSize = 99;
        var oldQuantity = 50;
        var item = Fixture.Build<Entry<DummyItem>>().With(x => x.Quantity, oldQuantity).Create();
        Instance.Add(item.Item, oldQuantity);

        var newQuantity = 50;

        //Act
        var action = () => ((ICollection<Entry<DummyItem>>)Instance).Add(item with { Quantity = newQuantity });

        //Assert
        action.Should().Throw<InventoryStackFullException>();
    }

    [TestMethod]
    public void AddPredicate_WhenPredicateHasMultipleMatchesAndOneOfThoseWillBustStackSizeWithNewQuantity_Throw()
    {
        //Arrange
        Instance.StackSize = 50;

        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create())
            .With(x => x.Quantity, 20).CreateMany().ToList();

        var bustingEntry = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, Fixture.Build<DummyItem>().With(y => y.Id, id).Create())
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

    [TestMethod]
    public void TryAddPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<DummyItem, bool> predicate = null!;
        var quantity = Fixture.Create<int>();

        //Act
        var action = () => Instance.TryAdd(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryAddPredicate_WhenQuantityIsNegative_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = -Fixture.Create<int>();

        //Act
        var action = () => Instance.TryAdd(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryAddPredicate_WhenQuantityIsZero_Throw()
    {
        //Arrange
        var predicate = Fixture.Create<Func<DummyItem, bool>>();
        var quantity = 0;

        //Act
        var action = () => Instance.TryAdd(predicate, quantity);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TryAddPredicate_WhenThereIsZeroPredicateMatch_DoNotModifyStock()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
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
    public void TryAddPredicate_WhenThereIsZeroPredicateMatch_DoNotTriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(x => x.Id == Fixture.Create<int>(), quantity);

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void TryAddPredicate_WhenThereIsZeroPredicateMatch_ReturnZeroItemsAdded()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = Fixture.Create<int>();

        //Act
        var result = Instance.TryAdd(x => x.Id == Fixture.Create<int>(), quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryAddResult(0, quantity));
    }

    [TestMethod]
    public void TryAddPredicate_WhenThereIsOneMatch_AddToThatItemOnly()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
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
    public void TryAddPredicate_WhenThereIsOneMatch_TriggerChange()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = Fixture.Create<int>();

        var entry = entries.GetRandom();
        var id = entry.Item.Id;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(x => x.Id == id, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = new List<Entry<DummyItem>>
                    {
                        new(entry.Item, quantity)
                    }
                }
            });
    }

    [TestMethod]
    public void TryAddPredicate_WhenThereIsOneMatch_ReturnItemsAdded()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
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
    public void TryAddPredicate_WhenThereAreMultipleMatches_AddToAllThoseItems()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).CreateMany().ToList();

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
    public void TryAddPredicate_WhenThereAreMultipleMatches_TriggerChange()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = Fixture.Create<int>();

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(x => x.Id == id, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = quantity }).ToList()
                }
            });
    }

    [TestMethod]
    public void TryAddPredicate_WhenThereAreMultipleMatches_ReturnAllItemsAdded()
    {
        //Arrange
        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = Fixture.Create<int>();

        //Act
        var result = Instance.TryAdd(x => x.Id == id, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryAddResult(quantity * entries.Count, 0));
    }

    [TestMethod]
    public void TryAddPredicate_WhenItemsWouldNormallyBustStackLimit_SetStacksToMaximum()
    {
        //Arrange
        Instance.StackSize = 50;

        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

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
    public void TryAddPredicate_WhenItemsWouldNormallyBustStackLimit_TriggerChange()
    {
        //Arrange
        Instance.StackSize = 50;

        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = 20;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.TryAdd(x => x.Id == id, quantity);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Entry<DummyItem>>>
            {
                new()
                {
                    NewValues = entries.Select(x => x with { Quantity = 10 }).ToList()
                }
            });
    }

    [TestMethod]
    public void TryAddPredicate_WhenItemsWouldNormallyBustStackLimit_ReturnItemsAddedAndNotAdded()
    {
        //Arrange
        Instance.StackSize = 50;

        var id = Fixture.Create<int>();

        var entries = Fixture.Build<Entry<DummyItem>>()
            .With(x => x.Item, () => Fixture.Build<DummyItem>().With(y => y.Id, id).Create()).With(x => x.Quantity, 40).CreateMany().ToList();

        foreach (var (dummy, entryQuantity) in entries)
            Instance.Add(dummy, entryQuantity);

        var quantity = 20;

        var eventArgs = new List<CollectionChangeEventArgs<Entry<DummyItem>>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        var result = Instance.TryAdd(x => x.Id == id, quantity);

        //Assert
        result.Should().BeEquivalentTo(new TryAddResult(30, 30));
    }

    [TestMethod]
    public void IndexOf_WhenCollectionIsEmpty_ReturnMinusOne()
    {
        //Arrange

        //Act
        var result = Instance.IndexOf(Fixture.Create<DummyItem>());

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void IndexOf_WhenItemIsNotInCollection_ReturnMinusOne()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        //Act
        var result = Instance.IndexOf(Fixture.Create<DummyItem>());

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void IndexOf_WhenItemIsInCollection_ReturnItsIndex()
    {
        //Arrange
        var entries = Fixture.CreateMany<Entry<DummyItem>>().ToList();
        foreach (var (dummy, quantity) in entries)
            Instance.Add(dummy, quantity);

        var index = entries.GetRandomIndex();

        //Act
        var result = Instance.IndexOf(Instance[index].Item);

        //Assert
        result.Should().Be(index);
    }

}