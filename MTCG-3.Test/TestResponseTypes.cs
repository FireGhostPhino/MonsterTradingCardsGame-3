using MonsterTradingCardsGame_3.Database;
using MonsterTradingCardsGame_3.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestResponseTypes
    {
        [SetUp]
        public void Setup()
        {
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
            command.CommandText = "INSERT INTO testtable (name) VALUES (@name)";
            DBCreateParameter.AddParameterWithValue(command, "name", DbType.String, name);
            command.ExecuteNonQuery();
            command.Connection.Close();

            connection = new NpgsqlConnection(connectionString);
            command = connection.CreateCommand();
            connection.Open();
            command.CommandText = "SELECT id, name FROM testtable WHERE name = @name";
            DBCreateParameter.AddParameterWithValue(command, "name", DbType.String, name);
            IDataReader reader = command.ExecuteReader();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader.GetInt32(0), Is.GreaterThan(1));
                Assert.That(reader.GetString(1), Is.EqualTo(name));
            });
        }

        [Test]
        public void TestDeckFormatPlain()
        {
            //Arrange
            HTTP_Response response = new();
            string parameter = "format=plain";

            //Assert
            Assert.That(response.plainOutput, Is.EqualTo(false));

            //Act
            MonsterTradingCardsGame_3.ResponseTypes.PathDeck.DeckFormatPlain(response, parameter);

            //Assert
            Assert.That(response.plainOutput, Is.EqualTo(true));
        }
    }
}
