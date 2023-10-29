using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Model.DTOs;
using Model.Models;
using Model.DTOs.Response;

namespace Controller.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _configuration;


    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _configuration = config;
        _userRepo = userRepo;
    }
    public async Task<Response<bool>> RegisterUser(UserRegDto userCred)
    {
        User user = new User();
        string passwordHash
            = BCrypt.Net.BCrypt.HashPassword(userCred.Password);
        
        user.Username = userCred.Username;
        user.Email = userCred.Email;
        user.FirstName = userCred.FirstName;
        user.LastName = userCred.LastName;
        user.PasswordHash = passwordHash;
        
        try
        {
            await _userRepo.AddUser(user);
        }
        catch (SqlException e)
        {
            foreach (SqlError error in e.Errors)
            {
                Console.WriteLine(error.Message);
            }

            return new Response<bool>.Failure(e);
        }

        return new Response<bool>.Success(true);
    }

    public async Task<Response<string>> LoginUser(AuthDto auth)
    {
        User? user =  await _userRepo.GetUser(auth.UsernameOrEmail);
        
        if (user == null)
        {
            return new Response<string>.Failure(new Exception("User Not Found"));
        }

        if (!BCrypt.Net.BCrypt.Verify(auth.Password, user.PasswordHash))
        {
            return new Response<string>.Failure(new Exception("Password Incorrect"));
        }

        await _userRepo.UpdateLastLogin(user.Id);

        return new Response<string>.Success(CreateToken(user));
    }
    
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Email, user.Email),
        };
        if (user.Role != "Viewer")
        {
            claims.Add(new (ClaimTypes.Role, user.Role));
        }

        var key = 
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JWT:Key").Value!
                )
            );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            issuer: "airtime.pk",
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}