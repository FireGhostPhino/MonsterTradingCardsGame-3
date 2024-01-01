using MonsterTradingCardsGame_3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MonsterTradingCardsGame_3.Cards
{
    public class Deck
    {
        public Deck() { }

        private string _username = "";
        private int _cardid1 = -1;
        private int _cardid2 = -1;
        private int _cardid3 = -1;
        private int _cardid4 = -1;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public int CardId1
        {
            get { return _cardid1; }
            set { _cardid1 = value; }
        }

        public int CardId2
        {
            get { return _cardid2; }
            set { _cardid2 = value; }
        }

        public int CardId3
        {
            get { return _cardid3; }
            set { _cardid3 = value; }
        }

        public int CardId4
        {
            get { return _cardid4; }
            set { _cardid4 = value; }
        }

        public override string ToString()
        {
            return $"Username: {Username}, CardId1: {CardId1}, CardId2: {CardId2}, CardId3: {CardId3}, CardId4: {CardId4}";
        }
    }

}
