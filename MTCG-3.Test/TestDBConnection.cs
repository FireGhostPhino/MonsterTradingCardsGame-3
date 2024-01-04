using MonsterTradingCardsGame_3.Users;
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
        public void TestCreateDBConnection()
        {
            //Arrange
            string connectionString = "Host = localhost; Database = testdb; Username = postgres; Password = postgres";

            //Act
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();

            //Assert
            connection.Open();

            //If no Exception Error, Test will pass
            Assert.Pass();
        }

        [Test]
        public void TestDBDataRequest()
        {
            //Arrange
            string connectionString = "Host = localhost; Database = testdb; Username = postgres; Password = postgres";
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            using IDbCommand command = connection.CreateCommand();

            //Act
            connection.Open();
            command.CommandText = "SELECT name FROM testtable WHERE id = 1";
            using IDataReader reader = command.ExecuteReader();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader.GetString(0), Is.EqualTo("Hugo"));
            });
        }

        [Test]
        public void TestDBDataSave()
        {
            //Arrange
            string connectionString = "Host = localhost; Database = testdb; Username = postgres; Password = postgres";
            IDbConnection connection = new NpgsqlConnection(connectionString);
            IDbCommand command = connection.CreateCommand();
            string name = "MaxMuster";

            //Act
            connection.Open();
            command.CommandText = "INSERT INTO testtable (name) VALUES ('" + name + "')";
            command.ExecuteNonQuery();
            command.Connection.Close();

            connection = new NpgsqlConnection(connectionString);
            command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "SELECT id, name FROM testtable WHERE name = '" + name + "'";
            IDataReader reader = command.ExecuteReader();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader.GetInt32(0), Is.GreaterThan(1));
                Assert.That(reader.GetString(1), Is.EqualTo(name));
            });
        }
    }
}
