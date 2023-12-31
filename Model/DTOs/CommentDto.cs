namespace Model.DTOs;

public class CommentDto
{
    
    public int? Id { get; set; } = null;
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string? Username { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; } = null;
    public int? Likes { get; set; } = null;
}