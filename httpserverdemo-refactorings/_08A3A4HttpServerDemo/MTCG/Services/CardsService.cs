using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class CardsService
    {
        private readonly NpgsqlConnection connection;
        public PackageDao PackageDao { get; private set; }

        public CardDao CardDao { get; private set; }

        public UserDao UserDao { get; private set; }
        public DeckDao DeckDao { get; private set; }


        public CardsService(PackageDao packageDao, CardDao cardDao, UserDao userDao)
        {
            PackageDao = packageDao;
            CardDao = cardDao;
            UserDao = userDao;
        }

        public List<Card> GetCardsByUserId(string uid)
        {
            try
            {
                return CardDao.GetByUserID(uid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return null;
        }

    }
}
