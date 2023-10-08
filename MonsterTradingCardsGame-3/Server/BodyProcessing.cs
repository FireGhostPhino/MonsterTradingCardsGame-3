using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Server
{
    internal class BodyProcessing
    {
        public int BodyProcesser(int content_length, StreamReader reader)
        {
            string command = BodyReader(content_length, reader);
            if (command != 0.ToString())
            {
                return CommandListener(command);
            }
            return 0;
        }

        public string BodyReader(int content_length, StreamReader reader)
        {
            //read the body if existing
            if (content_length > 0)
            {
                var data = new StringBuilder(200);
                char[] chars = new char[1024];
                int bytesReadTotal = 0;
                while (bytesReadTotal < content_length)
                {
                    var bytesRead = reader.Read(chars, 0, chars.Length);
                    bytesReadTotal += bytesRead;
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    data.Append(chars, 0, bytesRead);
                }
                Console.WriteLine(data.ToString());
                return data.ToString();
            }
            return 0.ToString();
        }

        public int CommandListener(string command)
        {
            if (command == "quit")
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
