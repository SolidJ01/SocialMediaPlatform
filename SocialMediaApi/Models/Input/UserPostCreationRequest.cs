namespace SocialMediaApi.Models.Input
{
    public class UserPostCreationRequest : UserAuthorizationInput
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
