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
            using IDbCommand command = DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "category", DbType.String, card.CardCategorie.ToString());
            DBCreateParameter.AddParameterWithValue(command, "cardtype", DbType.String, card.CardType.ToString());
            DBCreateParameter.AddParameterWithValue(command, "elementtype", DbType.String, card.ElementType.ToString());
            DBCreateParameter.AddParameterWithValue(command, "damage", DbType.Int32, card.Damage);
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "INSERT INTO usercards (category, cardtype, elementtype, damage, username) VALUES (@category, @cardtype, @elementtype, @damage, @username)";

            command.ExecuteNonQuery();
        }
    }
}
