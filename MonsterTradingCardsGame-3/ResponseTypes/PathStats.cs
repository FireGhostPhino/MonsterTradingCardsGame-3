using MonsterTradingCardsGame_3.Database;
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
    internal class PathStats
    {
        public PathStats(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            Console.WriteLine("Test PathStats requestHandler");

            if (headerInfos[1] == "")
            {
                throw new InvalidDataException("2");
            }

            string requestType = headerInfos[2];
            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(headerInfos, response);
            }
            else
            {
                throw new InvalidDataException("2");
            }
        }

        private void GetRequest(string[] headerInfos, HTTP_Response response)
        {
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            string[] parts = headerInfos[1].Split(' ');
            string username = (parts[1].Split('-'))[0];
            Console.WriteLine(username);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT username, elo FROM users WHERE username=@username";
            using IDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                throw new InvalidDataException("4");
            }

            response.scoreboard = new List<string>
            {
                reader.GetString(0) + ": " + reader.GetInt32(1)
            };
            Console.WriteLine(response.scoreboard);
        }
    }
}
