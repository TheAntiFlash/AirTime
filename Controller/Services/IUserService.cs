using Model.Models;

namespace Controller.Services;

public interface IUserService
{
    public Task<List<User>> GetAllUsers();
}