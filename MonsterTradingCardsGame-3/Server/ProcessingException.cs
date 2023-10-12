using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class ProcessingException : Exception
    {
        public int ErrorCode { get; set; }

        public ProcessingException(int errorCode) 
        {
            this.ErrorCode = errorCode;
        }
    }
}
