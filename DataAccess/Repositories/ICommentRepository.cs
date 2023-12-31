using Model.DTOs;
using Model.DTOs.Response;

namespace DataAccess.Repositories;

public interface ICommentRepository
{
    public Task<Response<List<CommentDto>>> GetAllComments(int postId);

    public Task<Response<bool>> AddComment(CommentDto comment);

    public Task<Response<bool>> AddLike(CommentLikeDto like);
    
    public Task<Response<bool>> DeleteLike(CommentLikeDto like);
    
    
}