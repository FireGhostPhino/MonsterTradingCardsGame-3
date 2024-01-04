using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsers;
using MonsterTradingCardsGame_3.GeneralHelpFunctions;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathStats
    {
        public PathStats(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            if (headerInfos[1] == "")
            {
                throw new InvalidDataException("2 (Token Error)");
            }

            string requestType = headerInfos[2];
            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(headerInfos, response);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void GetRequest(string[] headerInfos, HTTP_Response response)
        {
            string[] tokenparts;
            string username;
            try
            {
                tokenparts = headerInfos[1].Split(' ');
                username = (tokenparts[1].Split('-'))[0];
            }
            catch (Exception e)
            {
                throw new InvalidDataException("17 (Error at Token)");
            }

            ReadTableUsers.GetUserStats(response, username);
        }
    }
}
