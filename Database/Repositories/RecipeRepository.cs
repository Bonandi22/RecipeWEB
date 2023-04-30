using Microsoft.Data.SqlClient;
using Database;
using Common.Models;

namespace Database.Repositories
{
    public class RecipeRepository
    {
        public void CreateRecipe(Recipe recipe)
        {
            string connectionString = Database.DatabaseHelper.MyConnection();
            SqlConnection connection = new(connectionString);
            try
            {
                string queryInsert = "INSERT INTO Recipe (Name, Description, PrepationMethod, PreparationTime)" +
                    " VALUES (@Name, @Description, @PrepationMethod, @PreparationTime)";

                SqlCommand command = new(queryInsert, connection);
                command.Parameters.AddWithValue("@Name", recipe.Name);
                command.Parameters.AddWithValue("@Description", recipe.Description);
                command.Parameters.AddWithValue("@PrepationMethod", recipe.PrepationMethod);
                command.Parameters.AddWithValue("@PreparationTime", recipe.PreparationTime);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

        }
        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipes = new();
            string connectionString = Database.DatabaseHelper.MyConnection();
            SqlConnection connection = new(connectionString);

            connection.Open();
            string querySelect = "SELECT * FROM Recipe";
            SqlCommand command = new(querySelect, connection);
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    recipes.Add(new Recipe
                    {
                        Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                        Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : "",
                        Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : "",
                        DificultId = reader["DificultId"] != DBNull.Value ? (int)reader["DificultId"] : 0,
                        CategaryId = reader["CategaryId"] != DBNull.Value ? (int)reader["CategaryId"] : 0,
                        PrepationMethod = reader["PrepationMethod"] != DBNull.Value ? (string)reader["PrepationMethod"] : "",
                        PreparationTime = reader["PreparationTime"] != DBNull.Value ? (string)reader["PreparationTime"] : ""
                    });
                }

                return recipes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return recipes;
            }
            finally
            {
                connection.Close();
            }
        }
        public Recipe GetRecipeById(int id)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM Recipe WHERE Id = @Id";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Recipe recipe = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        PrepationMethod = reader.GetString(reader.GetOrdinal("PrepationMethod")),
                        PreparationTime = reader.GetString(reader.GetOrdinal("PreparationTime"))
                    };
                    return recipe;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return null;
        }
        public bool UpdateRecipe(Recipe recipe)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Recipe SET Name = @Name, Description = @Description, PrepationMethod = @PrepationMethod, PreparationTime = @PreparationTime WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", recipe.Id);
            cmd.Parameters.AddWithValue("@Name", recipe.Name);
            cmd.Parameters.AddWithValue("@Description", recipe.Description);
            cmd.Parameters.AddWithValue("@PrepationMethod", recipe.PrepationMethod);
            cmd.Parameters.AddWithValue("@PreparationTime", recipe.PreparationTime);

            try
            {
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception )
            {             
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public void Delete(int id)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);

            try
            {

                var command = new SqlCommand("DELETE FROM Recipe WHERE Id = @Id", connection);
                connection.Open();
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { connection.Close(); }
        }
        public void CreateIngredient(Ingredient ingredient)
        {
            string connectionString = DatabaseHelper.MyConnection();
            SqlConnection connection = new(connectionString);
            try
            {
                string queryInsert = "INSERT INTO Ingredient (Title, Description)" +
                    " VALUES (@Title, @Description)";

                SqlCommand command = new(queryInsert, connection);
                command.Parameters.AddWithValue("@Name", ingredient.Title);
                command.Parameters.AddWithValue("@Description", ingredient.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

        }



    }
}
