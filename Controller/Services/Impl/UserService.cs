using DataAccess.Repositories;
using Model.DTOs;
using Model.DTOs.Response;
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

    public async Task<Response<EditProfileDto>> GetUserForUpdate(int id)
    {
        return await _repo.GetUserEditableData(id);
    }

    public async Task<Response<bool>> UpdateUserData(EditProfileDto data)
    {
        string currentPassword;
        try
        {
            currentPassword = await _repo.GetPasswordHash(data.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<bool>.Failure(e);
        }

        if (data is { OldPassword: not null, NewPassword: not null })
        {
            if (!BCrypt.Net.BCrypt.Verify(data.OldPassword, currentPassword))
            {
                return new Response<bool>.Failure(new Exception("Password Incorrect"));
            }

            data.NewPassword
                = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
        }
        else
        {
            data.NewPassword = null;
        }


        var success = await _repo.UpdateUserData(data);

        if (success)
        {
            return new Response<bool>.Success(success);
        }

        return new Response<bool>.Failure(new Exception("Something went Wrong updating data"));

    }
}