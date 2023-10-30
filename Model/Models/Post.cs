namespace Model.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string SubCategoryName { get; set; } = String.Empty;
    public string Body { get; set; } = String.Empty;
    public int AuthorId { get; set; }
    public string Status { get; set; } = "Pending-Approval";
    public bool Approved { get; set; } = false;
    public int ApprovedBy { get; set; }
    public DateTime ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}