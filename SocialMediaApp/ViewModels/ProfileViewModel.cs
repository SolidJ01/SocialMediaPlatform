using SocialMediaApp.Models.API.Responses;
using SocialMediaApp.Services;
using SocialMediaApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialMediaApp.ViewModels
{
    public class ProfileViewModel : AuthenticatedPageViewModel
    {
        private string baseURL = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5166" : "http://localhost:5166";
        public string Username
        {
            get
            {
                return UserData != null ? UserData.username : "Loading...";
            }
        }

        public string PFPSource
        {
            get
            {
                return UserData != null ? Path.Combine(baseURL, UserData.profilePictureSource) : "";
            }
        }

        public string Followers
        {
            get
            {
                return UserData != null ? $"{UserData.followerCount} Followers" : "";
            }
        }

        public ICommand EditProfileCommand { get; }
        public ICommand LogOutCommand { get; }

        public ICommand GoBackCommand { get; }

        public ProfileViewModel(AuthenticationService authenticationService) : base(authenticationService)
        {
            EditProfileCommand = new Command(EditProfile);
            LogOutCommand = new Command(LogOut);

            GoBackCommand = new Command(GoBack);
        }

        private async void EditProfile()
        {
            await Shell.Current.GoToAsync(nameof(ProfileEditPage), true);
        }

        private async void LogOut()
        {
            await _authenticationService.LogOutUser();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void GoBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        protected override void AfterLoad()
        {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(PFPSource));
            OnPropertyChanged(nameof(Followers));
        }
    }
}
