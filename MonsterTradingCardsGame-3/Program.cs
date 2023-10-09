using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Users;
using System;

//Opera übernimmt Verbindung und kein anderer kann dann mehr Anfragen schicken! Wieso?

namespace MonsterTradingCardsGame_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("V3");

            Users.AllUsers userList = new Users.AllUsers();
            Users.User user1 = new Users.User("Herbert", "abcde123", userList);
            Users.User user2 = new Users.User("Anne", "myPasswor3", userList);

            List<User> listOfUsers = new List<User>();
            listOfUsers.Add(user1);
            listOfUsers.Add(user2);

            userList.PrintMultipleUsers(listOfUsers);

            CardGenerator generator = new CardGenerator();
            generator.GeneratePackageLoop(user1);
            user1.PrintCardStack();

            Server.Control server = new Server.Control();
            server.ServerControl(userList);
            Console.WriteLine("End Server loop");

            /*Console.WriteLine("V3: Branch: Development");



            /*CardGenerator gen = new CardGenerator();
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(gen.RandomNumberGenerator(0, 10));
            }*/

            /*Enums.CardTypes test = Enums.CardTypes.Elve;
            Console.WriteLine(test);
            Console.WriteLine((int)Enums.CardTypes.Elve);
            if ((int)Enums.CardTypes.Elve == 4)
            {
                Console.WriteLine("richtig!");
            }*/


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