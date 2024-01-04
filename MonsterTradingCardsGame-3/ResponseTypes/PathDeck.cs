using Microsoft.VisualBasic;
using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUserdeck;
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
using static System.Net.Mime.MediaTypeNames;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    public class PathDeck
    {
        public PathDeck(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response, string parameters)
        {
            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response, bodyInformation, parameters);
            }
            else if (requestType == Enums.RequestTypes.PUT.ToString())
            {
                PutRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else
            {
                throw new InvalidDataException("3");
            }
        }

        public static void DeckFormatPlain(HTTP_Response response, string parameters)
        {
            if (parameters != "")
            {
                var parameterInfo = parameters.Split('=');
                if (parameterInfo[0] == "format" && parameterInfo[1] == "plain")
                {
                    response.plainOutput = true;
                }
            }
        }

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation, string parameters)
        {
            DeckFormatPlain(response, parameters);

            List<int> usercardsids = new();


            //IDbCommand command = Database.DBConnection.ConnectionCreate();

            string[] tokenparts = headerInfos[1].Split(' ');
            string username = (tokenparts[1].Split('-'))[0];

            usercardsids = ReadTableUserdeck.GetUserdeck(username);

            /*DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT usercardsid FROM userdeck WHERE username=@username";

            IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                usercardsids.Add(reader.GetInt32(0));
            }*/

            //command.Connection.Close();

            if (usercardsids.Count == 4)
            {
                //command = Database.DBConnection.ConnectionCreate();

                ReadTableUsercards.GetUserdeckValues(response, username, usercardsids);

                /*DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
                DBCreateParameter.AddParameterWithValue(command, "id1", DbType.Int32, usercardsids[0]);
                DBCreateParameter.AddParameterWithValue(command, "id2", DbType.Int32, usercardsids[1]);
                DBCreateParameter.AddParameterWithValue(command, "id3", DbType.Int32, usercardsids[2]);
                DBCreateParameter.AddParameterWithValue(command, "id4", DbType.Int32, usercardsids[3]);
                command.CommandText = "SELECT id, category,cardtype,elementtype,damage FROM usercards WHERE (id=@id1 OR id=@id2 OR id=@id3 OR id=@id4) AND username=@username";

                reader = command.ExecuteReader();

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

        private void PutRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            Deck deck = new();
            try
            {
                var body = bodyInformation.Split('[');
                body = body[1].Split(']');
                body = body[0].Split(',');
                List<string> ids = new();
                foreach(var line in body)
                {
                    var id = line.Trim();
                    ids.Add(id.Split('"')[1]);
                }

                deck.CardId1 = Int32.Parse(ids[0]);
                deck.CardId2 = Int32.Parse(ids[1]);
                deck.CardId3 = Int32.Parse(ids[2]);
                deck.CardId4 = Int32.Parse(ids[3]);

                if (deck == null)
                {
                    throw new InvalidDataException("11");
                }

                string[] tokenparts = headerInfos[1].Split(' ');
                deck.Username = (tokenparts[1].Split('-'))[0];
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11 (Body reading error)");
            }

            if (deck.Username == "")
            {
                throw new InvalidDataException("10 (Error at Token)");
            }

            if(deck.CardId1 == -1 || deck.CardId2 == -1 || deck.CardId3 == -1 || deck.CardId4 == -1)
            {
                throw new InvalidDataException("12 (too few cards given)");
            }

            if(deck.CardId1 == deck.CardId2 || deck.CardId2 == deck.CardId3 || deck.CardId3 == deck.CardId4 || 
               deck.CardId1 == deck.CardId3 || deck.CardId1 == deck.CardId4 || deck.CardId2 == deck.CardId4)
            {
                throw new InvalidDataException("13 (every card is only once allowed in the deck)");
            }

            //IDbCommand command = Database.DBConnection.ConnectionCreate();

            /*DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
            command.CommandText = "SELECT username FROM userdeck WHERE username=@username";*/

            //IDataReader reader = command.ExecuteReader();

            bool existingDeck = false;
            /*if (reader.Read())
            {
                existingDeck = true;
            }*/

            if(ReadTableUserdeck.UserdeckExist(deck.Username) == true)
            {
                existingDeck = true;
            }

            //command.Connection.Close();


            //command = Database.DBConnection.ConnectionCreate();
            List<Card> cards = new();
            int allCardsOwned = 0;

            cards = ReadTableUsercards.GetUserOwnedCardIds(deck.Username);

            /*DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
            command.CommandText = "SELECT id FROM usercards WHERE username=@username";*/

            /*reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                });
            }*/

            if(cards.Count < 4)
            {
                throw new InvalidDataException("14 (too few cards owned)");
            }

            foreach (var card in cards)
            {
                if(card.Id == deck.CardId1 || card.Id == deck.CardId2 || card.Id == deck.CardId3 || card.Id == deck.CardId4)
                {
                    allCardsOwned++;
                }
            }

            if(allCardsOwned != 4)
            {
                throw new InvalidDataException("15 (not all for deck selected cards owned)");
            }

            if (existingDeck)
            {
                WriteTableUserdeck.DeleteUserDeck(deck.Username);

                /*command = Database.DBConnection.ConnectionCreate();

                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                command.CommandText = "DELETE FROM userdeck WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();*/
            }

            try
            {
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId1);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId2);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId3);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId4);

                /*command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId1);
                command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId2);
                command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId3);
                command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId4);
                command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";
                command.ExecuteNonQuery();
                command.Connection.Close();*/
            }
            catch (Exception e)
            {
                throw new InvalidDataException("16 (userdeck cards adding DB error)");
            }
        }
    }
}
