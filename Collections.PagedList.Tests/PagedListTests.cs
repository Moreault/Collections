namespace Collections.PagedList.Tests;

[TestClass]
public class PagedListTests
{
    private readonly Dummy _dummy = new();

    private static JsonSerializerOptions PagedListOptions() => new JsonSerializerOptions().WithPagedListConverters();

    [TestMethod]
    public void Empty_Always_ReturnEmptyPage()
    {
        //Act
        var result = PagedList<Garbage>.Empty;

        //Assert
        result.Should().BeEmpty();
        result.Count.Should().Be(0);
        result.PageNumber.Should().Be(1);
        result.TotalCount.Should().Be(0);
        result.PageCount.Should().Be(0);
    }

    [TestMethod]
    public void Empty_Always_ReturnSameReference()
    {
        //Act
        var result1 = PagedList<Garbage>.Empty;
        var result2 = PagedList<Garbage>.Empty;

        //Assert
        result1.Should().BeSameAs(result2);
    }

    [TestMethod]
    public void Constructor_WhenItemsIsNull_Throw()
    {
        //Act
        var action = () => new PagedList<Garbage>(null!, 1, 10, 0);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Constructor_WhenPageNumberIsLessThanOne_Throw()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => new PagedList<Garbage>(items, 0, 10, items.Count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Constructor_WhenPageSizeIsNegative_Throw()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => new PagedList<Garbage>(items, 1, -1, items.Count);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Constructor_WhenTotalCountIsNegative_Throw()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => new PagedList<Garbage>(items, 1, 10, -1);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Constructor_WhenValid_SetMetadata()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();

        //Act
        var result = new PagedList<Garbage>(items, 2, 5, 23);

        //Assert
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(23);
        result.Count.Should().Be(5);
        result.Items.Should().ContainInOrder(items.ToArray());
    }

    [TestMethod]
    public void Constructor_Always_CopyItemsSoCallerMutationsDoNotLeak()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(3).ToList();
        var instance = new PagedList<Garbage>(items, 1, 3, 3);

        //Act
        items.Add(_dummy.Create<Garbage>());

        //Assert
        instance.Count.Should().Be(3);
    }

    [TestMethod]
    public void PageCount_Always_RoundUp()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(10).ToList();

        //Act
        var result = new PagedList<Garbage>(items, 1, 10, 23);

