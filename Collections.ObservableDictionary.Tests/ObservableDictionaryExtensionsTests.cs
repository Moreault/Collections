namespace Collections.ObservableDictionary.Tests;

[TestClass]
public sealed class ObservableDictionaryExtensionsTests : Tester
{
    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Dummy> source = null!;

        //Act
        var action = () => source.ToObservableDictionary(x => x.Id);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.Create<IEnumerable<KeyValuePair<int, Dummy>>>();
        Func<KeyValuePair<int, Dummy>, int> keySelector = null!;

        //Act
        var action = () => source.ToObservableDictionary(keySelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsEmpty_ReturnEmpty()
    {
        //Arrange
        var source = Enumerable.Empty<Dummy>();

        //Act
        var result = source.ToObservableDictionary(x => x.Id);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsNotEmpty_ReturnEquivalentDictionary()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();

        //Act
        var result = source.ToObservableDictionary(x => x.Id);

        //Assert
        result.Should().BeEquivalentTo(source.ToDictionary(x => x.Id));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<int, Dummy>> source = null!;
        var keySelector = Fixture.Create<Func<KeyValuePair<int, Dummy>, int>>();
        var elementSelector = Fixture.Create<Func<KeyValuePair<int, Dummy>, Dummy>>();

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.Create<IEnumerable<KeyValuePair<int, Dummy>>>();
        Func<KeyValuePair<int, Dummy>, int> keySelector = null!;
        var elementSelector = Fixture.Create<Func<KeyValuePair<int, Dummy>, Dummy>>();

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenElementSelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.Create<IEnumerable<KeyValuePair<int, Dummy>>>();
        var keySelector = Fixture.Create<Func<KeyValuePair<int, Dummy>, int>>();
        Func<KeyValuePair<int, Dummy>, Dummy> elementSelector = null!;

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(elementSelector));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsEmpty_ReturnEmpty()
    {
        //Arrange
        var source = Enumerable.Empty<KeyValuePair<int, Dummy>>();

        //Act
        var result = source.ToObservableDictionary(x => x.Key, x => x.Value);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsNotEmpty_ReturnEquivalentDictionary()
    {
        //Arrange
        var source = Fixture.Create<Dictionary<int, Dummy>>();

        //Act
        var result = source.ToObservableDictionary(x => x.Key, x => x.Value);

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}