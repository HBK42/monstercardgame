using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.Services
{
    internal class DatabaseService
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=hbk42;Database=postgres";
        private NpgsqlConnection connection;    
       

        public void OpenConnection()
        {
            connection = new NpgsqlConnection(connString);
            connection.Open();
        }

        public NpgsqlConnection GetConnection => connection;

        public void CloseConnection() => connection.Close();

        


    }
}
