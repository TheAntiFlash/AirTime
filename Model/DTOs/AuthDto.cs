namespace Model.DTOs;

public class AuthDto
{
    public string UserId { get; set; } = String.Empty;
    
    public required string Username { get; set; } = String.Empty;
    public required string Password { get; set; } = String.Empty;
}