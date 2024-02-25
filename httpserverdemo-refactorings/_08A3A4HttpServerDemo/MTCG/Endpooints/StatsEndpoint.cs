using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _08A3A4HttpServerDemo.MTCG.Services.TransaktionService;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    internal class StatsEndpoint : IHttpEndpoint
    {

        private readonly StatsService statsService;

        private readonly UserService userService;
        public StatsEndpoint(NpgsqlConnection connection)
        {

            var statsDao = new StatsDao(connection);
            var userDao = new UserDao(connection);
            this.statsService = new StatsService(statsDao);
            this.userService = new UserService(userDao);
        }
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                GetStats(rq, rs);
                return true;
            }

            return false;
        }


        private void GetStats(HttpRequest request, HttpResponse rs)
        {

            User user = this.userService.getUserByToken(request.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var cards = this.statsService.GetStats(user.UserId);

            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(cards);
        }
    }
}
