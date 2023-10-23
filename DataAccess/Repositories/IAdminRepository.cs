using Model.Models;

namespace DataAccess.Repositories;

public interface IAdminRepository
{
    Task<int>  RegisterAdmin(Admin admin);
    Admin? GetAdmin(string username);
}