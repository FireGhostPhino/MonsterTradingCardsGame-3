using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Enums
{
    public enum RequestTypes
    {
        POST,
        GET,
        PUT,
        DELETE
    }

    public enum PathTypes
    {
        cards,
        deck,
        packages,
        scoreboard,
        sessions,
        stats,
        tradings,
        transactions,
        users,
        chatroom,
        battles
    }

    internal class EnumRequestTypes
    {

    }
}
