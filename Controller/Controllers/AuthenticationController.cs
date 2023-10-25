
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

        var token = CreateToken(admin);
        return Ok(token);
    }


    private string CreateToken(Admin admin)
    {
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.Name, admin.AdminUserName)
        };

        var key = 
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JWT:Key").Value!
                    )
                );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}