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
            Console.WriteLine("Test PathUsers requestHandler");

            if (headerInfos[1] == "" || ((pathSplitted.Length > 1) &&
                (headerInfos[1] != (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost))))
            {
                throw new InvalidDataException("2");
            }

            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            string requestType = headerInfos[2];
            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, bodyInformation, command, headerInfos, response);
            }
            else if(requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(pathSplitted, bodyInformation, command, headerInfos);
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

        private void GetRequest(string[] pathSplitted, string bodyInformation, IDbCommand command, string[] headerInfos, HTTP_Response response)
        {
            if ((pathSplitted.Length > 1) &&
                (headerInfos[1] == (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))
            {
                string username = pathSplitted[1];

                AddParameterWithValue(command, "username", DbType.String, username);
                command.CommandText = "SELECT * FROM users WHERE username=@username";

                using IDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User()
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
                        Coins = reader.GetInt32(4)
                    });
                }
            }
        }

        private void PostRequest(string[] pathSplitted, string bodyInformation, IDbCommand command, string[] headerInfos)
        {
            var user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");

            AddParameterWithValue(command, "username", DbType.String, user.Username);
            command.CommandText = "SELECT username FROM users WHERE username='@username'";
            
            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("Username vorhanden");
                throw new DuplicateNameException("4");
            }
            else
            {
                Console.WriteLine("neuer Username");
            }

            command.Connection.Close();
            command = Database.DBConnection.ConnectionCreate();

            command.CommandText = "INSERT INTO users (username, password, elo, coins) " +
            "VALUES (@username, @password, @elo, @coins)";
            AddParameterWithValue(command, "username", DbType.String, user.Username);
            AddParameterWithValue(command, "password", DbType.String, user.Password);
            AddParameterWithValue(command, "elo", DbType.Int32, user.Elo);
            AddParameterWithValue(command, "coins", DbType.Int32, user.Coins);
            command.ExecuteNonQuery();
        }

        private void PutRequest(string[] pathSplitted, string bodyInformation, IDbCommand command, string[] headerInfos)
        {
            Console.WriteLine("1_");
            var user = JsonSerializer.Deserialize<User>(bodyInformation ?? "");
            Console.WriteLine(pathSplitted[1]);
            user.Username = pathSplitted[1];
            Console.WriteLine("2_");

            AddParameterWithValue(command, "username", DbType.String, user.Username);
            command.CommandText = "SELECT password FROM users WHERE username=@username";
            using IDataReader reader = command.ExecuteReader();
            Console.WriteLine("3_");
            if (reader.Read())
            {
                Console.WriteLine("4_");
            }
            else
            {
                Console.WriteLine("5_");
                throw new InvalidDataException("4");
            }
            Console.WriteLine("6_");
            string password = reader.GetString(0);
            command.Connection.Close();
            command = Database.DBConnection.ConnectionCreate();

            //Console.WriteLine("token got: " + headerInfos[1] + ", own: " + (StandardValues.tokenPre + user.Username + StandardValues.tokenPost));
            Console.WriteLine("7_");
            if ((password == user.Password) && (headerInfos[1] == (StandardValues.tokenPre + user.Username + StandardValues.tokenPost)))
            {
                Console.WriteLine("8_");
                AddParameterWithValue(command, "username", DbType.String, user.Username);
                AddParameterWithValue(command, "password", DbType.String, user.NewPassword);
                command.CommandText = "UPDATE users SET password=@password WHERE username=@username";
                command.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine("9_");
                throw new InvalidDataException("6");
            }
        }

        private static void AddParameterWithValue(IDbCommand command, string parameterName, DbType dbType, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
