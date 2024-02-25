using _08A3A4HttpServerDemo.MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class TradingDao : IDAO<Trading>
    {
        private NpgsqlConnection connection;
        public TradingDao(NpgsqlConnection connection) { this.connection = connection; }

        public Trading Create(Trading trading)
        {

            Trading returnTrading = null;
            string query = "INSERT INTO trading (id, cardtotrade, type, minimumdamage) VALUES (@Id, @CardToTrade, @Type, @MinimumDamage)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", trading.Id);
                cmd.Parameters.AddWithValue("@CardToTrade", trading.CardToTrade);
                cmd.Parameters.AddWithValue("@Type", trading.Type);
                cmd.Parameters.AddWithValue("@MinimumDamage", trading.MinimumDamage);

                // Execute the query
                int rowsAffected = cmd.ExecuteNonQuery();

                // Check if any rows were affected (insert successful)
                if (rowsAffected > 0)
                    returnTrading = trading;
            }

            return trading;

        }

        public void Delete(Trading t)
        {


            string query = "DELETE FROM trading WHERE id=@Id";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", t.Id);

                // Execute the query
                cmd.ExecuteNonQuery();
            }

        }

        public List<Trading> getAll()
        {
            List<Trading> trades = new List<Trading>();

            string query = "SELECT * FROM trading";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trade = new Trading(
                            reader["id"].ToString(),
                            reader["cardtotrade"].ToString(),
                            reader["type"].ToString(),
                            Convert.ToInt32(reader["minimumdamage"])
                        );

                        trades.Add(trade);
                    }

                    reader.Close();
                }

            }

            return trades;
        }

        public Trading Read(Trading trading)
        {

            Trading returnTrading = null;
            string query = "SELECT * FROM trading WHERE id = @Id";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", trading.Id);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnTrading = new Trading(
                           reader["id"].ToString(),
                           reader["cardtotrade"].ToString(),
                           reader["type"].ToString(),
                           Convert.ToInt32(reader["minimumdamage"])
                       );
                    }

                    reader.Close();

                }
            }


            return returnTrading;
        }

        public Trading Update(Trading t)
        {
            throw new NotImplementedException();
        }
    }
}
