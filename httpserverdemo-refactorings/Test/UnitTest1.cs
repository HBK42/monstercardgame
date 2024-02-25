using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Net;
using System.Text;
using static _08A3A4HttpServerDemo.MTCG.Services.TransaktionService;

namespace Test
{
    public class Tests
    {

        private Mock<CardDao> mockCardDao;
        private Mock<UserDao> mockUserDao;
        private Mock<StatsDao> mockStatsDao;
        private Mock<PackageDao> mockPackageDao;
        private Mock<DeckDao> mockDeckDao;

        private UserService userService;
        private CardsService cardService;
        private DeckService deckService;
        private PackageService packageService;
        private TransactionService transaktionService;

        [SetUp]
        public void Setup()
        {
            mockUserDao = new Mock<UserDao>();
            mockStatsDao = new Mock<StatsDao>();
            mockPackageDao = new Mock<PackageDao>();
            mockCardDao = new Mock<CardDao>();
            mockDeckDao = new Mock<DeckDao>();


            cardService = new CardsService(mockPackageDao.Object, mockCardDao.Object, mockUserDao.Object);
            userService = new UserService(mockUserDao.Object, mockStatsDao.Object);
            deckService = new DeckService(mockPackageDao.Object, mockCardDao.Object, mockUserDao.Object, mockDeckDao.Object);
            packageService = new PackageService(mockPackageDao.Object, mockCardDao.Object, mockUserDao.Object);
            transaktionService = new TransactionService(mockPackageDao.Object,mockCardDao.Object,mockUserDao.Object);
        }






        [Test]
        public void GetDeck_ValidUserId_ReturnsListOfCards()
        {
            // Arrange
            string userId = "exampleUserId";
            string deckId = "exampleDeckId";
            List<Card> expectedCards = new List<Card>
            {
                new SpellCard("id","name",100),

            };
            mockDeckDao.Setup(dao => dao.GetDeckIdByUserId(userId)).Returns(deckId);
            mockCardDao.Setup(dao => dao.GetByDeckId(deckId)).Returns(expectedCards);

            // Act
            List<Card> actualCards = deckService.GetDeck(userId);

            // Assert
            Assert.IsNotNull(actualCards);
            Assert.AreEqual(expectedCards.Count, actualCards.Count);

        }

        [Test]
        public void GetDeck_NullOrEmptyDeckId_ReturnsEmptyList()
        {
            // Arrange
            string userId = "exampleUserId";
            mockDeckDao.Setup(dao => dao.GetDeckIdByUserId(userId)).Returns((string)null); // or Returns(string.Empty)

            // Act
            List<Card> actualCards = deckService.GetDeck(userId);

            // Assert
            Assert.IsNotNull(actualCards);
            Assert.AreEqual(0, actualCards.Count);
        }

        [Test]
        public void GetCardsByUserId_ValidUserId_ReturnsListOfCards()
        {
            // Arrange
            string userId = "exampleUserId";
            List<Card> expectedCards = new List<Card>
            {
                new SpellCard("id","name",100),
                 new SpellCard("id","name",100),
                  new SpellCard("id","name",100),

            };
            mockCardDao.Setup(dao => dao.GetByUserID(userId)).Returns(expectedCards);

            // Act
            List<Card> actualCards = cardService.GetCardsByUserId(userId);

            // Assert
            Assert.IsNotNull(actualCards);
            Assert.AreEqual(expectedCards.Count, actualCards.Count);

        }

        [Test]
        public void GetCardsByUserId_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            string userId = "exampleUserId";
            mockCardDao.Setup(dao => dao.GetByUserID(userId)).Throws(new Exception("Simulated exception"));

            // Act
            List<Card> actualCards = cardService.GetCardsByUserId(userId);

            // Assert
            Assert.IsNull(actualCards);
        }

        [Test]
        public void CreateUser_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            User inputUser = new User("testUser", "passwoord");
            User createdUser = new User("testUser", "password");
            Statistik expectedStats = new Statistik(inputUser.Username, 100, 0, 0, createdUser.UserId, Guid.NewGuid().ToString(), 0, 0);

            mockUserDao.Setup(dao => dao.Create(inputUser));
            mockUserDao.Setup(dao => dao.Read(inputUser)).Returns(createdUser);
            mockStatsDao.Setup(dao => dao.Create(expectedStats));

            // Act
            User returnedUser = userService.createUser(inputUser);


            // Assert
            Assert.IsNotNull(returnedUser);

            Assert.AreEqual(createdUser.UserId, returnedUser.UserId);
            Assert.AreEqual(createdUser.Username, returnedUser.Username);

        }

        [Test]
        public void GetUserByToken_ValidToken_ReturnsUser()
        {
            // Arrange
            string validToken = "validToken";
            User expectedUser = new User("testuser", "password");

            mockUserDao.Setup(dao => dao.GetByToken(validToken)).Returns(expectedUser);

            // Act
            User returnedUser = userService.getUserByToken(validToken);

            // Assert
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(expectedUser, returnedUser);
        }

        [Test]
        public void GetUserByToken_NullOrEmptyToken_ReturnsNull()
        {
            // Act
            User returnedUserNullOrEmptyToken = userService.getUserByToken(null);
            User returnedUserEmptyToken = userService.getUserByToken("");

            // Assert
            Assert.IsNull(returnedUserNullOrEmptyToken);
            Assert.IsNull(returnedUserEmptyToken);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            User inputUser = new User("testuser", "password");
            User databaseUser = new User("testuser", "password");
            mockUserDao.Setup(dao => dao.Read(inputUser)).Returns(databaseUser);

            // Act
            User returnedUser = userService.login(inputUser);

            // Assert
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(databaseUser, returnedUser);
        }


        [Test]
        public void Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            User inputUser = new User("testuser", "password");
            User databaseUser = new User("testuser", "incorrectPassword");
            mockUserDao.Setup(dao => dao.Read(inputUser)).Returns(databaseUser);

            // Act
            User returnedUser = userService.login(inputUser);

            // Assert
            Assert.IsNull(returnedUser);
        }



        [Test]
        public void Shuffle_ListIsShuffled()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            Battle.Shuffle(list);

            // Assert
            CollectionAssert.AreNotEqual(new List<int> { 1, 2, 3, 4, 5 }, list);
        }

        [Test]
        public void Shuffle_EmptyList_DoesNotThrowException()
        {
            var list = new List<int>();

            Assert.DoesNotThrow(() => Battle.Shuffle(list));
        }


        [Test]
        public void Shuffle_ListOrderChanges()
        {

            List<int> originalList = new List<int> { 1, 2, 3, 4, 5 };
            List<int> shuffledList = new List<int>(originalList);

            Battle.Shuffle(shuffledList);

            CollectionAssert.AreNotEqual(originalList, shuffledList, "Erwartet wurde, dass die Shuffle-Methode die Reihenfolge der Elemente in der Liste ändert.");
        }

    }
}