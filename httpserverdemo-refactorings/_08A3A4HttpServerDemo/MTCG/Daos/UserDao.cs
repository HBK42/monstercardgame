using _08A3A4HttpServerDemo.MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class UserDao : IDAO<User>
    {

        private NpgsqlConnection connection;
        public UserDao(NpgsqlConnection connection) { this.connection = connection; }

        public UserDao() { }
        public virtual User Create(User t)

        {
            string query = "INSERT INTO users (userid, username, password, coins, token) VALUES (@userid,@username, @password, @coins, @token)";

            User returnUser = null;

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    // Parameter hinzufügen und Werte festlegen
                    cmd.Parameters.AddWithValue("username", t.Username);
                    cmd.Parameters.AddWithValue("password", t.Password);
                    cmd.Parameters.AddWithValue("userid", Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("coins", t.Coins);
                    cmd.Parameters.AddWithValue("@token", t.Username + "-mtcgToken");

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        returnUser = t;
                }
            }
            catch (NpgsqlException ex)
            {
                // Hier wird der Fehler abgefangen, wenn ein Datenbankfehler auftritt
                if (ex.Message.Contains("duplicate key value violates unique constraint"))
                {
                    // Hier können Sie entsprechend reagieren, wenn ein Duplikatfehler auftritt
                    Console.WriteLine("Benutzername bereits vorhanden. Bitte wählen Sie einen anderen Benutzernamen.");
                }
                else
                {
                    // Hier können Sie andere Datenbankfehler behandeln oder protokollieren
                    Console.WriteLine("Datenbankfehler: " + ex.Message);
                }
            }



            return returnUser;

        }

        public void Delete(User t)
        {
            throw new NotImplementedException();
        }

        public List<User> getAll()
        {
            throw new NotImplementedException();
        }

        public virtual User Read(User user)
        {
            string query = "SELECT * FROM users WHERE username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        string fetchedPassword = reader.GetString(2);
                        string fetchedUserid = reader.GetString(0);
                        string fetchedUsername = reader.GetString(1);
                        int fetchedCoins = reader.GetInt32(3);
                        string token = reader.GetString(4);


                        return new User(fetchedUsername, fetchedPassword, fetchedUserid, fetchedCoins, token);
                    }
                }
            }
            return null;
        }


        public User Update(User t)
        {
            throw new NotImplementedException();
        }
        public virtual User GetById(string id)
        {
            string query = "SELECT * FROM users WHERE userid = @userId";
            User user = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", id);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        user = new User(reader.GetString(1), reader.GetString(2), reader.GetString(0), reader.GetInt32(3), reader.GetString(4));
                    }
                    reader.Close();
                }
            }
            return user;
        }

        public virtual User GetByToken(string token)
        {
            string query = "SELECT * FROM users WHERE token = @token";
            string connString = "Host=localhost;Port=5432;Username=postgres;Password=hbk42;Database=postgres";
            User user = null;

            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();


                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new User(reader.GetString(1), reader.GetString(2), reader.GetString(0), reader.GetInt32(3), reader.GetString(4));
                        }

                        reader.Close();
                    }
                }
            }
            return user;
        }

        public void UpdateUserCoins(string uid, int updatedCoins)
        {
            string query = "UPDATE users SET coins = @updatedCoins WHERE userid = @userId";
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@updatedCoins", updatedCoins);
                cmd.Parameters.AddWithValue("@userId", uid);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
