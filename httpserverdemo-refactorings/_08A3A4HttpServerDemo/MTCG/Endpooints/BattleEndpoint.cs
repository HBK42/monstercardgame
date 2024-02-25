using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    public class BattleEndpoint : IHttpEndpoint
    {

        private BattleService battleService;
        private readonly UserService userService;

        public BattleEndpoint(NpgsqlConnection connection)
        {
            var cardDao = new CardDao(connection);
            var userDao = new UserDao(connection);
            var deckDao = new DeckDao(connection);
            var gameDao = new StatsDao(connection);


            battleService = new BattleService(userDao, cardDao, deckDao, gameDao);
            userService = new UserService(userDao);

        }
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                Battle(rq, rs);
                return true;
            }

            return false;
        }

        private void Battle(HttpRequest request, HttpResponse rs)
        {

            var user = userService.getUserByToken(request.AuthorizationToken);


            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var result = battleService.Battles(user.UserId);



            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(result);
        }
    }
}
