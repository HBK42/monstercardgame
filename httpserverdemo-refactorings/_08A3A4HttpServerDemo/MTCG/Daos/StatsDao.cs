using _08A3A4HttpServerDemo.MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class StatsDao : IDAO<Statistik>
    {
        private NpgsqlConnection connection;
        public StatsDao(NpgsqlConnection connection) { this.connection = connection; }

        public StatsDao() { }   
        public virtual Statistik Create(Statistik statistik)
        {

            Statistik returnStatistik = null;

            string query = "INSERT INTO statistik (statistikid, userid, name, wins, losses, elo, winloseratio, draw) VALUES (@statistikId, @userId, @name, @wins, @losses, @elo, @winLoseRatio, @draw)";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@statistikId", statistik.Id);
                command.Parameters.AddWithValue("@userId", statistik.UserId);
                command.Parameters.AddWithValue("@name", statistik.Name);
                command.Parameters.AddWithValue("@wins", statistik.Wins);
                command.Parameters.AddWithValue("@losses", statistik.Losses);
                command.Parameters.AddWithValue("@elo", statistik.Elo);
                command.Parameters.AddWithValue("@winLoseRatio", statistik.WinLoseRatio);
                command.Parameters.AddWithValue("@draw", statistik.Draw);

                if (command.ExecuteNonQuery() > 0)
                {
                    returnStatistik = statistik;
                }
            }
            return returnStatistik;
        }

        public void Delete(Statistik t)
        {
            throw new NotImplementedException();
        }

        public List<Statistik> getAll()
        {
            List<Statistik> stats = new List<Statistik>();
            string query = "SELECT * FROM statistik";



            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var stat = new Statistik(
                            reader["name"].ToString(),
                            Convert.ToInt32(reader["elo"]),
                            Convert.ToInt32(reader["wins"]),
                            Convert.ToInt32(reader["losses"]),
                            reader["userid"].ToString(),
                            reader["statistikid"].ToString(),
                            Convert.ToDouble(reader["winloseratio"]),
                            Convert.ToInt32(reader["draw"])
                        );

                        stats.Add(stat);
                    }

                    reader.Close();
                }
            }

            return stats;
        }


        public Statistik Read(Statistik user)
        {
            throw new NotImplementedException();
        }

        public Statistik Update(Statistik statistik)
        {
            string query = "UPDATE statistik SET wins = @wins, losses = @losses, winloseratio = @winLoseRatio, elo = @elo, draw = @draw WHERE statistikid = @statistikId";

            Statistik stat = null;

          

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@wins", statistik.Wins);
                command.Parameters.AddWithValue("@losses", statistik.Losses);
                command.Parameters.AddWithValue("@winLoseRatio", statistik.WinLoseRatio);
                command.Parameters.AddWithValue("@elo", statistik.Elo);
                command.Parameters.AddWithValue("@draw", statistik.Draw);
                command.Parameters.AddWithValue("@statistikId", statistik.Id);

                if (command.ExecuteNonQuery() > 0)
                    stat = statistik;
            }

            return stat;
        }

        public virtual Statistik GetByUserId(string userId)
        {
            string query = "SELECT * FROM statistik WHERE userid = @userId";


            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Statistik(
                            reader["name"].ToString(),
                            Convert.ToInt32(reader["elo"]),
                            Convert.ToInt32(reader["wins"]),
                            Convert.ToInt32(reader["losses"]),
                            reader["userid"].ToString(),
                            reader["statistikid"].ToString(),
                            Convert.ToDouble(reader["winloseratio"]),
                            Convert.ToInt32(reader["draw"])
                        );
                    }
                }
            }


            return null;
        }


    }
}
