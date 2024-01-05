using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Battle
{
    internal class CardElementPairCheck
    {
        public int IsSuperiorCardElement(Card cardUser1, Card cardUser2, List<Element> cardElements)
        {
            foreach (Element elementPair in cardElements)
            {
                string cardUser1ET = cardUser1.ElementType.ToString();
                string cardUser2ET = cardUser2.ElementType.ToString();
                if (cardUser1ET == elementPair.ElementType && cardUser2ET == elementPair.InferiorElementType)
                {
                    return 1;
                }
                else if (cardUser2ET == elementPair.ElementType && cardUser1ET == elementPair.InferiorElementType)
                {
                    return 2;
                }
            }
            return 0;
        }
    }
}
