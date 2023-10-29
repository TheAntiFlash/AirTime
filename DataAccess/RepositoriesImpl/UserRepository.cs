using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Abstractions;
using Model.Models;

namespace DataAccess.RepositoriesImpl;

public class UserRepository : IUserRepository
{
    public async Task<int> AddUser(User user)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();

        SqlCommand cmd = new SqlCommand("usp_UserInsert", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", user.Username);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@first_name", user.FirstName);
        cmd.Parameters.AddWithValue("@last_name", user.LastName);
        cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
        
        
        await cmd.ExecuteNonQueryAsync();

                
        await con.CloseAsync();
        return 1;
    }

    public async Task<User?> GetUser(string username)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_UserSelectByUsernameOrEmail", con);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            var email = new MailAddress(username);
            cmd.Parameters.AddWithValue("@email", username);
        }
        catch (FormatException)
        {
            cmd.Parameters.AddWithValue("@username", username);
        }
        Console.WriteLine("parawhat: " + cmd.Parameters[0].ToString());
        User? user = null;
        try
        {
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            reader.Read();
            user = new User
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = Convert.ToString(reader["username"])!,
                PasswordHash = Convert.ToString(reader["password_hash"])!,
                Email = Convert.ToString(reader["email"])!,
                FirstName = Convert.ToString(reader["first_name"])!,
                LastName = Convert.ToString(reader["last_name"])!,
                Role = Convert.ToString(reader["role_name"])!,
                CreatedAt = Convert.ToDateTime(reader["created_at"])
            };
        }
        catch (InvalidOperationException e)
        {
            return null;
        }
        catch (Exception e)
        {
            Debug.Print("Exception occured when logging in User: " + e);
        }
        await con.CloseAsync();
        return user;
    }

    public async Task UpdateLastLogin(int id)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_UserUpdateLastLogin", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@user_id", id);
        
        await cmd.ExecuteNonQueryAsync();
        
        await con.CloseAsync();

    }
}