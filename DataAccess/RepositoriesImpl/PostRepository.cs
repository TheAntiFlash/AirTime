using System.Data;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Model.DTOs.Response;
using Model.Models;

namespace DataAccess.RepositoriesImpl;

public class PostRepository: IPostRepository
{
    private readonly ILogger<PostRepository> _logger;

    public PostRepository(ILogger<PostRepository> logger)
    {
        _logger = logger;
    }

    public async Task<Response<bool>> AddPost(PostDto post)
    {
        SqlConnection con = DbContext.GetConnection();
        
        await con.OpenAsync();

        SqlCommand cmd = new SqlCommand("usp_PostInsert", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@title", post.Title);
        cmd.Parameters.AddWithValue("@image_src", "");
        cmd.Parameters.AddWithValue("@meta_description", post.MetaDescription);
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

    public async Task<List<PostDto>> GetPostsForYou(int userId, int offset = 0, int pageSize = 20)
    {
        SqlConnection con = DbContext.GetConnection();
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_PostSelectByFollowingAndDate", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@Offset", offset);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        SqlDataReader reader = await cmd.ExecuteReaderAsync(); 
        List<PostDto> posts = new List<PostDto>();
        while (await reader.ReadAsync())
        {
            posts.Add(
                new ()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = Convert.ToString(reader["title"])!,
                    ImageSrc = Convert.ToString(reader["image_src"]),
                    SubCategoryId = Convert.ToInt32(reader["sub_category_id"]),
                    SubCategoryName = Convert.ToString(reader["sub_category_name"]),
                    Body = Convert.ToString(reader["body"])!,
                    AuthorId = Convert.ToInt32(reader["author_id"]),
                    AuthorName = Convert.ToString(reader["author_name"]),
                    PostApprovalTime = Convert.ToDateTime(reader["approved_at"]),
                    MetaDescription = Convert.ToString(reader["meta_description"])!,
                }
            );
        }

        await con.CloseAsync();
        return posts;
    }

    public async Task<List<PostForApprovalDto>> GetAllPostsForApproval()
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
                    PostId = Convert.ToInt32(reader["id"]),
                    PostBody = Convert.ToString(reader["meta_desc"])!,
                    AuthorName = Convert.ToString(reader["username"])!,
                }
            );
        }

        await con.CloseAsync();
        return posts;
    }

    public async Task UpdatePostStatus(int postId,bool status, int approvedById)
    {
        SqlConnection con = DbContext.GetConnection();
        SqlCommand cmd = new SqlCommand();
        await con.OpenAsync();
        if (status)
        {
            cmd = new SqlCommand("usp_PostUpdateApprove", con);
            
        }
        else
        {
            cmd = new SqlCommand("usp_PostUpdateDisapprove", con);

        }
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@postId", postId);
        cmd.Parameters.AddWithValue("@approvedBy", approvedById);
        await cmd.ExecuteNonQueryAsync();
        
        await con.CloseAsync();
    }
    
    public async Task<int> GetTotalCountOfPosts()
    {
        SqlConnection con = DbContext.GetConnection();
        await con.OpenAsync();
        SqlCommand query = new SqlCommand("SELECT COUNT(*) FROM dbo.[post] WHERE approved = 1 AND status = 'Approved';", con);
        int postsCount = (int)query.ExecuteScalar();
        await con.CloseAsync();
        return postsCount;
    }

    public async Task<Response<PostDto>> GetPost(int postId)
    {
        SqlConnection con = DbContext.GetConnection();
        await con.OpenAsync();
        SqlCommand cmd = new SqlCommand("usp_PostSelectById", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@postId", postId);
        Response<PostDto> response;
        try
        {
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                PostDto post = new()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = Convert.ToString(reader["title"])!,
                    SubCategoryId = Convert.ToInt32(reader["sub_category_id"]),
                    SubCategoryName = Convert.ToString(reader["sub_category_name"]),
                    Body = Convert.ToString(reader["body"])!,
                    Approved = Convert.ToBoolean(reader["approved"]),
                    AuthorId = Convert.ToInt32(reader["author_id"]),
                    AuthorName = Convert.ToString(reader["author_name"]),
                    PostApprovalTime = Convert.ToDateTime(reader["approved_at"]),
                    MetaDescription = Convert.ToString(reader["meta_description"])!,
                    ImageSrc = Convert.ToString(reader["image_src"]),
                };
                response = new Response<PostDto>.Success(post);
            }
            else
            {
                throw new Exception("Post Not Found");
            }

           
        }
        catch (Exception e)
        {
            _logger.LogError(e, "PostRepository - GetPostById Method");
            response = new Response<PostDto>.Failure(e);
        }

        await con.CloseAsync();
        return response;
    }
}