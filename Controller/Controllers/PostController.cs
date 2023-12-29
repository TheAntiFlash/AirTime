using Controller.Services;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.DTOs.Response;
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

    /**
     * <summary>
     * Gets the posts for optimized for a user according to who he follows in chunks of pageSize.
     * </summary>
     * <param name="userId">Id of the user currently logged in</param>
     * <param name="pageSize">How many posts per page</param>
     * <param name="postsOffset">skipping the first x posts and fetching the next posts == pageSize</param>
     * <example> "api/post/20/20" gets posts 21-40</example>
     */
    [HttpGet]
    [Route("{postsOffset:int}/{pageSize:int}")]
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
    [Route("approval/{postId:int}/{isApproved:bool}")]
    public async Task<IActionResult> ApprovePost(int postId, bool isApproved, int approvedById)
    {
        await _postService.ChangePostStatus(postId, isApproved, approvedById);
        return Ok();
    }
    
    /**
     * <summary>
     * Get Total number of approved posts to show how many pages.
     * i.e x/y at the bottom of home page
     * </summary>
     */
    [HttpGet]
    [Route("approved-count")]
    public async Task<IActionResult> GetPostsApprovedCount()
    {
        var count = await _postService.GetTotalNumberOfPosts();

        return Ok(count);
    }

    /**
     * <summary>
     * Get specific post by Id
     * </summary>
     * <param name="postId">Id of post to fetch</param>
     * <returns>200 OK Response when post found.
     * 404 Not Found Response when post does not exist.
     * 500 Problem Response when some server side error occurs.</returns>
     */
    [HttpGet]
    [Route("{postId:int}")]
    public async Task<IActionResult> GetPost(int postId)
    {
        var response = await _postService.GetPostById(postId);
        return response switch
        {
            Response<PostDto>.Success success => Ok(success.Data),
            Response<PostDto>.Failure failure => failure.E.Message == "Post Not Found"
                ? NotFound(failure.E.Message)
                : Problem("Something Went Wrong", statusCode: 500),
            _ => Problem("Something Went Wrong", statusCode: 500)
        };
    }
}