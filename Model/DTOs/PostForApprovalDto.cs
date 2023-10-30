namespace Model.DTOs;

public class PostForApprovalDto
{
    public int Postid { get; set; }
    public string PostBody { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
}