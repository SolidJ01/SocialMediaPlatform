using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Models.API.Responses
{
    public class AuthenticatedUserResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string profilePictureSource { get; set; }
        public int followerCount { get; set; }
    }
}
