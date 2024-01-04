using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestDeck
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructorDeck()
        {
            //Arrange
            //Act
            Deck deck = new();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(deck.CardId1, Is.EqualTo(-1));
                Assert.That(deck.Username, Is.EqualTo(""));
            });
        }

        [Test]
        public void TestConstructorDeckSetValues()
        {
            //Arrange
            int id = 5;
            string username = "test";

            //Act
            Deck deck = new Deck() { 
                CardId1 = id,
                Username = username,
            };

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(deck.CardId1, Is.EqualTo(id));
                Assert.That(deck.Username, Is.EqualTo(username));
            });
        }

        [Test]
        public void TestDeckSetValues()
        {
            //Arrange
            Deck deck = new();
            int id = 5;
            string username = "test";

            //Act
            deck.CardId1 = id;
            deck.Username = username;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(deck.CardId1, Is.EqualTo(id));
                Assert.That(deck.Username, Is.EqualTo(username));
            });
        }
    }
}
