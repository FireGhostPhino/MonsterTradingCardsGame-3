using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards
{
    internal class ReadTableUsercards
    {
        public static void GetUserdeckValues(HTTP_Response response, string username, List<int> usercardsids)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "id1", DbType.Int32, usercardsids[0]);
            DBCreateParameter.AddParameterWithValue(command, "id2", DbType.Int32, usercardsids[1]);
            DBCreateParameter.AddParameterWithValue(command, "id3", DbType.Int32, usercardsids[2]);
            DBCreateParameter.AddParameterWithValue(command, "id4", DbType.Int32, usercardsids[3]);
            command.CommandText = "SELECT id, category,cardtype,elementtype,damage FROM usercards WHERE (id=@id1 OR id=@id2 OR id=@id3 OR id=@id4) AND username=@username";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                    CardCategorie = (Enums.CardCategories)Enum.Parse(typeof(Enums.CardCategories), reader.GetString(1)),
                    CardType = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(2)),
                    ElementType = (Enums.Elements)Enum.Parse(typeof(Enums.Elements), reader.GetString(3)),
                    Damage = reader.GetInt32(4),
                });
            }

            connection.Close();
        }

        public static List<Card> ReturnUserdeckValues(string username, List<int> usercardsids)
        {
            List<Card> cards = new();

            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "id1", DbType.Int32, usercardsids[0]);
            DBCreateParameter.AddParameterWithValue(command, "id2", DbType.Int32, usercardsids[1]);
            DBCreateParameter.AddParameterWithValue(command, "id3", DbType.Int32, usercardsids[2]);
            DBCreateParameter.AddParameterWithValue(command, "id4", DbType.Int32, usercardsids[3]);
            command.CommandText = "SELECT id, category,cardtype,elementtype,damage FROM usercards WHERE (id=@id1 OR id=@id2 OR id=@id3 OR id=@id4) AND username=@username";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                    CardCategorie = (Enums.CardCategories)Enum.Parse(typeof(Enums.CardCategories), reader.GetString(1)),
                    CardType = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(2)),
                    ElementType = (Enums.Elements)Enum.Parse(typeof(Enums.Elements), reader.GetString(3)),
                    Damage = reader.GetInt32(4),
                });
            }

            connection.Close();
            return cards;
        }

        public static List<Card> GetUserOwnedCardIds(string username)
        {
            List<Card> cards = new();

            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT id FROM usercards WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                });
            }

            connection.Close();
            return cards;
        }

        public static void GetUserOwnedCards(HTTP_Response response, string username)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT id, category,cardtype,elementtype,damage FROM usercards WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                    CardCategorie = (Enums.CardCategories)Enum.Parse(typeof(Enums.CardCategories), reader.GetString(1)),
                    CardType = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(2)),
                    ElementType = (Enums.Elements)Enum.Parse(typeof(Enums.Elements), reader.GetString(3)),
                    Damage = reader.GetInt32(4),
                });
            }

            connection.Close();
        }
    }
}
