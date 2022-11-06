namespace SocialMediaApi.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int PosterId { get; set; }
        public User Poster { get; set; }
        public DateTime Posted { get; set; }
        public List<User> LikedBy { get; set; }
    }
}
