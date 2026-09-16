using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        private readonly string connStr = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<Blogpost> GetAllBlogposts()
        {
            List<Blogpost> blogposts = new();

            var conn = new MySqlConnector.MySqlConnection(connStr);

            conn.Open();

            string sql = "SELECT * FROM blogpost";

            var cmd = new MySqlConnector.MySqlCommand(sql, conn);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogpost = new Blogpost
                {
                    Id = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Content = dataReader.GetString(2),
                    PostTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    BlogId = dataReader.GetInt32(5),
                };
                blogposts.Add(blogpost);
            }
            conn.Close();

            return blogposts;
        }

        [HttpPost]
        public Blogpost AddNewBlogpost(AddPostDTO blogpost)
        {
            var conn = new MySqlConnector.MySqlConnection(connStr);
            conn.Open();

            var post = new Blogpost
            {
                Title = blogpost.Title,
                Content = blogpost.Content,
                PostTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                BlogId = blogpost.BlogId
            };

            string sql = "INSERT INTO blogpost (Title, Content, PostTime, UpdateTime, blogId) VALUES (@Title, @Content, @PostTime, @UpdateTime, @blogId);";
            var cmd = new MySqlConnector.MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", post.Title);
            cmd.Parameters.AddWithValue("@Content", post.Content);
            cmd.Parameters.AddWithValue("@postTime", post.PostTime);
            cmd.Parameters.AddWithValue("@updateTime", post.UpdateTime);
            cmd.Parameters.AddWithValue("@blogId", post.BlogId);
            
            cmd.ExecuteNonQuery();
            conn.Close();
            return post;
        }

        [HttpPut]
        public object UpdatePost([FromQuery] int id, [FromBody] UpdatePostDTO updatePostDto)
        {
            var connector = new MySqlConnection(connStr);

            connector.Open();

            string sql = @"UPDATE `blogpost` SET `title`=@title,`content`=@content,`updateTim`=@updateTime,`blogId`=@blogId
                WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@title", updatePostDto.Title);
            cmd.Parameters.AddWithValue("@content", updatePostDto.Content);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@blogId", id);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedPost = new UpdatePostDTO
            {
                Title = updatePostDto.Title,
                Content = updatePostDto.Content
            };

            connector.Close();

            return new { message = "BlogPost updated successfully", result = updatePostDto };
        }

        [HttpDelete]
        public object DeleteBlogpost(int id)
        {
            var conn = new MySqlConnector.MySqlConnection(connStr);
            conn.Open();
            string sql = "DELETE FROM blogpost WHERE Id=@Id";
            var cmd = new MySqlConnector.MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            conn.Close();
            return new { message = "BlogPost deleted successfully" };
        }
    }
}
