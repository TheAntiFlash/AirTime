using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.DTOs.Response;

namespace Controller.Controllers;

[Route("api/user/")]
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

    /**
     * <summary>
     * Get the data to show the user for his current profile. So user can decide what data to update.
     * </summary>
     * <param name="userId">
     * Id of the currently logged in user</param>
     */
    [HttpGet]
    [Route("update")]
    public async Task<IActionResult> GetUserForUpdate(int userId)
    {
        var response = await _userService.GetUserForUpdate(userId);
        return response switch
        {
            Response<EditProfileDto>.Success success => Ok(success.Data),
            Response<EditProfileDto>.Failure failure => failure.E.Message == "User Not Found"
                ? NotFound(failure.E.Message)
                : Problem("Something Went Wrong", statusCode: 500),
            _ => Problem("Something Went Wrong", statusCode: 500)
        };
    }

    /**
     * <summary>
     *Updates the user data that is sent.
     * </summary>
     * <param name="userData">
     * Any parameter other than id can be null. 
     * Only set values of the parameters you want to update.
     * Checks for old password to match, and email to be unique are set.
     * Front end should check if password and email are in correct format.</param>
     */
    [HttpPatch]
    [Route("update")]
    public async Task<IActionResult> UpdateUserProfile(EditProfileDto userData)
    {
        var response = await _userService.UpdateUserData(userData);
        return response switch
        {
            Response<bool>.Success success => Ok(success.Data),
            Response<bool>.Failure failure => Problem("Something Went Wrong: " + failure.E.Message, statusCode: 500),
            _ => Problem("Something Went Wrong", statusCode: 500)
        };
    }
    
}