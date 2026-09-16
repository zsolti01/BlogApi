using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models;
using BlogApi.Models.DTOs;

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
                BlogId = blogpost.BlogId
            };

            string sql = "INSERT INTO blogpost (Title, Content, PostTime, UpdateTime, BloggerId) VALUES (@Title, @Content, @PostTime, @UpdateTime, @BloggerId);";
            var cmd = new MySqlConnector.MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", post.Title);
            cmd.Parameters.AddWithValue("@Content", post.Content);
            cmd.Parameters.AddWithValue("@BloggerId", post.BlogId);
            
            cmd.ExecuteNonQuery();
            conn.Close();
            return post;
        }

        [HttpPut]
        public Blogpost UpdateBlogpost(Blogpost blogpost)
        {
            var conn = new MySqlConnector.MySqlConnection(connStr);
            conn.Open();
            string sql = "UPDATE blogpost SET Title=@Title, Content=@Content, PostTime=@PostTime, UpdateTime=@UpdateTime, BloggerId=@BloggerId WHERE Id=@Id";
            var cmd = new MySqlConnector.MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", blogpost.Id);
            cmd.Parameters.AddWithValue("@Title", blogpost.Title);
            cmd.Parameters.AddWithValue("@Content", blogpost.Content);
            cmd.Parameters.AddWithValue("@PostTime", blogpost.PostTime);
            cmd.Parameters.AddWithValue("@UpdateTime", blogpost.UpdateTime);
            cmd.Parameters.AddWithValue("@BloggerId", blogpost.BlogId);
            cmd.ExecuteNonQuery();
            conn.Close();
            return blogpost;
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
