using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ToolBX.Collections.Deck;
using ToolBX.Eloquentest;

namespace Collections.Deck.Tests;

[TestClass]
public class DeckExtensionsTester
{
    [TestClass]
    public class ToDeck : Tester
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            string[] collection = null;

            //Act
            var action = () => collection.ToDeck();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCollectionIsNotNull_ReturnDeck()
        {
            //Arrange
            var collection = Fixture.CreateMany<string>().ToList();

            //Act
            var result = collection.ToDeck();

            //Assert
            result.Should().BeOfType<Deck<string>>();
            result.Should().BeEquivalentTo(collection);
        }
    }
}