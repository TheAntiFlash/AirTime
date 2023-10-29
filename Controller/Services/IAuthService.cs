using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace Controller.Services;

public interface IAuthService
{
    Task<Response<bool>> RegisterUser(UserRegDto userCred);

    Task<Response<string>> LoginUser(AuthDto auth);
}