using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Database
{
    internal class DBConnection
    {
        public static IDbConnection ConnectionCreate()
        {
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";
            IDbConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        public static IDbCommand ConnectionOpen(IDbConnection connection)
        {
            IDbCommand command = connection.CreateCommand();
            connection.Open();
            return command;
        }

        public static IDbCommand ConnectionCreateOrigial()
        {
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";
            IDbConnection connection = new NpgsqlConnection(connectionString);
            IDbCommand command = connection.CreateCommand();
            connection.Open();
            return command;
        }
    }
}
