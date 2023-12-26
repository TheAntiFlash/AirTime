using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace Controller.Controllers;

[Route("api/")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController( IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserRegDto req)
    {
        var response = await _authService.RegisterUser(req);
        return response switch
        {
            Response<bool>.Success => Accepted(),
            Response<bool>.Failure failure => BadRequest(failure.E?.Message),
            _ => StatusCode(500)
        };
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(AuthDto req)
    {
        var res = await _authService.LoginUser(req);

        if (res is Response<object>.Failure failure)
        {
            return BadRequest(failure.E!.Message);
        }

        var response = (res as Response<object>.Success)!.Data as UserSession;
        return Ok(response);
    }
}
    