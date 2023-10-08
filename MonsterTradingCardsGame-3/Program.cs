using System;

namespace MonsterTradingCardsGame_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Enums.CardTypes test = Enums.CardTypes.Elve;
            Console.WriteLine(test);
            Console.WriteLine((int)Enums.CardTypes.Elve);
            if ((int)Enums.CardTypes.Elve == 4)
            {
                Console.WriteLine("richtig!");
            }

            /*Server.Control server = new Server.Control();

            server.ServerControl();
            Console.WriteLine("fertig");*/

            /*Console.WriteLine("Branch: Development");
            Console.WriteLine("Programm MonsterTradingCardsGame Start!\n");

            Card cardMonster = new MonsterCard();
            Card cardSpell = new SpellCard();

            cardMonster.TestMethod();
            cardSpell.TestMethod();
            Console.WriteLine($"Type of Card: {cardMonster.CardType}");
            Console.WriteLine($"Type of Card: {cardSpell.CardType}");

            cardMonster.TestParentClass();

            Console.WriteLine("\nProgramm End!");*/
        }
    }
}