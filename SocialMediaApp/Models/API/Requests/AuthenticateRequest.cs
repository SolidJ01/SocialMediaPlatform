using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Models.API.Requests
{
    public class AuthenticateRequest
    {
        public string LoginToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
    }
}
