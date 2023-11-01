﻿using MonsterTradingCardsGame_3.Users;
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
        public List<User> allUserData = new List<User>();

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
            writer.WriteLine("</body></html>");
        }


        public void CreateERRORResponse(StreamWriter writer, int errorCode)
        {
            writer.WriteLine("HTTP/1.1 400 Bad Request");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html><body><h1>Error in request occured!</h1></body></html>");
        }
    }
}
