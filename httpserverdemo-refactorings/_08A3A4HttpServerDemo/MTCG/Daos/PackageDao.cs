using _08A3A4HttpServerDemo.MTCG.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class PackageDao : IDAO<Package>
    {
        private NpgsqlConnection connection;
        public PackageDao(NpgsqlConnection connection) { this.connection = connection; }

        public PackageDao() { }
        public Package Create(Package t)
        {
            string query = "INSERT INTO Package(packageid, cost) VALUES (@packageid, @cost)";
            Package returnPackage = null;

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    // Parameter hinzufügen und Werte festlegen
                    cmd.Parameters.AddWithValue("@packageid", t.Id);
                    cmd.Parameters.AddWithValue("@cost", t.PackageCost);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Überprüfen, ob das Einfügen erfolgreich war
                    if (rowsAffected > 0)
                    {
                        return t;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                // Fehlerbehandlung für Datenbankfehler
                Console.WriteLine("Datenbankfehler: " + ex.Message);
            }
            return returnPackage;
        }


        public void Delete(Package package)
        {
            string query = "DELETE FROM package WHERE packageid = @id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", package.Id);
                command.ExecuteNonQuery();
            }

        }


        public List<Package> getAll()
        {
            throw new NotImplementedException();
        }

        public Package Read(Package user)
        {
            throw new NotImplementedException();
        }

        public Package Update(Package t)
        {
            throw new NotImplementedException();
        }
        public Package GetOnePackage()
        {
            string query = "SELECT * FROM package LIMIT 1";

            Package package = null;

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        package = new Package(reader.GetString(0));
                    }
                }
            }

            return package;
        }

    }
}
