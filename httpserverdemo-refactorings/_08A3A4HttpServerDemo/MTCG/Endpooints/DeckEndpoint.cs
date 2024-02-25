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
    internal class DeckEndpoint : IHttpEndpoint
    {
        private readonly DeckService deckService;

        private readonly UserService userService;
        public DeckEndpoint(NpgsqlConnection connection)
        {
            var packageDao = new PackageDao(connection);
            var userDao = new UserDao(connection);
            var cardDao = new CardDao(connection);
            var deckDao = new DeckDao(connection);
            this.deckService = new DeckService(packageDao, cardDao, userDao, deckDao);
            this.userService = new UserService(userDao);
        }

        public bool HandleRequest(HttpRequest request, HttpResponse response)
        {

            if (request.Method == HttpMethod.GET)
            {

                GetDeck(request, response);
                return true;
            }
            else if (request.Method == HttpMethod.PUT)
            {
                ConfigureDeck(request, response);
            }

            return false;
        }


        private void GetDeck(HttpRequest request, HttpResponse rs)
        {

            User user = this.userService.getUserByToken(request.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var deck = deckService.GetDeck(user.UserId);

            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(deck);
        }

        private void ConfigureDeck(HttpRequest request, HttpResponse rs)
        {

            User user = this.userService.getUserByToken(request.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }


            List<string> cardIds = JsonConvert.DeserializeObject<List<string>>(request.Content);

            var deck = deckService.ConfigureDeck(user.UserId, cardIds);

            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(deck);
        }
    }
}
