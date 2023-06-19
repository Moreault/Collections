using AutoFixture;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ToolBX.Eloquentest;

namespace Collections.ReadOnly.Tests;

[TestClass]
public class ReadOnlyListTester
{
    [TestClass]
    public class Empty : Tester
    {
        [TestMethod]
        public void Always_ReturnsEmptyReadOnlyList()
        {
            //Arrange

            //Act
            var result = ReadOnlyList<Dummy>.Empty;

            //Assert
            result.Should().BeOfType<ReadOnlyList<Dummy>>();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void Always_ReturnSameReference()
        {
            //Arrange

            //Act
            var result1 = ReadOnlyList<Dummy>.Empty;
            var result2 = ReadOnlyList<Dummy>.Empty;

            //Assert
            result1.Should().BeSameAs(result2);
        }
    }

    [TestClass]
    public class Indexer : Tester
    {
        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var index = -Fixture.Create<int>();

            //Act
            var action = () => instance[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsOutOfRange_Throw()
        {
            //Arrange
            var instance = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var index = instance.Count;

            //Act
            var action = () => instance[index];

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsWithinRange_ReturnItem()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToArray();
            var instance = items.ToReadOnlyList();
            var index = 1;

            //Act
            var result = instance[index];

            //Assert
            result.Should().Be(items[index]);
        }
    }

    [TestClass]
    public class Count : Tester
    {
        [TestMethod]
        public void WhenEmpty_ReturnZero()
        {
            //Arrange

            //Act
            var result = new ReadOnlyList<Dummy>().Count;

            //Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void WhenContainsItems_ReturnAmount()
        {
            //Arrange
            var items = Fixture.CreateMany<Dummy>().ToArray();
            var instance = items.ToReadOnlyList();

            //Act
            var result = instance.Count;

            //Assert
            result.Should().Be(items.Length);
        }
    }

    [TestClass]
    public class Constructors : Tester
    {
        [TestMethod]
        public void WhenUsingDefault_CreateEmpty()
        {
            //Arrange

            //Act
            var result = new ReadOnlyList<Dummy>();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> source = null!;

            //Act
            var action = () => new ReadOnlyList<Dummy>(source);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenSourceIsNotNull_CopySource()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = new ReadOnlyList<Dummy>(source);

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenSourceIsNotNullAndOriginalIsModified_ReadOnlyListIsUnaffected()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToList();
            var instance = new ReadOnlyList<Dummy>(source);

            //Act
            source.Add(Fixture.Create<Dummy>());

            //Assert
            instance.Should().NotBeEquivalentTo(source);
        }
    }

    [TestClass]
    public class Equality : Tester
    {
        [TestMethod]
        public void WhenOtherIsNull_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            ReadOnlyList<Dummy> other = null!;

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsNullWithOperators_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            ReadOnlyList<Dummy> other = null!;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsSameReference_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());

            //Act
            var result = instance.Equals(instance);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsSameReferenceWithOperators_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = instance;

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsDifferent_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentWithOperators_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentAndDifferentType_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsDifferentAndDifferentTypeWithOperators_ReturnFalse()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOtherIsNotSameInstanceButSameContent_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = new ReadOnlyList<Dummy>(instance);

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsNotSameInstanceButSameContentWithOperators_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = new ReadOnlyList<Dummy>(instance);

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsNotSameTypeButSameContent_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = instance.ToList();

            //Act
            var result = instance.Equals(other);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOtherIsNotSameTypeButSameContentWithOperators_ReturnTrue()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>());
            var other = instance.ToList();

            //Act
            var result = instance == other;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void WhenIsEmpty_ReturnEmptyMessage()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("Empty ReadOnlyList<Dummy>");
        }

        [TestMethod]
        public void WhenIsNotEmpty_ReturnCount()
        {
            //Arrange
            var instance = new ReadOnlyList<Dummy>(Fixture.CreateMany<Dummy>(3));

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be("ReadOnlyList<Dummy> with 3 elements");
        }
    }

    [TestClass]
    public class ToReadOnlyList : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IEnumerable<Dummy> source = null!;

            //Act
            var action = () => source.ToReadOnlyList();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenSourceIsNotNull_Copy()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = source.ToReadOnlyList();

