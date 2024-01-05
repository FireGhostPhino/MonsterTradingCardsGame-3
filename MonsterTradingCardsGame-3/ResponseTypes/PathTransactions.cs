using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsers;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathTransactions
    {
        public PathTransactions(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(pathSplitted, headerInfos, response, bodyInformation);
            }
            else
            {
                throw new InvalidDataException("3  (invalid request type)");
            }
        }

        private void PostRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            if(pathSplitted.Length >= 2 && pathSplitted[1] == Enums.PathTypes.packages.ToString())
            {
                string[] tokenparts;
                string username;
                try
                {
                    tokenparts = headerInfos[1].Split(' ');
                    username = (tokenparts[1].Split('-'))[0];
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("401 (Token Error)");
                }

                int usercoins = ReadTableUsers.GetCoins(username);

                if (usercoins < StandardValues.packageCost)
                {
                    throw new InvalidDataException("403 (not enough coins)");
                }
                else
                {
                    CardGenerator generator = new();
                    List<Card> package = new();
                    package = generator.GeneratePackage(package);

                    foreach (var card in package)
                    {
                        WriteTableUsercards.InsertCard(card, username);
                    }


                    usercoins -= StandardValues.packageCost;

                    WriteTableUsers.UpdateCoins(usercoins, username);
                }
            }
            else
            {
                throw new InvalidDataException("18 (No Valid Path)");
            }
        }
    }
}
