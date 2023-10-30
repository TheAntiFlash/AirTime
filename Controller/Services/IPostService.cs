using Model.DTOs;

namespace Controller.Services;

public interface IPostService
{
    public Task AddPost(PostDto post);

    public Task<List<PostForApprovalDto>> GetPostsForApproval();

    public Task ChangePostStatus(int postId, bool approved);
}