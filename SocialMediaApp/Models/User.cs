using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureSource { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
        public List<Post> Posts { get; set; }
    }
}
