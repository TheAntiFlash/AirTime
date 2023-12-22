namespace Model.DTOs;

public class PostDto
{
    public int? Id { get; set; } = null;
    public string Title { get; set; } = String.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string Body { get; set; } = String.Empty;
    public int AuthorId { get; set; }
    
    public int? CategoryId { get; set; } = null;
    public int SubCategoryId { get; set; }
    public string? ImageSrc { get; set; } = null;
    
    // for displaying Posts
    public string? AuthorName { get; set; } = null;
    public string? SubCategoryName { get; set; } = null;
    public DateTime? PostApprovalTime { get; set; } = null;


}