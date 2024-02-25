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
    internal class SessionEndpoint : IHttpEndpoint
    {
        private UserService userService;
        public SessionEndpoint(NpgsqlConnection connection)
        {
            var userDao = new UserDao(connection);
            userService = new UserService(userDao);

        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                Login(rq, rs);
                return true;
            }
            else if (rq.Method == HttpMethod.GET)
            {
                return true;
            }
            return false;
        }

        public void Login(HttpRequest rq, HttpResponse rs)
        {
            // ToDo: Check if Login Body is Empty

            var user = JsonConvert.DeserializeObject<User>(rq.Content ?? "");

            User databaseUser = userService.login(user);

            if (databaseUser == null)
            {
                rs.ResponseCode = 409;
                rs.ResponseMessage = "Cannot get User";
                rs.Content = rs.ResponseMessage;
                return;
            }
            else
            {
                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
                rs.Content = JsonConvert.SerializeObject(databaseUser);
            }

        }
    }
}
