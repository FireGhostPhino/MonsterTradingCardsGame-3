using MonsterTradingCardsGame_3.Database.DBCommands.TableUsers;
using MonsterTradingCardsGame_3.GeneralHelpFunctions;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathScoreboard
    {
        public PathScoreboard(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            if (headerInfos[1] == "")
            {
                throw new InvalidDataException("2 (Token Error)");
            }

            string requestType = headerInfos[2];
            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(response);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void GetRequest(HTTP_Response response)
        {
            ReadTableUsers.GetScoreboardData(response);
        }
    }
}
