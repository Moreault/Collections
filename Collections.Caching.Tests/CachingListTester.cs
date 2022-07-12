namespace Collections.Caching.Tests;

[TestClass]
public class CachingListTester
{
    [TestClass]
    public class Limit : Tester<CachingList<Dummy>>
    {
        [TestMethod]
        public void WhenUnset_ReturnIntMaxValueByDefault()
        {
            //Arrange

            //Act
            var result = Instance.Limit;

            //Assert
            result.Should().Be(int.MaxValue);
        }

        [TestMethod]
        public void WhenLimitIsSetToZero_ClearCollection()
        {
            //Arrange
            Instance.Add(Fixture.CreateMany<Dummy>());

            //Act
            Instance.Limit = 0;

            //Assert
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsSetToZero_PreventAnyNewItemFromBeingAdded()
        {
            //Arrange

            //Act
            Instance.Limit = 0;

            //Assert
            Instance.Add(Fixture.CreateMany<Dummy>());
            Instance.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsSetToZeroAndCollectionIsNotEmpty_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToList();
            Instance.Add(items);

            var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

            //Act
            Instance.Limit = 0;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new() { OldValues = items }
            });
        }

        [TestMethod]
        public void WhenLimitIsSetToZeroAndCollectionIsEmpty_DoNotTriggerEvent()
        {
            //Arrange
            var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

            //Act
            Instance.Limit = 0;

            //Assert
            eventArgs.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenLimitIsSetToHalfCollection_CutFirstHalfOfCollectionOut()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(14).ToList();
            Instance.Add(items);

            //Act
            Instance.Limit = 7;

            //Assert
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[7],
                items[8],
                items[9],
                items[10],
                items[11],
                items[12],
                items[13],
            });
        }

        [TestMethod]
        public void WhenLimitIsSetToHalfCollection_TrimDownTheBeginingOfCollectionToThatSizeAutomaticallyWhenAddingNewItems()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(14).ToList();
            Instance.Add(items);

            //Act
            Instance.Limit = 7;

            //Assert
            var newItems = Fixture.CreateMany<Dummy>(3).ToList();
            Instance.Add(newItems);
            Instance.Should().BeEquivalentTo(new List<Dummy>
            {
                items[10],
                items[11],
                items[12],
                items[13],
                newItems[0],
                newItems[1],
                newItems[2]
            });
        }

        [TestMethod]
        public void WhenLimitIsSetToHalfCollection_TriggerEvent()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>(14).ToList();
            Instance.Add(items);

            var eventArgs = new List<CollectionChangeEventArgs<Dummy>>();
            Instance.CollectionChanged += (_, args) => eventArgs.Add(args);

            //Act
            Instance.Limit = 7;

            //Assert
            eventArgs.Should().BeEquivalentTo(new List<CollectionChangeEventArgs<Dummy>>
            {
                new()
                {
                    OldValues = new List<Dummy>
                    {
                        items[0],
                        items[1],
                        items[2],
                        items[3],
                        items[4],
                        items[5],
                        items[6],
                    }
                }
            });
        }
    }
}