using Model.DTOs;
using Model.DTOs.Response;

namespace DataAccess.Repositories;

public interface IPostRepository
{
    public Task<Response<bool>> AddPost(PostDto post);
    public Task<List<PostDto>> GetPostsForYou(int userId, int offset = 0, int pageSize = 20);
    public Task<List<PostForApprovalDto>> GetAllPostsForApproval();

    public Task UpdatePostStatus(int postId, bool status);
    public Task<int> GetTotalCountOfPosts();

}