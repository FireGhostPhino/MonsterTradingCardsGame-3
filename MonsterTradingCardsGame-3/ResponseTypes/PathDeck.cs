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
                throw new InvalidDataException("3 (invalid request type)");
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

            string[] tokenparts;
            string username;
            try
            {
                tokenparts = headerInfos[1].Split(' ');
                username = (tokenparts[1].Split('-'))[0];
            }
            catch (Exception e)
            {
                throw new InvalidDataException("401 (Token error)");
            }

            usercardsids = ReadTableUserdeck.GetUserdeck(username);

            if (usercardsids.Count == 4)
            {
                ReadTableUsercards.GetUserdeckValues(response, username, usercardsids);
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
                throw new InvalidDataException("401 (Body reading error)");
            }

            if (deck.Username == "")
            {
                throw new InvalidDataException("401 (Error at Token)");
            }

            if(deck.CardId1 == -1 || deck.CardId2 == -1 || deck.CardId3 == -1 || deck.CardId4 == -1)
            {
                throw new InvalidDataException("400 (too few cards given)");
            }

            if(deck.CardId1 == deck.CardId2 || deck.CardId2 == deck.CardId3 || deck.CardId3 == deck.CardId4 || 
               deck.CardId1 == deck.CardId3 || deck.CardId1 == deck.CardId4 || deck.CardId2 == deck.CardId4)
            {
                throw new InvalidDataException("403 (every card is only once allowed in the deck)");
            }

            bool existingDeck = false;

            if(ReadTableUserdeck.UserdeckExist(deck.Username) == true)
            {
                existingDeck = true;
            }

            List<Card> cards = new();
            int allCardsOwned = 0;

            cards = ReadTableUsercards.GetUserOwnedCardIds(deck.Username);

            if(cards.Count < 4)
            {
                throw new InvalidDataException("403 (too few cards owned)");
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
                throw new InvalidDataException("403 (not all for deck selected cards owned)");
            }

            if (existingDeck)
            {
                WriteTableUserdeck.DeleteUserDeck(deck.Username);
            }

            try
            {
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId1);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId2);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId3);
                WriteTableUserdeck.AddUserdeckCard(deck.Username, deck.CardId4);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("16 (userdeck cards adding DB error)");
            }
        }
    }
}