        //Assert
        result.PageCount.Should().Be(3);
    }

    [TestMethod]
    public void Navigation_WhenOnMiddlePage_HasPreviousAndNext()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(10).ToList();

        //Act
        var result = new PagedList<Garbage>(items, 2, 10, 30);

        //Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
        result.IsFirstPage.Should().BeFalse();
        result.IsLastPage.Should().BeFalse();
    }

    [TestMethod]
    public void Navigation_WhenOnFirstPage_HasNoPrevious()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(10).ToList();

        //Act
        var result = new PagedList<Garbage>(items, 1, 10, 30);

        //Assert
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
        result.IsFirstPage.Should().BeTrue();
        result.IsLastPage.Should().BeFalse();
    }

    [TestMethod]
    public void Navigation_WhenOnLastPage_HasNoNext()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(10).ToList();

        //Act
        var result = new PagedList<Garbage>(items, 3, 10, 30);

        //Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
        result.IsFirstPage.Should().BeFalse();
        result.IsLastPage.Should().BeTrue();
    }

    [TestMethod]
    public void Indexer_WhenWithinRange_ReturnItem()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();
        var instance = new PagedList<Garbage>(items, 1, 5, 5);

        //Act
        var result = instance[2];

        //Assert
        result.Should().Be(items[2]);
    }

    [TestMethod]
    public void Indexer_WhenOutOfRange_Throw()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();
        var instance = new PagedList<Garbage>(items, 1, 5, 5);

        //Act
        var action = () => instance[instance.Count];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Enumeration_Always_YieldsCurrentPageInOrder()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();
        var instance = new PagedList<Garbage>(items, 1, 5, 5);

        //Act
        var result = instance.ToList();

        //Assert
        result.Should().ContainInOrder(items.ToArray());
    }

    [TestMethod]
    public void Equality_WhenSameItemsAndMetadata_AreEqual()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();
        var a = new PagedList<Garbage>(items, 2, 5, 23);
        var b = new PagedList<Garbage>(items, 2, 5, 23);

        //Assert
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
        (a == b).Should().BeTrue();
    }

    [TestMethod]
    public void Equality_WhenMetadataDiffers_AreNotEqual()
    {
        //Arrange
        var items = _dummy.CreateMany<Garbage>(5).ToList();
        var a = new PagedList<Garbage>(items, 2, 5, 23);
        var b = new PagedList<Garbage>(items, 3, 5, 23);

        //Assert
        a.Should().NotBe(b);
    }

    [TestMethod]
    public void Equality_WhenItemsDiffer_AreNotEqual()
    {
        //Arrange
        var a = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 1, 5, 5);
        var b = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 1, 5, 5);

        //Assert
        a.Should().NotBe(b);
    }

    [TestMethod]
    public void ToPagedList_WhenPageNumberIsLessThanOne_Throw()
    {
        //Arrange
        var source = _dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.ToPagedList(0, 10);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void ToPagedList_WhenPageSizeIsLessThanOne_Throw()
    {
        //Arrange
        var source = _dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.ToPagedList(1, 0);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void ToPagedList_Always_ReturnRequestedPageWithTotalCount()
    {
        //Arrange
        var source = _dummy.CreateMany<Garbage>(23).ToList();

        //Act
        var result = source.ToPagedList(2, 10);

        //Assert
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(23);
        result.PageCount.Should().Be(3);
        result.Items.Should().ContainInOrder(source.Skip(10).Take(10).ToArray());
    }

    [TestMethod]
    public void ToPagedList_WhenLastPageIsPartial_ReturnRemainingItems()
    {
        //Arrange
        var source = _dummy.CreateMany<Garbage>(23).ToList();

        //Act
        var result = source.ToPagedList(3, 10);

        //Assert
        result.Count.Should().Be(3);
        result.IsLastPage.Should().BeTrue();
    }

    [TestMethod]
    public void Serialization_WhenUsingSystemText_RoundTrips()
    {
        //Arrange
        var instance = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 2, 5, 23);
        var options = PagedListOptions();

        //Act
        var json = JsonSerializer.Serialize(instance, options);
        var result = JsonSerializer.Deserialize<PagedList<Garbage>>(json, options);

        //Assert
        result.Should().Be(instance);
    }

    [TestMethod]
    public void Serialization_Always_WritesAnObjectWithMetadataRatherThanABareArray()
    {
        //Arrange
        var instance = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 2, 5, 23);
        var options = PagedListOptions();

        //Act
        var json = JsonSerializer.Serialize(instance, options);

        //Assert
        json.TrimStart().Should().StartWith("{");
        json.Should().Contain("\"PageNumber\"");
        json.Should().Contain("\"PageSize\"");
        json.Should().Contain("\"TotalCount\"");
        json.Should().Contain("\"PageCount\"");
        json.Should().Contain("\"Items\"");
    }

    [TestMethod]
    public void Serialization_WhenUsingCamelCaseNamingPolicy_RoundTrips()
    {
        //Arrange
        var instance = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 2, 5, 23);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }.WithPagedListConverters();

        //Act
        var json = JsonSerializer.Serialize(instance, options);
        var result = JsonSerializer.Deserialize<PagedList<Garbage>>(json, options);

        //Assert
        json.Should().Contain("\"pageNumber\"");
        result.Should().Be(instance);
    }

    [TestMethod]
    public void Serialization_WhenNestedInAnotherObject_RoundTrips()
    {
        //Arrange
        var instance = new Envelope
        {
            Page = new PagedList<Garbage>(_dummy.CreateMany<Garbage>(5).ToList(), 2, 5, 23)
        };
        var options = PagedListOptions();

        //Act
        var result = JsonSerializer.Deserialize<Envelope>(JsonSerializer.Serialize(instance, options), options);

        //Assert
        result!.Page.Should().Be(instance.Page);
    }

    [TestMethod]
    public void Serialization_WhenValueIsNull_RoundTripsAsNull()
    {
        //Arrange
        PagedList<Garbage>? instance = null;
        var options = PagedListOptions();

        //Act
        var json = JsonSerializer.Serialize(instance, options);
        var result = JsonSerializer.Deserialize<PagedList<Garbage>>(json, options);

        //Assert
        result.Should().BeNull();
    }

    private sealed record Envelope
    {
        public PagedList<Garbage> Page { get; init; } = PagedList<Garbage>.Empty;
    }
}
