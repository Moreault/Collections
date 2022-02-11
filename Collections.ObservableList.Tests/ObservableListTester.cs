using AutoFixture;
using Collections.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBX.Collections.ObservableList;
using ToolBX.Eloquentest;
using ToolBX.Eloquentest.Extensions;

namespace Collections.ObservableList.Tests;

[TestClass]
public class ObservableListTester
{
    [TestClass]
    public class LastIndex : Tester
    {
        [TestMethod]
        public void WhenCollectionIsEmpty_ReturnMinusOdne()
        {
            //Arrange
            var observableList = new ObservableList<Dummy>();

            //Act
            var result = observableList.LastIndex;

            //Assert
            result.Should().Be(-1);
        }

        [TestMethod]
        public void WhenCollectionContainsOneItem_ReturnZero()
        {
            //Arrange
            var observableList = new ObservableList<Dummy> { Fixture.Create<Dummy>() };

            //Act
            var result = observableList.LastIndex;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenCollectionContainsABunchOfItems_ReturnLastIndex()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();

            //Act
            var result = observableList.LastIndex;

            //Assert
            result.Should().Be(observableList.Count - 1);
        }
    }

    [TestClass]
    public class IsReadOnly : Tester
    {
        [TestMethod]
        public void Always_ReturnFalse()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();

            //Act
            var result = observableList.IsReadOnly;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Indexer_Get : Tester
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = -1;

            //Act
            var action = () => observableList[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = observableList.LastIndex + 1;

            //Act
            var action = () => observableList[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsZero_ReturnFirstItem()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = 0;

            //Act
            var result = observableList[index];

            //Assert
            result.Should().Be(observableList.First());
        }

        [TestMethod]
        public void WhenIndexIsLastIndex_ReturnLastItem()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = observableList.LastIndex;

            //Act
            var result = observableList[index];

            //Assert
            result.Should().Be(observableList.Last());
        }

        [TestMethod]
        public void WhenIndexIsWhatever_ReturnWhatever()
        {
            //Arrange
            var list = Fixture.CreateMany<string>().ToList();
            var observableList = list.ToObservableList();
            var index = observableList.GetRandomIndex();

            //Act
            var result = observableList[index];

            //Assert
            result.Should().Be(list[index]);
        }
    }

    [TestClass]
    public class Indexer_Set : Tester
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = -1;
            var value = Fixture.Create<string>();

            //Act
            var action = () => observableList[index] = value;

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = observableList.LastIndex + 1;
            var value = Fixture.Create<string>();

            //Act
            var action = () => observableList[index] = value;

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsWithinBounds_ReplaceItemAtIndex()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = observableList.GetRandomIndex();
            var value = Fixture.Create<string>();

            //Act
            observableList[index] = value;

