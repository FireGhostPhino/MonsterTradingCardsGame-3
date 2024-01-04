using MonsterTradingCardsGame_3.Cards;
using MonsterTradingCardsGame_3.GeneralHelpFunctions;
using MonsterTradingCardsGame_3.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUserdeck
{
    internal class ReadTableUserdeck
    {
        public static List<int> GetUserdeck(string username)
        {
            List<int> usercardsids = new();

            using IDbCommand command = DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT usercardsid FROM userdeck WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                usercardsids.Add(reader.GetInt32(0));
            }

            return usercardsids;
        }

        public static bool UserdeckExist(string username)
        {
            using IDbCommand command = DBConnection.ConnectionCreate();

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "SELECT username FROM userdeck WHERE username=@username";

            using IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
