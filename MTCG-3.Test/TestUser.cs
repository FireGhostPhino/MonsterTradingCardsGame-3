using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestUser
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestUserConstructorCardStack()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new();

            //Assert
            Assert.That(user.cardStack, Is.Not.EqualTo(null));
        }

        [Test]
        public void TestUserConstructorCardDeck()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new();

            //Assert
            Assert.That(user.cardDeck, Is.Not.EqualTo(null));
        }

        [Test]
        public void TestUserConstructorCoins()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new();

            //Assert
            Assert.That(user.Coins, Is.EqualTo(MonsterTradingCardsGame_3.StandardValues.startCoins));
        }
    }
}
