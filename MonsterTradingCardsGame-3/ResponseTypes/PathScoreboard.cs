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
                throw new InvalidDataException("2");
            }

            string requestType = headerInfos[2];
            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(response);
            }
            else
            {
                throw new InvalidDataException("2");
            }
        }

        private void GetRequest(HTTP_Response response)
        {
            //using IDbCommand command = Database.DBConnection.ConnectionCreate();

            /*command.CommandText = "SELECT username,elo, wins, loses FROM users ORDER BY elo DESC";
            using IDataReader reader = command.ExecuteReader();
            response.scoreboard = new List<string>();*/

            ReadTableUsers.GetScoreboardData(response);

            /*int winloseR = 0;
            int wins = 0;
            int loses = 0;
            while (reader.Read())
            {
                wins = reader.GetInt32(2);
                loses = reader.GetInt32(3);
                winloseR = WinLoseRatio.WinLoseRatioCalc(wins, loses);
                response.scoreboard.Add(reader.GetString(0) + ": " + reader.GetInt32(1) + " - " + wins + "/" + loses + " - " + winloseR);
            }*/
        }
    }
}
