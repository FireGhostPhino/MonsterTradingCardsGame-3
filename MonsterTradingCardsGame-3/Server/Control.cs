﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MonsterTradingCardsGame_3.Users;

namespace MonsterTradingCardsGame_3.Server
{
    internal class Control
    {
        public Control()
        {
            serverquit = 0;
            threadquit = -1;
        }

        private int serverquit;
        private int threadquit;

        //Server
        public void ServerThreads()
        {
            Console.WriteLine("Server erreichbar unter: http://localhost:10001/");

            var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            httpServer.Start();

            List<Thread?> threads = new();
            int i = 0;

            while (true)
            {
                if (serverquit == (-2))
                {
                    break;
                }
                var clientSocket = httpServer.AcceptTcpClient();
                threads.Add(new(() => ServerControl(clientSocket, i)));     //Start von einzelnen Threads für Anfragen
                threads[^1]?.Start();
                if(threadquit != -1 && threads[threadquit] != null)
                {
                    threads[threadquit]?.Join();
                    threads[threadquit] = null;
                    threadquit = -1;
                }
                if(serverquit == (-2))
                {
                    break;
                }
                i++;
            }
        }

        //Server - Thread
        public void ServerControl(TcpClient clientSocket, int threadNumber)
        {
            while (true)
            {
                if(clientSocket.Connected ==  false)
                {
                    threadquit = threadNumber;
                    return;
                }
                using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(clientSocket.GetStream());

                //read the request
                string? line;
                string requestInformation;
                bool isBody = false;
                int lineNumber = 0;
                requestInformation = string.Empty;
                string token = "";
                string[] headerInfos = new string[4];
                headerInfos[0] = "0";
                headerInfos[1] = "";

                if (clientSocket.Connected == false)
                {
                    threadquit = threadNumber;
                    return;
                }

                while ((line = reader.ReadLine()) != null)
                {
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
                            headerInfos[0] = parts[1].Trim();
                        }
                        else if(parts.Length == 2 && parts[0] == "Authorization")
                        {
                            token = parts[1].Trim();
                            headerInfos[1] = parts[1].Trim();
                        }
                    }
                    lineNumber++;
                }

                //Check ob Command in Body enthalten ist
                BodyProcessing body = new();
                string bodyInformation = body.BodyProcesser(int.Parse(headerInfos[0]), reader);

                try
                {
                    RequestReacter reactor = new();
                    HTTP_Response response = new();

                    //Haupt-Verarbeitung der Anfrage
                    reactor.ProcessRequest(requestInformation, bodyInformation, headerInfos, response);
                    //Ok Response bei keinem Fehler (throw error)
                    response.CreateOKResponse(writer);
                }
                catch (ProcessingException e)
                {
                    HTTP_Response response = new();
                    //Bei Fehler ERROR zurück schicken
                    response.CreateERRORResponse(writer, e.ErrorCode.ToString());
                }
                catch(Exception e)
                {
                    HTTP_Response response = new();
                    //Bei Fehler ERROR zurück schicken
                    response.CreateERRORResponse(writer, e.Message);
                }

                //Reaktion auf empfangenes Kommando
                if (bodyInformation == "-1" || clientSocket.Connected == false)
                {
                    threadquit = threadNumber;
                    return;
                }
                else if(bodyInformation == "-2")    //Nach nächster fertiger Iteration wird Programm/Server beendet
                {
                    serverquit = -2;
                    return;
                }

                /*writer.Flush();
                writer.Close();
                reader.Close();*/
                //Console.WriteLine("cs d: " + clientSocket.Connected);
            }
        }
    }
}
