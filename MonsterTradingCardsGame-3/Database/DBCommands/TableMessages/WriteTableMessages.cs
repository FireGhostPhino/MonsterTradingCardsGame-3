using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableMessages
{
    internal class WriteTableMessages
    {
        public static void SaveMessage(Message? message, DateTime dateTime)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            command.CommandText = "INSERT INTO messages (username, message, messagetime) " +
            "VALUES (@username, @message, @messagetime)";
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, message.Username);
            DBCreateParameter.AddParameterWithValue(command, "message", DbType.String, message.MessageText);
            DBCreateParameter.AddParameterWithValue(command, "messagetime", DbType.DateTime2, dateTime);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
