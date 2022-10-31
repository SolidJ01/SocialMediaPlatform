using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Models.API.Requests
{
    internal class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
    }
}
