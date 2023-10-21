using System.ComponentModel.DataAnnotations;

namespace Model;

public class LoginModel
{
    [Required(ErrorMessage = "*Email is required")]
    public string? EmailId { get; set; }
    
    [Required(ErrorMessage = "*Password is required")]
    public string? Password { get; set; }
}