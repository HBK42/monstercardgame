using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model
{
    public class User
    {
        [JsonConstructor]
        public User(string username, string password) {
            this.Username = username;
            this.Password = password;
        }   
        public User(string username, string password, string userid, int coins, string token)
        {
            this.Username = username;
            this.Password = password;
            this.UserId = userid;
            this.Coins = coins;
            this.Token = token;
        }

        [JsonProperty("Coins")]
        public int Coins { get; private set; } = 20;

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

    }
}
