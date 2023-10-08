using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Users
{
    internal class User
    {
        private string _username;
        private string _password;
        private int _coins;
        private int _elo;
        public List<Card> cardStack;
        public List<Card> cardDeck;

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

        public bool AddUser(string username, string password, AllUsers userList)
        {
            if (userList.IsNewUsername(username))
            {
                Username = username;
                Password = password;
                userList.AddUser(this);
                Coins = StandardValues.startCoins;
                Elo = StandardValues.startElo;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeUsername(string username, string password, string newUsername, AllUsers userList)
        {
            if (userList.IsNewUsername(newUsername) && userList.IsCorrectPassword(username, password))
            {
                Username = newUsername;
                userList.ChangeUsername(username, newUsername);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
