using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.GeneralHelpFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonsterTradingCardsGame_3.Users
{
    public class User
    {
        public User()
        {
            SetStartValues();
        }

        private int _id;
        private string _username = "";
        private string _password = "";
        private string _newpassword = "";
        private int _coins;
        private int _elo;
        private int _wins;
        private int _loses;
        private string? _token = null;
        public List<Card>? cardStack = null;
        public List<Card>? cardDeck = null;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string NewPassword
        {
            get { return _newpassword; }
            set { _newpassword = value; }
        }

        public int Coins
        {
            get { return _coins; }
            set { _coins = value; }
        }

        public int Elo
        {
            get { return _elo; }
            set { _elo = value; }
        }

        public int Wins
        {
            get { return _wins; }
            set { _wins = value; }
        }

        public int Loses
        {
            get { return _loses; }
            set { _loses = value; }
        }

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        public void AddUser(string username, string password)
        {
            Username = username;
            Password = password;
            Coins = StandardValues.startCoins;
            Elo = StandardValues.startElo;
            Wins = StandardValues.startWinsLoses;
            Loses = StandardValues.startWinsLoses;
            cardStack = new List<Card>();
            cardDeck = new List<Card>();
        }

        public void SetStartValues()
        {
            Coins = StandardValues.startCoins;
            Elo = StandardValues.startElo;
            Wins = StandardValues.startWinsLoses;
            Loses = StandardValues.startWinsLoses;
            cardStack = new List<Card>();
            cardDeck = new List<Card>();
        }

        public override string ToString()
        {
            int winloseR = WinLoseRatio.WinLoseRatioCalc(Wins, Loses);
            return $"Id: {Id}, Username: {Username}, Password: {Password}, Coins: {Coins}, Elo: {Elo}, Wins: {Wins}, Loses {Loses}, Win/Lose ratio: {winloseR}";
        }
    }
}
