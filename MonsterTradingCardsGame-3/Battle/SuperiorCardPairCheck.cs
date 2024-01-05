using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Battle
{
    internal class SuperiorCardPairCheck
    {
        public int IsSuperiorCardPair(Card cardUser1, Card cardUser2, List<SuperiorCardPair> cardSuperiors)
        {
            //Geht nicht - wieso?
            int i = 0;
            foreach(SuperiorCardPair cardPair in cardSuperiors)
            {
                Console.WriteLine("\ni: " + i);
                string cardUser1CT = cardUser1.CardType.ToString();
                string cardUser2CT = cardUser2.CardType.ToString();
                string cardPairCT = cardPair.CardType.ToString();
                string cardPairSE = cardPair.SuperiorElement.ToString();
                string cardPairET = cardPair.ElementType;
                Console.WriteLine(cardUser1CT + ", " + cardPairCT);
                Console.WriteLine(cardUser2CT + ", " + cardPairSE);
                if (cardUser1CT == cardPairCT && cardUser2CT == cardPairSE)
                {
                    Console.WriteLine("if");
                    Console.WriteLine(":" + cardPairET + ":");
                    Console.WriteLine(cardUser2.ElementType.ToString() + ", " + cardPairET);

                    if(cardPairET != "" && cardUser2.ElementType.ToString() == cardPairET)
                    {
                        return 2;
                    }
                    else if(cardPairET == "")
                    {
                        return 2;
                    }
                }
                else if (cardUser2CT == cardPairCT && cardUser1CT == cardPairSE)
                {
                    Console.WriteLine("else if");
                    Console.WriteLine(":" + cardPairET + ":");
                    Console.WriteLine(cardUser2.ElementType.ToString() + ", " + cardPairET);

                    if (cardPairET != "" && cardUser1.ElementType.ToString() == cardPairET)
                    {
                        return 1;
                    }
                    else if (cardPairET == "")
                    {
                        return 1;
                    }
                }
                i++;
            }
            return 0;
        }
    }
}
