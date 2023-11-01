using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestCardGenerator
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void testGenerateCardDamage()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new MonsterTradingCardsGame_3.Cards.CardGenerator();

            //Act
            MonsterTradingCardsGame_3.Cards.Card card = cardGenerator.GenerateCard();

            //Assert
            Assert.That(card.Damage, Is.GreaterThan(MonsterTradingCardsGame_3.StandardValues.minDamage));
        }

        [Test]
        public void testGenerateCardCategorie()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new MonsterTradingCardsGame_3.Cards.CardGenerator();

            //Act
            MonsterTradingCardsGame_3.Cards.Card card = cardGenerator.GenerateCard();

            //Assert
            Assert.That(card.CardCategorie, Is.EqualTo(MonsterTradingCardsGame_3.Enums.CardCategories.MonsterCard).Or.EqualTo(MonsterTradingCardsGame_3.Enums.CardCategories.SpellCard));
        }

        [Test]
        public void testRandomNumberGenerator()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new MonsterTradingCardsGame_3.Cards.CardGenerator();
            int minimum = 5;
            int maximum = 10;

            //Act
            int random = cardGenerator.RandomNumberGenerator(minimum, maximum);

            //Assert
            Assert.That(random, Is.GreaterThanOrEqualTo(minimum).And.LessThanOrEqualTo(maximum));
        }
    }
}
