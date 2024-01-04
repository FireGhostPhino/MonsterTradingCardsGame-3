using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_3.Test
{
    internal class TestGeneralHelpFunctions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCurrentTimeFunction()
        {
            //Arrange
            DateTime dateTime;

            //Act
            dateTime = MonsterTradingCardsGame_3.GeneralHelpFunctions.CurrentTime.CurrentDateTime();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(dateTime.Year, Is.EqualTo(DateTime.Now.Year));
                Assert.That(dateTime.Month, Is.EqualTo(DateTime.Now.Month));
            });
        }

        [Test]
        public void TestWinLoseRatioWithLoses()
        {
            //Arrange
            int wins = 10;
            int loses = 3;
            int winloseR = 0;

            //Act
            winloseR = MonsterTradingCardsGame_3.GeneralHelpFunctions.WinLoseRatio.WinLoseRatioCalc(wins, loses);

            //Assert
            Assert.That(winloseR, Is.EqualTo(10/3));
        }

        [Test]
        public void TestWinLoseRatioNoLoses()
        {
            //Arrange
            int wins = 5;
            int loses = 0;
            int winloseR = 0;

            //Act
            winloseR = MonsterTradingCardsGame_3.GeneralHelpFunctions.WinLoseRatio.WinLoseRatioCalc(wins, loses);

            //Assert
            Assert.That(winloseR, Is.EqualTo(wins));
        }
    }
}
