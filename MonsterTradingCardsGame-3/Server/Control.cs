using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class Control
    {
        public void ServerControl()
        {
            Console.WriteLine("Server erreichbar unter: http://localhost:10001/");

            var httpServer = new TcpListener(IPAddress.Loopback, 10001);
            httpServer.Start();

            while (true)
            {
                var clientSocket = httpServer.AcceptTcpClient();
                using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(clientSocket.GetStream());

                //read the request
                string? line;
                bool isBody = false;
                int content_length = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
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
                }

                BodyProcessing body = new BodyProcessing();
                int command = body.BodyProcesser(content_length, reader);

                HTTP_Response response = new HTTP_Response();
                response.HTTPResponse(writer);

                if (command == -1)
                {
                    break;
                }
            }
        }
    }
}
