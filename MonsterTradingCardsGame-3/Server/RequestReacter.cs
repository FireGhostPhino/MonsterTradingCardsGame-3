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
        public void ProcessRequest(string requestInformation, string bodyInformation)
        {
            Console.WriteLine(requestInformation.Split(' '));   //wieso geht es nicht?

            string requestType = string.Empty;
            string path = string.Empty;
            int currPosition = 0;
            foreach (var character in requestInformation)
            {
                if (character != ' ')
                {
                    requestType += character;
                }
                if(character == ' ')
                {
                    break;
                }
                currPosition++;
            }
            for(int i = currPosition+1; i < requestInformation.Length; i++)
            {
                if (requestInformation[i] != ' ')
                {
                    path = path + requestInformation[i];
                }
                if (requestInformation[i] == ' ')
                {
                    break;
                }
            }
            Console.WriteLine("RequestType: " + requestType);
            Console.WriteLine("Path: " + path + "\n");

            Console.WriteLine(path.Split('/'));     //hier auch nicht

            /*Dictionary<string, string> requestType = string.Empty;
            string path = string.Empty;
            int currPosition = 0;
            foreach (var character in requestInformation)
            {
                if (character != ' ')
                {
                    requestType += character;
                }
                if (character == ' ')
                {
                    break;
                }
                currPosition++;
            }
            for (int i = currPosition + 1; i < requestInformation.Length; i++)
            {
                if (requestInformation[i] != ' ')
                {
                    path = path + requestInformation[i];
                }
                if (requestInformation[i] == ' ')
                {
                    break;
                }
            }
            Console.WriteLine("RequestType: " + requestType);
            Console.WriteLine("Path: " + path + "\n");*/


            /*Console.WriteLine("\nHandler: body:\n" + bodyInformation + "\n");
            string part = bodyInformation.Trim();
            Console.WriteLine("body:\n" + part + "\n");
            Console.WriteLine(part.Split(" "));

            var test2 = string.Empty;
            foreach (var item in bodyInformation)
            {
                test2 += item;
            }
            Console.WriteLine(test2);
            Console.WriteLine(test2.Split(' '));
            Console.WriteLine(test2.Split(" "));

            Console.WriteLine("\n---------\n");

            //string requestType = requestInformation.Split(' ');
            Console.WriteLine(requestInformation);
            string all = requestInformation.ToString();
            Console.WriteLine(all);
            Console.WriteLine(all.Split(" "));
            var part1 = all.Split(' ');
            Console.WriteLine(part1);

            var test = string.Empty;
            foreach (var item in requestInformation)
            {
                test += item;
            }
            Console.WriteLine("---");
            Console.WriteLine(test);
            Console.WriteLine(test.Split(' '));*/

            /*string requestType = requestInformation[0].ToString();
            Console.WriteLine(requestType);
            string requestPath = requestInformation[1].ToString();
            Console.WriteLine(requestPath);*/
        }
    }
}