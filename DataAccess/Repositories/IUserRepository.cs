using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace DataAccess.Repositories;

public interface IUserRepository
{
    public Task<int> AddUser(User user);
    public Task<User?> GetUser(string username);

    public Task UpdateLastLogin(int id);

    public Task<List<User>> GetAllUsers();

    public Task<Response<EditProfileDto>> GetUserEditableData(int id);

    public Task<bool> UpdateUserData(EditProfileDto data);

    public Task<string> GetPasswordHash(int userId);

}