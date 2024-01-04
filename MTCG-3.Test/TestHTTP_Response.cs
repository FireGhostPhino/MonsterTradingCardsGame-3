using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestHTTP_Response
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestHTTPAnswerSettingsContenttype()
        {
            //Arrange
            MonsterTradingCardsGame_3.Server.HTTP_Response response = new();
            string responseType = "Content-Type: text/html; charset=utf-8";

            //Act
            string returnvalue = response.AnswerSettingsContenttype();

            //Assert
            Assert.That(returnvalue, Is.EqualTo(responseType));
        }
    }
}
