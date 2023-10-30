namespace Model.DTOs;

public class PostDto
{
    public string Title { get; set; } = String.Empty;
    public string Body { get; set; } = String.Empty;
    public int AuthorId { get; set; }
}