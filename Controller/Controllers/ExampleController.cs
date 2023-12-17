using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controller.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    [HttpGet("posts")]
    [Authorize]
    public IActionResult GetPosts()
    {
        var post = new 
        {
            postId = 1,
            postTitle = "TestPost",
            postAuthor = "admin"
        };
        return Ok(post);
    }
}