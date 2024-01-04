namespace Collections.ObservableList.Tests;

[TestClass]
public class ObservableListTests : ObservableListTester<ObservableList<Dummy>, Dummy>
{
    [TestMethod]
    public void Constructor_WhenUsingDefaultConstructor_InitializeWithEmptyArray()
    {
        //Arrange

        //Act
        var result = new ObservableList<Dummy>();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenUsingParams_InitializeWithThoseItems()
    {
        //Arrange
        var items = Fixture.Create<Dummy[]>();

        //Act
        var result = new ObservableList<Dummy>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Constructor_WhenPassingNull_Throw()
    {
        //Arrange

        //Act
        var action = () => new ObservableList<Dummy>(null!);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Constructor_WhenPassingList_InitializeWithListItems()
    {
        //Arrange
        var items = Fixture.Create<List<Dummy>>();

        //Act
        var result = new ObservableList<Dummy>(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Constructor_WhenUsingInitializer_AddThoseItemsToObservableList()
    {
        //Arrange
        var item1 = Fixture.Create<Dummy>();
        var item2 = Fixture.Create<Dummy>();
        var item3 = Fixture.Create<Dummy>();

        //Act
        var result = new ObservableList<Dummy> { item1, item2, item3 };

        //Assert
        result.Should().BeEquivalentTo(new List<Dummy> { item1, item2, item3 });
    }
}