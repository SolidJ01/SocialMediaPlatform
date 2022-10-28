namespace SocialMediaApi.Models.Input
{
    public class UserAuthorizationInput
    {
        public string LoginToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceType { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
    }
}
