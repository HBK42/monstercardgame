using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    public class TradingEndpoint : IHttpEndpoint
    {
        private readonly TradingService tradingService;

        private readonly UserService userService;
        public TradingEndpoint(NpgsqlConnection connection)
        {

            var cardDao = new CardDao(connection);
            var userDao = new UserDao(connection);
            var tradingDao = new TradingDao(connection);

            this.tradingService = new TradingService(cardDao,tradingDao);
            this.userService = new UserService(userDao);
        }
        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                GetTrading(rq,rs);
                return true;
            }
            else if (rq.Method == HttpMethod.POST)
            {
            
                if(rq.Path.Length == 2)
                {
                    CreateTrading(rq, rs);
                }
                else
                {
                    Trade(rq,rs);
                }

               
                return true;

            }
            else if (rq.Method == HttpMethod.DELETE)
            {
                Delete(rq,rs);
                return true;
            }

            return false;
        }

        private void GetTrading(HttpRequest rq, HttpResponse rs)
        {

            User user = this.userService.getUserByToken(rq.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var list = tradingService.GetAllTrades();

           

            rs.ResponseCode = 409;
            rs.ResponseMessage = "List";
            rs.Content = JsonConvert.SerializeObject(list);

        }

        private void CreateTrading(HttpRequest rq, HttpResponse rs)
        {
            User user = this.userService.getUserByToken(rq.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

     

            var trading = JsonConvert.DeserializeObject<Trading>(rq.Content ?? "");

            Card c = tradingService.CheckIfCardIsLocked(user.UserId, trading.CardToTrade);



            bool exists = tradingService.CheckIfIdExists(trading);

            tradingService.CreateTrading(trading);
            rs.ResponseCode = 200;
            rs.ResponseMessage = "Created";
            rs.Content = JsonConvert.SerializeObject(rs.ResponseMessage); ;

        }

        private void Trade(HttpRequest rq, HttpResponse rs)
        {
            User user = this.userService.getUserByToken(rq.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            string myTradingCardId = JsonConvert.DeserializeObject<string>(rq.Content ?? "");

            string tradingId = rq.Path[2];


            Card c = tradingService.CheckIfCardIsLocked(user.UserId, myTradingCardId);

            if (c != null)
            {
                rs.ResponseCode = 403;
                rs.ResponseMessage = "The deal contains a card that is not owned by the user or locked in the deck.";
                rs.Content = rs.ResponseMessage;
                return;
            }


            var trading = new Trading();

            bool exists = tradingService.CheckIfIdExists(trading);

            if (!exists)
            {
                rs.ResponseCode = 404;
                rs.ResponseMessage = "Id not Found";
                rs.Content = rs.ResponseMessage;
                return;
            }

            bool worked = tradingService.Trade(user.UserId, trading, myTradingCardId);

            

            rs.ResponseCode = 200;
            rs.ResponseMessage = "worked";
            rs.Content = rs.ResponseMessage;


        }

        private void Delete(HttpRequest rq, HttpResponse rs)
        {
            User user = this.userService.getUserByToken(rq.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }


            Trading trading = new Trading(rq.Path[2]);

            bool worked = tradingService.DeleteTrade(trading);


            rs.ResponseCode = 200;
            rs.ResponseMessage = "worked";
            rs.Content = rs.ResponseMessage;
        }

    }
}
