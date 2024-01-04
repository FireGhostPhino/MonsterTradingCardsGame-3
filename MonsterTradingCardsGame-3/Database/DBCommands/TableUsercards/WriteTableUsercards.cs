using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards
{
    internal class WriteTableUsercards
    {
        public static void InsertCard(Card? card, string username)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "category", DbType.String, card.CardCategorie.ToString());
            DBCreateParameter.AddParameterWithValue(command, "cardtype", DbType.String, card.CardType.ToString());
            DBCreateParameter.AddParameterWithValue(command, "elementtype", DbType.String, card.ElementType.ToString());
            DBCreateParameter.AddParameterWithValue(command, "damage", DbType.Int32, card.Damage);
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "INSERT INTO usercards (category, cardtype, elementtype, damage, username) VALUES (@category, @cardtype, @elementtype, @damage, @username)";

            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void ChangeCardOwner(string username, int cardId)
        {
            //using IDbCommand command = DBConnection.ConnectionCreate();
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "usercardid", DbType.Int32, cardId);
            command.CommandText = "UPDATE usercards SET username=@username WHERE id=@usercardid";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
