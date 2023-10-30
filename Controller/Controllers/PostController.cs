using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.Models;

namespace Controller.Controllers;

[Route("api/post")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPost(PostDto req)
    {
        await _postService.AddPost(req);
        return Ok();
    }
    

    [HttpGet]
    [Route("/approval")]
    public async Task<IActionResult> GetPostsForApproval()
    {
        var posts = await _postService.GetPostsForApproval();

        return Ok(posts);
    }
    
    
}