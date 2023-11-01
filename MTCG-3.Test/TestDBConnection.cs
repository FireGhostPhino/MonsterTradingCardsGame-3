using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestDBConnection
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void testDBConnection()
        {
            //Arrange
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";

            //Act
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();

            //Assert
            connection.Open();
        }

        [Test]
        public void testDBDataRequest()
        {
            //Arrange
            string connectionString = "Host = localhost; Database = mtcgdb; Username = postgres; Password = postgres";
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();

            //Act
            connection.Open();
            command.CommandText = "SELECT id, username FROM users";
            using IDataReader reader = command.ExecuteReader();

            //Assert
            Assert.That(reader.Read(), Is.True);
        }
    }
}
