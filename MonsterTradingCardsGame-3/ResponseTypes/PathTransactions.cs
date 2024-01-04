using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database;
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
                throw new InvalidDataException("2");
            }
        }

        private void PostRequest(string[] pathSplitted, string[] headerInfos, HTTP_Response response, string bodyInformation)
        {
            if(pathSplitted.Length >= 2 && pathSplitted[1] == Enums.PathTypes.packages.ToString())
            {
                string[] tokenparts;
                string username = "";
                try
                {
                    tokenparts = headerInfos[1].Split(' ');
                    username = (tokenparts[1].Split('-'))[0];
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("17");
                }

                IDbCommand command = Database.DBConnection.ConnectionCreate();

                DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
                command.CommandText = "SELECT coins FROM users WHERE username=@username";

                IDataReader reader = command.ExecuteReader();
                int usercoins = 0;
                if (reader.Read())
                {
                    usercoins = reader.GetInt32(0);
                }
                else
                {
                    throw new InvalidDataException("19");
                }
                command.Connection.Close();

                if (usercoins < StandardValues.packageCost)
                {
                    throw new InvalidDataException("20");
                }
                else
                {
                    CardGenerator generator = new CardGenerator();
                    List<Card> package = new();
                    package = generator.GeneratePackage(package);

                    int i = 1;
                    foreach (var card in package)
                    {
                        Console.WriteLine(i);
                        i++;

                        command = Database.DBConnection.ConnectionCreate();

                        DBCreateParameter.AddParameterWithValue(command, "category", DbType.String, card.CardCategorie.ToString());
                        DBCreateParameter.AddParameterWithValue(command, "cardtype", DbType.String, card.CardType.ToString());
                        DBCreateParameter.AddParameterWithValue(command, "elementtype", DbType.String, card.ElementType.ToString());
                        DBCreateParameter.AddParameterWithValue(command, "damage", DbType.Int32, card.Damage);
                        DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
                        command.CommandText = "INSERT INTO usercards (category, cardtype, elementtype, damage, username) VALUES (@category, @cardtype, @elementtype, @damage, @username)";

                        command.ExecuteNonQuery();
                        command.Connection.Close();
                    }


                    usercoins -= StandardValues.packageCost;

                    command = Database.DBConnection.ConnectionCreate();

                    DBCreateParameter.AddParameterWithValue(command, "coins", DbType.Int32, usercoins);
                    DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
                    command.CommandText = "UPDATE users SET coins=@coins WHERE username=@username";
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            else
            {
                throw new InvalidDataException("18");
            }
        }
    }
}
