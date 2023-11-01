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
        public void testUserConstructorCardStack()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new MonsterTradingCardsGame_3.Users.User();

            //Assert
            Assert.That(user.cardStack, Is.Not.EqualTo(null));
        }

        [Test]
        public void testUserConstructorCardDeck()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new MonsterTradingCardsGame_3.Users.User();

            //Assert
            Assert.That(user.cardDeck, Is.Not.EqualTo(null));
        }

        [Test]
        public void testUserConstructorCoins()
        {
            //Act
            MonsterTradingCardsGame_3.Users.User user = new MonsterTradingCardsGame_3.Users.User();

            //Assert
            Assert.That(user.Coins, Is.EqualTo(MonsterTradingCardsGame_3.StandardValues.startCoins));
        }

        [Test]
        public void testPayPackage()
        {
            //Arrange
            string testUsern = "testuser";
            string testPassw = "testpassw";
            MonsterTradingCardsGame_3.Users.User user = new MonsterTradingCardsGame_3.Users.User()
            {
                Username = testUsern,
                Password = testPassw,
            };

            //Act
            user.PayPackage();

            //Assert
            Assert.That(user.Coins, Is.EqualTo(MonsterTradingCardsGame_3.StandardValues.startCoins - MonsterTradingCardsGame_3.StandardValues.packageCost));
        }
    }
}
