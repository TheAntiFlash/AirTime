using System.Data;
using System.Data.SqlClient;
using DataAccess.Repositories;
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
        
        var statusCode = await cmd.ExecuteNonQueryAsync();
        con.Close();
        return statusCode;
    }

    public Admin? GetAdmin(string username)
    {
        SqlConnection con = DbContext.GetConnection();
        
        con.Open();

        SqlCommand cmd = new SqlCommand("Select * FROM Admin WHERE AdminUserName=" + username, con);
        SqlDataReader reader = cmd.ExecuteReader();
        con.Close();

        Admin? admin = null;
        while (reader.Read())
        {
            admin = new Admin();
            admin.AdminId = Convert.ToString(reader["AdminID"]);
            admin.AdminUserName = Convert.ToString(reader["AdminUserName"]);
            admin.PasswordHash = Convert.ToString(reader["Password"]);
            return admin;
        }
        return admin;
    }
}