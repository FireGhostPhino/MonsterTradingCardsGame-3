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
                Console.WriteLine("2");

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
                        Coins = reader.GetInt32(4)
                    });
                }
            }
        }

        private void PostRequest(string bodyInformation, IDbCommand command)
        {
            User user;
            try
            {
                user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            Database.DBCommands.DBPathUsers.CommandUserExist(command, user.Username);
            
            using IDataReader reader = command.ExecuteReader();

            Console.WriteLine(user.Username);
            if (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
                Console.WriteLine("Username vorhanden");
                throw new ArgumentException("4");
            }
            else
            {
                Console.WriteLine("neuer Username");
            }

            command.Connection.Close();
            command = Database.DBConnection.ConnectionCreate();

            Database.DBCommands.DBPathUsers.CommandUserInsert(command, user);

            command.ExecuteNonQuery();
        }

        private void PutRequest(string[] pathSplitted, string bodyInformation, IDbCommand command, string[] headerInfos)
        {
            var user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");
            Console.WriteLine(pathSplitted[1]);
            user.Username = pathSplitted[1];

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

            //Console.WriteLine("token got: " + headerInfos[1] + ", own: " + (StandardValues.tokenPre + user.Username + StandardValues.tokenPost));

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
