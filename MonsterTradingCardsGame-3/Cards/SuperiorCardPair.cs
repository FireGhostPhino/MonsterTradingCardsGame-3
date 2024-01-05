using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    internal class SuperiorCardPair
    {
        public SuperiorCardPair() 
        {
            ElementType = "";
        }

        private int _id;
        private Enums.CardTypes _cardtype;
        private Enums.CardTypes _superiorElement;
        private string _elementType = "";
        private string _damagetype;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Enums.CardTypes CardType
        {
            get { return _cardtype; }
            set { _cardtype = value; }
        }

        public Enums.CardTypes SuperiorElement
        {
            get { return _superiorElement; }
            set { _superiorElement = value; }
        }

        public string ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        public string DamageType
        {
            get { return _damagetype; }
            set { _damagetype = value; }
        }

        public override string ToString()
        {
            return $"Id: {Id}, CardType: {CardType}, SuperiorElement: {SuperiorElement}, ElementType: {ElementType}, DamageType: {DamageType}";
        }
    }
}
