﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Cards
{
    public class Card
    {
        private Enums.CardCategories _cardcategorie;
        private Enums.CardTypes _cardtype;
        private Enums.Elements _elementType;
        private int _damage = 0;

        public Enums.CardCategories CardCategorie
        {
            get { return _cardcategorie; }
            set { _cardcategorie = value; }
        }

        public Enums.CardTypes CardType
        {
            get { return _cardtype; }
            set { _cardtype = value; }
        }

        public Enums.Elements ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
    }
}
