using System.Data;
using System.Diagnostics;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Abstractions;
using Model.Models;

namespace DataAccess.RepositoriesImpl;

public class AdminRepository : IAdminRepository
{
    public async Task<int> RegisterAdmin(Admin admin)
    {
        SqlConnection con = DbContext.GetConnection();
        
        con.Open();

        SqlCommand cmd = new SqlCommand("SP_RegisterAdmin", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@AdminUserName", admin.AdminUserName);
        cmd.Parameters.AddWithValue("@Password", admin.PasswordHash);
        
        SqlParameter returnParameter = new SqlParameter
        {
            ParameterName = "@ReturnCode",
            Direction = ParameterDirection.ReturnValue
        };
        cmd.Parameters.Add(returnParameter);
        
        await cmd.ExecuteNonQueryAsync();

        var statusCode = (int)returnParameter.Value;
        
        con.Close();
        return statusCode;
    }

    public Admin? GetAdmin(string username)
    {
        SqlConnection con = DbContext.GetConnection();
        
        con.Open();
        string query = "Select * FROM Admin WHERE AdminUserName= \'" + username + "\'";
        SqlCommand cmd = new SqlCommand(query, con);

        Admin? admin = null;
        try
        {
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            admin = new Admin
            {
                AdminId = Convert.ToString(reader["AdminID"]),
                AdminUserName = Convert.ToString(reader["AdminUserName"]),
                PasswordHash = Convert.ToString(reader["Password"])
            };
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (Exception e)
        {
            Debug.Print("Exception occured when logging in Admin: " + e);
        }
        con.Close();
        return admin;
    }
}