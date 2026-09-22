using BlogDataLibrary.Database;
using BlogDataLibrary.Data;
using Microsoft.Extensions.Configuration;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("Default");

            ISqlDataAccess db = new SqlDataAccess(connectionString);

            SqlData data = new SqlData(db);

            var users = data.GetUsers();


            var posts = data.GetPosts();

            foreach (var post in posts)
            {
                Console.WriteLine(
                    $"{post.Id} - {post.Title} - {post.Body}"
                );
            }

            var postsWithUsers = data.GetPostsWithUsers();

            foreach (var post in postsWithUsers)
            {
                Console.WriteLine(
                    $"{post.Id} - {post.Title} - {post.UserName}"
                );
            }

            foreach (var user in users)
            {
                Console.WriteLine(
                    $"{user.Id} - {user.UserName} - {user.FirstName} {user.LastName}"
                );
            }

            Console.ReadLine();
        }
    }
}