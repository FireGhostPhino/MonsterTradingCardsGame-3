using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    internal class CardGenerator
    {
        public int RandomNumberGenerator(int minimum, int maximum)
        {
            Random random = new Random();
            return random.Next(minimum, maximum);
        }

        public void GeneratePackage()
        {
            for(int i = 0; i < StandardValues.packageSize; i++)
            {
                Card card = GenerateCard();
                Console.WriteLine($"\nCard {i}: ");
                Console.WriteLine(card.ElementType);
                Console.WriteLine(card.CardType);

            }
        }

        public Card GenerateCard()
        {
            Card card = new Card();
            int element = RandomNumberGenerator(0, Enum.GetNames(typeof(Enums.Elements)).Length);
            card.ElementType = (Enums.Elements)element;
            int cardType = RandomNumberGenerator(0, Enum.GetNames(typeof(Enums.CardTypes)).Length);
            card.CardType = (Enums.CardTypes)cardType;
            return card;
        }
    }
}
