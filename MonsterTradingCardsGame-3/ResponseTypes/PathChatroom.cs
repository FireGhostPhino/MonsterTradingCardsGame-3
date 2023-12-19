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
    internal class PathChatroom
    {
        public PathChatroom(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType != Enums.RequestTypes.POST.ToString() &&
                (headerInfos[1] == "" || ((pathSplitted.Length > 1) &&
                (headerInfos[1] != (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))))
            {
                throw new InvalidDataException("2");
            }

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else if (requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else
            {
                throw new InvalidDataException("2");
            }
        }

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            command.CommandText = "SELECT * FROM messages ORDER BY id ASC";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.allMessages.Add(new Message()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    MessageText = reader.GetString(2),
                    MessageTime = reader.GetDateTime(3)
                });
            }
        }

        private void PostRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            Message message;
            try
            {
                message = JsonSerializer.Deserialize<Message>(bodyInformation ?? "");
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            if (message.Username == "" || message.MessageText == "")
            {
                throw new InvalidDataException("10");
            }

            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            DateTime dateTime = DateTime.Now;

            command.CommandText = "INSERT INTO messages (username, message, messagetime) " +
            "VALUES (@username, @message, @messagetime)";
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, message.Username);
            DBCreateParameter.AddParameterWithValue(command, "message", DbType.String, message.MessageText);
            DBCreateParameter.AddParameterWithValue(command, "messagetime", DbType.DateTime2, dateTime);

            command.ExecuteNonQuery();
        }
    }
}