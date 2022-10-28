namespace SocialMediaApi.Models.Output
{
    public class AuthenticatedUserOutput
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureSource { get; set; }
        public int FollowerCount { get; set; }
        //  TODO: pass along lists of followers and following, 
        //  In the appropriate output types of course
        //  Note: may not be possible, since the API might not
        //  permit including them
    }
}
