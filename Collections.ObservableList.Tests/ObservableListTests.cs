namespace Collections.ObservableList.Tests;

[TestClass]
public class ObservableListTests : ObservableListTester<ObservableList<Garbage>, Garbage>
{
    [TestMethod]
    public void Constructor_WhenUsingDefaultConstructor_InitializeWithEmptyArray()
    {
        //Arrange

        //Act
        var result = new ObservableList<Garbage>();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenUsingParams_InitializeWithThoseItems()
    {
        //Arrange
        var items = Dummy.Create<Garbage[]>();

        //Act
        var result = new ObservableList<Garbage>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Constructor_WhenPassingNull_Throw()
    {
        //Arrange

        //Act
        var action = () => new ObservableList<Garbage>(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Constructor_WhenPassingList_InitializeWithListItems()
    {
        //Arrange
        var items = Dummy.Create<List<Garbage>>();

        //Act
        var result = new ObservableList<Garbage>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Constructor_WhenUsingInitializer_AddThoseItemsToObservableList()
    {
        //Arrange
        var item1 = Dummy.Create<Garbage>();
        var item2 = Dummy.Create<Garbage>();
        var item3 = Dummy.Create<Garbage>();

        //Act
        var result = new ObservableList<Garbage> { item1, item2, item3 };

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage> { item1, item2, item3 });
    }
}