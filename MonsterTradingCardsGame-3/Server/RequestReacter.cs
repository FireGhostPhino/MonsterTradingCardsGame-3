using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MonsterTradingCardsGame_3.Cards;
using System.Reflection;

namespace MonsterTradingCardsGame_3.Server
{
    internal class RequestReacter
    {
        public void ProcessRequest(string requestInformation, string bodyInformation, string[] headerInfos, HTTP_Response response)
        {
            string[] requestSplitted = requestInformation.Split(' ');
            string requestType;
            string requestPath;
            string[] pathSplitted;
            string parameters;

            if(requestSplitted.Length > 1 ) 
            {
                requestType = requestSplitted[0];
                requestPath = requestSplitted[1];

                Console.WriteLine("\n\nRequest: ");
                Console.WriteLine("Length: " + requestSplitted.Length);
                Console.WriteLine("Type: " + requestType);
                Console.WriteLine("Path: " + requestPath);
            }
            else
            {
                throw new ProcessingException(1);
                //return 1;
            }

            bool isValidRequest = false;
            for (int i = 0; i < Enum.GetNames(typeof(Enums.RequestTypes)).Length; i++)
            {
                if (requestType == Enum.GetNames(typeof(Enums.RequestTypes))[i])
                {
                    isValidRequest = true;
                    break;
                }
            }
            if (!isValidRequest) 
            {
                throw new ProcessingException(2);
                //return 2;
            }

            string[] bodyLines;
            if (int.Parse(headerInfos[0]) > 0 )
            {
                bodyLines = bodyInformation.Split("\n");
                Console.WriteLine("\nBody: ");
                Console.WriteLine("Number of Lines: " + bodyLines.Length);
                for(int i = 0; i < bodyLines.Length; i++)
                {
                    Console.WriteLine(bodyLines[i]);
                }
            }

            string[] parametersSplitted;
            if (requestPath.Length > 1 && isValidRequest)
            {
                pathSplitted = requestPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
                parametersSplitted = pathSplitted[pathSplitted.Length - 1].Split("?");
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

                headerInfos[2] = requestType;
                headerInfos[3] = requestPath;
                RequestFunctionCaller requestFunctionCaller = new RequestFunctionCaller(pathSplitted, headerInfos, bodyInformation, response);
            }

            //return 0;
        }
    }
}