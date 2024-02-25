using _08A3A4HttpServerDemo.HTTP;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using _08A3A4HttpServerDemo.MTCG.Services;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = _08A3A4HttpServerDemo.HTTP.HttpMethod;

namespace _08A3A4HttpServerDemo.MTCG.Endpooints
{
    internal class PackageEndpoint : IHttpEndpoint
    {
        private PackageService packageService;
        public PackageEndpoint(NpgsqlConnection connection)
        {
            var packageDao = new PackageDao(connection);
            var userDao = new UserDao(connection);
            var cardDao = new CardDao(connection);

            packageService = new PackageService(packageDao, cardDao, userDao);

        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                createPackage(rq, rs);
                return true;
            }

            return false;
        }



        private void createPackage(HttpRequest rq, HttpResponse rs)
        {
            //das mit dem Tree Fehlt noch, da es ein JsonArray ist und man zwischen den verschiedenen Card unterscheiden muss

            List<Card> cards = new List<Card>();
            JArray array = JArray.Parse(rq.Content);


            foreach (JObject jsonObj in array)
            {
                string cardName = jsonObj["Name"].ToString();

                if (cardName.Contains("Spell"))
                {
                    SpellCard spellCard = jsonObj.ToObject<SpellCard>();
                    cards.Add(spellCard);
                }
                else
                {
                    MonsterCard monsterCard = jsonObj.ToObject<MonsterCard>();
                    cards.Add(monsterCard);
                }
            }



            String returnValue = packageService.CreatePackagesAndCards(cards);

            if (returnValue == "201")
            {
                rs.ResponseCode = 201;
                rs.ResponseMessage = "OK";
                rs.Content = rs.ResponseMessage;
            }
        }
    }
}
