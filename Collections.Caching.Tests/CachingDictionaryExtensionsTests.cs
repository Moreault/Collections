namespace Collections.Caching.Tests;

[TestClass]
public sealed class CachingDictionaryExtensionsTests : Tester
{
    [TestMethod]
    public void ToCachingDictionaryKey_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Dummy> source = null!;

        //Act
        var action = () => source.ToCachingDictionary(x => x.Id);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToCachingDictionaryKey_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();
        Func<Dummy, int>? keySelector = null!;

        //Act
        var action = () => source.ToCachingDictionary(keySelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToCachingDictionaryKey_WhenIsEmpty_ConvertToEmptyCachingDictionary()
    {
        //Arrange
        var source = Enumerable.Empty<Dummy>();

        //Act
        var result = source.ToCachingDictionary(x => x.Id);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToCachingDictionaryKey_WhenIsNotEmpty_ConvertToCachingDictionary()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();

        //Act
        var result = source.ToCachingDictionary(x => x.Id);

        //Assert
        result.Should().BeEquivalentTo(source.ToDictionary(x => x.Id));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyElement_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Dummy> source = null!;

        //Act
        var action = () => source.ToCachingDictionary(x => x.Id, x => x.Description);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyElement_WhenKeySelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();
        Func<Dummy, int>? keySelector = null!;
        Func<Dummy, string> elementSelector = x => x.Description;

        //Act
        var action = () => source.ToCachingDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(keySelector));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyElement_WhenElementSelectorIsNull_Throw()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();
        Func<Dummy, int>? keySelector = x => x.Id;
        Func<Dummy, string> elementSelector = null!;

        //Act
        var action = () => source.ToCachingDictionary(keySelector, elementSelector);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(elementSelector));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyElement_WhenIsEmpty_ConvertToEmptyCachingDictionary()
    {
        //Arrange
        var source = Enumerable.Empty<Dummy>();

        //Act
        var result = source.ToCachingDictionary(x => x.Id, x => x.Description);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToCachingDictionaryKeyElement_WhenIsNotEmpty_ConvertToCachingDictionary()
    {
        //Arrange
        var source = Fixture.CreateMany<Dummy>().ToList();

        //Act
        var result = source.ToCachingDictionary(x => x.Id, x => x.Description);

        //Assert
        result.Should().BeEquivalentTo(source.ToDictionary(x => x.Id, x => x.Description));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyValuePair_WhenSourceIsNull_Throw()
    {
        //Arrange
        Dictionary<int, Dummy> source = null!;

        //Act
        var action = () => source.ToCachingDictionary();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToCachingDictionaryKeyValuePair_WhenSourceIsEmpty_ReturnEmpty()
    {
        //Arrange
        var source = new Dictionary<int, Dummy>();

        //Act
        var result = source.ToCachingDictionary();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToCachingDictionaryKeyValuePair_WhenSourceIsNotEmpty_ReturnEquivalent()
    {
        //Arrange
        var source = Fixture.Create<Dictionary<int, Dummy>>();

        //Act
        var result = source.ToCachingDictionary();

        //Assert
        result.Should().BeEquivalentTo(source);
    }
}