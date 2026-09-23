using BlogDataLibrary.Database;
using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
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

            string connectionString =
                config.GetConnectionString("Default") ?? "";

            ISqlDataAccess dbAccess =
                new SqlDataAccess(connectionString);

            SqlData db = new SqlData(dbAccess);

            //features:
            //Authenticate(db)
            //Register(db)
            //AddPost(db)
            //ListPosts(db)
            //ShowPostDetails(db)
            ShowPostDetails(db);

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static UserModel? GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            UserModel? user = db.Authenticate(username, password);

            return user;
        }

        static void Authenticate(SqlData db)
        {
            UserModel? user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
            }
            else
            {
                Console.WriteLine($"Welcome, {user.UserName}");
            }
        }

        static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Enter new password: ");
            string password = Console.ReadLine() ?? "";

            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine() ?? "";

            db.Register(
                username,
                firstName,
                lastName,
                password
            );

            Console.WriteLine("Registration successful.");
        }

        static void AddPost(SqlData db)
        {
            UserModel? user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            Console.Write("Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Body: ");
            string body = Console.ReadLine() ?? "";

            PostModel post = new PostModel
            {
                UserId = user.Id,
                Title = title,
                Body = body,
                DateCreated = DateTime.Now
            };

            db.AddPost(post);

            Console.WriteLine("Post added successfully.");
        }

        static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            foreach (var post in posts)
            {
                string bodyPreview =
                    post.Body.Length > 20
                    ? post.Body.Substring(0, 20)
                    : post.Body;

                Console.WriteLine($"Post ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Author: {post.UserName}");
                Console.WriteLine($"Date Created: {post.DateCreated}");
                Console.WriteLine($"Body: {bodyPreview}");
                Console.WriteLine();
            }
        }

        static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter post ID: ");

            int id = int.Parse(
                Console.ReadLine() ?? "0"
            );

            List<ListPostModel> posts =
                db.ShowPostDetails(id);

            if (posts.Count == 0)
            {
                Console.WriteLine("Post not found.");
                return;
            }

            foreach (var post in posts)
            {
                Console.WriteLine();
                Console.WriteLine($"Post ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");

                Console.WriteLine(
                    $"Author: {post.FirstName} {post.LastName} ({post.UserName})"
                );

                Console.WriteLine(
                    $"Date Created: {post.DateCreated}"
                );

                Console.WriteLine(
                    $"Body: {post.Body}"
                );
            }
        }
    }
}