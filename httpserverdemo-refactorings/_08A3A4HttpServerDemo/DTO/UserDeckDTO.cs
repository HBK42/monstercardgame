using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.DTO
{
    public class UserDeckDTO
    {
        public User User { get; private set; }

        public List<Card> Deck { get; private set; } = new List<Card>();

        public UserDeckDTO(User user, List<Card> deck)
        {
            User = user;
            Deck = deck;
        }


    }
}
