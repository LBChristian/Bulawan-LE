using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System.Data;

namespace BlogDataLibrary.Data
{
    public interface ISqlData1
    {
        void AddPost(PostModel post);
        UserModel? Authenticate(string username, string password);
        List<PostModel> GetPosts();
        List<ListPostModel> GetPostsWithUsers();
        List<UserModel> GetUsers();
        void InsertPost(PostModel post);
        void InsertUser(UserModel user);
        List<ListPostModel> ListPosts();
        void Register(string username, string firstName, string lastName, string password);
        List<ListPostModel> ShowPostDetails(int id);
    }

    public class SqlData : ISqlData, ISqlData1
    {
        public List<ListPostModel> ShowPostDetails(int id)
        {
            return db.LoadData<ListPostModel, dynamic>(
                "dbo.spPosts_Detail",
                new { id },
                CommandType.StoredProcedure
            );
        }

        public List<ListPostModel> ListPosts()
        {
            return db.LoadData<ListPostModel, dynamic>(
                "dbo.spPosts_List",
                new { },
                CommandType.StoredProcedure
            );
        }

        public void AddPost(PostModel post)
        {
            db.SaveData(
                "dbo.spPosts_Insert",
                new
                {
                    post.UserId,
                    post.Title,
                    post.Body,
                    post.DateCreated
                },
                CommandType.StoredProcedure
            );
        }

        public void Register(
            string username,
            string firstName,
            string lastName,
            string password)
        {
            db.SaveData(
                "dbo.spUsers_Register",
                new
                {
                    username,
                    firstName,
                    lastName,
                    password
                },
                CommandType.StoredProcedure
            );
        }
        private readonly ISqlDataAccess db;

        public SqlData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public List<UserModel> GetUsers()
        {
            string sql = "SELECT Id, UserName, FirstName, LastName, Password FROM dbo.Users";

            return db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public List<PostModel> GetPosts()
        {
            string sql = "SELECT Id, UserId, Title, Body, DateCreated FROM dbo.Posts";

            return db.LoadData<PostModel, dynamic>(sql, new { });
        }

        public void InsertUser(UserModel user)
        {
            string sql = @"INSERT INTO dbo.Users
                           (UserName, FirstName, LastName, Password)
                           VALUES
                           (@UserName, @FirstName, @LastName, @Password)";

            db.SaveData(sql, user);
        }

        public void InsertPost(PostModel post)
        {
            string sql = @"INSERT INTO dbo.Posts
                           (UserId, Title, Body, DateCreated)
                           VALUES
                           (@UserId, @Title, @Body, @DateCreated)";

            db.SaveData(sql, post);
        }

        public List<ListPostModel> GetPostsWithUsers()
        {
            string sql = @"SELECT
                           p.Id,
                           p.Title,
                           p.Body,
                           p.DateCreated,
                           u.UserName
                       FROM dbo.Posts p
                       INNER JOIN dbo.Users u
                           ON p.UserId = u.Id";

            return db.LoadData<ListPostModel, dynamic>(sql, new { });
        }

        public UserModel? Authenticate(string username, string password)
        {
            UserModel result = db.LoadData<UserModel, dynamic>(
                "dbo.spUsers_Authenticate",
                new { username, password },
                CommandType.StoredProcedure
            ).FirstOrDefault();

            return result;
        }
    }
}