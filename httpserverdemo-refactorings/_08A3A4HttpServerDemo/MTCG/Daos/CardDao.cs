using _08A3A4HttpServerDemo.MTCG.Model;
using _08A3A4HttpServerDemo.MTCG.Model.Card;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class CardDao : IDAO<Card>
    {
        private NpgsqlConnection connection;
        public CardDao(NpgsqlConnection connection) { this.connection = connection; }

        public CardDao() { }
        public Card Create(Card card)
        {
            string query = "INSERT INTO card (cardid, name, damage, typ, weakness, typeweakness, nameandtype, packageid) VALUES (@cardid, @name, @damage, @typ, @weakness, @typeweakness, @nameandtype, @packageid)";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@cardid", card.Id);
                    cmd.Parameters.AddWithValue("@name", card.Name);
                    cmd.Parameters.AddWithValue("@damage", card.Damage);
                    cmd.Parameters.AddWithValue("@typ", card.Type.ToString());
                    cmd.Parameters.AddWithValue("@weakness", card.Weakness);
                    cmd.Parameters.AddWithValue("@typeweakness", card.TypeWeakness.ToString());
                    cmd.Parameters.AddWithValue("@nameandtype", card.NameAndType);
                    cmd.Parameters.AddWithValue("@packageid", card.PackageId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    // Rückgabe, ob das Einfügen erfolgreich war
                    return rowsAffected > 0 ? card : null;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Datenbankfehler: " + ex.Message);
                return null;
            }

        }

        public void Delete(Card t)
        {
            throw new NotImplementedException();
        }

        public List<Card> getAll()
        {
            List<Card> cards = new List<Card>();
            string query = "SELECT * FROM card";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cards.Add(CreateCard(reader));
                    }
                }
            }
            return cards;
        }

        public Card Read(Card card)
        {
            throw new NotImplementedException();
        }

        public Card Read(String id)
        {
            string query = "SELECT * FROM card WHERE cardid = @CardId";

            Card c = null;

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@CardId", id);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c =  CreateCard(reader);
                    }

                    reader.Close();
                }
            }

            return c;
        }

        public Card Update(Card t)
        {
            throw new NotImplementedException();
        }
        private Card CreateCard(IDataRecord record)
        {
            Card c;
            string cardId = record.GetString(0);
            string name = record.GetString(1);
            int damage = record.GetInt32(2);
            Element type = Enum.Parse<Element>(record.GetString(3));
            string weakness = record.GetString(4);
            Element typeWeakness = Enum.Parse<Element>(record.GetString(5));
            string nameAndType = record.GetString(6);
            string packageId;
            string userID = "";

            if (!record.IsDBNull(7))
            {
                packageId = record.GetString(7);
            }
            else
            {
                packageId = null;
            }


            if (name.Contains("Spell"))
            {
                c = new SpellCard(type, name, damage, weakness, typeWeakness, cardId, nameAndType, packageId, userID);
            }
            else
            {
                c = new MonsterCard(type, name, damage, weakness, typeWeakness, cardId, nameAndType, packageId, userID);
            }
            return c;
        }
        public List<Card> GetCardsOfSpecificPackage(string packageId)
        {
            string query = "SELECT cardid, name, damage, typ, weakness, typeweakness, nameandtype, Card.packageid, userid FROM Card WHERE Card.packageid = @packageId";
            List<Card> cards = new List<Card>();



            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@packageId", packageId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cards.Add(CreateCard(reader));
                    }

                    reader.Close();
                }
            }


            return cards;
        }



        public bool UpdatePackageId(string packageId, string cardId)
        {
            string query = "UPDATE card SET packageid = @packageId WHERE cardid = @cardId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                if (packageId == null)
                    command.Parameters.AddWithValue("@packageId", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@packageId", packageId);
                command.Parameters.AddWithValue("@cardId", cardId);

                return command.ExecuteNonQuery() > 0;
            }

        }

        public bool UpdateUserId(string cardId, string userId)
        {
            string query = "UPDATE card SET userid = @userId WHERE cardid = @cardId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@cardId", cardId);

                return command.ExecuteNonQuery() > 0;
            }

        }

        public int UpdateDeckID(string cardId, string deckId, string userId)
        {
            string query = "UPDATE card SET deckid = @deckId WHERE cardid = @cardId AND (userid = @userId OR userid IS NULL)";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@deckId", deckId);
                command.Parameters.AddWithValue("@cardId", cardId);
                command.Parameters.AddWithValue("@userId", userId);

                return command.ExecuteNonQuery();
            }

        }

        public virtual int UpdateDeckIdByCardId(string cardId, string newDeckId)
        {
            string query = "UPDATE card SET deckid = @newDeckId WHERE cardid = @cardId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                if (string.IsNullOrEmpty(newDeckId))
                    command.Parameters.AddWithValue("@newDeckId", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@newDeckId", newDeckId);
                command.Parameters.AddWithValue("@cardId", cardId);

                return command.ExecuteNonQuery();
            }

        }


        public virtual List<Card> GetByUserID(string userID)
        {
            List<Card> cards = new List<Card>();
            string query = "SELECT * FROM card WHERE userid = @userID";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userID", userID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cards.Add(CreateCard(reader));
                    }

                    reader.Close();
                }
            }


            return cards;
        }

        public Card GetCardsByUserIdAndCheckIfLocked(string userId, string cardId)
        {
            string query = "SELECT * FROM card WHERE userid = @UserId AND cardid = @CardId AND deckid IS NULL";

            Card c = null;

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CardId", cardId);



                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        c = CreateCard(reader);
                    }

                    reader.Close();
                }


            }

            return c;
        }


        public virtual List<Card> GetByDeckId(string deckId)
        {
            List<Card> cards = new List<Card>();
            string query = "SELECT * FROM card WHERE deckid = @deckId";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@deckId", deckId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cards.Add(CreateCard(reader));
                    }

                    reader.Close();
                }
            }
            return cards;
        }



    }
}
