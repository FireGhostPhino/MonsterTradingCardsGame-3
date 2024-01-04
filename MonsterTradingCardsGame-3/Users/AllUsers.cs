using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Users
{
    internal class AllUsers
    {
        public List<User> userList;

        public void CreateList(User user)
        {
            userList = new List<User>();
            userList.Add(user);
        }

        public void AddUser(User newUser)
        {
            /*User newUser = new User(username, password, );
            userList.Add(user);*/
            userList.Add(newUser);
        }

        public bool UserListNull()
        {
            if (userList == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeUsername(string username, string newUsername)
        {
            if (UserListNull())
            {
                return;
            }
            int userPosition = 0;
            while (userPosition < userList.Count)
            {
                if (userList[userPosition].Username == username)
                {
                    userList[userPosition].Username = newUsername;
                    break;
                }
                userPosition++;
            }
        }

        public bool IsNewUsername(string username)
        {
            if(UserListNull())
            {
                return true;
            }
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].Username == username)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsCorrectPassword(string username, string password)
        {
            if (UserListNull())
            {
                return false;
            }
            int userPosition = 0;
            while (userPosition < userList.Count)
            {
                if (userList[userPosition].Username == username && userList[userPosition].Password == password)
                {
                    return true;
                }
                userPosition++;
            }
            return false;
        }

        /*public void PrintAllUsers()
        {
            if (UserListNull() == false)
            {
                Console.WriteLine("Keine Liste vorhanden!");
                return;
            }
            Console.WriteLine(userList.Count);
            for (int i = 0; i < userList.Count; i++)
            {
                Console.WriteLine("\nUser %d:", i);
                Console.WriteLine(userList[i].Username);
                Console.WriteLine(userList[i].Password);
                Console.WriteLine(userList[i].Coins);
                Console.WriteLine(userList[i].Elo);
            }
        }*/

        /*public void PrintMultipleUsers(List<User> listOfUsers)
        {
            Console.WriteLine(listOfUsers.Count);
            for (int i = 0; i < listOfUsers.Count; i++)
            {
                Console.WriteLine($"\nUser {i+1}:");
                Console.WriteLine(listOfUsers[i].Username);
                Console.WriteLine(listOfUsers[i].Password);
                Console.WriteLine(listOfUsers[i].Coins);
                Console.WriteLine(listOfUsers[i].Elo);
            }
        }*/
    }
}
