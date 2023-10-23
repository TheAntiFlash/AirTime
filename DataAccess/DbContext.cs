using System.Data.SqlClient;

namespace DataAccess;

public class DbContext
{
    private const string AZURE_SQL_CONNECTIONSTRING =
        "Server=tcp:airtime-au-pk.database.windows.net,1433;Initial Catalog=airtime;Persist Security Info=False;User ID=airtime-admin;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";
    public static SqlConnection GetConnection()
    {
        SqlConnection con = new SqlConnection(AZURE_SQL_CONNECTIONSTRING);
        return con;
    }
    
}