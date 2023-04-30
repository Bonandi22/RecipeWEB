using System.Collections.Generic;
using System;
using Microsoft.Data.SqlClient;

namespace Database
{
    public class DatabaseHelper
    {
        private static readonly string connectionString = @"Server=localhost\MSSQLSERVER02;Database=FoodRecipe;Trusted_Connection=True;TrustServerCertificate=True;";

        private readonly SqlConnection sqlConnection = new SqlConnection(connectionString);

        public static string MyConnection()
        {
            //esse metodo é usado para passar a conexao com o banco de dados.
            return connectionString;
        }
    }
}