            //Assert
            result.Should().BeEquivalentTo(source);
        }
    }

    [TestClass]
    public class With_Params : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.With(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsIsEmpty_ReturnCopy()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.With();

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenOneItem_AddItAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item = Fixture.Create<Dummy>();

            //Act
            var result = source.With(item);

            //Assert
            result.Should().BeEquivalentTo(source.Concat(new[] { item }));
        }

        [TestMethod]
        public void WhenOneItem_InstanceIsUnchanged()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item = Fixture.Create<Dummy>();

            //Act
            source.With(item);

            //Assert
            source.Should().NotContain(item);
        }

        [TestMethod]
        public void WhenMultipleItems_AddThemAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item1 = Fixture.Create<Dummy>();
            var item2 = Fixture.Create<Dummy>();
            var item3 = Fixture.Create<Dummy>();

            //Act
            var result = source.With(item1, item2, item3);

            //Assert
            result.Should().BeEquivalentTo(source.Concat(new[] { item1, item2, item3 }));
        }

        [TestMethod]
        public void WhenMultipleItems_InstanceIsUnchanged()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item1 = Fixture.Create<Dummy>();
            var item2 = Fixture.Create<Dummy>();
            var item3 = Fixture.Create<Dummy>();

            //Act
            source.With(item1, item2, item3);

            //Assert
            source.Should().NotContain(item1);
            source.Should().NotContain(item2);
            source.Should().NotContain(item3);
        }
    }

    [TestClass]
    public class With_Enumerable : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var action = () => source.With(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsIsNull_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            IEnumerable<Dummy> items = null!;

            //Act
            var action = () => source.With(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
        }

        [TestMethod]
        public void WhenItemsIsEmpty_ReturnCopy()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.With();

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenOneItem_AddItAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item = Fixture.Create<Dummy>();

            //Act
            var result = source.With(new List<Dummy> { item });

            //Assert
            result.Should().BeEquivalentTo(source.Concat(new[] { item }));
        }

        [TestMethod]
        public void WhenOneItem_InstanceIsUnchanged()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var item = Fixture.Create<Dummy>();

            //Act
            source.With(new List<Dummy> { item });

            //Assert
            source.Should().NotContain(item);
        }

        [TestMethod]
        public void WhenMultipleItems_AddThemAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = source.With(items);

            //Assert
            result.Should().BeEquivalentTo(source.Concat(items));
        }

        [TestMethod]
        public void WhenMultipleItems_InstanceIsUnchanged()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            source.With(items);

            //Assert
            source.Should().NotContain(items);
        }
    }

    [TestClass]
    public class Without_Predicate : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var predicate = Fixture.Create<Func<Dummy, bool>>();

            //Act
            var action = () => source.Without(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            Func<Dummy, bool> predicate = null!;

            //Act
            var action = () => source.Without(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(predicate));
        }

        [TestMethod]
        public void WhenPredicateDoesNotCorrespondToAnything_DoNotChangeCollection()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.Without(x => x.Id < 0);

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenPredicateIsEqualToEverything_RemoveEverything()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.Without(x => x.Id >= 0);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenPredicateIsEqualToEverything_DoNotAffectSource()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var original = source.ToReadOnlyList();

            //Act
            source.Without(x => x.Id >= 0);

            //Assert
            source.Should().BeEquivalentTo(original);
        }

        [TestMethod]
        public void WhenPredicateIsOnlyEqualToSomeStuff_OnlyRemoveThat()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.Without(x => x.Id == source[1].Id);

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy>
            {
                source[0],
                source[2],
            });
        }
    }

    [TestClass]
    public class Without_Params : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.Without(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsEmpty_ReturnSameThing()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();

            //Act
            var result = source.Without();

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenItemsIsNotEmptyButAlsoNotInCollection_ReturnSameThing()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var result = source.Without(items);

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenItemsContainsItemsThatAreInSource_RemoveThem()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var item = source[2];

            //Act
            var result = source.Without(item);

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy>
            {
                source[0],
                source[1]
            });
        }
    }

    [TestClass]
    public class Without_Enumerable : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.Without(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsEmpty_ReturnSameThing()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            IEnumerable<Dummy> items = null!;

            //Act
            var action = () => source.Without(items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
        }

        [TestMethod]
        public void WhenItemsIsNotEmptyButAlsoNotInCollection_ReturnSameThing()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = source.Without(items);

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenItemsContainsItemsThatAreInSource_RemoveThem()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var items = new List<Dummy>
            {
                source[1],
                source[0]
            };

            //Act
            var result = source.Without(items);

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy>
            {
                source[2]
            });
        }
    }

    [TestClass]
    public class WithAt_Params : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var index = Fixture.Create<int>();
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsIsEmptyButIndexIsWithinRange_DoNotAddAnything()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = 1;

            //Act
            var result = source.WithAt(index);

            //Assert
            result.Should().BeEquivalentTo(source);
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = -Fixture.Create<int>();
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsOutOfRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = source.Count + 1;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsEqualToCount_AddAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = source.Count;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(source.Concat(items));
        }

        [TestMethod]
        public void WhenIndexIsZero_AddAtTheStart()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = 0;
            var items = Fixture.CreateMany<Dummy>().ToArray();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(items.Concat(source));
        }

        [TestMethod]
        public void WhenIndexIsOne_AddBetween()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>(3).ToReadOnlyList();
            var index = 1;
            var items = Fixture.CreateMany<Dummy>(3).ToArray();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(new List<Dummy>
            {
                source[0],
                items[0],
                items[1],
                items[2],
                source[1],
                source[2],
            });
        }
    }

    [TestClass]
    public class WithAt_Enumerable : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var index = Fixture.Create<int>();
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenItemsIsNullButIndexIsWithinRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = 1;
            IEnumerable<Dummy> items = null!;

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(items));
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = -Fixture.Create<int>();
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsOutOfRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = source.Count + 1;
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var action = () => source.WithAt(index, items);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsEqualToCount_AddAtTheEnd()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = source.Count;
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(source.Concat(items));
        }

        [TestMethod]
        public void WhenIndexIsZero_AddAtTheStart()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = 0;
            var items = Fixture.CreateMany<Dummy>().ToList();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(items.Concat(source));
        }

        [TestMethod]
        public void WhenIndexIsOne_AddBetween()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>(3).ToReadOnlyList();
            var index = 1;
            var items = Fixture.CreateMany<Dummy>(3).ToList();

            //Act
            var result = source.WithAt(index, items);

            //Assert
            result.Should().ContainInOrder(new List<Dummy>
            {
                source[0],
                items[0],
                items[1],
                items[2],
                source[1],
                source[2],
            });
        }
    }

    [TestClass]
    public class WithoutAt : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var index = Fixture.Create<int>();

            //Act
            var action = () => source.WithoutAt(index);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenIndexIsNegative_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = -Fixture.Create<int>();

            //Act
            var action = () => source.WithoutAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsOutOfRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = source.Count;

            //Act
            var action = () => source.WithoutAt(index);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenIndexIsWithinRange_Remove()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var index = 1;

            //Act
            var result = source.WithoutAt(index);

            //Assert
            result.Should().BeEquivalentTo(new List<Dummy>
            {
                source[0], source[2]
            });
        }
    }

    [TestClass]
    public class WithSwapped : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<Dummy> source = null!;
            var current = Fixture.Create<int>();
            var destination = Fixture.Create<int>();

            //Act
            var action = () => source.WithSwapped(current, destination);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(source));
        }

        [TestMethod]
        public void WhenCurrentIsNegative_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = -Fixture.Create<int>();
            var destination = 0;

            //Act
            var action = () => source.WithSwapped(current, destination);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenCurrentIsOutOfRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = source.Count;
            var destination = 1;

            //Act
            var action = () => source.WithSwapped(current, destination);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenDestinationIsNegative_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = 2;
            var destination = -Fixture.Create<int>();

            //Act
            var action = () => source.WithSwapped(current, destination);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenDestinationIsOutOfRange_Throw()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = 1;
            var destination = source.Count;

            //Act
            var action = () => source.WithSwapped(current, destination);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenBothCurrentAndDestinationAreTheSame_ChangeNothing()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = 1;
            var destination = 1;

            //Act
            var result = source.WithSwapped(current, destination);

            //Assert
            result.Should().ContainInOrder(source);
        }

        [TestMethod]
        public void WhenBothCurrentAndDestinationAreDifferentAndWithinRange_Swap()
        {
            //Arrange
            var source = Fixture.CreateMany<Dummy>().ToReadOnlyList();
            var current = 2;
            var destination = 0;

            //Act
            var result = source.WithSwapped(current, destination);

            //Assert
            result.Should().ContainInOrder(new List<Dummy>
            {
                source[2],
                source[1],
                source[0]
            });
        }
    }

    [TestClass]
    public class Serialization : Tester
    {
        [TestMethod]
        public void WhenUsingSystemText_DeserializeReadOnlyList()
        {
            //Arrange
            var instance = new DummyWithListInside
            {
                Id = Fixture.Create<Guid>(),
                Name = Fixture.Create<string>(),
                Stuff = Fixture.CreateMany<Dummy>().ToReadOnlyList()
            };

            var json = System.Text.Json.JsonSerializer.Serialize(instance);

            //Act
            var result = System.Text.Json.JsonSerializer.Deserialize<DummyWithListInside>(json);

            //Assert
            result.Should().BeEquivalentTo(instance);
        }
    }
}