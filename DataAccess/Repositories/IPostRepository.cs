using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace DataAccess.Repositories;

public interface IPostRepository
{
    public Task<Response<bool>> AddPost(PostDto post);

    public Task<List<PostForApprovalDto>> GetAllPosts();
}