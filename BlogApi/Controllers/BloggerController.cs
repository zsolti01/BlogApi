using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();

            var conn = new MySqlConnector.MySqlConnection(ConnectionString);

            conn.Open();

            string sql = "SELECT * FROM blogger";

            var cmd = new MySqlConnector.MySqlCommand(sql, conn);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new Blogger
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegTime = dataReader.GetDateTime(5)
                };
                bloggers.Add(blogger);
            }

            conn.Close();

            return bloggers;
        }

        [HttpPost]
        public Blogger AddNewBlogger(Blogger blogger)
        {
            var conn = new MySqlConnector.MySqlConnection(ConnectionString);

            conn.Open();

            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegTime`) VALUES (@name, @email, @age, @password, @regTime)";

            var cmd = new MySqlConnector.MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@regTime", blg.RegTime);

            cmd.ExecuteNonQuery();
                
            conn.Close();

            return blg;
        }

        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            return null;
        }

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var conn = new MySqlConnector.MySqlConnection(ConnectionString);

            conn.Open();

            var sql = $"DELETE FROM blogger WHERE Id = @id";

            var cmd = new MySqlConnector.MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue(@"id", id);

            cmd.ExecuteNonQuery();

            conn.Close();

            return new { message = "Blogger deleted successfully" };
        }
    }
}
