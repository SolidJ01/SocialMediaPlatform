namespace SocialMediaApp.Models.API.Requests
{
    public class UserProfileUpdateRequest : AuthenticateRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureSource { get; set; }
    }
}
