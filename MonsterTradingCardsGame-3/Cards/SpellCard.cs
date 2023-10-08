using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    internal class SpellCard : Card
    {
        public SpellCard()
        {
            CardCategorie = Enums.CardCategories.SpellCard;
        }

        public override void TestMethod()
        {
            Console.WriteLine("Print Class-Child SpellCard");
        }
    }
}
