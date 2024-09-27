namespace ToolBX.Collections.UnitTesting.ObservableList;

public abstract class ObservableListTester<TList, TItem> : Tester<TList> where TList : ObservableList<TItem>, new()
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void LastIndex_WhenCollectionIsEmpty_ReturnMinusOdne()
    {
        //Arrange
        var observableList = new TList();

        //Act
        var result = observableList.LastIndex;

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void LastIndex_WhenCollectionContainsOneItem_ReturnZero()
    {
        //Arrange
        var observableList = new TList { Dummy.Create<TItem>() };

        //Act
        var result = observableList.LastIndex;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void LastIndex_WhenCollectionContainsABunchOfItems_ReturnLastIndex()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        //Act
        var result = observableList.LastIndex;

        //Assert
        result.Should().Be(observableList.Count - 1);
    }

    [TestMethod]
    public void IsReadOnly_Always_ReturnFalse()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        //Act
        var result = ((ICollection<TItem>)observableList).IsReadOnly;

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IndexerGet_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = -1;

        //Act
        var action = () => observableList[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void IndexerGet_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = observableList.LastIndex + 1;

        //Act
        var action = () => observableList[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void IndexerGet_WhenIndexIsZero_ReturnFirstItem()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = 0;

        //Act
        var result = observableList[index];

        //Assert
        result.Should().Be(observableList.First());
    }

    [TestMethod]
    public void IndexerGet_WhenIndexIsLastIndex_ReturnLastItem()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = observableList.LastIndex;

        //Act
        var result = observableList[index];

        //Assert
        result.Should().Be(observableList.Last());
    }

    [TestMethod]
    public void IndexerGet_WhenIndexIsWhatever_ReturnWhatever()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var list = observableList.ToList();
        var index = observableList.GetRandomIndex();

        //Act
        var result = observableList[index];

        //Assert
        result.Should().Be(list[index]);
    }

    [TestMethod]
    public void IndexerSet_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = -1;
        var value = Dummy.Create<TItem>();

        //Act
        var action = () => observableList[index] = value;

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void IndexerSet_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = observableList.LastIndex + 1;
        var value = Dummy.Create<TItem>();

        //Act
        var action = () => observableList[index] = value;

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void IndexerSet_WhenIndexIsWithinBounds_ReplaceItemAtIndex()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = observableList.GetRandomIndex();
        var value = Dummy.Create<TItem>();

        //Act
        observableList[index] = value;

        //Assert
        observableList[index].Should().Be(value);
    }

    [TestMethod]
    public void IndexerSet_WhenIndexIsWithinBounds_TriggerCollectionChanged()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        var index = observableList.GetRandomIndex();
        var value = Dummy.Create<TItem>();

        var oldItem = observableList[index];

        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        observableList.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        observableList[index] = value;

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    OldValues = new[] { oldItem },
                    NewValues = new[] { value }
                }
            });
    }

    [TestMethod]
    public void Add_WhenUsingTheICollectionOverload_AddToObservableList()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        //Act
        ((ICollection<TItem>)Instance).Add(item);

        //Assert
        Instance.Should().Contain(item);
    }

    [TestMethod]
    public void Add_WhenUsingTheICollectionOverload_SetCountToOne()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        //Act
        ((ICollection<TItem>)Instance).Add(item);

        //Assert
        Instance.Count.Should().Be(1);
    }

    [TestMethod]
    public void Add_WhenAddingSingleItem_ItemIsAdded()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        //Act
        Instance.Add(item);

        //Assert
        Instance.Should().Contain(item);
    }

    [TestMethod]
    public void Add_WhenAddingSingleItem_TriggerChange()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(item);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { NewValues = new[] { item } }
            });
    }

    [TestMethod]
    public void Add_WhenAddingSingleItem_SetCountToOne()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        //Act
        Instance.Add(item);

        //Assert
        Instance.Count.Should().Be(1);
    }

    [TestMethod]
    public void Add_WhenAddingABunchOfItems_AddAllOfThem()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();

        //Act
        Instance.Add(items);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Add_WhenAddingABunchOfItems_TriggerChangeOnlyOnce()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        var triggered = 0;
        Instance.CollectionChanged += (_, _) => triggered++;

        //Act
        Instance.Add(items);

        //Assert
        triggered.Should().Be(1);
    }

    [TestMethod]
    public void Add_WhenAddingABunchOfItems_TriggerChangeWithAllNewItems()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(items);

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { NewValues = items }
            });
    }

    [TestMethod]
    public void Add_WhenAddingABunchOfItems_SetCountToTwenty()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Add(items);

        //Assert
        Instance.Count.Should().Be(20);
    }

    [TestMethod]
    public void Add_WhenAddingNullCollection_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Add((IEnumerable<TItem>)null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Add_WhenAddingSingleNullItem_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.Add(null!);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Add_WhenAddingSingleNullItem_AddNull()
    {
        //Arrange

        //Act
        Instance.Add(null!);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem> { (TItem)(object)null! });
    }

    [TestMethod]
    public void Add_WhenAddingMultipleNullItems_DoNotThrow()
    {
        //Arrange

        //Act
        var action = () => Instance.Add(new List<TItem> { (TItem)(object)null!, (TItem)(object)null!, (TItem)(object)null! });

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Add_WhenAddingMultipleNullItems_AddAllNullItems()
    {
        //Arrange

        //Act
        Instance.Add((TItem)(object)null!, (TItem)(object)null!, (TItem)(object)null!);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem> { (TItem)(object)null!, (TItem)(object)null!, (TItem)(object)null! });
    }

    [TestMethod]
    public void Clear_Always_RemoveAllItems()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        Instance.Add(items);

        //Act
        Instance.Clear();

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void Clear_WhenContainsItems_TriggerCollectionChanged()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear();

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Clear_Always_SetCountToZero()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(20).ToArray();
        Instance.Add(items);

        //Act
        Instance.Clear();

        //Assert
        Instance.Count.Should().Be(0);
    }

    [TestMethod]
    public void Clear_WhenThereAreThreeItems_EventShouldOnlyHaveThreeItemsAndNotAFourthNullObject()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToArray();
        Instance.Add(items);

        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear();

        //Assert
        eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void Clear_WhenIsAlreadyEmpty_DoNotTriggerEvent()
    {
        //Arrange
        var eventArgs = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

        //Act
        Instance.Clear();

        //Assert
        eventArgs.Should().BeEmpty();
    }

    [TestMethod]
    public void Contains_WhenItemIsInObservableList_ReturnTrue()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);

        //Act
        var result = ((ICollection<TItem>)Instance).Contains(item);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Contains_WhenItemIsNotInObservableList_ReturnFalse()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        //Act
        var result = ((ICollection<TItem>)Instance).Contains(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CopyTo_Always_CopyToArray()
    {
        //Arrange
        var array = new TItem[3];
        Instance.Add(Dummy.CreateMany<TItem>(3));

        //Act
        ((ICollection<TItem>)Instance).CopyTo(array, 0);

        //Assert
        array.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void Copy_WhenIndexIsZero_ReturnAnExactCopy()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void Copy_WhenIndexIsZero_ShouldNotBeSameReference()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.Copy();

        //Assert
        result.Should().NotBeSameAs(Instance);
    }

    [TestMethod]
    public void Copy_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var startingIndex = -Dummy.Create<int>();

        //Act
        var action = () => Instance.Copy(startingIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Copy_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var startingIndex = Instance.LastIndex + 1;

        //Act
        var action = () => Instance.Copy(startingIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Copy_WhenStartingIndexIsLastIndex_ReturnObservableListWithOnlyTheLastItemInIt()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var startingIndex = Instance.LastIndex;

        //Act
        var result = Instance.Copy(startingIndex);

        //Assert
        result.Should().BeEquivalentTo(new List<TItem> { Instance.Last() });
    }

    [TestMethod]
    public void Copy_WhenIndexIsGreaterThanZero_CopyObservableListStartingFromIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(6).ToList();
        var startingIndex = 3;
        Instance.Add(items);

        //Act
        var result = Instance.Copy(startingIndex);

        //Assert
        result.Should().BeEquivalentTo(new List<TItem>
            {
                items[3], items[4], items[5]
            });
    }

    [TestMethod]
    public void CopyWithCount_WhenStartingIndexIsNegative_Throw()
    {
        //Arrange
        var index = -Dummy.Create<int>();
        var count = Dummy.Create<int>();

        //Act
        var action = () => Instance.Copy(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void CopyWithCount_WhenStartingIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>(3));

        var index = 3;
        var count = Dummy.Create<int>();

        //Act
        var action = () => Instance.Copy(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void CopyWithCount_WhenCountIsNegative_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>(3));
        var index = Instance.GetRandomIndex();
        var count = -Dummy.Create<int>();

        //Act
        var action = () => Instance.Copy(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void CopyWithCount_WhenStartingIndexPlusCountIsGreaterThanCollectionSize_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>(3));
        var index = 0;
        var count = 4;

        //Act
        var action = () => Instance.Copy(index, count);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CopyWithCount_WhenStartingIndexPlusCountIsWithinBounds_ReturnACopyOfThat()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(8).ToList();
        Instance.Add(items);

        var index = 3;
        var count = 2;

        //Act
        var result = Instance.Copy(index, count);

        //Assert
        result.Should().BeEquivalentTo(new List<TItem>
            {
                items[3], items[4]
            });
    }

    [TestMethod]
    public void CopyWithCount_WhenStartingIndexIsZeroAndCountIsTheSameAsCollection_ReturnCopyOfObservableList()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(8).ToList();
        Instance.Add(items);

        var index = 0;
        var count = 8;

        //Act
        var result = Instance.Copy(index, count);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void IndexOfIList_WhenItemIsInObservableList_ReturnItsIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var expectedIndex = items.GetRandomIndex();

        //Act
        var result = ((IList<TItem>)Instance).IndexOf(items[expectedIndex]);

        //Assert
        result.Should().Be(expectedIndex);
    }

    [TestMethod]
    public void IndexOfIList_WhenItemIsNotInObservableList_ReturnMinusOne()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();

        //Act
        var result = ((IList<TItem>)Instance).IndexOf(item);

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void FirstIndexOfItem_WhenItemIsInObservableList_ReturnItsIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var expectedIndex = items.GetRandomIndex();

        //Act
        var result = Instance.FirstIndexOf(items[expectedIndex]);

        //Assert
        result.Should().Be(expectedIndex);
    }

    [TestMethod]
    public void FirstIndexOfItem_WhenItemIsNotInObservableList_ReturnMinusOne()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();

        //Act
        var result = Instance.FirstIndexOf(item);

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void FirstIndexOfPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.FirstIndexOf(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void FirstIndexOfPredicate_WhenContainsDummyThatFitsConditions_ReturnIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var index = items.GetRandomIndex();
        var item = items[index];

        //Act
        var result = Instance.FirstIndexOf(x => x!.Equals(item));

        //Assert
        result.Should().Be(index);
    }

    [TestMethod]
    public void FirstIndexOfPredicate_WhenThereAreMultipleDummiesThatFitConditions_ReturnOnlyTheFirst()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        var item = items.GetRandom();
        var identicalItems = new List<TItem> { item, item, item };

        Instance.Add(identicalItems);
        Instance.Add(items);

        //Act
        var result = Instance.FirstIndexOf(x => x!.Equals(item));

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FirstIndexOfPredicate_WhenNoDummiesFitConditions_ReturnMinusOne()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();

        //Act
        var result = Instance.FirstIndexOf(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void LastIndexOfItem_WhenItemIsInObservableList_ReturnItsIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var expectedIndex = items.GetRandomIndex();

        //Act
        var result = Instance.LastIndexOf(items[expectedIndex]);

        //Assert
        result.Should().Be(expectedIndex);
    }

    [TestMethod]
    public void LastIndexOfItem_WhenItemIsNotInObservableList_ReturnMinusOne()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();

        //Act
        var result = Instance.LastIndexOf(item);

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void LastIndexOfPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.LastIndexOf(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void LastIndexOfPredicate_WhenContainsDummyThatFitsConditions_ReturnIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var index = items.GetRandomIndex();
        var item = items[index];

        //Act
        var result = Instance.LastIndexOf(x => x!.Equals(item));

        //Assert
        result.Should().Be(index);
    }

    [TestMethod]
    public void LastIndexOfPredicate_WhenThereAreMultipleDummiesThatFitConditions_ReturnOnlyTheLast()
    {
        //Arrange
        var id = Dummy.Create<int>();

        var items = Dummy.CreateMany<TItem>(3).ToList();

        var itemOfInterest = Dummy.Create<TItem>();
        items.AddRange(itemOfInterest, itemOfInterest, itemOfInterest);
        items.Add(Dummy.Create<TItem>());

        Instance.Add(items);

        //Act
        var result = Instance.LastIndexOf(x => x!.Equals(itemOfInterest));

        //Assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void LastIndexOfPredicate_WhenNoDummiesFitConditions_ReturnMinusOne()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.LastIndexOf(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereAreNoOccurences_ReturnEmpty()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereIsOnlyOneOccurence_ReturnItsIndexInAObservableList()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);
        Instance.Add(item);

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { Instance.LastIndex });
    }

    [TestMethod]
    public void IndexesOfItem_WhenThereAreMultipleOccurences_ReturnThemAll()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(item);
        Instance.Add(items);
        Instance.Add(item);
        Instance.Add(item);

        //Act
        var result = Instance.IndexesOf(item);

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 0, 4, 5 });
    }

    [TestMethod]
    public void IndexesOfPrecidate_WhenPredicateIsNull_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.IndexesOf(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void IndexesOfPrecidate_WhenThereAreNoCorrespondingItems_ReturnEmpty()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.IndexesOf(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void IndexesOfPrecidate_WhenThereIsOneCorrespondingItem_ReturnIndex()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();

        var item = Dummy.Create<TItem>();
        Instance.Add(items);
        Instance.Add(item);

        //Act
        var result = Instance.IndexesOf(x => x!.Equals(item));

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 3 });
    }

    [TestMethod]
    public void IndexesOfPrecidate_WhenThereAreMultipleCorrespondingItems_ReturnThoseIndexes()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var id = Dummy.Create<int>();
        var itemsOfInterest = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(itemsOfInterest);

        //Act
        var result = Instance.IndexesOf(x => itemsOfInterest.Contains(x));

        //Assert
        result.Should().BeEquivalentTo(new List<int> { 3, 4, 5 });
    }

    [TestMethod]
    public void Swap_WhenCurrentIndexIsNegative_Throw()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = -1;
        var destinationIndex = 1;

        //Act
        var action = () => Instance.Swap(currentIndex, destinationIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenCurrentIndexIsOutOfRange_Throw()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = Instance.LastIndex + 1;
        var destinationIndex = 1;

        //Act
        var action = () => Instance.Swap(currentIndex, destinationIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenDestinationIndexIsNegative_Throw()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = 0;
        var destinationIndex = -1;

        //Act
        var action = () => Instance.Swap(currentIndex, destinationIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenDestinationIndexIsOutOfRange_Throw()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = 2;
        var destinationIndex = Instance.LastIndex + 1;

        //Act
        var action = () => Instance.Swap(currentIndex, destinationIndex);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationIndexesAreEqual_DoNotModifyCollection()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>().ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = entries.GetRandomIndex();
        var destinationIndex = currentIndex;

        var copy = Instance.Copy();

        //Act
        Instance.Swap(currentIndex, destinationIndex);

        //Assert
        Instance.Should().ContainInOrder(copy);
    }

    [TestMethod]
    public void Swap_WhenCurrentAndDestinationAreBothInTheMiddleOfCollection_SwapThemTogether()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>(4).ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = 2;
        var destinationIndex = 1;

        //Act
        Instance.Swap(currentIndex, destinationIndex);

        //Assert
        Instance.Should().ContainInOrder(new List<TItem>
        {
            entries[0],
            entries[2],
            entries[1],
            entries[3]
        });
    }

    [TestMethod]
    public void Swap_WhenSwappingFirstWithLastIndex_Swap()
    {
        //Arrange
        var entries = Dummy.CreateMany<TItem>(4).ToList();
        foreach (var entry in entries)
            Instance.Add(entry);

        var currentIndex = Instance.LastIndex;
        var destinationIndex = 0;

        //Act
        Instance.Swap(currentIndex, destinationIndex);

        //Assert
        Instance.Should().ContainInOrder(new List<TItem>
        {
            entries[3],
            entries[1],
            entries[2],
            entries[0]
        });
    }

    [TestMethod]
    public void TrimStartDownTo_WhenMaxSizeIsNegative_Throw()
    {
        //Arrange
        var maxSize = -Dummy.Create<int>();

        //Act
        var action = () => Instance.TrimStartDownTo(maxSize);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TrimStartRequiresPositiveMaxSize, maxSize));
    }

    [TestMethod]
    public void TrimStartDownTo_WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(0);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsAndTrimToZero_RemoveEverything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimStartDownTo(0);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsAndTrimToZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(0);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimStartDownTo(items.Count + 1);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(items.Count + 1);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimStartDownTo(items.Count);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(items.Count);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsAndTrimToHalf_RemoveFirstHalfOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(12).ToList();
        Instance.Add(items);

        //Act
        Instance.TrimStartDownTo(6);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
        {
            items[6],
            items[7],
            items[8],
            items[9],
            items[10],
            items[11],
        });
    }

    [TestMethod]
    public void TrimStartDownTo_WhenContainsItemsAndTrimToHalf_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(12).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimStartDownTo(6);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new()
            {
                OldValues =
                [
                    items[0],
                    items[1],
                    items[2],
                    items[3],
                    items[4],
                    items[5],
                ]
            }
        });
    }

    [TestMethod]
    public void TrimEndDownTo_WhenMaxSizeIsNegative_Throw()
    {
        //Arrange
        var maxSize = -Dummy.Create<int>();

        //Act
        var action = () => Instance.TrimEndDownTo(maxSize);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.TrimEndRequiresPositiveMaxSize, maxSize));
    }

    [TestMethod]
    public void TrimEndDownTo_WhenTrimEmptyCollectionToZero_DoNotTriggerEvent()
    {
        //Arrange
        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(0);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsAndTrimToZero_RemoveEverything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimEndDownTo(0);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsAndTrimToZero_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(0);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { OldValues = items }
            });
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimEndDownTo(items.Count + 1);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsButTrimIsMoreThanCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(items.Count + 1);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotRemoveAnything()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TrimEndDownTo(items.Count);

        //Assert
        Instance.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsButTrimIsEqualToCount_DoNotTrigger()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(items.Count);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsAndTrimToHalf_RemoveFirstHalfOfItems()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(12).ToList();
        Instance.Add(items);

        //Act
        Instance.TrimEndDownTo(6);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0],
                items[1],
                items[2],
                items[3],
                items[4],
                items[5],
            });
    }

    [TestMethod]
    public void TrimEndDownTo_WhenContainsItemsAndTrimToHalf_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(12).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TrimEndDownTo(6);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    OldValues =
                    [
                        items[6],
                        items[7],
                        items[8],
                        items[9],
                        items[10],
                        items[11],
                    ]
                }
            });
    }

    [TestMethod]
    public void TryRemoveAllItem_WhenContainsItem_RemoveAllOfThem()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.TryRemoveAll(item);

        //Assert
        Instance.Should().BeEquivalentTo(otherItems);
    }

    [TestMethod]
    public void TryRemoveAllItem_WhenDoesNotContainItem_DoNotThrow()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        var action = () => Instance.TryRemoveAll(item);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllItem_WhenContainsItem_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.TryRemoveAll(item);

        //Assert
        Instance.Should().HaveCount(otherItems.Count);
    }

    [TestMethod]
    public void TryRemoveAllItem_WhenContainsItem_TriggerEventOnce()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>> { new() { OldValues = [item, item, item] } });
    }

    [TestMethod]
    public void RemoveAllItem_WhenContainsItem_RemoveAllOfThem()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.RemoveAll(item);

        //Assert
        Instance.Should().BeEquivalentTo(otherItems);
    }

    [TestMethod]
    public void RemoveAllItem_WhenDoesNotContainItem_Throw()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        var action = () => Instance.RemoveAll(item);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveAllItem_WhenContainsItem_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.RemoveAll(item);

        //Assert
        Instance.Should().HaveCount(otherItems.Count);
    }

    [TestMethod]
    public void RemoveAllItem_WhenContainsItem_TriggerEventOnce()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item);
        Instance.Add(item);
        Instance.Add(item);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>> { new() { OldValues = [item, item, item] } });
    }

    [TestMethod]
    public void RemoveAllParams_WhenOneItemIsNotInCollection_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToArray();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, Instance.GetType().GetHumanReadableName()));
    }

    [TestMethod]
    public void RemoveAllParams_WhenItemsIsEmpty_Throw()
    {
        //Arrange
        var items = Array.Empty<TItem>();

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage("items should not be empty");
    }

    [TestMethod]
    public void RemoveAllParams_WhenNoneOfTheItemsAreInCollection_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToArray();

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, Instance.GetType().GetHumanReadableName()));
    }

    [TestMethod]
    public void RemoveAllParams_WhenOneItemIsNotInCollection_DoNotRemoveTheOthers()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.Concat(Dummy.Create<TItem>()).ToArray();

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveAllParams_WhenOneItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.Concat(Dummy.Create<TItem>()).ToArray();

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllParams_WhenAllItemsAreInCollection_RemoveAllOfThem()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToArray();
        Instance.Add(items);

        //Act
        Instance.RemoveAll(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveAllParams_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToArray();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = items}
            });
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<TItem> items = null!;

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenItemsIsEmpty_Throw()
    {
        //Arrange
        var items = new List<TItem>();

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage("items should not be empty");
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenOneItemIsNotInCollection_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, Instance.GetType().GetHumanReadableName()));
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenNoneOfTheItemsAreInCollection_Throw()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.CannotRemoveItemsBecauseOneIsNotInCollection, Instance.GetType().GetHumanReadableName()));
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenOneItemIsNotInCollection_DoNotRemoveTheOthers()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenOneItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        var action = () => Instance.RemoveAll(items);

        //Assert
        action.Should().Throw<InvalidOperationException>();
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenAllItemsAreInCollection_RemoveAllOfThem()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.RemoveAll(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void RemoveAllEnumerable_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = items}
            });
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenOneItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        var action = () => Instance.TryRemoveAll(items.ToArray());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenItemsIsEmpty_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        //Act
        var action = () => Instance.TryRemoveAll(Array.Empty<TItem>());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenItemsIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(Array.Empty<TItem>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenNoneOfTheItemsAreInCollection_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToArray();

        //Act
        var action = () => Instance.TryRemoveAll(items);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenOneItemIsNotInCollection_RemoveThose()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        Instance.TryRemoveAll(items.ToArray());

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenOneItemIsNotInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(items.ToArray());

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = content}
            });
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenAllItemsAreInCollection_RemoveAllOfThem()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveAll(items.ToArray());

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveAllParams_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(items.ToArray());

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = items}
            });
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        IEnumerable<TItem> items = null!;

        //Act
        var action = () => Instance.TryRemoveAll(items);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenItemsIsEmpty_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        //Act
        var action = () => Instance.TryRemoveAll(new List<TItem>());

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenItemsIsEmpty_DoNotTriggerChange()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(new List<TItem>());

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenOneItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        var action = () => Instance.TryRemoveAll(items);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenNoneOfTheItemsAreInCollection_DoNotThrow()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();

        //Act
        var action = () => Instance.TryRemoveAll(items);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenOneItemIsNotInCollection_RemoveThose()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        //Act
        Instance.TryRemoveAll(items);

        //Assert
        Instance.Should().BeEmpty();
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenOneItemIsNotInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(content);

        var items = content.ToList();
        items.Add(Dummy.Create<TItem>());

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = content}
            });
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenAllItemsAreInCollection_RemoveAllOfThem()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveAll(items);

        //Assert
        Instance.Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void TryRemoveAllEnumerable_WhenAllItemsAreInCollection_TriggerEvent()
    {
        //Arrange
        var content = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(content);

        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = items}
            });
    }

    [TestMethod]
    public void TryRemoveAllPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.TryRemoveAll(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryRemoveAllPredicate_WhenContainsNoItemCorrespondingToPredicate_DoNotThrow()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        var action = () => Instance.TryRemoveAll(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveAllPredicate_WhenContainsItemsThatCorrespondToPredicate_RemoveThem()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        Instance.Shuffle();

        //Act
        Instance.TryRemoveAll(x => x!.Equals(toRemove));

        //Assert
        Instance.Should().BeEquivalentTo(otherItems);
    }

    [TestMethod]
    public void TryRemoveAllPredicate_WhenContainsItem_DecreaseCount()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        Instance.Shuffle();

        //Act
        Instance.TryRemoveAll(x => x!.Equals(toRemove));

        //Assert
        Instance.Should().HaveCount(otherItems.Count);
    }

    [TestMethod]
    public void TryRemoveAllPredicate_WhenContainsItem_TriggerEventOnce()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        Instance.Shuffle();

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveAll(x => x!.Equals(toRemove));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>> { new() { OldValues = [toRemove, toRemove, toRemove] } });
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.RemoveAll(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenContainsNoItemCorrespondingToPredicate_Throw()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        var action = () => Instance.RemoveAll(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenContainsItemsThatCorrespondToPredicate_RemoveThem()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.RemoveAll(x => x!.Equals(toRemove));

        //Assert
        Instance.Should().BeEquivalentTo(otherItems);
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenContainsItemsThatCorrespondToPredicate_DecreaseCount()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        //Act
        Instance.RemoveAll(x => x!.Equals(toRemove));

        //Assert
        Instance.Should().HaveCount(otherItems.Count);
    }

    [TestMethod]
    public void RemoveAllPredicate_WhenContainsItem_TriggerEventOnce()
    {
        //Arrange
        var toRemove = Dummy.Create<TItem>();
        Instance.Add(toRemove, toRemove);

        var otherItems = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAll(x => x!.Equals(toRemove));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>> { new() { OldValues = [toRemove, toRemove] } });
    }

    [TestMethod]
    public void InsertIList_Always_Insert()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        ((IList<TItem>)Instance).Insert(1, item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                otherItems[0],
                item,
                otherItems[1],
                otherItems[2]
            });
    }

    [TestMethod]
    public void InsertIList_Always_IncrementCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        ((IList<TItem>)Instance).Insert(1, item);

        //Assert
        Instance.Should().HaveCount(4);
    }

    [TestMethod]
    public void InsertIList_Always_TriggerEventOnce()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((IList<TItem>)Instance).Insert(1, item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    NewValues = new[] { item }
                }
            });
    }

    [TestMethod]
    public void InsertIList_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var index = -Dummy.Create<int>();
        var item = Dummy.Create<TItem>();

        //Act
        var action = () => ((IList<TItem>)Instance).Insert(index, item);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void InsertIList_WhenItemIsNull_InsertNullValue()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        ((IList<TItem>)Instance).Insert(1, (TItem)(object)null!);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                otherItems[0], (TItem)(object)null!, otherItems[1], otherItems[2]
            });
    }

    [TestMethod]
    public void InsertEnumerableNoIndex_WhenItemsAreNull_Throw()
    {
        //Arrange

        //Act
        var action = () => Instance.Insert((IEnumerable<TItem>)null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void InsertEnumerableNoIndex_Always_InsertAtTheBegining()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(items);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0],
                items[1],
                items[2],
                otherItems[0],
                otherItems[1],
                otherItems[2],
            });
    }

    [TestMethod]
    public void InsertEnumerableNoIndex_Always_IncrementCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(items);

        //Assert
        Instance.Should().HaveCount(items.Count + otherItems.Count);
    }

    [TestMethod]
    public void InsertEnumerableNoIndex_Always_TriggerEventOnce()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Insert(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new()
            {
                NewValues = items
            }
        });
    }

    [TestMethod]
    public void InsertParamsNoIndex_WhenPassingSingleNull_InsertNullValue()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(null!);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                (TItem)(object)null!, otherItems[0], otherItems[1], otherItems[2]
            });
    }

    [TestMethod]
    public void InsertParamsNoIndex_Always_InsertAtTheBegining()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(items);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0],
                items[1],
                items[2],
                otherItems[0],
                otherItems[1],
                otherItems[2],
            });
    }

    [TestMethod]
    public void InsertParamsNoIndex_Always_IncrementCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(items);

        //Assert
        Instance.Should().HaveCount(items.Length + otherItems.Count);
    }

    [TestMethod]
    public void InsertParamsNoIndex_Always_TriggerEventOnce()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Insert(items);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    NewValues = items
                }
            });
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var index = -Dummy.Create<int>();
        var items = Dummy.CreateMany<TItem>();

        //Act
        var action = () => Instance.Insert(index, items);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenItemsAreNull_Throw()
    {
        //Arrange
        var index = Dummy.Create<int>();

        //Act
        var action = () => Instance.Insert(index, (IEnumerable<TItem>)null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenIndexIsZero_InsertAtTheBegining()
    {
        //Arrange
        var index = 0;
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(index, items);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0],
                items[1],
                items[2],
                otherItems[0],
                otherItems[1],
                otherItems[2],
            });
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenIndexIsLastIndex_InsertAtTheEndOfObservableListSameAsAddWould()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);
        var index = Instance.LastIndex;


        //Act
        Instance.Insert(index, items);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                otherItems[0],
                otherItems[1],
                otherItems[2],
                items[0],
                items[1],
                items[2],
            });
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenIndexIsInTheMiddleOfObservableList_InsertInTheMiddle()
    {
        //Arrange
        var index = 2;
        var items = Dummy.CreateMany<TItem>(3).ToArray();

        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        //Act
        Instance.Insert(index, items);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                otherItems[0],
                otherItems[1],
                items[0],
                items[1],
                items[2],
                otherItems[2],
            });
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenInsertingObservableListIntoItself_InsertIt()
    {
        //Arrange
        var index = 1;

        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        //Act
        Instance.Insert(index, Instance);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0],
                items[0],
                items[1],
                items[2],
                items[1],
                items[2],
            });
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenInsertingObservableListIntoItself_IncreaseCount()
    {
        //Arrange
        var index = 1;

        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        //Act
        Instance.Insert(index, Instance);

        //Assert
        Instance.Should().HaveCount(6);
    }

    [TestMethod]
    public void InsertEnumerableWithIndex_WhenInsertingObservableListIntoItself_TriggerEventOnce()
    {
        //Arrange
        var index = 1;

        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.Insert(index, Instance);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    NewValues = items
                }
            });
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsInCollection_ReturnTrue()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        //Act
        var result = ((ICollection<TItem>)Instance).Remove(toRemove);

        //Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsNotInCollection_ReturnFalse()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();

        //Act
        var result = ((ICollection<TItem>)Instance).Remove(item);

        //Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((ICollection<TItem>)Instance).Remove(toRemove);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new()
                {
                    OldValues = new []{toRemove}
                }
            });
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        ((ICollection<TItem>)Instance).Remove(item);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var toRemove = items.GetRandom();

        var originalCount = Instance.Count;

        //Act
        ((ICollection<TItem>)Instance).Remove(toRemove);

        //Assert
        Instance.Should().HaveCount(originalCount - 1);
    }

    [TestMethod]
    public void RemoveICollection_WhenItemIsNotInCollection_DoNotChangeCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var item = Dummy.Create<TItem>();
        var originalCount = Instance.Count;

        //Act
        ((ICollection<TItem>)Instance).Remove(item);

        //Assert
        Instance.Should().HaveCount(originalCount);
    }

    [TestMethod]
    public void RemoveAt_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var index = -Dummy.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAt_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>());

        var index = Instance.LastIndex + 1;

        //Act
        var action = () => Instance.RemoveAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAt_WhenItemIsRemoved_DecreaseCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);
        var index = items.GetRandomIndex();
        var originalCount = Instance.Count;

        //Act
        Instance.RemoveAt(index);

        //Assert
        Instance.Should().HaveCount(originalCount - 1);
    }

    [TestMethod]
    public void RemoveAt_WhenItemIsRemoved_MoveSubsequentItemsToTheLeft()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);
        var index = 1;
        var originalCount = Instance.Count;

        //Act
        Instance.RemoveAt(index);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0], items[2]
            });
    }

    [TestMethod]
    public void RemoveAt_WhenItemIsRemoved_TriggerCollectionChanged()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);
        var index = items.GetRandomIndex();

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(index);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = new []{items[index]}}
            });
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexIsNegative_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>());

        var index = -Dummy.Create<int>();
        var count = Dummy.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexIsGreaterThanLastIndex_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>());

        var index = Instance.LastIndex + 1;
        var count = Dummy.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void RemoveAtRange_WhenCountIsNegative_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>());

        var index = Instance.GetRandomIndex();
        var count = -Dummy.Create<int>();

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexPlusCountWouldFallOutsideRange_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>(3));

        var index = 1;
        var count = 3;

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void RemoveAtRange_WhenCountIsZero_Throw()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>());

        var index = Instance.GetRandomIndex();
        var count = 0;

        //Act
        var action = () => Instance.RemoveAt(index, count);

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexAndCountAreWithinBounds_RemoveItemsWithinThoseBounds()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(6).ToList();
        Instance.Add(items);

        var index = 3;
        var count = 2;

        //Act
        Instance.RemoveAt(index, count);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                items[0], items[1], items[2], items[5]
            });
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexAndCountAreWithinBounds_TriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(6).ToList();
        Instance.Add(items);

        var index = 3;
        var count = 2;

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (sender, args) => triggers.Add(args);

        //Act
        Instance.RemoveAt(index, count);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new() { OldValues = new[] { items[3], items[4] } }
            });
    }

    [TestMethod]
    public void RemoveAtRange_WhenIndexAndCountAreWithinBounds_AdjustCollectionCountAccordingly()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(6).ToList();
        Instance.Add(items);

        var index = 3;
        var count = 2;

        //Act
        Instance.RemoveAt(index, count);

        //Assert
        Instance.Should().HaveCount(4);
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveFirst(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item
            });
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var buffer = Dummy.Create<TItem>();
        Instance.Add(item, buffer, item, item);

        //Act
        Instance.TryRemoveFirst(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                buffer, item, item
            });
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveFirst(item);

        //Assert
        Instance.Should().HaveCount(2);
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveFirst(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = new []{item}}
            });
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        var action = () => Instance.TryRemoveFirst(someOtherItem);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsNotInCollection_DoNotChangeCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        Instance.TryRemoveFirst(someOtherItem);

        //Assert
        Instance.Should().HaveCount(3);
    }

    [TestMethod]
    public void TryRemoveFirstItem_WhenItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        Instance.TryRemoveFirst(someOtherItem);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveFirstItem_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveFirst(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item
            });
    }

    [TestMethod]
    public void RemoveFirstItem_WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var buffer = Dummy.Create<TItem>();
        Instance.Add(item, buffer, item, item);

        //Act
        Instance.RemoveFirst(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                buffer, item, item
            });
    }

    [TestMethod]
    public void RemoveFirstItem_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveFirst(item);

        //Assert
        Instance.Should().HaveCount(2);
    }

    [TestMethod]
    public void RemoveFirstItem_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveFirst(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = new []{item}}
            });
    }

    [TestMethod]
    public void RemoveFirstItem_WhenItemIsNotInCollection_Throw()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        var action = () => Instance.RemoveFirst(someOtherItem);

        //Assert
        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.TryRemoveFirst(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();

        Instance.Add(otherItems);
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveFirst(x => x!.Equals(item));

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
        {
            otherItems[0], otherItems[1], otherItems[2], item, item
        });
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();

        Instance.Add(otherItems);
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveFirst(x => x!.Equals(item));

        //Assert
        Instance.Should().HaveCount(5);
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();

        Instance.Add(otherItems);
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveFirst(x => x!.Equals(item));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new() { OldValues = new [] { item } }
        });
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var action = () => Instance.TryRemoveFirst(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsNotInCollection_DoNotChangeCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveFirst(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        Instance.Should().HaveCount(3);
    }

    [TestMethod]
    public void TryRemoveFirstPredicate_WhenItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveFirst(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveFirstPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.RemoveFirst(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemoveFirstPredicate_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveFirst(x => x!.Equals(item));

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
        {
            otherItems[0], otherItems[1], otherItems[2], item, item
        });
    }

    [TestMethod]
    public void RemoveFirstPredicate_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveFirst(x => x!.Equals(item));

        //Assert
        Instance.Should().HaveCount(5);
    }

    [TestMethod]
    public void RemoveFirstPredicate_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var otherItems = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(otherItems);

        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveFirst(x => x!.Equals(item));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new(){OldValues = new []{item}}
        });
    }

    [TestMethod]
    public void RemoveFirstPredicate_WhenItemIsNotInCollection_Throw()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>();
        Instance.Add(items);

        //Act
        var action = () => Instance.RemoveFirst(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveLast(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item
            });
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var buffer = Dummy.Create<TItem>();
        Instance.Add(item, item, buffer, item);

        //Act
        Instance.TryRemoveLast(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item, buffer
            });
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.TryRemoveLast(item);

        //Assert
        Instance.Should().HaveCount(2);
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveLast(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = new []{item}}
            });
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        var action = () => Instance.TryRemoveLast(someOtherItem);

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsNotInCollection_DoNotChangeCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        Instance.TryRemoveLast(someOtherItem);

        //Assert
        Instance.Should().HaveCount(3);
    }

    [TestMethod]
    public void TryRemoveLastItem_WhenItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        Instance.TryRemoveLast(someOtherItem);

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveLastItem_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveLast(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item
            });
    }

    [TestMethod]
    public void RemoveLastItem_WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
    {
        //Arrange
        var item = Dummy.Create<TItem>();

        var buffer = Dummy.Create<TItem>();
        Instance.Add(item, item, buffer, item);

        //Act
        Instance.RemoveLast(item);

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item, buffer
            });
    }

    [TestMethod]
    public void RemoveLastItem_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        //Act
        Instance.RemoveLast(item);

        //Assert
        Instance.Should().HaveCount(2);
    }

    [TestMethod]
    public void RemoveLastItem_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveLast(item);

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
            {
                new(){OldValues = new []{item}}
            });
    }

    [TestMethod]
    public void RemoveLastItem_WhenItemIsNotInCollection_Throw()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var someOtherItem = Dummy.Create<TItem>();

        //Act
        var action = () => Instance.RemoveLast(someOtherItem);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.TryRemoveLast(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsInCollection_RemoveFirstOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        //Act
        Instance.TryRemoveLast(x => x!.Equals(item));

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
        {
            item, item, buffer[0], buffer[1], buffer[2]
        });
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        //Act
        Instance.TryRemoveLast(x => x!.Equals(item));

        //Assert
        Instance.Should().HaveCount(5);
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveLast(x => x!.Equals(item));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new() { OldValues = new [] { item } }
        });
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsNotInCollection_DoNotThrow()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var action = () => Instance.TryRemoveLast(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsNotInCollection_DoNotChangeCount()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        Instance.TryRemoveLast(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        Instance.Should().HaveCount(3);
    }

    [TestMethod]
    public void TryRemoveLastPredicate_WhenItemIsNotInCollection_DoNotTriggerEvent()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.TryRemoveLast(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        triggers.Should().BeEmpty();
    }

    [TestMethod]
    public void RemoveLastPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        Func<TItem, bool> predicate = null!;

        //Act
        var action = () => Instance.RemoveLast(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void RemoveLastPredicate_WhenItemIsInCollection_RemoveLastOccurenceOnly()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        //Act
        Instance.RemoveLast(x => x!.Equals(item));

        //Assert
        Instance.Should().BeEquivalentTo(new List<TItem>
            {
                item, item, buffer[0], buffer[1], buffer[2]
            });
    }

    [TestMethod]
    public void RemoveLastPredicate_WhenItemIsInCollection_DecreaseCount()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        //Act
        Instance.RemoveLast(x => x!.Equals(item));

        //Assert
        Instance.Should().HaveCount(5);
    }

    [TestMethod]
    public void RemoveLastPredicate_WhenItemIsInCollection_TriggerEvent()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        var triggers = new List<CollectionChangeEventArgs<TItem>>();
        Instance.CollectionChanged += (_, args) => triggers.Add(args);

        //Act
        Instance.RemoveLast(x => x!.Equals(item));

        //Assert
        triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<TItem>>
        {
            new() { OldValues = new [] { item } }
        });
    }

    [TestMethod]
    public void RemoveLastPredicate_WhenItemIsNotInCollection_Throw()
    {
        //Arrange
        var item = Dummy.Create<TItem>();
        Instance.Add(item, item, item);

        var buffer = Dummy.CreateMany<TItem>(3).ToList();
        Instance.Add(buffer);

        //Act
        var action = () => Instance.RemoveLast(x => x!.Equals(Dummy.Create<TItem>()));

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnEmptyMessage()
    {
        //Arrange

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"Empty {Instance.GetType().GetHumanReadableName()}");
    }

    [TestMethod]
    public void ToString_WhenContainsItems_ReturnNumberOfItems()
    {
        //Arrange
        Instance.Add(Dummy.CreateMany<TItem>(3));

        //Act
        var result = Instance.ToString();

        //Assert
        result.Should().Be($"{Instance.GetType().GetHumanReadableName()} with 3 items");
    }

    [TestMethod]
    public void Enumerator_Always_CorrectlyEnumeratesEveryItem()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        var enumeratedItems = new List<TItem>();

        //Act
        foreach (var item in observableList)
            enumeratedItems.Add(item);

        //Assert
        enumeratedItems.Should().NotBeEmpty();
        enumeratedItems.Should().BeEquivalentTo(observableList);
        enumeratedItems.Should().HaveCount(observableList.Count);
    }

    [TestMethod]
    public void Enumerator_WhenCollectionIsModifiedDuringEnumeration_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        //Act
        var action = () =>
        {
            foreach (var item in observableList)
                observableList.RemoveFirst(item);
        };

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Enumerator_WhenUsingResetAfterCollectionChanged_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        using var enumerator = observableList.GetEnumerator();
        observableList.RemoveAt(observableList.GetRandomIndex());

        //Act
        var action = () => enumerator.Reset();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Enumerator_WhenUsingResetWhileCollectionIsStillUnchanged_SetCurrentToDefault()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        using var enumerator = observableList.GetEnumerator();

        //Act
        enumerator.Reset();

        //Assert
        enumerator.Current.Should().BeNull();
    }

    [TestMethod]
    public void InterfaceEnumerator_WhenUsingResetAfterCollectionChanged_Throw()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();

        using var enumerator = ((IEnumerable)observableList).GetEnumerator() as IEnumerator<TItem>;
        observableList.RemoveAt(observableList.GetRandomIndex());

        //Act
        var action = () => enumerator.Reset();

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void InterfaceEnumerator_WhenUsingResetWhileCollectionIsStillUnchanged_SetCurrentToDefault()
    {
        //Arrange
        var observableList = Dummy.Create<TList>();
        using var enumerator = ((IEnumerable)observableList).GetEnumerator() as IEnumerator<TItem>;

        //Act
        enumerator.Reset();

        //Assert
        enumerator.Current.Should().BeNull();
    }

    [TestMethod]
    public void SystemTextSerialization_Always_DeserializeEquivalentObject()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>();
        foreach (var item in items)
            Instance.Add(item);

        var json = JsonSerializer.Serialize(Instance);

        //Act
        var result = JsonSerializer.Deserialize<TList>(json);

        //Assert
        result.Should().BeEquivalentTo(Instance);
    }

    [TestMethod]
    public void GetHashCode_Always_ReturnInternalListHashCode()
    {
        //Arrange
        var internalList = GetFieldValue<List<TItem>>("_items")!;

        //Act
        var result = Instance.GetHashCode();

        //Assert
        result.Should().Be(internalList.GetHashCode());
    }

    [TestMethod]
    public void GetRandom_WhenContainsItems_ReturnAnyItemInCollection()
    {
        //Arrange
        var items = Dummy.CreateMany<TItem>().ToList();
        Instance.Add(items);

        //Act
        var result = Instance.GetRandom();

        //Assert
        items.Should().Contain(result);
    }

    [TestMethod]
    public void GetRandom_WhenIsEmpty_ReturnDefault()
    {
        //Arrange

        //Act
        var result = Instance.GetRandom();

        //Assert
        result.Should().Be(default);
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<TList>(Dummy, JsonSerializerOptions.WithObservableListConverters());

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<TList>(Dummy, JsonSerializerOptions.WithObservableListConverters());

}