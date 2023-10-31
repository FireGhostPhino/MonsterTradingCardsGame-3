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
        public RequestFunctionCaller(string[] pathSplitted, string[] headerInfos, string bodyInformation, HTTP_Response response)
        {
            if (pathSplitted[0] == Enums.PathTypes.cards.ToString())
            {
                ResponseTypes.PathCards requestHandler = new ResponseTypes.PathCards(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.deck.ToString())
            {
                ResponseTypes.PathDeck requestHandler = new ResponseTypes.PathDeck(headerInfos, pathSplitted, bodyInformation);
            }
            else if(pathSplitted[0] == Enums.PathTypes.packages.ToString())
            {
                ResponseTypes.PathPackages requestHandler = new ResponseTypes.PathPackages(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.scoreboard.ToString())
            {
                ResponseTypes.PathScoreboard requestHandler = new ResponseTypes.PathScoreboard(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.sessions.ToString())
            {
                ResponseTypes.PathSessions requestHandler = new ResponseTypes.PathSessions(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.stats.ToString())
            {
                ResponseTypes.PathStats requestHandler = new ResponseTypes.PathStats(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.tradings.ToString())
            {
                ResponseTypes.PathTradings requestHandler = new ResponseTypes.PathTradings(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.transactions.ToString())
            {
                ResponseTypes.PathTransactions requestHandler = new ResponseTypes.PathTransactions(headerInfos, pathSplitted, bodyInformation);
            }
            else if (pathSplitted[0] == Enums.PathTypes.users.ToString())
            {
                ResponseTypes.PathUsers requestHandler = new ResponseTypes.PathUsers(headerInfos, pathSplitted, bodyInformation, response);
            }
            else
            {
                throw new ProcessingException(3);
                        //return 3;
            }
        }
    }
}
