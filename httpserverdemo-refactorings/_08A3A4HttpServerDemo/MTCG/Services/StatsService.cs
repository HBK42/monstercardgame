using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class StatsService
    {
        public StatsDao StatsDao { get; private set; }


        public StatsService(StatsDao statsDao) { this.StatsDao = statsDao; }


        public Statistik GetStats(string userId)
        {
            try
            {
                return StatsDao.GetByUserId(userId);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error getting statistics.", e);
            }
        }

    }
}
