using DataAccess.Repositories;
using Model.DTOs;
using Model.DTOs.Response;


namespace Controller.Services.Impl;

public class PostService: IPostService
{
    private readonly IPostRepository _postRepo;
    private readonly ICommentRepository _commentRepo;
    

    public PostService(IPostRepository postRepo, ICommentRepository commentRepo)
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
    }

    public async Task AddPost(PostDto post)
    {
        try
        {
            await _postRepo.AddPost(post);
        }
        catch (Exception e)
        {
            
        }
    }

    public async Task<List<PostForApprovalDto>> GetPostsForApproval()
    {
        return await _postRepo.GetAllPostsForApproval();
    }

    public async Task ChangePostStatus(int postId, bool approved, int approvedById)
    {
        await _postRepo.UpdatePostStatus(postId, approved, approvedById);
    }

    public async Task<int> GetTotalNumberOfPosts()
    {
        return await _postRepo.GetTotalCountOfPosts();
    }

    public async Task<List<PostDto>> GetAllPostsForUser(int userId, int offset, int pageSize)
    {
        return await _postRepo.GetPostsForYou(userId, offset, pageSize);
    }

    public async Task<Response<PostDto>> GetPostById(int postId)
    {
        return await _postRepo.GetPost(postId);
    }

    public async Task<Response<List<CommentDto>>> GetCommentsForPost(int postId)
    {
        return await _commentRepo.GetAllComments(postId);
    }

    public async Task<Response<bool>> AddComment(CommentDto data)
    {
        return await _commentRepo.AddComment(data);
    }

    public async Task<Response<bool>> AddCommentLike(CommentLikeDto data)
    {
        return await _commentRepo.AddLike(data);
    }

    public async Task<Response<bool>> DeleteCommentLike(CommentLikeDto data)
    {
        return await _commentRepo.DeleteLike(data);
    }
}