using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards;
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
    internal class PathCards
    {
        public PathCards(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            string[] tokenparts;
            string username = "";
            try
            {
                tokenparts = headerInfos[1].Split(' ');
                username = (tokenparts[1].Split('-'))[0];
            }
            catch (Exception e)
            {
                throw new InvalidDataException("17 (Error at Token)");
            }

            ReadTableUsercards.GetUserOwnedCards(response, username);

            /*IDbCommand command = Database.DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT id, category,cardtype,elementtype,damage FROM usercards WHERE username=@username";

            IDataReader reader = command.ExecuteReader();

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
            }*/
        }
    }
}
