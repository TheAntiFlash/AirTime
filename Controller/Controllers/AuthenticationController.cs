using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.DTOs.Response;

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
        Response<bool> response = await _authService.RegisterUser(req);
        if (response is Response<bool>.Success)
        {
            return Accepted();
        }
        if (response is Response<bool>.Failure failure)
        {
            return BadRequest(failure.E?.Message);
        }
        return StatusCode(500);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(AuthDto req)
    {
        Response<string> res = await _authService.LoginUser(req);

        if (res is Response<string>.Failure failure)
        {
            return BadRequest(failure.E!.Message);
        }

        string token = (res as Response<string>.Success)!.Data;
        return Ok(token);
    }
}
    