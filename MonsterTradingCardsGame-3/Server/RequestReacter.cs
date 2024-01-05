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
        //Verarbeitung des Headers und weitergeben von wichtigen, aufbereiteten Header Daten
        public void ProcessRequest(string requestInformation, string bodyInformation, string[] headerInfos, HTTP_Response response)
        {
            string[] requestSplitted = requestInformation.Split(' ');
            string requestType;
            string requestPath;
            string[] pathSplitted;

            if(requestSplitted.Length > 1 ) 
            {
                requestType = requestSplitted[0];
                requestPath = requestSplitted[1];

                Console.WriteLine("\nRequest Path: " + requestPath);
            }
            else
            {
                throw new ProcessingException(1);
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
            }

            string[] parametersSplitted;
            string parameters = "";
            if (requestPath.Length > 1 && isValidRequest)
            {
                pathSplitted = requestPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
                parametersSplitted = pathSplitted[^1].Split("?");
                pathSplitted[^1] = parametersSplitted[0];
                if (pathSplitted[0] != "favicon.ico")
                {
                    Console.WriteLine("Number of paths: " + pathSplitted.Length);
                    if(parametersSplitted.Length > 1)
                    {
                        parameters = parametersSplitted[1];
                    }
                }

                headerInfos[2] = requestType;
                headerInfos[3] = requestPath;
                //Aufruf von entsprechender Klasse/Funktion für Anfragepfad
                RequestFunctionCaller requestFunctionCaller = new(pathSplitted, headerInfos, bodyInformation, response, parameters);
            }
        }
    }
}