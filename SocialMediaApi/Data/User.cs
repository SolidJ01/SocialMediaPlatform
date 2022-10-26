using System.ComponentModel.DataAnnotations;

namespace SocialMediaApi.Data
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureSource { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        public List<LoginToken> LoginTokens { get; set; }
    }
}
