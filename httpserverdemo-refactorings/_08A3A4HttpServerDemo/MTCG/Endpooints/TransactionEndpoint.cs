using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Transactions;
using static _08A3A4HttpServerDemo.MTCG.Services.TransaktionService;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    internal class TransactionEndpoint : IHttpEndpoint
    {
        private readonly TransactionService transactionService;

        private readonly UserService userService;
        public TransactionEndpoint(NpgsqlConnection connection)
        {
            var packageDao = new PackageDao(connection);
            var userDao = new UserDao(connection);
            var cardDao = new CardDao(connection);
            this.transactionService = new TransactionService(packageDao, cardDao, userDao);
            this.userService = new UserService(userDao);
        }

        public bool HandleRequest(HttpRequest request, HttpResponse response)
        {

            if (request.Method == HttpMethod.POST)
            {

                BuyPackages(request, response);
                return true;
            }

            return false;
        }

        private void BuyPackages(HttpRequest request, HttpResponse rs)
        {

            User user = this.userService.getUserByToken(request.AuthorizationToken);

            if (user == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "User already exists";
                rs.Content = rs.ResponseMessage;
                return;
            }

            var cards = this.transactionService.AcquirePackage(user.UserId);

            rs.ResponseCode = 200;
            rs.ResponseMessage = "Transaction processed successfully";
            rs.Content = rs.Content = JsonConvert.SerializeObject(cards);
        }
    }
}