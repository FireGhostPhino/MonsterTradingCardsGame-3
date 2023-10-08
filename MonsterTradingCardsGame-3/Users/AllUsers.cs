using System;
using System.Collections.Generic;
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

        public void AddUser(User user)
        {
            userList.Add(user);
        }

        public void ChangeUsername(string username, string newUsername)
        {
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
    }
}
