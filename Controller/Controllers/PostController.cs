using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.Models;

namespace Controller.Controllers;

[Route("api/post/")]
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
    [Route("{userId}/{postsOffset}/{pageSize}")]
    public async Task<IActionResult> GetPostsForUser(int userId, int postsOffset, int pageSize)
    {
        try
        {
            var posts = await _postService.GetAllPostsForUser(userId, postsOffset, pageSize);
            return Ok(posts);

        }
        catch (Exception e)
        {
            return Problem(detail: e.Message);
        }
    }

    [HttpGet]
    [Route("approval")]
    public async Task<IActionResult> GetPostsForApproval()
    {
        var posts = await _postService.GetPostsForApproval();

        return Ok(posts);
    }

    [HttpPatch]
    [Route("approval/{post_id}/{is_approved}")]
    public async Task<IActionResult> ApprovePost(int post_id, bool is_approved)
    {
        await _postService.ChangePostStatus(post_id, is_approved);
        return Ok();
    }
    
    
}