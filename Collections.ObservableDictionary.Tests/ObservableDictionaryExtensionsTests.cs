namespace Collections.ObservableDictionary.Tests;

[TestClass]
public sealed class ObservableDictionaryExtensionsTests : ToolBX.Collections.UnitTesting.Tester
{
    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> source = null!;

        //Act
        var action = () => source.ToObservableDictionary(x => x.Id);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Dummy.Create<IEnumerable<KeyValuePair<int, Garbage>>>();
        Func<KeyValuePair<int, Garbage>, int> keySelector = null!;

        //Act
        var action = () => source.ToObservableDictionary(keySelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsEmpty_ReturnEmpty()
    {
        //Arrange
        var source = Enumerable.Empty<Garbage>();

        //Act
        var result = source.ToObservableDictionary(x => x.Id);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToObservableDictionaryKeySelector_WhenSourceIsNotEmpty_ReturnEquivalentDictionary()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.ToObservableDictionary(x => x.Id);

        //Assert
        result.Should().BeEquivalentTo(source.ToDictionary(x => x.Id));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<KeyValuePair<int, Garbage>> source = null!;
        var keySelector = Dummy.Create<Func<KeyValuePair<int, Garbage>, int>>();
        var elementSelector = Dummy.Create<Func<KeyValuePair<int, Garbage>, Garbage>>();

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Dummy.Create<IEnumerable<KeyValuePair<int, Garbage>>>();
        Func<KeyValuePair<int, Garbage>, int> keySelector = null!;
        var elementSelector = Dummy.Create<Func<KeyValuePair<int, Garbage>, Garbage>>();

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenElementSelectorIsNull_Throw()
    {
        //Arrange
        var source = Dummy.Create<IEnumerable<KeyValuePair<int, Garbage>>>();
        var keySelector = Dummy.Create<Func<KeyValuePair<int, Garbage>, int>>();
        Func<KeyValuePair<int, Garbage>, Garbage> elementSelector = null!;

        //Act
        var action = () => source.ToObservableDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(elementSelector));
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsEmpty_ReturnEmpty()
    {
        //Arrange
        var source = Enumerable.Empty<KeyValuePair<int, Garbage>>();

        //Act
        var result = source.ToObservableDictionary(x => x.Key, x => x.Value);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToObservableDictionaryBothSelectors_WhenSourceIsNotEmpty_ReturnEquivalentDictionary()
    {
        //Arrange
        var source = Dummy.Create<Dictionary<int, Garbage>>();

        //Act
        var result = source.ToObservableDictionary(x => x.Key, x => x.Value);

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}