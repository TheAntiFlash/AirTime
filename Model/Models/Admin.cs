namespace Model.Models;

public class Admin
{
    public string AdminId { get; set; } = string.Empty;
    public string AdminUserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}