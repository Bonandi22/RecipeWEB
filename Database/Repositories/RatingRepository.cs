using Common.Models;
using Microsoft.Data.SqlClient;

namespace Database.Repositories
{
    public class RatingRepository
    {
        public int GetUserIdByEmail(string email)
        {
            string connectionString = DatabaseHelper.MyConnection();
            SqlConnection connection = new(connectionString);
            int userId = 0;
            try
            {
                string query = "SELECT Id FROM [User] WHERE Email = @Email";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = reader.GetInt32(reader.GetOrdinal("Id"));
                }
                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
        public void CreateRating(int userId, string comment, string rating)
        {
            string connectionString = Database.DatabaseHelper.MyConnection();
            SqlConnection connection = new(connectionString);
            try
            {
                string queryInsert = "INSERT INTO Rating (UserId, Rating, Comment) VALUES (@UserId, @Rating, @Comment)";
                SqlCommand command = new(queryInsert, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Comment", comment);
                command.Parameters.AddWithValue("@Rating", rating);
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
        public List<Rating> GetAllRating()
        {
            List<Rating> ratings = new List<Rating>();
            string connectionString = DatabaseHelper.MyConnection();
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = ("SELECT [USER].Name, [User].Email, Rating.Comment, Rating.Id " +
                                "FROM [User] " +
                                "JOIN Rating ON CAST([USER].Id AS VARCHAR(MAX)) = Rating.UserId");

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Rating rating = new Rating();
                    rating.Id = reader.GetInt32(3);
                    rating.UserName = reader.GetString(0);
                    rating.UserEmail = reader.GetString(1);
                    rating.Comment = reader.GetString(2);
                    ratings.Add(rating);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return ratings;
        }
        public Rating GetRatingById(int id)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string query = ("SELECT [USER].Name, [User].Email, Rating.Comment, Rating.Id " +
                        "FROM [User] " +
                        "JOIN Rating ON CAST([USER].Id AS VARCHAR(MAX)) = Rating.UserId " +
                        "WHERE Rating.Id = @ratingId");

            //string query = "SELECT * FROM Rating WHERE Id = @Id";
            try
            {
                SqlCommand command = new (query, connection);
                command.Parameters.AddWithValue("@ratingId", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Rating rating = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserName = reader.GetString(reader.GetOrdinal("Name")),
                        UserEmail = reader.GetString(reader.GetOrdinal("Email")),
                        Comment = reader.GetString(reader.GetOrdinal("Comment")),
                    };
                    return rating;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return null;
        }
        public bool UpdateRating(Rating rating)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Recipe SET Comment = @Comment WHERE Id = @Id", connection);
            //cmd.Parameters.AddWithValue("@Id", rating.Id);
            //cmd.Parameters.AddWithValue("@Name", rating.UserName);
            //cmd.Parameters.AddWithValue("@Description", rating.UserEmail);
            cmd.Parameters.AddWithValue("@PrepationMethod", rating.Comment);

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
            catch (Exception)
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

                var command = new SqlCommand("DELETE FROM Rating WHERE Id = @Id", connection);
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




    }
}