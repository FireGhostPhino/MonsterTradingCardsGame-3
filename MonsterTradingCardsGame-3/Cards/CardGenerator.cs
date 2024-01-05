using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    public class CardGenerator
    {
        public int RandomNumberGenerator(int minimum, int maximum)
        {
            Random random = new();
            return random.Next(minimum, maximum);
        }

        public List<Card> GeneratePackage(List<Card> package)
        {
            for (int i = 0; i < StandardValues.packageSize; i++)
            {
                Card card = GenerateCard();
                package.Add(card);
            }
            return package;
        }

        public Card GenerateCard()
        {
            Card card = new();
            /*int cardType = RandomNumberGenerator(0, Enum.GetNames(typeof(Enums.CardTypes)).Length);
            card.CardType = (Enums.CardTypes)cardType;*/

            int cardType = RandomNumberGenerator(0, (Enum.GetNames(typeof(Enums.CardTypes)).Length) +7);
            if(cardType >= Enum.GetNames(typeof(Enums.CardTypes)).Length)
            {
                cardType = (int)Enums.CardTypes.Spell;
            }
            card.CardType = (Enums.CardTypes)cardType;

            if (card.CardType == Enums.CardTypes.Spell)
            {
                card.CardCategorie = Enums.CardCategories.SpellCard;
            }
            else
            {
                card.CardCategorie = Enums.CardCategories.MonsterCard;
            }

            bool isNonElementCard = false;
            for(int i = 0; i < Enum.GetNames(typeof(Enums.NonElementCardTypes)).Length; i++)
            {
                if(card.CardType.ToString() == Enum.GetNames(typeof(Enums.NonElementCardTypes))[i])
                {
                    isNonElementCard = true;
                    break;
                }
            }
            if(isNonElementCard)
            {
                card.ElementType = Enums.Elements.Regular;
            }
            else
            {
                int element = RandomNumberGenerator(0, Enum.GetNames(typeof(Enums.Elements)).Length);
                card.ElementType = (Enums.Elements)element;
            }

            int damage = RandomNumberGenerator(StandardValues.minDamage, StandardValues.maxDamage);
            card.Damage = damage;
            return card;
        }

        /*public void PrintPackage(List<Card> package)
        {
            for (int i = 0; i < package.Count; i++)
            {
                Console.WriteLine($"\nCard {i + 1}: ");
                Console.WriteLine(package[i].CardCategorie);
                Console.WriteLine(package[i].CardType);
                Console.WriteLine(package[i].ElementType);
                Console.WriteLine(package[i].Damage);
            }
        }*/
    }
}
