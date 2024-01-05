using MonsterTradingCardsGame_3.Server;
using MonsterTradingCardsGame_3.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database.DBCommands.TableUsers
{
    internal class WriteTableUsers
    {
        public static void InsertUser(User? user)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            command.CommandText = "INSERT INTO users (username, password, elo, coins, wins, loses) " +
            "VALUES (@username, @password, @elo, @coins, @wins, @loses)";
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, user.Username);
            DBCreateParameter.AddParameterWithValue(command, "password", DbType.String, user.Password);
            DBCreateParameter.AddParameterWithValue(command, "elo", DbType.Int32, user.Elo);
            DBCreateParameter.AddParameterWithValue(command, "coins", DbType.Int32, user.Coins);
            DBCreateParameter.AddParameterWithValue(command, "wins", DbType.Int32, user.Wins);
            DBCreateParameter.AddParameterWithValue(command, "loses", DbType.Int32, user.Loses);

            command.ExecuteNonQuery();
        }

        public static void UpdatePassword(string username, string password)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            DBCreateParameter.AddParameterWithValue(command, "password", DbType.String, password);
            command.CommandText = "UPDATE users SET password=@password WHERE username=@username";

            command.ExecuteNonQuery();
        }

        public static void UpdateCoins(int usercoins, string username)
        {
            using IDbConnection connection = DBConnection.ConnectionCreate();
            using IDbCommand command = DBConnection.ConnectionOpen(connection);

            DBCreateParameter.AddParameterWithValue(command, "coins", DbType.Int32, usercoins);
            DBCreateParameter.AddParameterWithValue(command, "username", DbType.String, username);
            command.CommandText = "UPDATE users SET coins=@coins WHERE username=@username";

            command.ExecuteNonQuery();
        }
    }
}
