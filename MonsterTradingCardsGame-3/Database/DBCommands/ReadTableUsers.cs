using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands
{
    internal class ReadTableUsers
    {
        public static void GetSingleUserData(HTTP_Response response, string username)
        {
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT * FROM users WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                User user = new()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Elo = reader.GetInt32(3),
                    Coins = reader.GetInt32(4),
                    Wins = reader.GetInt32(5),
                    Loses = reader.GetInt32(6),
                };

                response.UserData = user;
            }
            else
            {
                throw new ArgumentException("6");
            }
        }

        public static void GetAllUserData(HTTP_Response response)
        {
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            command.CommandText = "SELECT * FROM users ORDER BY id ASC";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.allUserData.Add(new User()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Elo = reader.GetInt32(3),
                    Coins = reader.GetInt32(4),
                    Wins = reader.GetInt32(5),
                    Loses = reader.GetInt32(6),
                });
            }
        }

        public static bool UsernameExist(string username)
        {
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT username FROM users WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
