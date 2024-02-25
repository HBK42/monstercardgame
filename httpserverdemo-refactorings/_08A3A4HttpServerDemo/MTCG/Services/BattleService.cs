using _08A3A4HttpServerDemo.DTO;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class BattleService
    {
        public DeckDao DeckDao { get; private set; }
        public CardDao CardDao { get; private set; }
        public UserDao UserDao { get; private set; }
        public StatsDao GameDao { get; private set; }
        public BattleService(UserDao userDao, CardDao cardDao, DeckDao deckDao, StatsDao gameDao)
        {
            UserDao = userDao;
            CardDao = cardDao;
            DeckDao = deckDao;
            GameDao = gameDao;
        }


        public Tuple<string, List<Round>>Battles(string uid)
        {
            User user = UserDao.GetById(uid);
            string deckId = DeckDao.GetDeckIdByUserId(uid);
            List<Card> deckCards = CardDao.GetByDeckId(deckId);

            UserDeckDTO userDTO = new UserDeckDTO(user, deckCards);

            var result = Battle.RegisterForBattle(userDTO);

            var winner = result.Item1;
            var log = result.Item2;

            ResetDeckId(userDTO.Deck);

            if (winner == null)
            {
                Statistik drawStats = GetStats(userDTO.User.UserId);


                drawStats.UpdateDrawByOne();
                //looserStats.UpdateDrawByOne();
                GameDao.Update(drawStats);
                //GameDao.Update(looserStats);
                return new Tuple<string, List<Round>>( "unentschieden", log);
            }


            if (winner.User.UserId == uid)
            {
                Statistik statistik = GetStats(winner.User.UserId);
                statistik.UpdateWins(statistik.Wins + 1);
                GameDao.Update(statistik);

                foreach (var c in winner.Deck)
                {
                    CardDao.UpdateUserId(c.Id, winner.User.UserId); //take-over
                }
                return new Tuple<string, List<Round>>("Du hast gewonnen", log);
            }
            else
            {
                Statistik statistik = GetStats(userDTO.User.UserId);
                statistik.UpdateLosses(statistik.Losses + 1);
                GameDao.Update(statistik);

                foreach (var c in userDTO.Deck)
                {
                    CardDao.UpdateUserId(c.Id, userDTO.User.UserId);
                }
                return new Tuple<string, List<Round>>("Du hast verloren", log);
            }

        }

        public bool ResetDeckId(List<Card> cards)
        {
            foreach (var c in cards)
            {
                try
                {
                    CardDao.UpdateDeckIdByCardId(c.Id, "");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        public Statistik GetStats(string userId)
        {
            try
            {
                return GameDao.GetByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching statistics for user ID: " + userId, ex);
            }
        }

    }
}
