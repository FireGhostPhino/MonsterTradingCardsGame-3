using MonsterTradingCardsGame_3.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class RequestFunctionCaller
    {
        public RequestFunctionCaller(string[] pathSplitted, string[] headerInfos, string bodyInformation, HTTP_Response response, string parameters)
        {
            //Aufruf von entsprechender Klasse/Funktion für Anfragepfad
            if (pathSplitted[0] == Enums.PathTypes.cards.ToString())
            {
                ResponseTypes.PathCards requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.deck.ToString())
            {
                ResponseTypes.PathDeck requestHandler = new(headerInfos, pathSplitted, bodyInformation, response, parameters);
            }
            else if (pathSplitted[0] == Enums.PathTypes.scoreboard.ToString())
            {
                ResponseTypes.PathScoreboard requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.sessions.ToString())
            {
                ResponseTypes.PathSessions requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.stats.ToString())
            {
                ResponseTypes.PathStats requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.transactions.ToString())
            {
                ResponseTypes.PathTransactions requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.users.ToString())
            {
                ResponseTypes.PathUsers requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.chatroom.ToString())
            {
                ResponseTypes.PathChatroom requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.battles.ToString())
            {
                ResponseTypes.PathBattles requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request path)");
            }
        }
    }
}
