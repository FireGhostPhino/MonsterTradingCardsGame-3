using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Enums;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathSessions
    {
        public PathSessions(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            Console.WriteLine("Test PathSessions requestHandler");

            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(bodyInformation, response);
            }
            else
            {
                throw new InvalidDataException("2");
            }
        }

        private void PostRequest(string bodyInformation, HTTP_Response response)
        {
            User? user;
            try
            {
                user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");

                if (user == null)
                {
                    throw new InvalidDataException("11");
                }
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            IDbCommand command = Database.DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, user.Username);
            DBCreateParameter.AddParameterWithValue(command, "password", DbType.String, user.Password);
            command.CommandText = "SELECT id FROM users WHERE username=@username AND password=@password";

            IDataReader reader = command.ExecuteReader();

            Console.WriteLine(user.Username);
            if (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0));
                Console.WriteLine("Username vorhanden");

                response.returnMessage = StandardValues.tokenPre + user.Username + StandardValues.tokenPost;
            }
            else
            {
                Console.WriteLine("Username nicht vorhanden");
                throw new ArgumentException("21");
            }

            command.Connection.Close();
        }
    }
}
