using Controller.Services;
using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await  _userService.GetAllUsers();
        Console.WriteLine(users.ToString());
        return Ok(users);
    }
}