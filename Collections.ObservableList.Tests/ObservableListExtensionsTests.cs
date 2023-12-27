namespace Collections.ObservableList.Tests;

[TestClass]
public class ObservableListExtensionsTests
{
    [TestClass]
    public class ToObservableList : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            string[] collection = null!;

            //Act
            var action = () => collection.ToObservableList();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsNotNull_ReturnObservableList()
        {
            //Arrange
            var collection = Fixture.CreateMany<string>().ToList();

            //Act
            var result = collection.ToObservableList();

            //Assert
            result.Should().BeOfType<ObservableList<string>>();
            result.Should().BeEquivalentTo(collection);
        }
    }
}