namespace Model.DTOs;

public class EditProfileDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? MobileNumber { get; set; } = null;
    public string? Email { get; set; } = null;
    public DateOnly? DoB { get; set; }
    public string? Description { get; set; } = null;
    public string? OldPassword { get; set; } = null;
    public string? NewPassword { get; set; } = null;
    
}