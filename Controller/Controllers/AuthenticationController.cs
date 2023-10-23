
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.DTOs;
using Model.Models;

namespace Controller.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{  
    private readonly IConfiguration _configuration;
    private readonly IAdminRepository _adminRepo;
    public AuthenticationController(IConfiguration configuration, IAdminRepository adminRepo)
    {
        _configuration = configuration;
        _adminRepo = adminRepo;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(AuthDto req)
    {
        Admin admin = new Admin();
        string passwordHash
            = BCrypt.Net.BCrypt.HashPassword(req.Password);
        admin.AdminId = req.UserId;
        admin.AdminUserName = req.Username;
        admin.PasswordHash = passwordHash;

        int statusCode = await _adminRepo.RegisterAdmin(admin);
        return Ok(statusCode);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(AuthDto req)
    {
        Admin? admin = _adminRepo.GetAdmin(req.Username);

        if (admin == null)
        {
            return BadRequest("User Not Found");
        }

        if (!BCrypt.Net.BCrypt.Verify(req.Password, admin.PasswordHash))
        {
            return BadRequest("Password Incorrect");
        }

        return Ok(admin);
    }
    
}