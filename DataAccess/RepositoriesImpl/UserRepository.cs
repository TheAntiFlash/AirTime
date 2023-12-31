using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Abstractions;
using Model.DTOs;
using Model.DTOs.Response;
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

    public async Task<List<User>> GetAllUsers()
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_UserSelectAll", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<User> users = new List<User>();
        while (await reader.ReadAsync())
        {
            users.Add(
                    new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Username = Convert.ToString(reader["username"])!,
                        PasswordHash = Convert.ToString(reader["password_hash"])!,
                        Email = Convert.ToString(reader["email"])!,
                        FirstName = Convert.ToString(reader["first_name"])!,
                        LastName = Convert.ToString(reader["last_name"])!,
                        Role = Convert.ToString(reader["role_name"])!,
                        CreatedAt = Convert.ToDateTime(reader["created_at"])
                    }
                );
        }
        await con.CloseAsync();
        return users;
    }

    public async Task<Response<EditProfileDto>> GetUserEditableData(int id)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_UserSelectById", con);
        cmd.Parameters.AddWithValue("@user_id", id);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        Response<EditProfileDto> response;
        try
        {
            if(await reader.ReadAsync())
            {
                response = new Response<EditProfileDto>.Success(new EditProfileDto()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Email = Convert.ToString(reader["email"])!,
                    FirstName = Convert.ToString(reader["first_name"])!,
                    LastName = Convert.ToString(reader["last_name"])!,
                    MobileNumber = Convert.ToString(reader["mobile_number"]),
                    Description = Convert.ToString(reader["description"]),
                    DoB = DateOnly.FromDateTime(Convert.ToDateTime(reader["date_of_birth"] is DBNull? DateTime.UnixEpoch: reader["date_of_birth"]))
                });
            }
            else
            {
                throw new Exception("User Not Found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response = new Response<EditProfileDto>.Failure(e);
            
        }
        
        await con.CloseAsync();
        return response;
    }

    public async Task<bool> UpdateUserData(EditProfileDto data)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();

        SqlCommand cmd = new SqlCommand("usp_UserUpdate", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", data.Id);
        cmd.Parameters.AddWithValue("@new_email", data.Email);
        cmd.Parameters.AddWithValue("@first_name", data.FirstName);
        cmd.Parameters.AddWithValue("@last_name", data.LastName);
        cmd.Parameters.AddWithValue("@new_password", data.NewPassword);
        cmd.Parameters.AddWithValue("@mobile_number", data.MobileNumber);
        cmd.Parameters.AddWithValue("@date_of_birth", data.DoB);
        cmd.Parameters.AddWithValue("@description", data.Description);

        try
        {
            await cmd.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await con.CloseAsync();
            return false;
        }

    }

    public async Task<string> GetPasswordHash(int userId)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_UserSelectById", con);
        cmd.Parameters.AddWithValue("@user_id", userId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        
            if(await reader.ReadAsync())
            {
                return Convert.ToString(reader["password_hash"])!;
               
            }
            throw new Exception("User Not Found");

    } 
    
}