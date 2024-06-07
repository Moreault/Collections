namespace Collections.ReadOnly.Tests;

[TestClass]
public class ReadOnlyListTests : RecordTester<ReadOnlyList<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }

    [TestMethod]
    public void Empty_Always_ReturnsEmptyReadOnlyList()
    {
        //Arrange

        //Act
        var result = ReadOnlyList<Garbage>.Empty;

        //Assert
        result.Should().BeOfType<ReadOnlyList<Garbage>>();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Empty_Always_ReturnSameReference()
    {
        //Arrange

        //Act
        var result1 = ReadOnlyList<Garbage>.Empty;
        var result2 = ReadOnlyList<Garbage>.Empty;

        //Assert
        result1.Should().BeSameAs(result2);
    }

    [TestMethod]
    public void Indexer_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var index = -Dummy.Create<int>();

        //Act
        var action = () => instance[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Indexer_WhenIndexIsOutOfRange_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var index = instance.Count;

        //Act
        var action = () => instance[index];

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void Indexer_WhenIndexIsWithinRange_ReturnItem()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToArray();
        var instance = items.ToReadOnlyList();
        var index = 1;

        //Act
        var result = instance[index];

        //Assert
        result.Should().Be(items[index]);
    }

    [TestMethod]
    public void Count_WhenEmpty_ReturnZero()
    {
        //Arrange

        //Act
        var result = new ReadOnlyList<Garbage>().Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void Count_WhenContainsItems_ReturnAmount()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToArray();
        var instance = items.ToReadOnlyList();

        //Act
        var result = instance.Count;

        //Assert
        result.Should().Be(items.Length);
    }

    [TestMethod]
    public void Constructors_WhenUsingDefault_CreateEmpty()
    {
        //Arrange

        //Act
        var result = new ReadOnlyList<Garbage>();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructors_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> source = null!;

        //Act
        var action = () => new ReadOnlyList<Garbage>(source);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void Constructors_WhenSourceIsNotNull_CopySource()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = new ReadOnlyList<Garbage>(source);

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void Constructors_WhenSourceIsNotNullAndOriginalIsModified_ReadOnlyListIsUnaffected()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();
        var instance = new ReadOnlyList<Garbage>(source);

        //Act
        source.Add(Dummy.Create<Garbage>());

        //Assert
        instance.Should().NotBeEquivalentTo(source);
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnEmptyMessage()
    {
        //Arrange
        var instance = new ReadOnlyList<Garbage>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("Empty ReadOnlyList<Garbage>");
    }

    [TestMethod]
    public void ToString_WhenIsNotEmpty_ReturnCount()
    {
        //Arrange
        var instance = new ReadOnlyList<Garbage>(Dummy.CreateMany<Garbage>(3));

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be("ReadOnlyList<Garbage> with 3 elements");
    }

    [TestMethod]
    public void ToReadOnlyList_WhenSourceIsNull_Throw()
    {
        //Arrange
        IEnumerable<Garbage> source = null!;

        //Act
        var action = () => source.ToReadOnlyList();

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void ToReadOnlyList_WhenSourceIsNotNull_Copy()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.ToReadOnlyList();

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithParams_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.With(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithParams_WhenItemsIsEmpty_ReturnCopy()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.With();

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithParams_WhenOneItem_AddItAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item = Dummy.Create<Garbage>();

        //Act
        var result = source.With(item);

        //Assert
        result.Should().BeEquivalentTo(source.Concat(new[] { item }));
    }

    [TestMethod]
    public void WithParams_WhenOneItem_InstanceIsUnchanged()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item = Dummy.Create<Garbage>();

        //Act
        source.With(item);

        //Assert
        source.Should().NotContain(item);
    }

    [TestMethod]
    public void WithParams_WhenMultipleItems_AddThemAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item1 = Dummy.Create<Garbage>();
        var item2 = Dummy.Create<Garbage>();
        var item3 = Dummy.Create<Garbage>();

        //Act
        var result = source.With(item1, item2, item3);

        //Assert
        result.Should().BeEquivalentTo(source.Concat(new[] { item1, item2, item3 }));
    }

    [TestMethod]
    public void WithParams_WhenMultipleItems_InstanceIsUnchanged()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item1 = Dummy.Create<Garbage>();
        var item2 = Dummy.Create<Garbage>();
        var item3 = Dummy.Create<Garbage>();

        //Act
        source.With(item1, item2, item3);

        //Assert
        source.Should().NotContain(item1);
        source.Should().NotContain(item2);
        source.Should().NotContain(item3);
    }

    [TestMethod]
    public void WithEnumerable_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.With(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithEnumerable_WhenItemsIsNull_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        IEnumerable<Garbage> items = null!;

        //Act
        var action = () => source.With(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void WithEnumerable_WhenItemsIsEmpty_ReturnCopy()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.With();

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithEnumerable_WhenOneItem_AddItAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item = Dummy.Create<Garbage>();

        //Act
        var result = source.With(new List<Garbage> { item });

        //Assert
        result.Should().BeEquivalentTo(source.Concat(new[] { item }));
    }

    [TestMethod]
    public void WithEnumerable_WhenOneItem_InstanceIsUnchanged()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var item = Dummy.Create<Garbage>();

        //Act
        source.With(new List<Garbage> { item });

        //Assert
        source.Should().NotContain(item);
    }

    [TestMethod]
    public void WithEnumerable_WhenMultipleItems_AddThemAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.With(items);

        //Assert
        result.Should().BeEquivalentTo(source.Concat(items));
    }

    [TestMethod]
    public void WithEnumerable_WhenMultipleItems_InstanceIsUnchanged()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        source.With(items);

        //Assert
        source.Should().NotContain(items);
    }

    [TestMethod]
    public void WithoutPredicate_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var predicate = Dummy.Create<Func<Garbage, bool>>();

        //Act
        var action = () => source.Without(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithoutPredicate_WhenPredicateIsNull_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        Func<Garbage, bool> predicate = null!;

        //Act
        var action = () => source.Without(predicate);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
    }

    [TestMethod]
    public void WithoutPredicate_WhenPredicateDoesNotCorrespondToAnything_DoNotChangeCollection()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.Without(x => x.Id < 0);

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithoutPredicate_WhenPredicateIsEqualToEverything_RemoveEverything()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.Without(x => x.Id >= 0);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void WithoutPredicate_WhenPredicateIsEqualToEverything_DoNotAffectSource()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var original = source.ToReadOnlyList();

        //Act
        source.Without(x => x.Id >= 0);

        //Assert
        source.Should().BeEquivalentTo(original);
    }

    [TestMethod]
    public void WithoutPredicate_WhenPredicateIsOnlyEqualToSomeStuff_OnlyRemoveThat()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.Without(x => x.Id == source[1].Id);

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage>
            {
                source[0],
                source[2],
            });
    }

    [TestMethod]
    public void WithoutParams_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.Without(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithoutParams_WhenItemsEmpty_ReturnSameThing()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();

        //Act
        var result = source.Without();

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithoutParams_WhenItemsIsNotEmptyButAlsoNotInCollection_ReturnSameThing()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var result = source.Without(items);

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithoutParams_WhenItemsContainsItemsThatAreInSource_RemoveThem()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var item = source[2];

        //Act
        var result = source.Without(item);

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage>
        {
            source[0],
            source[1]
        });
    }

    [TestMethod]
    public void WithoutEnumerable_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.Without(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithoutEnumerable_WhenItemsEmpty_ReturnSameThing()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        IEnumerable<Garbage> items = null!;

        //Act
        var action = () => source.Without(items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void WithoutEnumerable_WhenItemsIsNotEmptyButAlsoNotInCollection_ReturnSameThing()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.Without(items);

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithoutEnumerable_WhenItemsContainsItemsThatAreInSource_RemoveThem()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var items = new List<Garbage>
        {
            source[1],
            source[0]
        };

        //Act
        var result = source.Without(items);

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage>
        {
            source[2]
        });
    }

    [TestMethod]
    public void WithAtParams_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var index = Dummy.Create<int>();
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithAtParams_WhenItemsIsEmptyButIndexIsWithinRange_DoNotAddAnything()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = 1;

        //Act
        var result = source.WithAt(index);

        //Assert
        result.Should().BeEquivalentTo(source);
    }

    [TestMethod]
    public void WithAtParams_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = -Dummy.Create<int>();
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithAtParams_WhenIndexIsOutOfRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = source.Count + 1;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithAtParams_WhenIndexIsEqualToCount_AddAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = source.Count;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(source.Concat(items));
    }

    [TestMethod]
    public void WithAtParams_WhenIndexIsZero_AddAtTheStart()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = 0;
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(items.Concat(source));
    }

    [TestMethod]
    public void WithAtParams_WhenIndexIsOne_AddBetween()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>(3).ToReadOnlyList();
        var index = 1;
        var items = Dummy.CreateMany<Garbage>(3).ToArray();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(new List<Garbage>
            {
                source[0],
                items[0],
                items[1],
                items[2],
                source[1],
                source[2],
            });
    }

    [TestMethod]
    public void WithAtEnumerable_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var index = Dummy.Create<int>();
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithAtEnumerable_WhenItemsIsNullButIndexIsWithinRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = 1;
        IEnumerable<Garbage> items = null!;

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
    }

    [TestMethod]
    public void WithAtEnumerable_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = -Dummy.Create<int>();
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithAtEnumerable_WhenIndexIsOutOfRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = source.Count + 1;
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var action = () => source.WithAt(index, items);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithAtEnumerable_WhenIndexIsEqualToCount_AddAtTheEnd()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = source.Count;
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(source.Concat(items));
    }

    [TestMethod]
    public void WithAtEnumerable_WhenIndexIsZero_AddAtTheStart()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = 0;
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(items.Concat(source));
    }

    [TestMethod]
    public void WithAtEnumerable_WhenIndexIsOne_AddBetween()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>(3).ToReadOnlyList();
        var index = 1;
        var items = Dummy.CreateMany<Garbage>(3).ToList();

        //Act
        var result = source.WithAt(index, items);

        //Assert
        result.Should().ContainInOrder(new List<Garbage>
            {
                source[0],
                items[0],
                items[1],
                items[2],
                source[1],
                source[2],
            });
    }

    [TestMethod]
    public void WithoutAt_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var index = Dummy.Create<int>();

        //Act
        var action = () => source.WithoutAt(index);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithoutAt_WhenIndexIsNegative_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = -Dummy.Create<int>();

        //Act
        var action = () => source.WithoutAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithoutAt_WhenIndexIsOutOfRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = source.Count;

        //Act
        var action = () => source.WithoutAt(index);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithoutAt_WhenIndexIsWithinRange_Remove()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var index = 1;

        //Act
        var result = source.WithoutAt(index);

        //Assert
        result.Should().BeEquivalentTo(new List<Garbage>
        {
            source[0], source[2]
        });
    }

    [TestMethod]
    public void WithSwapped_WhenSourceIsNull_Throw()
    {
        //Arrange
        IReadOnlyList<Garbage> source = null!;
        var current = Dummy.Create<int>();
        var destination = Dummy.Create<int>();

        //Act
        var action = () => source.WithSwapped(current, destination);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
    }

    [TestMethod]
    public void WithSwapped_WhenCurrentIsNegative_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = -Dummy.Create<int>();
        var destination = 0;

        //Act
        var action = () => source.WithSwapped(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithSwapped_WhenCurrentIsOutOfRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = source.Count;
        var destination = 1;

        //Act
        var action = () => source.WithSwapped(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithSwapped_WhenDestinationIsNegative_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = 2;
        var destination = -Dummy.Create<int>();

        //Act
        var action = () => source.WithSwapped(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithSwapped_WhenDestinationIsOutOfRange_Throw()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = 1;
        var destination = source.Count;

        //Act
        var action = () => source.WithSwapped(current, destination);

        //Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestMethod]
    public void WithSwapped_WhenBothCurrentAndDestinationAreTheSame_ChangeNothing()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = 1;
        var destination = 1;

        //Act
        var result = source.WithSwapped(current, destination);

        //Assert
        result.Should().ContainInOrder(source);
    }

    [TestMethod]
    public void WithSwapped_WhenBothCurrentAndDestinationAreDifferentAndWithinRange_Swap()
    {
        //Arrange
        var source = Dummy.CreateMany<Garbage>().ToReadOnlyList();
        var current = 2;
        var destination = 0;

        //Act
        var result = source.WithSwapped(current, destination);

        //Assert
        result.Should().ContainInOrder(new List<Garbage>
            {
                source[2],
                source[1],
                source[0]
            });
    }

    [TestMethod]
    public void CreateParams_Always_ReturnNewReadOnlyList()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToArray();

        //Act
        var result = ReadOnlyList.Create(items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void CreateEnumerable_Always_ReturnNewReadOnlyList()
    {
        //Arrange
        var items = Dummy.CreateMany<Garbage>().ToList();

        //Act
        var result = ReadOnlyList.Create((IEnumerable<Garbage>)items);

        //Assert
        result.Should().BeEquivalentTo(items);
    }

    [TestMethod]
    public void Serialization_WhenUsingSystemText_SerializeAndDeserializeBack() => Ensure.IsJsonSerializable<GarbageWithListInside>(Dummy, JsonSerializerOptions.WithReadOnlyConverters());

    [TestMethod]
    public void Serialization_WhenUsingSystemTextOnListItself_SerializeAndDeserializeBack() => Ensure.IsJsonSerializable<ReadOnlyList<Garbage>>(Dummy, JsonSerializerOptions.WithReadOnlyConverters());

    [TestMethod]
    public void Serialization_WhenUsingNewtonsoftJson_SerializeAndDeserializeBack()
    {
        //Arrange
        var instance = Dummy.Create<GarbageWithListInside>();

        //Act
        var result = JsonConvert.DeserializeObject<GarbageWithListInside>(JsonConvert.SerializeObject(instance));

        //Assert
        result.Should().BeEquivalentTo(instance);
    }
}