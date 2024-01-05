using MonsterTradingCardsGame_3.Battle;
using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Database.DBCommands.TableCardsuperiors;
using MonsterTradingCardsGame_3.Database.DBCommands.TableElementtypes;
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
        public static int counter = 0;      //Speicher, ob Battle gestartet werden kann
        public static string battleRequestUsername = "";    //Speicher von 1. Battle Request Username

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
            //locken von Code, damit keine Battle Anfrage verloren geht
            lock (Lock)
            {
                string[] tokenparts;
                string username;
                //Überprüfung von Token und Extraktion von Username aus Token
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

                if (counter == 1)   //wenn keine vorhandene Anfrage, kommt man in die Warteschlange
                {
                    battleRequestUsername = username;
                }
                if (counter == 2)   //wenn Anfrage vorhanden, wird Battle gestartet
                {
                    string battleUser1 = battleRequestUsername;
                    string battleUser2 = username;
                    Console.WriteLine("2 Players: " + battleRequestUsername + ", " + battleUser2);
                    battleRequestUsername = "";
                    counter = 0;

                    Battle(response, battleUser1, battleUser2);  //Start von Battle mit beiden Usernamen
                }
            }
        }

        private void Battle(HTTP_Response response, string battleUser1, string battleUser2)
        {
            //Abruf von Userdecks (ids von Karten) für Users
            List<int> usercardidsUser1 = ReadTableUserdeck.GetUserdeck(battleUser1);
            List<int> usercardidsUser2 = ReadTableUserdeck.GetUserdeck(battleUser2);
            int i = 0;
            foreach (int id in usercardidsUser1)
            {
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

            //Abruf von zugehörigen Karten-Werten zu Deck-Ids
            List<Card> usercardsUser1 = ReadTableUsercards.ReturnUserdeckValues(battleUser1, usercardidsUser1);
            List<Card> usercardsUser2 = ReadTableUsercards.ReturnUserdeckValues(battleUser2, usercardidsUser2);

            int j = 0;
            foreach (Card card in usercardsUser1)
            {
                j++;
            }

            int k = 0;
            foreach (Card card in usercardsUser2)
            {
                k++;
            }

            //Abruf von Kartenpaaren aus der DB, welche Karte welcher gegenüber überlegen ist
            List<SuperiorCardPair> cardSuperiors = ReadTableCardsuperiors.GetAllCardsuperiors();
            //Abruf von Elementtypen von Karten aus der DB, mit Information welches Element welchem überlegen ist
            List<Element> cardElements = ReadTableElementtypes.GetAllElementtypes();

            //Mitloggen von Battle Verlauf
            BattleData battleData = new();
            battleData.StartBattleLog(battleUser1, battleUser2);

            //Main Battle Loop
            //Solange ein Spieler noch Karten hat, wird wiederholt, mit Begrenzung auf 100 gegen Endlosschleife
            while (usercardsUser1.Count > 0 && usercardsUser2.Count > 0 && battleData.RoundsTotal < 100)
            {
                int roundwinner = 0;
                battleData.IncreaseRoundsTotal();
                battleData.AddRoundCards(usercardsUser1[0], usercardsUser2[0]);

                //Aufruf von entsprechender Funktion für Runde für Karten, je nach Kartentypen
                if (usercardsUser1[0].CardCategorie == Enums.CardCategories.MonsterCard && 
                    usercardsUser2[0].CardCategorie == Enums.CardCategories.MonsterCard)
                {
                    roundwinner = battleMonsters(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors);
                }
                else if(usercardsUser1[0].CardCategorie == Enums.CardCategories.SpellCard && 
                        usercardsUser2[0].CardCategorie == Enums.CardCategories.SpellCard)
                {
                    roundwinner = battleSpells(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors, cardElements);
                }
                else
                {
                    roundwinner = battleMixed(usercardsUser1[0], usercardsUser2[0], battleData, cardSuperiors, cardElements);
                }

                //Runden Abschluss
                //Eintragung von Daten von Runde
                if(roundwinner == 0)            //0: Steht für ein draw/unentschieden
                {
                    battleData.AddRoundWinnerDraw();
                    usercardsUser1.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                    usercardsUser2.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                }
                else if(roundwinner == 1)       //1: Steht für Gewinner von Runde ist User 1
                {
                    battleData.AddRoundWinnerUser1(usercardsUser1[0]);
                    usercardsUser1.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                    usercardsUser1.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                }
                else if(roundwinner == 2)       //2: Steht für Gewinner von Runde ist User 2
                {
                    battleData.AddRoundWinnerUser2(usercardsUser2[0]);
                    usercardsUser2.Add(usercardsUser2[0]);
                    usercardsUser2.RemoveAt(0);
                    usercardsUser2.Add(usercardsUser1[0]);
                    usercardsUser1.RemoveAt(0);
                }
            }

            Console.WriteLine(battleData.ToString());
            Console.WriteLine(battleData.GetBattleWinner(battleUser1, battleUser2, usercardsUser1.Count, usercardsUser2.Count));
            //Übergabe von Battle Verlauf (Log) an Klasse der Antwort Sendung
            response.returnMessage = battleData.ToString() + "<br><br>" + battleData.GetBattleWinner(battleUser1, battleUser2, usercardsUser1.Count, usercardsUser2.Count);

            //Abschluss von Battle
            //Eintragung von Ausgang von Battle in DB (Elo, Wins, Loses, Kartenbesitzer ändern)
            if (usercardsUser2.Count == 0)
            {
                for(int l = 0; l < usercardsUser1.Count; l++)
                {
                    WriteTableUsercards.ChangeCardOwner(battleUser1, usercardsUser1[l].Id);
                }
                WriteTableUserdeck.DeleteUserDeck(battleUser2);
                WriteTableUsers.UpdateElo(ReadTableUsers.GetElo(battleUser1) + 3, battleUser1);
                int eloUser2 = ReadTableUsers.GetElo(battleUser2);
                if(eloUser2 > 5)
                {
                    WriteTableUsers.UpdateElo(eloUser2 - 5, battleUser2);
                }
                else if(eloUser2 <= 5 && eloUser2 >= 1)
                {
                    WriteTableUsers.UpdateElo(0, battleUser2);
                }
                WriteTableUsers.UpdateWins(ReadTableUsers.GetWins(battleUser1) + 1, battleUser1);
                WriteTableUsers.UpdateLoses(ReadTableUsers.GetLoses(battleUser2) + 1, battleUser2);
            }
            else if (usercardsUser1.Count == 0)
            {
                for (int l = 0; l < usercardsUser1.Count; l++)
                {
                    WriteTableUsercards.ChangeCardOwner(battleUser2, usercardsUser2[l].Id);
                }
                WriteTableUserdeck.DeleteUserDeck(battleUser1);
                WriteTableUsers.UpdateElo(ReadTableUsers.GetElo(battleUser2) + 3, battleUser2);
                int eloUser1 = ReadTableUsers.GetElo(battleUser1);
                if (eloUser1 > 5)
                {
                    WriteTableUsers.UpdateElo(eloUser1 - 5, battleUser1);
                }
                else if (eloUser1 <= 5 && eloUser1 >= 1)
                {
                    WriteTableUsers.UpdateElo(0, battleUser1);
                }
                WriteTableUsers.UpdateWins(ReadTableUsers.GetWins(battleUser2) + 1, battleUser2);
                WriteTableUsers.UpdateLoses(ReadTableUsers.GetLoses(battleUser1) + 1, battleUser1);
            }
        }

        //Kampf zwischen zwei Monsterkarten
        private int battleMonsters(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors)
        {
            //Überprüfung von Karten von aktueller Runde, ob spezielle Bedingungen zutreffen, returniert 0-2
            SuperiorCardPairCheck superiorCardCheck = new();
            int superiorCardUser = superiorCardCheck.IsSuperiorCardPair(cardUser1, cardUser2, cardSuperiors);

            battleData.AddRoundDamage(cardUser1.Damage, cardUser2.Damage);
            if (superiorCardUser == 1)
            {
                return 1;       //Steht für Gewinner von Runde ist User 1
            }
            else if(superiorCardUser == 2)
            {
                return 2;       //Steht für Gewinner von Runde ist User 2
            }

            if(cardUser1.Damage > cardUser2.Damage)
            {
                return 1;
            }
            else if(cardUser2.Damage > cardUser1.Damage) 
            { 
                return 2;
            }
            return 0;   //return 0 entspricht einem draw/gleichstand zwischen den Karten
        }

        //Kampf zwischen zwei Spellkarten
        private int battleSpells(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors, List<Element> cardElements)
        {
            CardElementPairCheck superiorElementCheck = new();
            int superiorElementUser = superiorElementCheck.IsSuperiorCardElement(cardUser1, cardUser2, cardElements);

            int cardDamageUser1 = cardUser1.Damage;
            int cardDamageUser2 = cardUser2.Damage;

            battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);

            if (superiorElementUser == 1)
            {
                cardDamageUser1 = cardDamageUser1 * StandardValues.cardDamageMultiplier;
                cardDamageUser2 = cardDamageUser2 / StandardValues.cardDamageDivisor;
                battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);
            }
            else if (superiorElementUser == 2)
            {
                cardDamageUser2 = cardDamageUser2 * StandardValues.cardDamageMultiplier;
                cardDamageUser1 = cardDamageUser1 / StandardValues.cardDamageDivisor;
                battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);
            }

            if (cardDamageUser1 > cardDamageUser2)
            {
                return 1;
            }
            else if (cardDamageUser2 > cardDamageUser1)
            {
                return 2;
            }
            return 0;
        }

        //Kampf zwischen einer Monster und einer Spell Karte
        private int battleMixed(Card cardUser1, Card cardUser2, BattleData battleData, List<SuperiorCardPair> cardSuperiors, List<Element> cardElements)
        {
            SuperiorCardPairCheck superiorCardCheck = new();
            int superiorCardUser = superiorCardCheck.IsSuperiorCardPair(cardUser1, cardUser2, cardSuperiors);

            //Überprüfung von Karten von aktueller Runde, welches Element stärker ist, returniert 0-2
            CardElementPairCheck superiorElementCheck = new();
            int superiorElementUser = superiorElementCheck.IsSuperiorCardElement(cardUser1, cardUser2, cardElements);

            int cardDamageUser1 = cardUser1.Damage;
            int cardDamageUser2 = cardUser2.Damage;

            battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);
            if (superiorCardUser == 1)
            {
                return 1;
            }
            else if (superiorCardUser == 2)
            {
                return 2;
            }

            if(superiorElementUser == 1)
            {
                cardDamageUser1 = cardDamageUser1 * StandardValues.cardDamageMultiplier;
                cardDamageUser2 = cardDamageUser2 / StandardValues.cardDamageDivisor;
                battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);
            }
            else if(superiorElementUser == 2)
            {
                cardDamageUser2 = cardDamageUser2 * StandardValues.cardDamageMultiplier;
                cardDamageUser1 = cardDamageUser1 / StandardValues.cardDamageDivisor;
                battleData.AddRoundDamage(cardDamageUser1, cardDamageUser2);
            }

            if (cardDamageUser1 > cardDamageUser2)
            {
                return 1;
            }
            else if (cardDamageUser2 > cardDamageUser1)
            {
                return 2;
            }
            return 0;
        }
    }
}
