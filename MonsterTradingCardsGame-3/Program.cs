using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Net.Sockets;
using System.Net;
using MonsterTradingCardsGame_3.Server;
using System.Threading;
using System.Data;
using Npgsql;
using System.Xml.Linq;

//Opera übernimmt Verbindung und kein anderer kann dann mehr Anfragen schicken! Wieso?

namespace MonsterTradingCardsGame_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("V3");




            /*string vergluname = "pat";
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "SELECT username FROM users";

            using IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if(reader.GetString(0) == vergluname)
                {
                    Console.WriteLine("Vorhanden: ");
                    Console.WriteLine(reader.GetString(0) + " = " + vergluname);
                }
            }

            Console.WriteLine();*/


            /*List<User> users = new List<User>();
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "SELECT id, username, password FROM users";

            using IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                });
            }
            foreach(User user in users)
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("\nEnde DB \n");*/





            /*Users.AllUsers userList = new Users.AllUsers();
            Users.User user1 = new Users.User("Herbert", "abcde123", userList);
            Users.User user2 = new Users.User("Anne", "myPasswor3", userList);

            List<User> listOfUsers = new List<User>();
            listOfUsers.Add(user1);
            listOfUsers.Add(user2);

            userList.PrintMultipleUsers(listOfUsers);

            CardGenerator generator = new CardGenerator();
            generator.GeneratePackageLoop(user1);*/
            //user1.PrintCardStack();

            Server.Control server = new Server.Control();
            //server.ServerControl(userList);
            server.ServerThreads();
            //Console.WriteLine("End Server loop");




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