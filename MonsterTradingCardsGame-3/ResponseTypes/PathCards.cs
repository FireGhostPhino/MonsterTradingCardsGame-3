using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathCards
    {
        public PathCards(string[] headerInfos, string[] pathSplitted, string bodyInformation)
        {
            Console.WriteLine("Test PathCards requestHandler");
        }
    }
}
