using Model.DTOs;
using Model.DTOs.Response;

namespace Controller.Services;

public interface IPostService
{
    public Task AddPost(PostDto post);

    public Task<List<PostForApprovalDto>> GetPostsForApproval();

    public Task ChangePostStatus(int postId, bool approved, int approvedById);

    public Task<int> GetTotalNumberOfPosts();

    public Task<List<PostDto>> GetAllPostsForUser(int userId, int offset, int pageSize);

    public Task<Response<PostDto>> GetPostById(int postId);

    public Task<Response<List<CommentDto>>> GetCommentsForPost(int postId);
    public Task<Response<bool>> AddComment(CommentDto data);

    public Task<Response<bool>> AddCommentLike(CommentLikeDto data);
    public Task<Response<bool>> DeleteCommentLike(CommentLikeDto data);
}