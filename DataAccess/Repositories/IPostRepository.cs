using Model.DTOs;
using Model.DTOs.Response;

namespace DataAccess.Repositories;

public interface IPostRepository
{
    public Task<Response<bool>> AddPost(PostDto post);
}