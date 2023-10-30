using DataAccess.Repositories;
using Model.DTOs;


namespace Controller.Services;

public class PostService: IPostService
{
    private readonly IPostRepository _repo;

    public PostService(IPostRepository repo)
    {
        _repo = repo;
    }

    public async Task AddPost(PostDto post)
    {
       await _repo.AddPost(post);
    }
}