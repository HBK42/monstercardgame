using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class DeckService
    {
        private readonly NpgsqlConnection connection;
        public PackageDao PackageDao { get; private set; }

        public CardDao CardDao { get; private set; }

        public UserDao UserDao { get; private set; }
        public DeckDao DeckDao { get; private set; }


        public DeckService(PackageDao packageDao, CardDao cardDao, UserDao userDao, DeckDao deckDao)
        {
            PackageDao = packageDao;
            CardDao = cardDao;
            UserDao = userDao;
            DeckDao = deckDao;
        }

        public bool ConfigureDeck(string userId, List<string> cardIds)
        {
            bool back = false;
            Deck deck = null;

            List<Card> oldDeck = GetDeck(userId);

            try
            {
                string oldDeckId = DeckDao.GetDeckIdByUserId(userId);
                string deckId = oldDeckId;

                if (string.IsNullOrEmpty(oldDeckId))
                {
                    deck = new Deck(Guid.NewGuid().ToString(), userId);
                    deckId = deck.DeckId;
                    DeckDao.Create(deck);
                }

                Console.WriteLine("IN Deck Service");

                foreach (var cardId in cardIds)
                {
                    Console.WriteLine("Deck loop");

                    int returnValue = CardDao.UpdateDeckID(cardId, deckId, userId);

                    //Console.WriteLine("returnValue " + returnValue);
                    //Console.WriteLine("------------------------");
                    //Console.WriteLine(returnValue);

                    if (returnValue == 0)
                    {
                        back = true;
                        break;
                    }
                }

                if (back)
                {
                    foreach (var card in oldDeck)
                    {
                        CardDao.UpdateDeckID(card.Id, oldDeckId, userId);
                    }

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public List<Card> GetDeck(string userId)
        {
            try
            {
                string deckId = DeckDao.GetDeckIdByUserId(userId);

                if (string.IsNullOrEmpty(deckId)) return new List<Card>();
                
                List<Card> cards = CardDao.GetByDeckId(deckId);
                return cards;
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

    }
}
