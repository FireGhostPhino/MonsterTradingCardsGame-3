using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableMessages;
using MonsterTradingCardsGame_3.Enums;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathChatroom
    {
        public PathChatroom(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType != Enums.RequestTypes.POST.ToString() &&
                (headerInfos[1] == "" || ((pathSplitted.Length > 1) &&
                (headerInfos[1] != (StandardValues.tokenPre + pathSplitted[1] + StandardValues.tokenPost)))))
            {
                throw new InvalidDataException("2 (Token Error)");
            }

            if (requestType == Enums.RequestTypes.GET.ToString())
            {
                GetRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else if (requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void GetRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            ReadTableMessages.GetAllMessages(response);
        }

        private void PostRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            Message? message;
            string[] tokenparts;
            try
            {
                message = JsonSerializer.Deserialize<Message>(bodyInformation ?? "");

                tokenparts = headerInfos[1].Split(' ');
                message.Username = (tokenparts[1].Split('-'))[0];

                if (message == null)
                {
                    throw new InvalidDataException("11");
                }
            }
            catch (Exception e)
            {
                throw new InvalidDataException("11 (Data reading error)");
            }

            if (message.Username == "" || message.MessageText == "")
            {
                throw new InvalidDataException("10 (missing Data)");
            }

            DateTime dateTime = GeneralHelpFunctions.CurrentTime.CurrentDateTime();

            WriteTableMessages.SaveMessage(message, dateTime);
        }
    }
}