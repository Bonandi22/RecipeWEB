using Common.Models;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace Database.Repositories
{
    public class UserRepository
    {
        public void CreateUser(User user)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();
                // Checks if the email is already registered in the User table
                string queryEmail = "SELECT COUNT(*) FROM [User] WHERE Email=@Email";
                SqlCommand commandEmail = new(queryEmail, connection);
                commandEmail.Parameters.AddWithValue("@Email", user.Email);
                int count = (int)commandEmail.ExecuteScalar();

                // If the email is already registered, check if the password is correct
                if (count > 0)
                {
                    throw new ArgumentException("The email is already registered.");

                }
                else
                {
                    // Convert password to hash using SHA256
                    byte[] passwordBytes = Encoding.Default.GetBytes(user.Password);
                    HashAlgorithm sha = SHA256.Create();
                    byte[] hashBytes = sha.ComputeHash(passwordBytes);
                    StringBuilder passwordHash = new();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        passwordHash.Append(hashBytes[i].ToString("X2"));
                    }                    
                    string queryInsert = "INSERT INTO [User] (Name, Email, hashedpassword) VALUES (@Name, @Email, @hashedpassword)";
                    SqlCommand command = new(queryInsert, connection);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@hashedpassword", passwordHash.ToString());
                
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public bool Login(LoginModel model)
        {
            bool validar = false;
            string connectionString = DatabaseHelper.MyConnection();//passing the connection with the SQLSERVER
            SqlConnection connection = new(connectionString);

            try
            {
                connection.Open();
                // Convert password to hash using SHA256
                byte[] passwordBytes = Encoding.Default.GetBytes(model.Password);
                HashAlgorithm sha = SHA256.Create();
                byte[] hashBytes = sha.ComputeHash(passwordBytes);
                StringBuilder passwordHash = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    passwordHash.Append(hashBytes[i].ToString("X2"));
                }

                // Verifies that the email and hashed password match a valid user
                string queryLogin = "SELECT COUNT(*) FROM [User] WHERE Email=@Email and hashedpassword=@hashedpassword";
                SqlCommand commandLogin = new(queryLogin, connection);
                commandLogin.Parameters.AddWithValue("@Email", model.Email);
                commandLogin.Parameters.AddWithValue("@hashedpassword", passwordHash.ToString());
                int countLogin = (int)commandLogin.ExecuteScalar();
                if (countLogin > 0)
                {
                    validar = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return validar;
        }
        public bool EmailExists(User user)
        {
            string connectionString = Database.DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();
                string queryEmail = "SELECT COUNT(*) FROM [User] WHERE Email=@Email";
                SqlCommand commandEmail = new(queryEmail, connection);
                commandEmail.Parameters.AddWithValue("@Email", user.Email);
                int count = (int)commandEmail.ExecuteScalar();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally { connection.Close(); }
        }
        public void UptadePass(string email, string newPassword)
        {
            string connectionString = Database.DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);

            try
            {
                string query = "UPDATE [User] SET hashedpassword = @newPassword WHERE Email = @Email";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                byte[] passwordBytes = Encoding.Default.GetBytes(newPassword);
                HashAlgorithm sha = SHA256.Create();
                byte[] hashBytes = sha.ComputeHash(passwordBytes);
                StringBuilder passwordHash = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    passwordHash.Append(hashBytes[i].ToString("X2"));
                }
                command.Parameters.AddWithValue("@newPassword", passwordHash.ToString());

                connection.Open();
                command.ExecuteNonQuery();

            }
            finally { connection.Close(); }
        }
        public string GetNameByEmail(string email)
        {
            string connectionString = Database.DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string name = null;
            string query = "SELECT Name FROM [User] WHERE Email = @Email";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                name = reader.GetString(reader.GetOrdinal("Name"));
            }
            reader.Close();

            return name;
        }
        public string GetADMByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string adm = null;
            string query = "SELECT IsAdmin FROM [User] WHERE Email = @Email";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int isAdminOrdinal = reader.GetOrdinal("IsAdmin");
                if (!reader.IsDBNull(isAdminOrdinal))
                {
                    string isAdminValue = reader.GetString(isAdminOrdinal);
                    if (isAdminValue == "YES")
                    {
                        adm = "YES";
                    }
                    else
                    {
                        adm = "not";
                    }
                }
                else
                {
                    adm = "not";
                }
            }
            reader.Close();

            return adm;
        }
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [User]", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2)
                    };

                    users.Add(user);
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return users;
        }
        public User GetUserById(int id)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM [User] WHERE Id = @Id";
            try
            {                
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                    User user = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))
                    };
                    return user;
                    }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return null;
        }
        public bool UpdateUser(User user)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE [User] SET Name = @Name, Email = @Email WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);

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
                
                var command = new SqlCommand("DELETE FROM [User] WHERE Id = @Id", connection);
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
        public User GetUserByIdAdm(string id)
        {
            string connectionString = DatabaseHelper.MyConnection();//conexao com o SQLSERVER
            SqlConnection connection = new(connectionString);
            string query = "SELECT * FROM [User] WHERE Id = @Id";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    User user = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))
                    };
                    return user;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }

            return null;
        }
    }
}
