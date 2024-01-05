using MonsterTradingCardsGame_3.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableMessages
{
    internal class ReadTableMessages
    {
        public static void GetAllMessages(HTTP_Response response)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

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

            connection.Close();
        }
    }
}
