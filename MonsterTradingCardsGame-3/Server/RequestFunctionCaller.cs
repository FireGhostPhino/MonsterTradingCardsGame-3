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
            if (pathSplitted[0] == Enums.PathTypes.cards.ToString())            //ok
            {
                ResponseTypes.PathCards requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.deck.ToString())        //ok
            {
                ResponseTypes.PathDeck requestHandler = new(headerInfos, pathSplitted, bodyInformation, response, parameters);
            }
            else if (pathSplitted[0] == Enums.PathTypes.scoreboard.ToString())  //ok
            {
                ResponseTypes.PathScoreboard requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.sessions.ToString())    //ok
            {
                ResponseTypes.PathSessions requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.stats.ToString())       //ok
            {
                ResponseTypes.PathStats requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.tradings.ToString())
            {
                ResponseTypes.PathTradings requestHandler = new(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.transactions.ToString()) //ok
            {
                ResponseTypes.PathTransactions requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.users.ToString())       //ok
            {
                ResponseTypes.PathUsers requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.chatroom.ToString())    //ok
            {
                ResponseTypes.PathChatroom requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else if (pathSplitted[0] == Enums.PathTypes.chatroom.ToString())
            {
                ResponseTypes.PathBattles requestHandler = new(headerInfos, pathSplitted, bodyInformation, response);
            }
            else
            {
                throw new ProcessingException(3);
            }
        }
    }
}
