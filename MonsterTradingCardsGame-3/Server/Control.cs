using System;
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

        public void ServerThreads(Users.AllUsers userList)
        {
            Console.WriteLine("Server erreichbar unter: http://localhost:10001/");

            var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            httpServer.Start();

            List<Thread?> threads = new List<Thread?>();
            int i = 0;

            while (true)
            {
                //Console.WriteLine("quit v: " + serverquit);
                if (serverquit == (-2))
                {
                    break;
                }
                var clientSocket = httpServer.AcceptTcpClient();
                threads.Add(new(() => ServerControl(userList, clientSocket, i)));
                threads[threads.Count - 1].Start();
                //Console.WriteLine("quit d: " + serverquit);
                if(threadquit != -1 && threads[threadquit] != null)
                {
                    //Console.WriteLine("Thread " + threadquit + "joined");
                    threads[threadquit].Join();
                    threads[threadquit] = null;
                    threadquit = -1;
                }
                if(serverquit == (-2))
                {
                    break;
                }
                i++;
            }
            /*Console.WriteLine("Server wird beendet!");
            for(int j = 0; j < threads.Count; j++)
            {
                if (threads[j] != null)
                {
                    threads[j].Join();
                }
            }
            Console.WriteLine("Alle Threads eingesammelt, Server beendet!");*/
        }

        public void ServerControl(Users.AllUsers userList, TcpClient clientSocket, int threadNumber)
        {
            //Console.WriteLine("Server erreichbar unter: http://localhost:10001/");

            string? requestType;
            string? path;

            //var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            //httpServer.Start();

            while (true)
            {
                //var clientSocket = httpServer.AcceptTcpClient();
                //Console.WriteLine("cs v: " + clientSocket.Connected);
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
                int content_length = 0;
                int lineNumber = 0;
                requestInformation = string.Empty;

                if (clientSocket.Connected == false)
                {
                    threadquit = threadNumber;
                    return;
                }

                while ((line = reader.ReadLine()) != null)
                {
                    //Console.WriteLine(lineNumber + ": ");
                    //Console.WriteLine(line);

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

                RequestReacter reactor = new RequestReacter();
                int returnCode = reactor.ProcessRequest(requestInformation, bodyInformation, userList, content_length);

                HTTP_Response response = new HTTP_Response();
                response.HTTPResponse(writer, returnCode);

                if (bodyInformation == "-1" || clientSocket.Connected == false)
                {
                    threadquit = threadNumber;
                    return;
                }
                else if(bodyInformation == "-2")
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
