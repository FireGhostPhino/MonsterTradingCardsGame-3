using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class HTTP_Response
    {
        private Users.User? _userData = null;
        public List<User> allUserData = new();
        public List<string>? scoreboard = null;
        public List<Message> allMessages = new();
        public List<Card> cards = new();
        public bool plainOutput = false;

        public Users.User UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }


        public void CreateOKResponse(StreamWriter writer)
        {
            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html><body><h1>Request ok</h1><a>" + UserData + "</a></b>");
            if(allUserData != null)
            {
                foreach(var user in allUserData)
                {
                    writer.WriteLine("<p>" + user.ToString() + "</p>");
                }
            }
            if (scoreboard != null && scoreboard.Count > 1)
            {
                int i = 0;
                writer.WriteLine("Platz: Username: Elo");
                foreach (var line in scoreboard)
                {
                    writer.WriteLine("<p>Platz " + (i+1) + ": " + line + "</p>");
                    i++;
                }
            }
            else if(scoreboard != null && scoreboard.Count == 1)
            {
                writer.WriteLine("Username: Elo");
                writer.WriteLine("<p>" + scoreboard[0] + "</p>");
            }
            if(allMessages != null)
            {
                foreach (var message in allMessages)
                {
                    writer.WriteLine("<p>" + message.ToString() + "</p>");
                }
            }
            if(cards != null && plainOutput == true)
            {
                foreach (var card in cards)
                {
                    writer.WriteLine("<p>" + card.ToString() + "</p>");
                }
            }
            else if(cards != null && plainOutput == false)
            {
                int i = 1;
                foreach (var card in cards)
                {
                    if(i % 2 == 0)
                    {
                        writer.WriteLine("<p><b>" + card.ToString() + "</b></p>");
                    }
                    else
                    {
                        writer.WriteLine("<p>" + card.ToString() + "</p>");
                    }
                    i++;
                }
            }
            writer.WriteLine("</body></html>");
        }


        public void CreateERRORResponse(StreamWriter writer, string errorCode)
        {
            writer.WriteLine("HTTP/1.1 400 Bad Request");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html><body><h1>Error in request occured!</h1><a>Error: " + errorCode + "</a></body></html>");
        }
    }
}
