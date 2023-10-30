using Model.Models;

namespace DataAccess.Repositories;

public interface IUserRepository
{
    public Task<int> AddUser(User user);
    public Task<User?> GetUser(string username);

    public Task UpdateLastLogin(int id);

    public Task<List<User>> GetAllUsers();
}