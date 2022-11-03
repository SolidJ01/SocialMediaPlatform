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
                return UserData != null ? Path.Combine("http://10.0.2.2:5166", UserData.profilePictureSource) : "";
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

        public ICommand GoBackCommand { get; }

        public ProfileViewModel(AuthenticationService authenticationService) : base(authenticationService)
        {
            EditProfileCommand = new Command(EditProfile);

            GoBackCommand = new Command(GoBack);
        }

        private async void EditProfile()
        {
            await Shell.Current.GoToAsync(nameof(ProfileEditPage), true);
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
