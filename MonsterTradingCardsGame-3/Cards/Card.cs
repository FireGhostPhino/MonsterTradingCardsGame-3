using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    internal class Card
    {
        private string _cardtype;
        private string _elementType;
        private int _damage;

        public string CardType
        {
            get { return _cardtype; }
            set { _cardtype = value; }
        }

        public string ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }




        public void TestParentClass()
        {
            Console.WriteLine("Parent Class Function call");
        }

        public virtual void TestMethod()
        {
            Console.WriteLine("Print Class-Parent Card");
        }
    }
}
