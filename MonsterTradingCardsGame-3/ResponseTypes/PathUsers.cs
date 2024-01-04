using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathUsers
    {
        public PathUsers(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType != Enums.RequestTypes.POST.ToString() &&
                (headerInfos[1] == "" || ((pathSplitted.Length > 1) &&
                (headerInfos[1] != (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))))
            {
                throw new InvalidDataException("2");
            }

            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, command, headerInfos, response);
            }
            else if(requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(bodyInformation, command);
            }
            else if(requestType == Enums.RequestTypes.PUT.ToString())
            {
                PutRequest(pathSplitted, bodyInformation, command, headerInfos);
            }
            else
            {
                throw new InvalidDataException("2");
            }
        }

        private void GetRequest(string[] pathSplitted, IDbCommand command, string[] headerInfos, HTTP_Response response)
        {
            if ((pathSplitted.Length > 1) &&
                (headerInfos[1] == (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))
            {
                string username = pathSplitted[1];

                Database.DBCommands.DBPathUsers.CommandSingleUserData(command, username);

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
            else if((headerInfos[1] == (StandardValues.tokenPre + "admin" + StandardValues.tokenPost)) ||
                    (headerInfos[1] == (StandardValues.tokenPre + "ADMIN" + StandardValues.tokenPost)))
            {
                Console.WriteLine("Admin Data request");

                Database.DBCommands.DBPathUsers.CommandAllUserData(command);

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
        }

        private void PostRequest(string bodyInformation, IDbCommand command)
        {
            User? user;
            try
            {
                user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");

                if (user == null || user.Username == "" || user.Password == "")
                {
                    throw new InvalidDataException("11");
                }
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            Database.DBCommands.DBPathUsers.CommandUserExist(command, user.Username);
            
            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                throw new ArgumentException("4");
            }

            command.Connection.Close();
            command = Database.DBConnection.ConnectionCreate();

            Database.DBCommands.DBPathUsers.CommandUserInsert(command, user);

            command.ExecuteNonQuery();
        }

        private void PutRequest(string[] pathSplitted, string bodyInformation, IDbCommand command, string[] headerInfos)
        {
            User? user;
            string username;
            try
            {
                user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");
                username = user.Username;
                user.Username = pathSplitted[1];

                if (user == null || user.Username == "" || user.Password == "" || user.NewPassword == "")
                {
                    throw new InvalidDataException("11");
                }
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            if (username != user.Username)
            {
                throw new InvalidDataException("22");
            }

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, user.Username);
            command.CommandText = "SELECT password FROM users WHERE username=@username";
            using IDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                throw new InvalidDataException("4");
            }
            string password = reader.GetString(0);
            command.Connection.Close();
            command = Database.DBConnection.ConnectionCreate();

            if ((password == user.Password) && (headerInfos[1] == (StandardValues.tokenPre + user.Username + StandardValues.tokenPost)))
            {
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, user.Username);
                DBCreateParameter.AddParameterWithValue(command, "password", DbType.String, user.NewPassword);
                command.CommandText = "UPDATE users SET password=@password WHERE username=@username";
                command.ExecuteNonQuery();
            }
            else
            {
                throw new InvalidDataException("6");
            }
        }
    }
}
