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
    public class PackageService
    {
        public PackageDao PackageDao { get; private set; }

        public CardDao CardDao { get; private set; }

        public UserDao UserDao { get; private set; }

        public PackageService(PackageDao packageDao, CardDao cardDao, UserDao userDao)
        {
            this.PackageDao = packageDao;
            this.CardDao = cardDao;
            this.UserDao  = userDao;
        }


        public string CreatePackagesAndCards(List<Card> cards)
        {
            Package p = new Package(Guid.NewGuid().ToString());
            List<Card> createdCards = new List<Card>();

            try
            {
                PackageDao.Create(p);

                foreach (var c in cards)
                {
                    c.ChangePackageId(p.Id);
                    CardDao.Create(c);
                    createdCards.Add(c);
                }

                return "201";
            }
            catch (NpgsqlException ex)
            {

                // Dieser Code dient dazu dass wenn während des Erstell Vorgangs zu einem Fehler kommt, dass dann die erstellten sachen h
                //gelöscht werden

                //try
                //{
                //    foreach (var c in createdCards)
                //    {
                //        CardDao.Delete(c.Id);
                //    }
                //    PackageDao.Delete(p.Id);
                //}
                //catch (NpgsqlException e)
                //{
                //    Console.WriteLine(e.Message);
                //}

                return null;
            }
        }
    }
}
