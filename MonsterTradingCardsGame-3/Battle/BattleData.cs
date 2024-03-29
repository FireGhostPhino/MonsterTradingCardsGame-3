﻿using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MonsterTradingCardsGame_3.Battle
{
    internal class BattleData
    {
        public BattleData() { }

        private string _battleLog = "";
        private int _roundswonUser1 = 0;
        private int _roundswonUser2 = 0;
        private int _roundsTotal = 0;

        public string BattleLog
        {
            get { return _battleLog; }
            set { _battleLog = value; }
        }

        public int RoundswonUser1
        {
            get { return _roundswonUser1; }
            set { _roundswonUser1 = value; }
        }

        public int RoundswonUser2
        {
            get { return _roundswonUser2; }
            set { _roundswonUser2 = value; }
        }

        public int RoundsTotal
        {
            get { return _roundsTotal; }
            set { _roundsTotal = value; }
        }

        public void IncreaseRoundsTotal()
        {
            RoundsTotal += 1;
        }


        public void StartBattleLog(string username1, string username2)
        {
            BattleLog = BattleLog + "User 1: " + username1 + " vs User 2: " + username2 + "<br>\n";
        }

        public void AddRoundCards(Card cardUser1, Card cardUser2)
        {
            BattleLog += cardUser1.ElementType;
            BattleLog += cardUser1.CardType;
            BattleLog = BattleLog + " (" + cardUser1.Damage + ")";
            BattleLog += " vs ";
            BattleLog += cardUser2.ElementType;
            BattleLog += cardUser2.CardType;
            BattleLog = BattleLog + " (" + cardUser2.Damage + ")";
        }

        public void AddRoundDamage(int damageUser1, int damageUser2)
        {
            BattleLog += " => ";
            BattleLog += damageUser1;
            BattleLog += " VS ";
            BattleLog += damageUser2;
        }

        public void AddRoundWinnerUser1(Card cardUser1)
        {
            BattleLog += " => ";
            BattleLog += cardUser1.ElementType;
            BattleLog += cardUser1.CardType;
            BattleLog += " wins<br>\n";
            RoundswonUser1 += 1;
        }

        public void AddRoundWinnerUser2(Card cardUser2)
        {
            BattleLog += " => ";
            BattleLog += cardUser2.ElementType;
            BattleLog += cardUser2.CardType;
            BattleLog += " wins<br>\n";
            RoundswonUser2 += 1;
        }

        public void AddRoundWinnerDraw()
        {
            BattleLog += " Draw<br>\n";
        }

        public string GetBattleWinner(string username1, string username2, int numberDeckCardsUser1, int numberDeckCardsUser2)
        {
            if (numberDeckCardsUser2 == 0)
            {
                return $"Winner: User 1: {username1}! Contratulations!";
            }
            else if (numberDeckCardsUser1 == 0)
            {
                return $"Winner: User 2: {username2}! Contratulations!";
            }
            else
            {
                return $"Draw! User 1: {username1} and User 2: {username2} are equally strong!";
            }
        }

        public override string ToString()
        {
            return $"BattleLog: <br>\n{BattleLog}<br>\nRounds Total: {RoundsTotal}<br>\nRounds won User1: {RoundswonUser1}<br>\nRounds won User2: {RoundswonUser2}";
        }
    }
}
