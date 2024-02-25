using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model
{
    public class Trading
    {
        [JsonProperty("Id")]
        public string Id { get; private set; }

        [JsonProperty("CardToTrade")]
        public string CardToTrade { get; private set; }

        [JsonProperty("Type")]
        public string Type { get; private set; }

        [JsonProperty("MinimumDamage")]
        public int MinimumDamage { get; private set; }

        public Trading()
        {
            // Default constructor
        }

        public Trading(string id)
        {
            Id = id;
        }

        public Trading(string id, string cardToTrade, string type, int minimumDamage)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type = type;
            MinimumDamage = minimumDamage;
        }

        public override string ToString()
        {
            return $"Trading{{id='{Id}', cardToTrade='{CardToTrade}', type='{Type}', minimumDamage={MinimumDamage}}}";
        }
    }
}
