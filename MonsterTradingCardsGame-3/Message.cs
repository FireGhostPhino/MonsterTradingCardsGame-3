using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3
{
    public class Message
    {
        public Message() { }

        private int _id;
        private string _username = "";
        private string _messageText = "";
        private DateTime _messageTime;

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

        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        }

        public DateTime MessageTime
        {
            get { return _messageTime; }
            set { _messageTime = value; }
        }

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, MessageText: {MessageText}, MessageTime: {MessageTime}";
        }
    }
}
