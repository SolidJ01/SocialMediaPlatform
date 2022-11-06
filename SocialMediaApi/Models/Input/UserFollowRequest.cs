namespace SocialMediaApi.Models.Input
{
    public class UserFollowRequest : UserAuthorizationInput
    {
        public int FollowId { get; set; }
    }
}
