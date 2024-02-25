using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class TradingService
    {
        public CardDao CardDao { get; private set; }
        public TradingDao TradingDao { get; private set; }
        public TradingService(CardDao cardDao, TradingDao tradingDao) {
            
            this.CardDao = cardDao;
            this.TradingDao = tradingDao;
        }

        public Card CheckIfCardIsLocked(string userId, string cardId)
        {
            try
            {
                return CardDao.GetCardsByUserIdAndCheckIfLocked(userId, cardId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool CheckIfIdExists(Trading trading)
        {
            try
            {
                Trading t = TradingDao.Read(trading);
                return t != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CreateTrading(Trading trading)
        {
            try
            {
                TradingDao.Create(trading);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Trading> GetAllTrades()
        {
            try
            {
                return TradingDao.getAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool DeleteTrade(Trading trading)
        {
            try
            {
                TradingDao.Delete(trading);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Trade(string userId, Trading trading, string myCardId)
        {
            try
            {
                Card myCard = CardDao.Read(myCardId);
                Trading trade = TradingDao.Read(trading);
                Card cardToTrade = CardDao.Read(trade.CardToTrade);

                if (cardToTrade.UserId == userId || myCard.Damage < trade.MinimumDamage || myCard.GetType().Name.Contains(trade.Type))
                {
                    return false;
                }

                myCard.ChangeUserId(cardToTrade.UserId);
                cardToTrade.ChangeUserId(userId);
                DeleteTrade(trading);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
