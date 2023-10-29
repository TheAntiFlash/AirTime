using System.ComponentModel.DataAnnotations;
namespace Model.DTOs;

public class UserRegDto
{
    [Required(ErrorMessage ="Username is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;
}