            //Assert
            observableList[index].Should().Be(value);
        }

        [TestMethod]
        public void WhenIndexIsWithinBounds_TriggerCollectionChanged()
        {
            //Arrange
            var observableList = Fixture.CreateMany<string>().ToObservableList();
            var index = observableList.GetRandomIndex();
            var value = Fixture.Create<string>();

            var oldItem = observableList[index];

            var eventArgs = new List<CollectionChangeEventArgs<string>>();
            observableList.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            observableList[index] = value;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    OldValues = new[] { oldItem },
                    NewValues = new[] { value }
                }
            });
        }
    }

    [TestClass]
    public class Constructor : Tester
    {
        [TestMethod]
        public void WhenUsingDefaultConstructor_InitializeWithEmptyArray()
        {
            //Arrange

            //Act
            var result = new ObservableList<string>();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenUsingParams_InitializeWithThoseItems()
        {
            //Arrange
            var items = Fixture.Create<string[]>();

            //Act
            var result = new ObservableList<string>(items);

            //Assert
            result.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenPassingNull_Throw()
        {
            //Arrange

            //Act
            var action = () => new ObservableList<Dummy>(null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPassingList_InitializeWithListItems()
        {
            //Arrange
            var items = Fixture.Create<List<string>>();

            //Act
            var result = new ObservableList<string>(items);

            //Assert
            result.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenUsingInitializer_AddThoseItemsToObservableList()
        {
            //Arrange
            var item1 = Fixture.Create<string>();
            var item2 = Fixture.Create<string>();
            var item3 = Fixture.Create<string>();

            //Act
            var result = new ObservableList<string> { item1, item2, item3 };

            //Assert
            result.Should().BeEquivalentTo(item1, item2, item3);
        }
    }

    [TestClass]
    public class Add : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenUsingTheICollectionOverload_AddToObservableList()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            ((ICollection<string>)Instance).Add(item);

            //Assert
            Instance.Should().Contain(item);
        }

        [TestMethod]
        public void WhenUsingTheICollectionOverload_SetCountToOne()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            ((ICollection<string>)Instance).Add(item);

            //Assert
            Instance.Count.Should().Be(1);
        }

        [TestMethod]
        public void WhenAddingSingleItem_ItemIsAdded()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(item);

            //Assert
            Instance.Should().Contain(item);
        }

        [TestMethod]
        public void WhenAddingSingleItem_TriggerChange()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var eventArgs = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(item);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new() { NewValues = new[] { item } }
            });
        }

        [TestMethod]
        public void WhenAddingSingleItem_SetCountToOne()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            Instance.Add(item);

            //Assert
            Instance.Count.Should().Be(1);
        }

        [TestMethod]
        public void WhenAddingABunchOfItems_AddAllOfThem()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();

            //Act
            Instance.Add(items);

            //Assert
            Instance.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void WhenAddingABunchOfItems_TriggerChangeOnlyOnce()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            var triggered = 0;
            Instance.CollectionChanged += (_, _) => triggered++;

            //Act
            Instance.Add(items);

            //Assert
            triggered.Should().Be(1);
        }

        [TestMethod]
        public void WhenAddingABunchOfItems_TriggerChangeWithAllNewItems()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            var eventArgs = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(items);

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new() { NewValues = items }
            });
        }

        [TestMethod]
        public void WhenAddingABunchOfItems_SetCountToTwenty()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            var eventArgs = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Add(items);

            //Assert
            Instance.Count.Should().Be(20);
        }

        [TestMethod]
        public void WhenAddingNullCollection_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Add((IEnumerable<string>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenAddingSingleNullItem_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.Add(null!);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingSingleNullItem_AddNull()
        {
            //Arrange

            //Act
            Instance.Add(null!);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string> { null! });
        }

        [TestMethod]
        public void WhenAddingMultipleNullItems_DoNotThrow()
        {
            //Arrange

            //Act
            var action = () => Instance.Add(null!, null!, null!);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenAddingMultipleNullItems_AddAllNullItems()
        {
            //Arrange

            //Act
            Instance.Add(null!, null!, null!);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string> { null!, null!, null! });
        }
    }

    [TestClass]
    public class Clear : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void Always_RemoveAllItems()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            Instance.Add(items);

            //Act
            Instance.Clear();

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void Always_TriggerCollectionChanged()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            Instance.Add(items);

            var eventArgs = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => eventArgs.Add(args);

            //Act
            Instance.Clear();

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void Always_SetCountToZero()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(20).ToArray();
            Instance.Add(items);

            //Act
            Instance.Clear();

            //Assert
            Instance.Count.Should().Be(0);
        }
    }

    [TestClass]
    public class Contains : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInObservableList_ReturnTrue()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);

            //Act
            var result = ((ICollection<string>)Instance).Contains(item);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenItemIsNotInObservableList_ReturnFalse()
        {
            //Arrange
            var item = Fixture.Create<string>();

            //Act
            var result = ((ICollection<string>)Instance).Contains(item);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class CopyTo : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void Always_CopyToArray()
        {
            //Arrange
            var array = new string[3];
            Instance.Add(Fixture.CreateMany<string>(3));

            //Act
            ((ICollection<string>)Instance).CopyTo(array, 0);

            //Assert
            array.Should().BeEquivalentTo(Instance);
        }
    }

    [TestClass]
    public class Copy : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenIndexIsZero_ReturnAnExactCopy()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Copy();

            //Assert
            result.Should().BeEquivalentTo(Instance);
        }

        [TestMethod]
        public void WhenIndexIsZero_ShouldNotBeSameReference()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Copy();

            //Assert
            result.Should().NotBeSameAs(Instance);
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var startingIndex = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Copy(startingIndex);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var startingIndex = Instance.LastIndex + 1;

            //Act
            var action = () => Instance.Copy(startingIndex);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenStartingIndexIsLastIndex_ReturnObservableListWithOnlyTheLastItemInIt()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var startingIndex = Instance.LastIndex;

            //Act
            var result = Instance.Copy(startingIndex);

            //Assert
            result.Should().BeEquivalentTo(Instance.Last());
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanZero_CopyObservableListStartingFromIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(6).ToList();
            var startingIndex = 3;
            Instance.Add(items);

            //Act
            var result = Instance.Copy(startingIndex);

            //Assert
            result.Should().BeEquivalentTo(new ObservableList<string>
            {
                items[3], items[4], items[5]
            });
        }
    }

    [TestClass]
    public class Copy_WithCount : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenStartingIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();
            var count = Fixture.Create<int>();

            //Act
            var action = () => Instance.Copy(index, count);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenStartingIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>(3));

            var index = 3;
            var count = Fixture.Create<int>();

            //Act
            var action = () => Instance.Copy(index, count);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenCountIsNegative_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>(3));
            var index = Instance.GetRandomIndex();
            var count = -Fixture.Create<int>();

            //Act
            var action = () => Instance.Copy(index, count);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenStartingIndexPlusCountIsGreaterThanCollectionSize_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>(3));
            var index = 0;
            var count = 4;

            //Act
            var action = () => Instance.Copy(index, count);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenStartingIndexPlusCountIsWithinBounds_ReturnACopyOfThat()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(8).ToList();
            Instance.Add(items);

            var index = 3;
            var count = 2;

            //Act
            var result = Instance.Copy(index, count);

            //Assert
            result.Should().BeEquivalentTo(new List<string>
            {
                items[3], items[4]
            });
        }

        [TestMethod]
        public void WhenStartingIndexIsZeroAndCountIsTheSameAsCollection_ReturnCopyOfObservableList()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(8).ToList();
            Instance.Add(items);

            var index = 0;
            var count = 8;

            //Act
            var result = Instance.Copy(index, count);

            //Assert
            result.Should().BeEquivalentTo(items);
        }
    }

    [TestClass]
    public class IndexOf_IList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInObservableList_ReturnItsIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var expectedIndex = items.GetRandomIndex();

            //Act
            var result = ((IList<string>)Instance).IndexOf(items[expectedIndex]);

            //Assert
            result.Should().Be(expectedIndex);
        }

        [TestMethod]
        public void WhenItemIsNotInObservableList_ReturnMinusOne()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();

            //Act
            var result = ((IList<string>)Instance).IndexOf(item);

            //Assert
            result.Should().Be(-1);
        }
    }

    [TestClass]
    public class FirstIndexOf_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInObservableList_ReturnItsIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var expectedIndex = items.GetRandomIndex();

            //Act
            var result = Instance.FirstIndexOf(items[expectedIndex]);

            //Assert
            result.Should().Be(expectedIndex);
        }

        [TestMethod]
        public void WhenItemIsNotInObservableList_ReturnMinusOne()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();

            //Act
            var result = Instance.FirstIndexOf(item);

            //Assert
            result.Should().Be(-1);
        }
    }

    [TestClass]
    public class FirstIndexOf_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.FirstIndexOf((Predicate<Dummy>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenContainsDummyThatFitsConditions_ReturnIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(items);

            var index = items.GetRandomIndex();
            var item = items[index];

            //Act
            var result = Instance.FirstIndexOf(x => x.Id == item.Id);

            //Assert
            result.Should().Be(index);
        }

        [TestMethod]
        public void WhenThereAreMultipleDummiesThatFitConditions_ReturnOnlyTheFirst()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.FirstIndexOf(x => x.Id == id);

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenNoDummiesFitConditions_ReturnMinusOne()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            var id = items.Sum(x => x.Id);

            //Act
            var result = Instance.FirstIndexOf(x => x.Id == id);

            //Assert
            result.Should().Be(-1);
        }
    }

    [TestClass]
    public class LastIndexOf_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInObservableList_ReturnItsIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var expectedIndex = items.GetRandomIndex();

            //Act
            var result = Instance.LastIndexOf(items[expectedIndex]);

            //Assert
            result.Should().Be(expectedIndex);
        }

        [TestMethod]
        public void WhenItemIsNotInObservableList_ReturnMinusOne()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();

            //Act
            var result = Instance.LastIndexOf(item);

            //Assert
            result.Should().Be(-1);
        }
    }

    [TestClass]
    public class LastIndexOf_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.LastIndexOf((Predicate<Dummy>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenContainsDummyThatFitsConditions_ReturnIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(items);

            var index = items.GetRandomIndex();
            var item = items[index];

            //Act
            var result = Instance.LastIndexOf(x => x.Id == item.Id);

            //Assert
            result.Should().Be(index);
        }

        [TestMethod]
        public void WhenThereAreMultipleDummiesThatFitConditions_ReturnOnlyTheLast()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.LastIndexOf(x => x.Id == id);

            //Assert
            result.Should().Be(Instance.LastIndex);
        }

        [TestMethod]
        public void WhenNoDummiesFitConditions_ReturnMinusOne()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            var id = items.Sum(x => x.Id);

            //Act
            var result = Instance.LastIndexOf(x => x.Id == id);

            //Assert
            result.Should().Be(-1);
        }
    }

    [TestClass]
    public class IndexesOf_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenThereAreNoOccurences_ReturnEmpty()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOnlyOneOccurence_ReturnItsIndexInAObservableList()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);
            Instance.Add(item);

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(new ObservableList<int> { Instance.LastIndex });
        }

        [TestMethod]
        public void WhenThereAreMultipleOccurences_ReturnThemAll()
        {
            //Arrange
            var item = Fixture.Create<string>();
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(item);
            Instance.Add(items);
            Instance.Add(item);
            Instance.Add(item);

            //Act
            var result = Instance.IndexesOf(item);

            //Assert
            result.Should().BeEquivalentTo(new ObservableList<int> { 0, 4, 5 });
        }
    }

    [TestClass]
    public class IndexesOf_Precidate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.IndexesOf((Predicate<Dummy>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereAreNoCorrespondingItems_ReturnEmpty()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(items);

            var id = items.Sum(x => x.Id);

            //Act
            var result = Instance.IndexesOf(x => x.Id == id);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenThereIsOneCorrespondingItem_ReturnIndex()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();

            var id = Fixture.Create<int>();
            var item = Fixture.Build<Dummy>().With(x => x.Id, id).Create();
            Instance.Add(items);
            Instance.Add(item);

            //Act
            var result = Instance.IndexesOf(x => x.Id == id);

            //Assert
            result.Should().BeEquivalentTo(new ObservableList<int> { 3 });
        }

        [TestMethod]
        public void WhenThereAreMultipleCorrespondingItems_ReturnThoseIndexes()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(3).ToList();
            Instance.Add(items);

            var id = Fixture.Create<int>();
            var itemsWithId = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany(3).ToList();
            Instance.Add(itemsWithId);

            //Act
            var result = Instance.IndexesOf(x => x.Id == id);

            //Assert
            result.Should().BeEquivalentTo(new ObservableList<int> { 3, 4, 5 });
        }
    }

    [TestClass]
    public class TryRemoveAll_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenContainsItem_RemoveAllOfThem()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.TryRemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(otherItems);
        }

        [TestMethod]
        public void WhenDoesNotContainItem_DoNotThrow()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            var action = () => Instance.TryRemoveAll(item);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenContainsItem_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.TryRemoveAll(item);

            //Assert
            Instance.Should().HaveCount(otherItems.Count);
        }

        [TestMethod]
        public void WhenContainsItem_TriggerEventOnce()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAll(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>> { new() { OldValues = new List<string> { item, item, item } } });
        }
    }

    [TestClass]
    public class RemoveAll_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenContainsItem_RemoveAllOfThem()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().BeEquivalentTo(otherItems);
        }

        [TestMethod]
        public void WhenDoesNotContainItem_Throw()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            var action = () => Instance.RemoveAll(item);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenContainsItem_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.RemoveAll(item);

            //Assert
            Instance.Should().HaveCount(otherItems.Count);
        }

        [TestMethod]
        public void WhenContainsItem_TriggerEventOnce()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item);
            Instance.Add(item);
            Instance.Add(item);

            var otherItems = Fixture.CreateMany<string>().ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>> { new() { OldValues = new List<string> { item, item, item } } });
        }
    }

    [TestClass]
    public class TryRemoveAll_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.TryRemoveAll(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenContainsNoItemCorrespondingToPredicate_DoNotThrow()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            var action = () => Instance.TryRemoveAll(x => x.Id == id);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenContainsItemsThatCorrespondToPredicate_RemoveThem()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.TryRemoveAll(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(otherItems);
        }

        [TestMethod]
        public void WhenContainsItem_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.TryRemoveAll(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(otherItems.Count);
        }

        [TestMethod]
        public void WhenContainsItem_TriggerEventOnce()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveAll(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>> { new() { OldValues = toRemove } });
        }
    }

    [TestClass]
    public class RemoveAll_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.RemoveAll(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenContainsNoItemCorrespondingToPredicate_Throw()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            var action = () => Instance.RemoveAll(x => x.Id == id);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenContainsItemsThatCorrespondToPredicate_RemoveThem()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.RemoveAll(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(otherItems);
        }

        [TestMethod]
        public void WhenContainsItemsThatCorrespondToPredicate_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            //Act
            Instance.RemoveAll(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(otherItems.Count);
        }

        [TestMethod]
        public void WhenContainsItem_TriggerEventOnce()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var toRemove = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(toRemove);

            var otherItems = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAll(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>> { new() { OldValues = toRemove } });
        }
    }

    [TestClass]
    public class Insert_IList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void Always_Insert()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            ((IList<string>)Instance).Insert(1, item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                otherItems[0],
                item,
                otherItems[1],
                otherItems[2]
            });
        }

        [TestMethod]
        public void Always_IncrementCount()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            ((IList<string>)Instance).Insert(1, item);

            //Assert
            Instance.Should().HaveCount(4);
        }

        [TestMethod]
        public void Always_TriggerEventOnce()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            ((IList<string>)Instance).Insert(1, item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    NewValues = new[] { item }
                }
            });
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();
            var item = Fixture.Create<string>();

            //Act
            var action = () => ((IList<string>)Instance).Insert(index, item);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenItemIsNull_InsertNullValue()
        {
            //Arrange
            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            ((IList<string>)Instance).Insert(1, null!);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                otherItems[0], null!, otherItems[1], otherItems[2]
            });
        }
    }

    [TestClass]
    public class Insert_Enumerable_NoIndex : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemsAreNull_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Insert((IEnumerable<string>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_InsertAtTheBegining()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(items);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void Always_IncrementCount()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(items);

            //Assert
            Instance.Should().HaveCount(items.Count + otherItems.Count);
        }

        [TestMethod]
        public void Always_TriggerEventOnce()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Insert(items);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    NewValues = items
                }
            });
        }
    }

    [TestClass]
    public class Insert_Params_NoIndex : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenPassingSingleNull_InsertNullValue()
        {
            //Arrange
            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(null!);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                null!, otherItems[0], otherItems[1], otherItems[2]
            });
        }

        [TestMethod]
        public void Always_InsertAtTheBegining()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(items);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void Always_IncrementCount()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(items);

            //Assert
            Instance.Should().HaveCount(items.Length + otherItems.Count);
        }

        [TestMethod]
        public void Always_TriggerEventOnce()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Insert(items);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    NewValues = items
                }
            });
        }
    }

    [TestClass]
    public class Insert_Enumerable_WithIndex : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();
            var items = Fixture.CreateMany<string>();

            //Act
            var action = () => Instance.Insert(index, items);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenItemsAreNull_Throw()
        {
            //Arrange
            var index = Fixture.Create<int>();

            //Act
            var action = () => Instance.Insert(index, (IEnumerable<string>)null!);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenIndexIsZero_InsertAtTheBegining()
        {
            //Arrange
            var index = 0;
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(index, items);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void WhenIndexIsLastIndex_InsertAtTheEndOfObservableListSameAsAddWould()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);
            var index = Instance.LastIndex;


            //Act
            Instance.Insert(index, items);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void WhenIndexIsInTheMiddleOfObservableList_InsertInTheMiddle()
        {
            //Arrange
            var index = 2;
            var items = Fixture.CreateMany<string>(3).ToArray();

            var otherItems = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(otherItems);

            //Act
            Instance.Insert(index, items);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void WhenInsertingObservableListIntoItself_InsertIt()
        {
            //Arrange
            var index = 1;

            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            //Act
            Instance.Insert(index, Instance);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
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
        public void WhenInsertingObservableListIntoItself_IncreaseCount()
        {
            //Arrange
            var index = 1;

            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            //Act
            Instance.Insert(index, Instance);

            //Assert
            Instance.Should().HaveCount(6);
        }

        [TestMethod]
        public void WhenInsertingObservableListIntoItself_TriggerEventOnce()
        {
            //Arrange
            var index = 1;

            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.Insert(index, Instance);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    NewValues = items
                }
            });
        }
    }

    [TestClass]
    public class Remove_ICollection : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInCollection_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var toRemove = items.GetRandom();

            //Act
            var result = ((ICollection<string>)Instance).Remove(toRemove);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();

            //Act
            var result = ((ICollection<string>)Instance).Remove(item);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var toRemove = items.GetRandom();

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            ((ICollection<string>)Instance).Remove(toRemove);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new()
                {
                    OldValues = new []{toRemove}
                }
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotTriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            ((ICollection<string>)Instance).Remove(item);

            //Assert
            triggers.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var toRemove = items.GetRandom();

            var originalCount = Instance.Count;

            //Act
            ((ICollection<string>)Instance).Remove(toRemove);

            //Assert
            Instance.Should().HaveCount(originalCount - 1);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotChangeCount()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(3).ToList();
            Instance.Add(items);

            var item = Fixture.Create<string>();
            var originalCount = Instance.Count;

            //Act
            ((ICollection<string>)Instance).Remove(item);

            //Assert
            Instance.Should().HaveCount(originalCount);
        }
    }

    [TestClass]
    public class RemoveAt : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var index = -Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            var index = Instance.LastIndex + 1;

            //Act
            var action = () => Instance.RemoveAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenItemIsRemoved_DecreaseCount()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);
            var index = items.GetRandomIndex();
            var originalCount = Instance.Count;

            //Act
            Instance.RemoveAt(index);

            //Assert
            Instance.Should().HaveCount(originalCount - 1);
        }

        [TestMethod]
        public void WhenItemIsRemoved_MoveSubsequentItemsToTheLeft()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);
            var index = 1;
            var originalCount = Instance.Count;

            //Act
            Instance.RemoveAt(index);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                items[0], items[2]
            });
        }

        [TestMethod]
        public void WhenItemIsRemoved_TriggerCollectionChanged()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);
            var index = items.GetRandomIndex();

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(index);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new(){OldValues = new []{items[index]}}
            });
        }
    }

    [TestClass]
    public class RemoveAt_Range : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            var index = -Fixture.Create<int>();
            var count = Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveAt(index, count);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsGreaterThanLastIndex_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            var index = Instance.LastIndex + 1;
            var count = Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveAt(index, count);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenCountIsNegative_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            var index = Instance.GetRandomIndex();
            var count = -Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveAt(index, count);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenIndexPlusCountWouldFallOutsideRange_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>(3));

            var index = 1;
            var count = 3;

            //Act
            var action = () => Instance.RemoveAt(index, count);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenCountIsZero_Throw()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            var index = Instance.GetRandomIndex();
            var count = 0;

            //Act
            var action = () => Instance.RemoveAt(index, count);

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WhenIndexAndCountAreWithinBounds_RemoveItemsWithinThoseBounds()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(6).ToList();
            Instance.Add(items);

            var index = 3;
            var count = 2;

            //Act
            Instance.RemoveAt(index, count);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                items[0], items[1], items[2], items[5]
            });
        }

        [TestMethod]
        public void WhenIndexAndCountAreWithinBounds_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(6).ToList();
            Instance.Add(items);

            var index = 3;
            var count = 2;

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (sender, args) => triggers.Add(args);

            //Act
            Instance.RemoveAt(index, count);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new() { OldValues = new[] { items[3], items[4] } }
            });
        }

        [TestMethod]
        public void WhenIndexAndCountAreWithinBounds_AdjustCollectionCountAccordingly()
        {
            //Arrange
            var items = Fixture.CreateMany<string>(6).ToList();
            Instance.Add(items);

            var index = 3;
            var count = 2;

            //Act
            Instance.RemoveAt(index, count);

            //Assert
            Instance.Should().HaveCount(4);
        }
    }

    [TestClass]
    public class TryRemoveFirst_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.TryRemoveFirst(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var buffer = Fixture.Create<string>();
            Instance.Add(item, buffer, item, item);

            //Act
            Instance.TryRemoveFirst(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                buffer, item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.TryRemoveFirst(item);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveFirst(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new(){OldValues = new []{item}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotThrow()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            var action = () => Instance.TryRemoveFirst(someOtherItem);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotChangeCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            Instance.TryRemoveFirst(someOtherItem);

            //Assert
            Instance.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotTriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var someOtherItem = Fixture.Create<string>();

            //Act
            Instance.TryRemoveFirst(someOtherItem);

            //Assert
            triggers.Should().BeEmpty();
        }
    }

    [TestClass]
    public class RemoveFirst_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.RemoveFirst(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var buffer = Fixture.Create<string>();
            Instance.Add(item, buffer, item, item);

            //Act
            Instance.RemoveFirst(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                buffer, item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.RemoveFirst(item);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveFirst(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new(){OldValues = new []{item}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_Throw()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            var action = () => Instance.RemoveFirst(someOtherItem);

            //Assert
            action.Should().Throw<Exception>();
        }
    }

    [TestClass]
    public class TryRemoveFirst_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.TryRemoveFirst(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveFirst(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[1], items[2]
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveFirst(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveFirst(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new(){OldValues = new []{items[0]}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotThrow()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemoveFirst(x => x.Id == someOtherId);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotChangeCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            Instance.TryRemoveFirst(x => x.Id == someOtherId);

            //Assert
            Instance.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotTriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var someOtherId = Fixture.Create<int>();

            //Act
            Instance.TryRemoveFirst(x => x.Id == someOtherId);

            //Assert
            triggers.Should().BeEmpty();
        }
    }

    [TestClass]
    public class RemoveFirst_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.RemoveFirst(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveFirst(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[1], items[2]
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveFirst(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveFirst(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new(){OldValues = new []{items[0]}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_Throw()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveFirst(x => x.Id == someOtherId);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    [TestClass]
    public class TryRemoveLast_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.TryRemoveLast(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var buffer = Fixture.Create<string>();
            Instance.Add(item, item, buffer, item);

            //Act
            Instance.TryRemoveLast(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item, buffer
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.TryRemoveLast(item);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveLast(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new(){OldValues = new []{item}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotThrow()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            var action = () => Instance.TryRemoveLast(someOtherItem);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotChangeCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            Instance.TryRemoveLast(someOtherItem);

            //Assert
            Instance.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotTriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var someOtherItem = Fixture.Create<string>();

            //Act
            Instance.TryRemoveLast(someOtherItem);

            //Assert
            triggers.Should().BeEmpty();
        }
    }

    [TestClass]
    public class RemoveLast_Item : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.RemoveLast(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveActualFirstAndNotSomeOtherOccurence()
        {
            //Arrange
            var item = Fixture.Create<string>();

            var buffer = Fixture.Create<string>();
            Instance.Add(item, item, buffer, item);

            //Act
            Instance.RemoveLast(item);

            //Assert
            Instance.Should().BeEquivalentTo(new List<string>
            {
                item, item, buffer
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            //Act
            Instance.RemoveLast(item);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var triggers = new List<CollectionChangeEventArgs<string>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveLast(item);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<string>>
            {
                new(){OldValues = new []{item}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_Throw()
        {
            //Arrange
            var item = Fixture.Create<string>();
            Instance.Add(item, item, item);

            var someOtherItem = Fixture.Create<string>();

            //Act
            var action = () => Instance.RemoveLast(someOtherItem);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    [TestClass]
    public class TryRemoveLast_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.TryRemoveLast(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveLast(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[0], items[1]
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.TryRemoveLast(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.TryRemoveLast(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new(){OldValues = new []{items[2]}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotThrow()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            var action = () => Instance.TryRemoveLast(x => x.Id == someOtherId);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotChangeCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            Instance.TryRemoveLast(x => x.Id == someOtherId);

            //Assert
            Instance.Should().HaveCount(3);
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_DoNotTriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            var someOtherId = Fixture.Create<int>();

            //Act
            Instance.TryRemoveLast(x => x.Id == someOtherId);

            //Assert
            triggers.Should().BeEmpty();
        }
    }

    [TestClass]
    public class RemoveLast_Predicate : Tester<ObservableList<Dummy>>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Predicate<Dummy> predicate = null!;

            //Act
            var action = () => Instance.RemoveLast(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenItemIsInCollection_RemoveFirstOccurenceOnly()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveLast(x => x.Id == id);

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[0], items[1]
            });
        }

        [TestMethod]
        public void WhenItemIsInCollection_DecreaseCount()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            //Act
            Instance.RemoveLast(x => x.Id == id);

            //Assert
            Instance.Should().HaveCount(2);
        }

        [TestMethod]
        public void WhenItemIsInCollection_TriggerEvent()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var triggers = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => triggers.Add(args);

            //Act
            Instance.RemoveLast(x => x.Id == id);

            //Assert
            triggers.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new(){OldValues = new []{items[2]}}
            });
        }

        [TestMethod]
        public void WhenItemIsNotInCollection_Throw()
        {
            //Arrange
            var id = Fixture.Create<int>();

            var items = Fixture.Build<Dummy>().With(x => x.Id, id).CreateMany().ToList();
            Instance.Add(items);

            var someOtherId = Fixture.Create<int>();

            //Act
            var action = () => Instance.RemoveLast(x => x.Id == someOtherId);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("Empty ObservableList<String>");
        }

        [TestMethod]
        public void WhenContainsItems_ReturnNumberOfItems()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>(3));

            //Act
            var result = Instance.ToString();

            //Assert
            result.Should().Be("ObservableList<String> with 3 items");
        }
    }

    [TestClass]
    public class EqualsMethod : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenComparedWithEquivalentArray_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToArray();
            Instance.Add(items);

            //Act
            var result = Instance.Equals(items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentList_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Equals(items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentObservableList_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(items);

            //Act
            var result = Instance.Equals(items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithSameReference_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance.Equals(Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Equals(null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentCollectionButInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(items[1]);
            Instance.Add(items[0]);
            Instance.Add(items[2]);

            //Act
            var result = Instance.Equals(items);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class EqualsMethod_Object : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenComparedWithEquivalentArray_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToArray();
            Instance.Add(items);

            //Act
            var result = Instance.Equals((object)items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentList_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToList();
            Instance.Add(items);

            //Act
            var result = Instance.Equals((object)items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentObservableList_ReturnTrue()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(items);

            //Act
            var result = Instance.Equals((object)items);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithSameReference_ReturnTrue()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance.Equals((object)Instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenComparedWithNull_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Equals((object)null!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenComparedWithEquivalentCollectionButInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var items = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(items[1]);
            Instance.Add(items[0]);
            Instance.Add(items[2]);

            //Act
            var result = Instance.Equals((object)items);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class EqualsOperator_ObservableList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<ObservableList<string>>();

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            ObservableList<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            ObservableList<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenAreTheSameReference_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance == Instance;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenAreDifferentReferencesButContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(b);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenAreDifferentReferencesButContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_ObservableList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<ObservableList<string>>();

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            ObservableList<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            ObservableList<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenAreTheSameReference_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance != Instance;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenAreDifferentReferencesButContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(b);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenAreDifferentReferencesButContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToObservableList();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class EqualsOperator_Array : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<string[]>();

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            string[] b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            string[] b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_Array : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<string[]>();

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            string[] b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            string[] b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenAreTheSameReference_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance != Instance;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class EqualsOperator_IEnumerable : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            IEnumerable<string> b = Fixture.Create<string[]>();

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            IEnumerable<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            IEnumerable<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            IEnumerable<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance == (IEnumerable<string>)b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            IEnumerable<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_IEnumerable : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            IEnumerable<string> b = Fixture.Create<string[]>();

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            IEnumerable<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            IEnumerable<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            IEnumerable<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance != (IEnumerable<string>)b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            IEnumerable<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class EqualsOperator_ICollection : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            ICollection<string> b = Fixture.Create<string[]>();

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            ICollection<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            ICollection<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            ICollection<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance == (ICollection<string>)b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            ICollection<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_ICollection : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            ICollection<string> b = Fixture.Create<string[]>();

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            ICollection<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            ICollection<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            ICollection<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance != (ICollection<string>)b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            ICollection<string> b = Fixture.CreateMany<string>().ToArray();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }
    }


    [TestClass]
    public class EqualsOperator_IList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<List<string>>();

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            List<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            List<string> b = null!;

            //Act
            var result = a == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(b);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance == b;

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class NotEqualsOperator_IList : Tester<ObservableList<string>>
    {
        [TestMethod]
        public void WhenFirstIsNull_ReturnFalse()
        {
            //Arrange
            ObservableList<string> a = null!;
            var b = Fixture.Create<List<string>>();

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenSecondIsNull_ReturnFalse()
        {
            //Arrange
            var a = Fixture.Create<ObservableList<string>>();
            List<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenBothAreNull_ReturnTrue()
        {
            //Arrange
            ObservableList<string> a = null!;
            List<string> b = null!;

            //Act
            var result = a != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInTheSameOrder_ReturnTrue()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(b);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenContainTheSameItemsInDifferentOrder_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(b[1]);
            Instance.Add(b[0]);
            Instance.Add(b[2]);

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainDifferentItems_ReturnFalse()
        {
            //Arrange
            var b = Fixture.CreateMany<string>().ToList();
            Instance.Add(Fixture.CreateMany<string>());

            //Act
            var result = Instance != b;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Enumerator : Tester
    {
        [TestMethod]
        public void WhenGettingNonGenericEnumerator_ReturnSameThingAsGeneric()
        {
            //Arrange
            var observableList = Fixture.CreateMany<Dummy>().ToObservableList();

            //Act
            var result = ((IEnumerable)observableList).GetEnumerator();

            //Assert
            result.Should().BeEquivalentTo(observableList.GetEnumerator());
        }

        [TestMethod]
        public void Always_CorrectlyEnumeratesEveryItem()
        {
            //Arrange
            var observableList = Fixture.CreateMany<Dummy>().ToObservableList();

            var enumeratedItems = new List<Dummy>();

            //Act
            foreach (var item in observableList)
                enumeratedItems.Add(item);

            //Assert
            enumeratedItems.Should().NotBeEmpty();
            enumeratedItems.Should().BeEquivalentTo(observableList);
            enumeratedItems.Should().HaveCount(observableList.Count);
        }

        [TestMethod]
        public void WhenCollectionIsModifiedDuringEnumeration_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<Dummy>().ToObservableList();

            //Act
            var action = () =>
            {
                foreach (var item in observableList)
                    observableList.Remove(item);
            };

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenUsingResetAfterCollectionChanged_Throw()
        {
            //Arrange
            var observableList = Fixture.CreateMany<Dummy>().ToObservableList();

            var enumerator = observableList.GetEnumerator();
            observableList.RemoveAt(observableList.GetRandomIndex());

            //Act
            var action = () => enumerator.Reset();

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenUsingResetWhileCollectionIsStillUnchanged_SetCurrentToDefault()
        {
            //Arrange
            var observableList = Fixture.CreateMany<Dummy>().ToObservableList();
            var enumerator = observableList.GetEnumerator();

            //Act
            enumerator.Reset();

            //Assert
            enumerator.Current.Should().BeNull();
        }

    }
}