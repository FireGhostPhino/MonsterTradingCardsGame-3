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
        public void testCommandListenerThreadQuit()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new MonsterTradingCardsGame_3.Server.BodyProcessing();
            string command = "quit";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo("-1"));
        }

        [Test]
        public void testCommandListenerServerQuit()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new MonsterTradingCardsGame_3.Server.BodyProcessing();
            string command = "server:quit";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo("-2"));
        }

        [Test]
        public void testCommandListenerOtherCommand()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.BodyProcessing body = new MonsterTradingCardsGame_3.Server.BodyProcessing();
            string command = "irgendeinCommand";

            //Act
            string returnvalue = body.CommandListener(command);

            //Assert
            Assert.That(returnvalue, Is.EqualTo(command));
        }
    }
}