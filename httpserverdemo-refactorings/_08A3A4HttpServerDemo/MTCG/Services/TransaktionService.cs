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
    public class TransaktionService
    {
        public class TransactionService
        {
            private readonly NpgsqlConnection connection;
            public PackageDao PackageDao { get; private set; }

            public CardDao CardDao { get; private set; }

            public UserDao UserDao { get; private set; }


            public TransactionService(PackageDao packageDao, CardDao cardDao, UserDao userDao)
            {
                PackageDao = packageDao;
                CardDao = cardDao;
                UserDao = userDao;
            }

            private bool CheckUserMoney(string uid)
            {
                try
                {
                    Package apackage = PackageDao.GetOnePackage();
                    User user = UserDao.GetById(uid);
                    if (user.Coins >= apackage.PackageCost)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                return false;
            }

            private bool CheckForPackage()
            {
                try
                {
                    Package p = PackageDao.GetOnePackage();

                    if (p == null)
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                return true;
            }

            public List<Card> AcquirePackage(string userId)
            {
                List<Card> cards = null;

                if (!CheckForPackage()) return null;

                if (!CheckUserMoney(userId)) return null;

                try
                {
                    Package apackage = PackageDao.GetOnePackage();

                    if (apackage == null)
                    {
                        return null;
                    }

                    User user = UserDao.GetById(userId);
                    cards = CardDao.GetCardsOfSpecificPackage(apackage.Id);
                    foreach (var c in cards)
                    {
                        c.ChangeUserId(userId);
                        CardDao.UpdateUserId(c.Id, userId);
                        CardDao.UpdatePackageId(null, c.Id);
                    }

                    int newValue = user.Coins - apackage.PackageCost;

                    UserDao.UpdateUserCoins(userId, newValue);
                    PackageDao.Delete(apackage);
                }
                catch (Exception e)
                {
                    throw new Exception();
                }

                return cards;
            }

        }
    }
}
