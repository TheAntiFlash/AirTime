using System.ComponentModel.DataAnnotations;

namespace Model.DTOs;

public class AuthDto
{
    [Required(ErrorMessage = "Username is required.")]
    public string UsernameOrEmail { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}