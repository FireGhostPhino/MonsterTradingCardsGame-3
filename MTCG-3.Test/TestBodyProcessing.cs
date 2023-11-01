using MonsterTradingCardsGame_3;

namespace MTCG_3.Test
{
    public class TestBodyProcessing
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCommandListenerThreadQuit()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new();
            string command = "quit";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo("-1"));
        }

        [Test]
        public void TestCommandListenerServerQuit()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new();
            string command = "server:quit";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo("-2"));
        }

        [Test]
        public void TestCommandListenerOtherCommand()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new();
            string command = "irgendeinCommand";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo(command));
        }
    }
}