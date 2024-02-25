using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class DeckDao : IDAO<Deck>
    {
        private NpgsqlConnection connection;
        public DeckDao(NpgsqlConnection connection) { this.connection = connection; }

        public DeckDao()
        {
            
        }

        public virtual string GetDeckIdByUserId(string userId)
        {
            string query = "SELECT deckid FROM deck WHERE userid = @userId";

            string id = "";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id =  reader.GetString(0);
                    }
                    reader.Close();
                }
            }


            return id;
        }

        public Deck Read(Deck user)
        {
            throw new NotImplementedException();
        }

        Deck IDAO<Deck>.Update(Deck t)
        {
            throw new NotImplementedException();
        }

        public void Delete(Deck t)
        {
            throw new NotImplementedException();
        }

        public List<Deck> getAll()
        {
            throw new NotImplementedException();
        }

        public Deck Create(Deck deck)
        {
            Deck returnDeck = null;
            string query = "INSERT INTO deck (deckid, userid) VALUES (@deckId, @userId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@deckId", deck.DeckId);
                command.Parameters.AddWithValue("@userId", deck.UserId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    returnDeck = deck;

                return returnDeck;
            }
        }
    }
}
