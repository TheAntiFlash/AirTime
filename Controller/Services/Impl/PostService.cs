using DataAccess.Repositories;
using Model.DTOs;
using Model.DTOs.Response;


namespace Controller.Services.Impl;

public class PostService: IPostService
{
    private readonly IPostRepository _repo;

    public PostService(IPostRepository repo)
    {
        _repo = repo;
    }

    public async Task AddPost(PostDto post)
    {
        try
        {
            await _repo.AddPost(post);
        }
        catch (Exception e)
        {
            
        }
    }

    public async Task<List<PostForApprovalDto>> GetPostsForApproval()
    {
        return await _repo.GetAllPostsForApproval();
    }

    public async Task ChangePostStatus(int postId, bool approved, int approvedById)
    {
        await _repo.UpdatePostStatus(postId, approved, approvedById);
    }

    public async Task<int> GetTotalNumberOfPosts()
    {
        return await _repo.GetTotalCountOfPosts();
    }

    public async Task<List<PostDto>> GetAllPostsForUser(int userId, int offset, int pageSize)
    {
        return await _repo.GetPostsForYou(userId, offset, pageSize);
    }

    public async Task<Response<PostDto>> GetPostById(int postId)
    {
        return await _repo.GetPost(postId);
    }
}