using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _08A3A4HttpServerDemo.DTO;
using _08A3A4HttpServerDemo.MTCG.Daos;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using System.Runtime.CompilerServices;

namespace _08A3A4HttpServerDemo
{
    public class Battle
    {
        private static ConcurrentQueue<UserDeckDTO> concurrentQueue = new ConcurrentQueue<UserDeckDTO>();
        public static Tuple<UserDeckDTO, List<Round>> RegisterForBattle(UserDeckDTO userDTO)
        {
            concurrentQueue.Enqueue(userDTO);

            while (true)
            {
                if (concurrentQueue.Count >= 2)
                {
                    UserDeckDTO player1;
                    UserDeckDTO player2;

                    if (concurrentQueue.TryDequeue(out player1) && concurrentQueue.TryDequeue(out player2))
                    {
                        return Battles(player1, player2);

                    }

                }
            }
        }

        private static Tuple<UserDeckDTO, List<Round>> Battles(UserDeckDTO u1, UserDeckDTO u2)
        {
            bool hundredRounds = true;
            List<Round> roundProtocol = new List<Round>();

            for (int i = 0; i < 100; i++)
            {
                Shuffle(u1.Deck);
                Shuffle(u2.Deck);
                
                

                if (u1.Deck.Count == 0 || u2.Deck.Count == 0)
                {
                    hundredRounds = false;
                    break;
                }

                Card user1Card = u1.Deck[0];
                Card user2Card = u2.Deck[0];

                UserDeckDTO winner = null;
                Round round = new Round();

                if (user1Card.Damage > user2Card.Damage)
                {
                    winner = u1;
                    round.AddMessage("Player Card 1 has won");
                }
                else if (user2Card.Damage > user1Card.Damage)
                {

                    round.AddMessage("Player Card 2 has won");
                    winner = u2;
                }
                else
                {
                    round.AddMessage("Draw, no card has won");
                }

                string card1Log = $"{user1Card.Name} Damage: {user1Card.Damage}";
                string card2Log = $"{user2Card.Name} Damage: {user2Card.Damage}";
                round.AddMessage($"{u1.User.Username} {card1Log} vs. {u2.User.Username} {card2Log}");

                roundProtocol.Add(round);



            }

            Console.WriteLine(roundProtocol.Count);
            Console.WriteLine(u1.Deck.Count);
            Console.WriteLine(u2.Deck.Count);
            Console.WriteLine(hundredRounds);


            using (StreamWriter writer = new StreamWriter($"../../../logoutput/{Guid.NewGuid()}.txt"))
            {
                foreach (var round in roundProtocol)
                {
                    writer.WriteLine(round.ToString());
                }
            }

            if (hundredRounds)
            {
                return Tuple.Create<UserDeckDTO, List<Round>>(null, roundProtocol);
            }
            else
            {
                if (u1.Deck.Count > u2.Deck.Count)
                {
                    return Tuple.Create<UserDeckDTO, List<Round>>(u1, roundProtocol);
                }
                else if (u2.Deck.Count > u1.Deck.Count)
                {
                    return Tuple.Create<UserDeckDTO, List<Round>>(u1, roundProtocol);
                }
            }


            return null;
        }

        public static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}