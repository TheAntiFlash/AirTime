using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace Controller.Services;

public interface IUserService
{
    public Task<List<User>> GetAllUsers();

    public Task<Response<EditProfileDto>> GetUserForUpdate(int id);

    public Task<Response<bool>> UpdateUserData(EditProfileDto data);

}