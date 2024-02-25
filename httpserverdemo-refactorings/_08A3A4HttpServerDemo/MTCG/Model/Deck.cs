using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Model
{
    public class Deck
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("deckId")]
        public string DeckId { get; set; }

     

        public Deck(string deckId, string userId)
        {
            DeckId = deckId;
            UserId = userId;
        }
    }
}
