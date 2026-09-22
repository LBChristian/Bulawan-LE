using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
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
    }
}