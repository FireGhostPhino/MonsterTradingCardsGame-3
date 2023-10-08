using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class HTTP_Response
    {
        public void HTTPResponse(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html><body><h1>Hello World!</h1></body></html>");
        }
    }
}
