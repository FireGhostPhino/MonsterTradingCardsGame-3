using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Net.Sockets;
using System.Net;
using MonsterTradingCardsGame_3.Server;
using System.Threading;
using System.Data;
using Npgsql;
using System.Xml.Linq;

//Opera übernimmt Verbindung und kein anderer kann dann mehr Anfragen schicken! Wieso?

namespace MonsterTradingCardsGame_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Programm MTCG-V3");

            Server.Control server = new();
            server.ServerThreads();

            //Console.WriteLine("End Programm MTCG-V3");
        }
    }
}