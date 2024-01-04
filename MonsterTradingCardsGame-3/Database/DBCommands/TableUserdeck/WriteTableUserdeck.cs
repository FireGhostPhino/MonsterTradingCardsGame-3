using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUserdeck
{
    internal class WriteTableUserdeck
    {
        public static void DeleteUserDeck(string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "DELETE FROM userdeck WHERE username=@username";

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void AddUserdeckCard(string username, int cardId)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, cardId);
            command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void ChangeCardOwner(string username, int cardId)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);


            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, cardId);
            command.CommandText = "UPDATE userdeck SET username=@username WHERE usercardsid=@usercardsid";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
