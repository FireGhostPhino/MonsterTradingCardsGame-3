using Microsoft.VisualBasic;
using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database;
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
    internal class PathDeck
    {
        public PathDeck(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            Console.WriteLine("Test PathDeck requestHandler");
            Console.WriteLine("in deck test");
            Console.WriteLine("in deck test 2");

            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response, bodyInformation);
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

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            Console.WriteLine("deck test in get pre");
            using IDbCommand command = Database.DBConnection.ConnectionCreate();

            string[] parts = headerInfos[1].Split(' ');
            string username = (parts[1].Split('-'))[0];
            Console.WriteLine(username);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT cardcategorie,cardtype,elementtype,damage FROM userdeck WHERE username=@username ORDER BY id ASC;";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                response.cards.Add(new Card()
                {
                    CardCategorie = (Enums.CardCategories) Enum.Parse(typeof(Enums.CardCategories), reader.GetString(0)),
                    CardType = (Enums.CardTypes)Enum.Parse(typeof(Enums.CardTypes), reader.GetString(1)),
                    ElementType = (Enums.Elements)Enum.Parse(typeof(Enums.Elements), reader.GetString(2)),
                    Damage = reader.GetInt32(3),
                    /*CardCategorie = Enum.TryParse(reader.GetString(0), out CardCategorie),
                    CardType = reader.GetString(1),
                    ElementType = reader.GetString(2),
                    Damage = reader.GetInt32(3),*/
                });
                Console.WriteLine($"<p>CardCategorie: {reader.GetString(0)}, CardType: {reader.GetString(1)}, ElementType: {reader.GetString(2)}, Damage: {reader.GetInt32(3)}</p>");
            }

            Console.WriteLine("deck test in get post");
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
                foreach(var part in body)
                {
                    var id = part.Trim();
                    ids.Add(id.Split('"')[1]);
                }
                /*foreach(var id in ids)
                {
                    Console.WriteLine(id);
                }*/

                deck.CardId1 = Int32.Parse(ids[0]);
                deck.CardId2 = Int32.Parse(ids[1]);
                deck.CardId3 = Int32.Parse(ids[2]);
                deck.CardId4 = Int32.Parse(ids[3]);

                if (deck == null)
                {
                    throw new InvalidDataException("11");
                }

                string[] parts = headerInfos[1].Split(' ');
                deck.Username = (parts[1].Split('-'))[0];
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11");
            }

            /*Console.WriteLine("print ids: ");
            Console.WriteLine(deck.ToString());
            Console.WriteLine(deck.CardId1);*/

            if (deck.Username == "")
            {
                throw new InvalidDataException("10");
            }

            if(deck.CardId1 == -1 || deck.CardId2 == -1 || deck.CardId3 == -1 || deck.CardId4 == -1)
            {
                throw new InvalidDataException("12");
            }

            if(deck.CardId1 == deck.CardId2 || deck.CardId2 == deck.CardId3 || deck.CardId3 == deck.CardId4 || 
               deck.CardId1 == deck.CardId3 || deck.CardId1 == deck.CardId4 || deck.CardId2 == deck.CardId4)
            {
                throw new InvalidDataException("13");
            }

            Console.WriteLine(deck.CardId1 + ", " + deck.CardId2 + ", " + deck.CardId3 + ", " + deck.CardId4);

            IDbCommand command = Database.DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
            command.CommandText = "SELECT username FROM userdeck WHERE username=@username";

            IDataReader reader = command.ExecuteReader();

            bool existingDeck = false;
            if (reader.Read())
            {
                existingDeck = true;
            }

            command.Connection.Close();


            command = Database.DBConnection.ConnectionCreate();
            List<Card> cards = new();
            int allCardsOwned = 0;

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
            command.CommandText = "SELECT id FROM usercards WHERE username=@username";

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                cards.Add(new Card()
                {
                    Id = reader.GetInt32(0),
                });
            }

            //Console.WriteLine(cards.Count);
            if(cards.Count < 4)
            {
                throw new InvalidDataException("14");
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
                throw new InvalidDataException("15");
            }

            if (existingDeck)
            {
                Console.WriteLine("delete deck");
                command = Database.DBConnection.ConnectionCreate();

                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                command.CommandText = "DELETE FROM userdeck WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();

                /*Console.WriteLine("update deck");

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId1);
                command.CommandText = "UPDATE userdeck SET usercardsid=@usercardsid WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId2);
                command.CommandText = "UPDATE userdeck SET usercardsid=@usercardsid WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId3);
                command.CommandText = "UPDATE userdeck SET usercardsid=@usercardsid WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();

                command = Database.DBConnection.ConnectionCreate();
                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId4);
                command.CommandText = "UPDATE userdeck SET usercardsid=@usercardsid WHERE username=@username";
                command.ExecuteNonQuery();
                command.Connection.Close();*/
            }
            /*else
            {
                Console.WriteLine("insert into deck");

                try
                {
                    command = Database.DBConnection.ConnectionCreate();
                    DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, deck.Username);
                    DBCreateParameter.AddParameterWithValue(command, "usercardsid", DbType.Int32, deck.CardId1);
                    command.CommandText = "INSERT INTO userdeck (usercardsid, username) VALUES (@usercardsid, @username)";
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("16");
                }

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
            //}

            Console.WriteLine("insert into deck");

            try
            {
                command = Database.DBConnection.ConnectionCreate();
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
                command.Connection.Close();
            }
            catch (Exception e)
            {
                throw new InvalidDataException("16");
            }
        }
    }
}
