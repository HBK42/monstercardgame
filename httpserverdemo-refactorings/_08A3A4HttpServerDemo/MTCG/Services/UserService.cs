using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Services
{
    public class UserService
    {

        public UserDao UserDao { get; private set; }

        public StatsDao StatsDao { get; private set; }


        public UserService(UserDao userDao, StatsDao statsDao)
        {
            this.UserDao = userDao;
            this.StatsDao = statsDao;
        }

        public UserService(UserDao userDao)
        {
            this.UserDao = userDao;

        }
        public virtual User createUser(User user)
        {
            UserDao.Create(user);
            
            User user2 = UserDao.Read(user);
            
            Statistik statistik = new Statistik(user.Username, 100, 0, 0, user2.UserId, Guid.NewGuid().ToString(), 0, 0);

            if (StatsDao != null)
                StatsDao.Create(statistik);


            return user2;

        }

        public virtual User getUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            return UserDao.GetByToken(token);
        }

        public User login(User user)
        {

            User databaseUser = UserDao.Read(user);

            if (databaseUser.Password == user.Password)
                return databaseUser;

            return null;

        }



    }
}
