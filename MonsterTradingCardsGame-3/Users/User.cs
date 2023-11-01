using MonsterTradingCardsGame_3.Cards;
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
        
        /*public User(string username, string password, AllUsers userList)
        {
            if(userList.IsNewUsername(username))
            {
                AddUser(username, password);
                //userList.AddUser(this);
            }
            else
            {
                Console.WriteLine("User mit diesem usernamen bereits vorhanden. Bitte anderen Usernamen wählen!");
            }
        }*/

        private int _id;
        private string _username = "";
        private string _password = "";
        private string _newpassword = "";
        private int _coins;
        private int _elo;
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
            cardStack = new List<Card>();
            cardDeck = new List<Card>();
        }

        public void SetStartValues()
        {
            Coins = StandardValues.startCoins;
            Elo = StandardValues.startElo;
            cardStack = new List<Card>();
            cardDeck = new List<Card>();
        }

        /*public bool ChangeUsername(string username, string password, string newUsername, AllUsers userList)
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
        }*/

        public void PayPackage()
        {
            Coins -= StandardValues.packageCost;
        }

        public void PrintUser()
        {
            Console.WriteLine("\nUser:");
            Console.WriteLine(Username);
            Console.WriteLine(Password);
            Console.WriteLine(Coins);
            Console.WriteLine(Elo);
        }

        public void PrintCardStack()
        {
            Console.WriteLine("\nAnzahl Karten im Stack: " + cardStack.Count);
            for (int i = 0; i < cardStack.Count; i++)
            {
                Console.WriteLine($"\nCard {i + 1}: ");
                Console.WriteLine(cardStack[i].CardCategorie);
                Console.WriteLine(cardStack[i].CardType);
                Console.WriteLine(cardStack[i].ElementType);
                Console.WriteLine(cardStack[i].Damage);
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Password: {Password}, Coins: {Coins}, Elo: {Elo}";
        }
    }
}
