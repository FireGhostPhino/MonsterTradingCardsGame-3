using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsers;
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
                throw new InvalidDataException("2 (Token Error)");
            }

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response);
            }
            else if(requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(bodyInformation);
            }
            else if(requestType == Enums.RequestTypes.PUT.ToString())
            {
                PutRequest(pathSplitted, bodyInformation, headerInfos);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response)
        {
            if ((pathSplitted.Length > 1) &&
                (headerInfos[1] == (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))
            {
                string username = pathSplitted[1];

                ReadTableUsers.GetSingleUserData(response, username);
            }
            else if((headerInfos[1] == (StandardValues.tokenPre + "admin" + StandardValues.tokenPost)) ||
                    (headerInfos[1] == (StandardValues.tokenPre + "ADMIN" + StandardValues.tokenPost)))
            {
                Console.WriteLine("Admin Data request");

                ReadTableUsers.GetAllUserData(response);
            }
        }

        private void PostRequest(string bodyInformation)
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
                throw new InvalidDataException("11 (Body reading error)");
            }

            if(ReadTableUsers.UsernameExist(user.Username) == true)
            {
                throw new ArgumentException("409 (Username already existing)");
            }

            WriteTableUsers.InsertUser(user);
        }

        private void PutRequest(string[] pathSplitted, string bodyInformation, string[] headerInfos)
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
                throw new InvalidDataException("11 (Body reading error)");
            }

            if (username != user.Username)
            {
                throw new InvalidDataException("22 (mismatching data)");
            }

            string password = ReadTableUsers.GetPassword(user.Username);

            if ((password == user.Password) && (headerInfos[1] == (StandardValues.tokenPre + user.Username + StandardValues.tokenPost)))
            {
                WriteTableUsers.UpdatePassword(user.Username, user.NewPassword);
            }
            else
            {
                throw new InvalidDataException("6 (mismatching data)");
            }
        }
    }
}
