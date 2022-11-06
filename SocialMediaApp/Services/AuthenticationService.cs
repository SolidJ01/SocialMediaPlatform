using SocialMediaApp.Models.API.Requests;
using SocialMediaApp.Models.API.Responses;
using SocialMediaApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocialMediaApp.Services
{
    public class AuthenticationService
    {
        private string? _token;
        public string? Token { get { return _token; } }
        private static readonly string tokenFileName = "login.token";
        private HttpClient _httpClient;
        private string baseURL = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "http://localhost:5166";

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _ = ReadToken();
        }

        #region Token RWD
        public async Task ReadToken()
        {
            var path = Path.Combine(FileSystem.Current.CacheDirectory, tokenFileName);
            if (File.Exists(path))
            {
                FileStream file = File.OpenRead(path);
                using (StreamReader reader = new StreamReader(file))
                {
                    _token = reader.ReadToEnd();
                }
                file.Close();
            }
            else
            {
                File.Create(path).Close();
            }

        }

        public async Task WriteToken(string token)
        {
            var path = Path.Combine(FileSystem.Current.CacheDirectory, tokenFileName);
            FileStream file;
            if (File.Exists(path))
            {
                file = File.OpenWrite(path);
            }
            else
            {
                file = File.Create(path);
            }
            byte[] info = new UTF8Encoding(true).GetBytes(token);
            await file.WriteAsync(info, 0, info.Length);
            file.Close();
        }

        public async Task ClearToken()
        {
            var path = Path.Combine(FileSystem.Current.CacheDirectory, tokenFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion

        /// <summary>
        /// Authenticates the user using the cached token, if it exists
        /// Call this method when the main page is navigated to (and maybe any other page as well?)
        /// </summary>
        /// <returns>The authenticated users data</returns>
        public async Task<AuthenticatedUserResponse> AuthenticateUser()
        {
            if (_token == null)
            {
                //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
                return null;
            }
            else
            {
                AuthenticateRequest request = new AuthenticateRequest
                {
                    LoginToken = _token,
                    DeviceIdiom = DeviceInfo.Current.Idiom.ToString(),
                    DeviceManufacturer = DeviceInfo.Current.Manufacturer,
                    DeviceModel = DeviceInfo.Current.Model,
                    DevicePlatform = DeviceInfo.Current.Platform.ToString(),
                    DeviceType = DeviceInfo.Current.DeviceType.ToString()
                };
                var jsonRequest = JsonSerializer.Serialize(request);
                StringContent requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(baseURL + "/Authenticate", requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    AuthenticatedUserResponse userData = JsonSerializer.Deserialize<AuthenticatedUserResponse>(responseContent);
                    return userData;
                }
                else
                {
                    //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    return null;
                }
            }

        }

        public async Task<bool> LoginUser(string username, string password)
        {
            LoginRequest request = new LoginRequest
            {
                Username = username,
                Password = password, 
                DeviceIdiom = DeviceInfo.Current.Idiom.ToString(), 
                DeviceManufacturer = DeviceInfo.Current.Manufacturer, 
                DeviceModel = DeviceInfo.Current.Model,
                DevicePlatform = DeviceInfo.Current.Platform.ToString(), 
                DeviceType = DeviceInfo.Current.DeviceType.ToString()
            };
            var jsonRequest = JsonSerializer.Serialize(request);
            StringContent requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(baseURL + "/Login", requestContent);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                await WriteToken(responseContent);
                await ReadToken();
                //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                return true;
            }
            else
            {
                Console.WriteLine("Login Unsuccessful");
                return false;
            }
        }

        public async Task<string> RegisterUser()
        {
            throw new NotImplementedException();
        }

        public async Task LogOutUser()
        {
            await ClearToken();
        }
    }
}
