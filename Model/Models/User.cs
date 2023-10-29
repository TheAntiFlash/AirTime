namespace Model.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? MobileNumber { get; set; }
    public DateOnly? DoB { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool? Gender { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
}


/***
username varchar[256] 
   email varchar[256]
   first_name varchar[50]
   last_name varchar[50]
   password_hash varchar[80]
   role_id int [ref: - role.id] 
   mobile_number int 
   date_of_birth date
   last_login timestamp
   gender bit
   description nvarchar[max] [null, note: "desc could be used for profile info"]
   created_at timestamp
*/