using MonsterTradingCardsGame_3.GeneralHelpFunctions;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUsers
{
    internal class ReadTableUsers
    {
        public static void GetSingleUserData(HTTP_Response response, string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

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
                throw new ArgumentException("404 (No user with this username)");
            }

            connection.Close();
        }

        public static void GetAllUserData(HTTP_Response response)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

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

            connection.Close();
        }

        public static bool UsernameExist(string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

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

        public static string GetPassword(string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT password FROM users WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetString(0);
            }
            else
            {
                throw new InvalidDataException("404 (No user with this username)");
            }
        }

        public static int GetCoins(string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT coins FROM users WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            else
            {
                throw new InvalidDataException("19 (No user with this username)");
            }
        }

        public static void GetUserStats(HTTP_Response response, string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT username, elo, wins, loses FROM users WHERE username=@username";
            using IDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                throw new InvalidDataException("4 (No user with this username)");
            }

            int winloseR = 0;
            int wins = reader.GetInt32(2);
            int loses = reader.GetInt32(3);
            winloseR = WinLoseRatio.WinLoseRatioCalc(wins, loses);
            response.scoreboard = new List<string>
            {
                reader.GetString(0) + ": " + reader.GetInt32(1) + " - " + wins + "/" + loses + " - " + winloseR
            };

            connection.Close();
        }

        public static void UsernamePasswordCheck(HTTP_Response response, string username, string password)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "password", DbType.String, password);
            command.CommandText = "SELECT id FROM users WHERE username=@username AND password=@password";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                response.returnMessage = StandardValues.tokenPre + username + StandardValues.tokenPost;
            }
            else
            {
                throw new ArgumentException("401 (Invalid username/password provided)");
            }
        }

        public static void GetScoreboardData(HTTP_Response response)
        {
            response.scoreboard = new List<string>();

            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            command.CommandText = "SELECT username,elo, wins, loses FROM users ORDER BY elo DESC";
            using IDataReader reader = command.ExecuteReader();

            int winloseR = 0;
            int wins = 0;
            int loses = 0;
            while (reader.Read())
            {
                wins = reader.GetInt32(2);
                loses = reader.GetInt32(3);
                winloseR = WinLoseRatio.WinLoseRatioCalc(wins, loses);
                response.scoreboard.Add(reader.GetString(0) + ": " + reader.GetInt32(1) + " - " + wins + "/" + loses + " - " + winloseR);
            }

            connection.Close();
        }
    }
}
