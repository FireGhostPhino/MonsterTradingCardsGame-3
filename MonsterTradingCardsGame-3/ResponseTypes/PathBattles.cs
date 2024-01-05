using MonsterTradingCardsGame_3.Battle;
using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database.DBCommands.TableCardsuperiors;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsercards;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUserdeck;
using MonsterTradingCardsGame_3.Database.DBCommands.TableUsers;
using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.ResponseTypes
{
    internal class PathBattles
    {
        public static readonly object Lock = new object();
        public static int counter = 0;
        public static string battleRequestUsername = "";

        public PathBattles(string[] headerInfos, string[] pathSplitted, string bodyInformation, HTTP_Response response)
        {
            string requestType = headerInfos[2];

            if (requestType == Enums.RequestTypes.POST.ToString())
            {
                PostRequest(headerInfos, bodyInformation, response);
            }
            else
            {
                throw new InvalidDataException("3 (invalid request type)");
            }
        }

        private void PostRequest(string[] headerInfos, string bodyInformation, HTTP_Response response)
        {
            lock (Lock)
            {
                Console.WriteLine("counter pre: " + counter);

                string[] tokenparts;
                string username;
                try
                {
                    tokenparts = headerInfos[1].Split(' ');
                    username = (tokenparts[1].Split('-'))[0];
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("401 (Error at Token)");
                }
                if (username == battleRequestUsername)
                {
                    throw new InvalidDataException("22 (battle already requested)");
                }
                List<int> usercardids = ReadTableUserdeck.GetUserdeck(username);
                if (usercardids.Count < 4)
                {
                    throw new InvalidDataException("403 (too few cards in deck)");
                }

                counter++;

                if (counter == 1)
                {
                    battleRequestUsername = username;
                }
                Console.WriteLine(counter + ", " + battleRequestUsername);
                if (counter == 2)
                {
                    string battleUser1 = battleRequestUsername;
                    string battleUser2 = username;
                    Console.WriteLine("2 Players: " + battleRequestUsername + ", " + battleUser2);
                    battleRequestUsername = "";
                    counter = 0;

                    battlePreparation(response, battleUser1, battleUser2);
                }
            }
        }

        private void battlePreparation(HTTP_Response response, string battleUser1, string battleUser2)
        {
            List<int> usercardidsUser1 = ReadTableUserdeck.GetUserdeck(battleUser1);
            List<int> usercardidsUser2 = ReadTableUserdeck.GetUserdeck(battleUser2);
            int i = 0;
            foreach (int id in usercardidsUser1)
            {
                Console.WriteLine(": " + id + ", " + usercardidsUser2[i]);
                i++;
            }

            if(usercardidsUser1.Count < 4)
            {
                throw new InvalidDataException("403 (too few cards in deck of user " + usercardidsUser1 + ")");
            }
            if (usercardidsUser2.Count < 4)
            {
                throw new InvalidDataException("403 (too few cards in deck of user " + usercardidsUser2 + ")");
            }

            List<Card> usercardsUser1 = ReadTableUsercards.ReturnUserdeckValues(battleUser1, usercardidsUser1);
            List<Card> usercardsUser2 = ReadTableUsercards.ReturnUserdeckValues(battleUser2, usercardidsUser2);


            List<SuperiorCardPair> cardSuperiors = ReadTableCardsuperiors.GetAllCardsuperiors();

            /*foreach (SuperiorCardPair card in cardSuperiors)
            {
                Console.WriteLine("-");
                Console.WriteLine(card.ToString());
            }*/


            Console.WriteLine("pre");
            Console.WriteLine(usercardsUser1.Count + ", " + usercardsUser2.Count);
            Console.WriteLine();




            BattleData battleData = new();
            battleData.StartBattleLog(battleUser1, battleUser2);
            while (usercardsUser1.Count > 0 && usercardsUser2.Count > 0)
            {
                int roundwinner = 0;
                battleData.IncreaseRoundsTotal();
                battleData.AddRoundCards(usercardsUser1[0], usercardsUser2[0]);
                if (usercardsUser1[0].CardCategorie == Enums.CardCategories.MonsterCard && 
                    usercardsUser2[0].CardCategorie == Enums.CardCategories.MonsterCard)
                {
                    roundwinner = battleMonsters(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors);
                }
                else if(usercardsUser1[0].CardCategorie == Enums.CardCategories.SpellCard && 
                        usercardsUser2[0].CardCategorie == Enums.CardCategories.SpellCard)
                {
                    roundwinner = battleSpells(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors);
                }
                else
                {
                    roundwinner = battleMixed(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors);
                }

                if(roundwinner == 0)
                {
                    battleData.AddRoundWinnerDraw();
                    usercardsUser1.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                    usercardsUser2.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                }
                else if(roundwinner == 1)
                {
                    battleData.AddRoundWinnerUser1(usercardsUser1[0]);
                    usercardsUser1.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                    usercardsUser1.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                }
                else if(roundwinner == 2)
                {
                    battleData.AddRoundWinnerUser2(usercardsUser2[0]);
                    usercardsUser2.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                    usercardsUser2.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                }

                Console.WriteLine(usercardsUser1.Count + ", " + usercardsUser2.Count);
                /*Console.WriteLine("1");
                usercardsUser2.Add(usercardsUser1[0]);
                Console.WriteLine("2");
                usercardsUser1.RemoveAt(0);
                Console.WriteLine("3");
                Console.WriteLine(usercardsUser1.Count + ", " + usercardsUser2.Count);
                Console.WriteLine();*/
            }



            Console.WriteLine(battleData.ToString());

            Console.WriteLine("post");
            Console.WriteLine(usercardsUser1.Count + ", " + usercardsUser2.Count);

            /*if(usercardsUser1.Count > 0)
            {
                for(int j = 0; j < usercardsUser1.Count; j++)
                {
                    WriteTableUsercards.ChangeCardOwner(battleUser1, usercardsUser1[j].Id);
                }
                WriteTableUserdeck.DeleteUserDeck(battleUser2);
                Console.WriteLine("End Battle - " + battleUser1 + " won!");
            }
            else
            {
                for (int k = 0; k < usercardsUser2.Count; k++)
                {
                    WriteTableUsercards.ChangeCardOwner(battleUser2, usercardsUser2[k].Id);
                }
                WriteTableUserdeck.DeleteUserDeck(battleUser1);
                Console.WriteLine("End Battle - " + battleUser2 + " won!");
            }*/
        }

        private int battleMonsters(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors)
        {
            SuperiorCardPairCheck superiorCardPair = new();
            int superiorCardUser = superiorCardPair.IsSuperiorCardPair(cardUser1, cardUser2, cardSuperiors);
            //int superiorCardUser = SuperiorCardPairCheck.IsSuperiorCardPair(cardUser1, cardUser2, cardSuperiors);

            battleData.AddRoundDamage(cardUser1.Damage, cardUser2.Damage);
            Console.WriteLine("____ " + superiorCardUser);
            if (superiorCardUser == 1)
            {
                return 1;
            }
            else if(superiorCardUser == 2)
            {
                return 2;
            }

            if(cardUser1.Damage > cardUser2.Damage)
            {
                return 1;
            }
            else if(cardUser2.Damage > cardUser1.Damage) 
            { 
                return 2;
            }
            return 0;
        }

        private int battleSpells(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors)
        {


            return 0;
        }

        private int battleMixed(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors)
        {


            return 0;
        }
    }
}
