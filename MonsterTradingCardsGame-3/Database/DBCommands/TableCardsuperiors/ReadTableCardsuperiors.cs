using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableCardsuperiors
{
    internal class ReadTableCardsuperiors
    {
        public static List<SuperiorCardPair> GetAllCardsuperiors()
        {
            List<SuperiorCardPair> cardSuperiors = new();

            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            command.CommandText = "SELECT * FROM cardsuperiors ORDER BY id ASC";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string elementType = "";
                try
                {
                    if (reader.GetString(3) != null)
                    {
                        elementType = reader.GetString(3);
                    }
                }
                catch (Exception e)
                {
                    elementType = "";
                }

                cardSuperiors.Add(new SuperiorCardPair()
                {
                    Id = reader.GetInt32(0),
                    CardType = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(1)),
                    SuperiorElement = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(2)),
                    ElementType = elementType,
                    DamageType = reader.GetString(4)
                });
            }

            connection.Close();
            return cardSuperiors;
        }
    }
}
