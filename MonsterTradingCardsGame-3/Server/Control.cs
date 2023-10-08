﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonsterTradingCardsGame_3.Server
{
    internal class Control
    {
        public void ServerControl()
        {
            Console.WriteLine("Server erreichbar unter: http://localhost:10001/");

            string? requestType;
            string? path;

            var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            httpServer.Start();

            while (true)
            {
                var clientSocket = httpServer.AcceptTcpClient();
                using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(clientSocket.GetStream());

                //read the request
                string? line;
                string requestInformation;
                bool isBody = false;
                int content_length = 0;
                int lineNumber = 0;
                requestInformation = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    //Console.WriteLine(lineNumber + ": ");
                    Console.WriteLine(line);

                    if(lineNumber == 0)
                    {
                        requestInformation = line;
                    }

                    if (line == "")
                    {
                        isBody = true;
                        break;
                    }

                    //Parse the header
                    if (!isBody)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && parts[0] == "Content-Length")
                        {
                            content_length = int.Parse(parts[1].Trim());
                        }
                    }
                    lineNumber++;
                }

                BodyProcessing body = new BodyProcessing();
                string bodyInformation = body.BodyProcesser(content_length, reader);
                //Console.WriteLine("body:\n" + bodyInformation + "\n");

                /*Console.WriteLine(":" + requestInformation);
                Console.WriteLine(":" + requestInformation.Split('/'));*/

                /*Console.WriteLine(requestInformation);
                Console.WriteLine(requestInformation[0]);
                Console.WriteLine(requestInformation.ToString());
                string test = requestInformation.ToString();
                string test1 = test.Split(" ");
                Console.WriteLine("test1: " + test1);
                Console.WriteLine(": " + requestInformation.Split(' '));

                string test2;
                foreach (var item in requestInformation)
                {
                    test2 += item;
                }*/

                RequestReacter reactor = new RequestReacter();
                reactor.ProcessRequest(requestInformation, bodyInformation);

                HTTP_Response response = new HTTP_Response();
                response.HTTPResponse(writer);

                if (bodyInformation == "-1")
                {
                    break;
                }
            }
        }
    }
}
