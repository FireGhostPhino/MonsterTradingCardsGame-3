using MonsterTradingCardsGame_3.Battle;
using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableElementtypes
{
    internal class ReadTableElementtypes
    {
        public static List<Element> GetAllElementtypes()
        {
            List<Element> cardElements = new();

            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            command.CommandText = "SELECT elementname, inferiorelement FROM elementtypes ORDER BY id ASC";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                cardElements.Add(new Element()
                {
                    ElementType = reader.GetString(0),
                    InferiorElementType = reader.GetString(1) 
                });
            }

            connection.Close();
            return cardElements;
        }
    }
}
