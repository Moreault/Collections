using ToolBX.Dummies;

namespace Collections.ObservableList.Tests;

[TestClass]
public class ObservableListExtensionsTests : ToolBX.Collections.UnitTesting.Tester
{
    [TestMethod]
    public void ToObservableList_WhenCollectionIsNull_Throw()
    {
        //Arrange
        string[] collection = null!;

        //Act
        var action = () => collection.ToObservableList();

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ToObservableList_WhenCollectionIsNotNull_ReturnObservableList()
    {
        //Arrange
        var collection = Dummy.CreateMany<string>().ToList();

        //Act
        var result = collection.ToObservableList();

        //Assert
        result.Should().BeOfType<ObservableList<string>>();
        result.Should().BeEquivalentTo(collection);
    }
}