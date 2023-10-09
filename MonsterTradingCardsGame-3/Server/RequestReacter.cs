using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonsterTradingCardsGame_3.Server
{
    internal class RequestReacter
    {
        public void ProcessRequest(string requestInformation, string bodyInformation, Users.AllUsers userList, int content_length)
        {
            string[] requestSplitted = requestInformation.Split(' ');
            string requestType = requestSplitted[0];
            var requestPath = requestSplitted[1].ToString();
            string[] pathSplitted;
            string parameters;

            Console.WriteLine("\n\nRequest: ");
            Console.WriteLine("Length: " + requestSplitted.Length);
            Console.WriteLine("Type: " + requestType);
            Console.WriteLine("Path: " + requestPath);

            if(requestPath.Length > 1)
            {
                pathSplitted = requestPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
                string[] parametersSplitted = pathSplitted[pathSplitted.Length - 1].Split("?");
                pathSplitted[pathSplitted.Length - 1] = parametersSplitted[0];
                if (pathSplitted[0] != "favicon.ico")
                {
                    Console.WriteLine("\nPath splitted:");
                    Console.WriteLine("Number of paths: " + pathSplitted.Length);
                    for (int i = 0; i < pathSplitted.Length; i++)
                    {
                        Console.WriteLine(i + ": " + pathSplitted[i]);
                    }
                    if(parametersSplitted.Length > 1)
                    {
                        parameters = parametersSplitted[1];
                        Console.WriteLine("Parameters: ");
                        Console.WriteLine(parameters);
                    }
                }
            }

            if(content_length > 0 )
            {
                string[] bodyLines = bodyInformation.Split("\n");
                Console.WriteLine("\nBody: ");
                Console.WriteLine("Number of Lines: " + bodyLines.Length);
                for(int i = 0; i < bodyLines.Length; i++)
                {
                    Console.WriteLine(bodyLines[i]);
                }
            }




        }
    }
}