using System.Data;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace DataAccess.RepositoriesImpl;

public class PostRepository: IPostRepository
{
    public async Task<Response<bool>> AddPost(PostDto post)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();

        SqlCommand cmd = new SqlCommand("usp_PostInsert", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@title", post.Title);
        cmd.Parameters.AddWithValue("@sub_category_id", 1);
        cmd.Parameters.AddWithValue("@body", post.Body);
        cmd.Parameters.AddWithValue("@author_id", post.AuthorId);
        
        
        var linesChanged = await cmd.ExecuteNonQueryAsync();
        await con.CloseAsync();

        bool response = false;
        if (linesChanged == 1)
        {
            response = true;
        }
        else
        {
            response = false;
        }
                
        return new Response<bool>.Success(response);
    }

    public async Task<List<PostForApprovalDto>> GetAllPosts()
    {
        SqlConnection con = DbContext.GetConnection();

        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_PostSelectAllForApproval", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<PostForApprovalDto> posts = new List<PostForApprovalDto>();
        while (await reader.ReadAsync())
        {
            posts.Add(
                new PostForApprovalDto()
                {
                    Postid = Convert.ToInt32(reader["id"]),
                    PostBody = Convert.ToString(reader["body"])!,
                    AuthorName = Convert.ToString(reader["username"])!,
                }
            );
        }

        await con.CloseAsync();
        return posts;
    }
}