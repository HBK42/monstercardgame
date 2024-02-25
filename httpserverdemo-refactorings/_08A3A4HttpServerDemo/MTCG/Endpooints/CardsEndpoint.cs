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
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;


namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    public class CardsEndpoint : IHttpEndpoint
    {
        private CardsService cardsService;
        private readonly UserService userService;

        public CardsEndpoint(NpgsqlConnection connection)
        {
            var packageDao = new PackageDao(connection);
            var userDao = new UserDao(connection);
            var cardDao = new CardDao(connection);

            cardsService = new CardsService(packageDao, cardDao, userDao);
            userService = new UserService(userDao);

        }

        public CardsEndpoint() { }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                GetCards(rq, rs);
                return true;
            }

            return false;
        }
        private void GetCards(HttpRequest request, HttpResponse rs)
        {

            var user = userService.getUserByToken(request.AuthorizationToken);


            if (user == null)
            {
                rs.ResponseCode = 404;
                rs.ResponseMessage = "Token missing/invalid";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var cards = this.cardsService.GetCardsByUserId(user.UserId);

            if(cards == null)
            {
                rs.ResponseCode = 500;
                rs.ResponseMessage = "Could not get any cards";
                rs.Content = rs.ResponseMessage;
                return;
            }

            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(cards);
        }

    }
}
