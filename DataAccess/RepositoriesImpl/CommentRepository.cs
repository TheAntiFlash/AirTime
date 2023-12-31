using System.Data;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Model.DTOs;
using Model.DTOs.Response;

namespace DataAccess.RepositoriesImpl;

public class CommentRepository : ICommentRepository
{
    public async Task<Response<List<CommentDto>>> GetAllComments(int postId)
    {
        SqlConnection con = DbContext.GetConnection();
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_CommentSelectByPostId", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PostId", postId);
        List<CommentDto> comments = new List<CommentDto>();
        Response<List<CommentDto>> response;
        try
        {
            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                comments.Add(
                    new CommentDto
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        UserId = Convert.ToInt32(reader["user_id"])!,
                        PostId = Convert.ToInt32(reader["post_id"]),
                        Content = Convert.ToString(reader["content"])!,
                        Username = Convert.ToString(reader["username"])!,
                        CreatedAt = Convert.ToDateTime(reader["created_at"]),
                        Likes = Convert.ToInt32(reader["total_likes"]),
                    }
                );
            }
            
            response = new Response<List<CommentDto>>.Success(comments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response = new Response<List<CommentDto>>.Failure(e);
            
        }
        await con.CloseAsync();
        return response;
    }

    public async Task<Response<bool>> AddComment(CommentDto comment)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();

        var cmd = new SqlCommand("usp_CommentInsert", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@user_id", comment.UserId);
        cmd.Parameters.AddWithValue("@post_id", comment.PostId);
        cmd.Parameters.AddWithValue("@content", comment.Content);
        
        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<bool>.Failure(e);
        }
        await con.CloseAsync();
        return new Response<bool>.Success(true);
    }

    public async Task<Response<bool>> AddLike(CommentLikeDto like)
    {
        var con = DbContext.GetConnection();
        await con.OpenAsync();
        var cmd = new SqlCommand("usp_CommentLikeInsert", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", like.UserId);
        cmd.Parameters.AddWithValue("@CommentID", like.CommentId);
        
        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<bool>.Failure(e);
        }
        await con.CloseAsync();
        return new Response<bool>.Success(true);
    }

    public async Task<Response<bool>> DeleteLike(CommentLikeDto like)
    {
        var con = DbContext.GetConnection();
        await con.OpenAsync();
        var cmd = new SqlCommand("usp_CommentLikeDelete", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", like.UserId);
        cmd.Parameters.AddWithValue("@CommentID", like.CommentId);
        
        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<bool>.Failure(e);
        }
        await con.CloseAsync();
        return new Response<bool>.Success(true);
    }
}