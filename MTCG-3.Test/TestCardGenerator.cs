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
        public void TestGenerateCardDamage()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new();

            //Act
            MonsterTradingCardsGame_3.Cards.Card card = cardGenerator.GenerateCard();

            //Assert
            Assert.That(card.Damage, Is.GreaterThan(MonsterTradingCardsGame_3.StandardValues.minDamage));
            /*Console.WriteLine(card.CardCategorie);
            Console.WriteLine(card.CardType);
            Console.WriteLine(card.ElementType);
            Console.WriteLine(card.Damage);*/
        }

        [Test]
        public void TestGenerateCardCategorie()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new();

            //Act
            MonsterTradingCardsGame_3.Cards.Card card = cardGenerator.GenerateCard();

            //Assert
            Assert.That(card.CardCategorie, Is.EqualTo(MonsterTradingCardsGame_3.Enums.CardCategories.MonsterCard).Or.EqualTo(MonsterTradingCardsGame_3.Enums.CardCategories.SpellCard));
        }

        [Test]
        public void TestRandomNumberGenerator()
        {
            //Arrange
            MonsterTradingCardsGame_3.Cards.CardGenerator cardGenerator = new();
            int minimum = 5;
            int maximum = 10;

            //Act
            int random = cardGenerator.RandomNumberGenerator(minimum, maximum);

            //Assert
            Assert.That(random, Is.GreaterThanOrEqualTo(minimum).And.LessThanOrEqualTo(maximum));
        }
    }
}
