using DataAccess.Repositories;
using Model.Models;

namespace Controller.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _repo.GetAllUsers();
    }
}