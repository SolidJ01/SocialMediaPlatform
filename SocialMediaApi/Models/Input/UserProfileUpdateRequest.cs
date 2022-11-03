namespace SocialMediaApi.Models.Input
{
    public class UserProfileUpdateRequest : UserAuthorizationInput
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureSource { get; set; }
    }
}
