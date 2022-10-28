namespace SocialMediaApi.Models.Input
{
    public class UserFileUploadInput : UserAuthorizationInput
    {
        public IFormFile File { get; set; }
    }
}
