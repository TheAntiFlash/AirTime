using System.Data;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Model.DTOs;
using Model.DTOs.Response;

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
}