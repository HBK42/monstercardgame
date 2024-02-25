using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Test
{
    internal class BattleTest
    {
        [Test]
        public void Battles_WithInvalidUserId_ThrowsException()
        {
            // Arrange
            var mockUserDao = new Mock<UserDao>();
            var mockCardDao = new Mock<CardDao>();
            var mockDeckDao = new Mock<DeckDao>();
            var mockGameDao = new Mock<StatsDao>();

            var userService = new BattleService(mockUserDao.Object, mockCardDao.Object, mockDeckDao.Object, mockGameDao.Object);

            string invalidUserId = "invalid"; // Invalid user ID

            // Setup mocks
            mockUserDao.Setup(dao => dao.GetById(invalidUserId)).Throws(new Exception("User not found"));

         
            Assert.Throws<Exception>(() => userService.Battles(invalidUserId));
        }

        [Test]
        public void ResetDeckId_AllCards_SuccessfullyReset_ReturnsTrue()
        {
            // Arrange
            var mockCardDao = new Mock<CardDao>();
            var userService = new BattleService(null, mockCardDao.Object, null, null);

            List<Card> cards = new List<Card>
            {
                new SpellCard("id","name",100),
                 new SpellCard("id","name",100),
                  new SpellCard("id","name",100),

            };

            // Setup mocks
            mockCardDao.Setup(dao => dao.UpdateDeckIdByCardId(It.IsAny<string>(), It.IsAny<string>())).Returns(5);

            // Act
            bool result = userService.ResetDeckId(cards);

            // Assert
            Assert.True(result);
        }


        [Test]
        public void GetStats_WithValidUserId_ReturnsStats()
        {
            // Arrange
            var mockGameDao = new Mock<StatsDao>();
            var userService = new BattleService(null, null, null, mockGameDao.Object);

            string userId = "123"; // Valid user ID

            // Setup mocks
            mockGameDao.Setup(dao => dao.GetByUserId(userId)).Returns(new Statistik());

            // Act
            var stats = userService.GetStats(userId);

            // Assert
            Assert.NotNull(stats);
        }

        [Test]
        public void GetStats_WithInvalidUserId_ThrowsException()
        {
            // Arrange
            var mockGameDao = new Mock<StatsDao>();
            var userService = new BattleService(null, null, null, mockGameDao.Object);

            string invalidUserId = "invalid"; // Invalid user ID

            // Setup mocks
            mockGameDao.Setup(dao => dao.GetByUserId(invalidUserId)).Throws(new Exception("Statistics not found"));

            // Act & Assert
            Assert.Throws<Exception>(() => userService.GetStats(invalidUserId));
        }

    }
}
