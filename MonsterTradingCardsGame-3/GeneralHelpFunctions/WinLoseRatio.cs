using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.GeneralHelpFunctions
{
    public class WinLoseRatio
    {
        public static int WinLoseRatioCalc(int wins, int loses)
        {
            int winloseR;
            if (loses > 0)
            {
                winloseR = wins / loses;
            }
            else
            {
                winloseR = wins;
            }
            return winloseR;
        }
    }
}
