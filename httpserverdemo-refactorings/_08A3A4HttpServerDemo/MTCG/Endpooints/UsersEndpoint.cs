using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json;
using Npgsql;
using System.Text.Json;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    public class UsersEndpoint : IHttpEndpoint
    {
   
        private UserService userService;
        private StatsService statsService;
        public UsersEndpoint(NpgsqlConnection connection)
        {
            var userDao = new UserDao(connection);
            var statsDao = new StatsDao(connection);

            userService = new UserService(userDao, statsDao);

        }

        public UsersEndpoint()
        {
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            Console.WriteLine(rq.Path);
            if (rq.Method == HttpMethod.POST)
            {
                CreateUser(rq, rs);
                return true;
            }
            else if (rq.Method == HttpMethod.GET)
            {
                GetUsers(rq, rs);
                return true;
            }
            return false;
        }


        public virtual void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(rq.Content ?? "");

                User users = userService.createUser(user);


                if (users == null)
                {
                    rs.ResponseCode = 409;
                    rs.ResponseMessage = "User already exists";
                    rs.Content = rs.ResponseMessage;
                    return;
                }

                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
                rs.Content = JsonConvert.SerializeObject(users);


            }
            catch (Exception ex)
            {

                rs.ResponseCode = 400;
                rs.Content = "Failed to parse User data! ";
            }
        }

        public void GetUsers(HttpRequest rq, HttpResponse rs)
        {
            //rs.Content = JsonSerializer.Serialize(new User[] { new User() { Username = "Max Muster", Password="1234" } });
            rs.Headers.Add("Content-Type", "application/json");
        }
    }
}
