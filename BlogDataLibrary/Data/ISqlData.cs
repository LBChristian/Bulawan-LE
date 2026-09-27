using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data
{
    public interface ISqlData
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
}