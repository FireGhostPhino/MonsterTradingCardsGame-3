using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class HTTP_Response
    {
        public void HTTPResponse(StreamWriter writer, int returnCode)
        {
            //write the HTTP response
            if(returnCode == 0)
            {
                writer.WriteLine("HTTP/1.1 200 OK");
                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine("<html><body><h1>Request ok</h1></body></html>");
            }
            else if(returnCode == 1)
            {
                writer.WriteLine("HTTP/1.1 400 Bad Request");
                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine("<html><body><h1>Error in request occured!</h1></body></html>");
            }
            else if (returnCode == 3)
            {
                writer.WriteLine("HTTP/1.1 404 Not Found");
                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine("<html><body><h1>Non existing Path entered!</h1></body></html>");
            }
            else
            {
                writer.WriteLine("HTTP/1.1 400 Bad Request");
                writer.WriteLine("Content-Type: text/html; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine("<html><body><h1>Unknown Error occured!</h1></body></html>");
            }
        }
    }
}
