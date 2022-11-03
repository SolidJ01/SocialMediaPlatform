using SocialMediaApp.Models;
using SocialMediaApp.Models.API.Responses;
using SocialMediaApp.Services;
using SocialMediaApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialMediaApp.ViewModels
{
    public class FeedViewModel : BaseViewModel
    {
        private AuthenticationService _authenticationService;
        public ObservableCollection<Post> Posts { get; set; }

        private AuthenticatedUserResponse? _userData;
        public string UserName
        {
            get
            {
                return _userData != null ? _userData.username : "Loading";
            }
        }

        public string PFPSource
        {
            get
            {
                return _userData != null ? Path.Combine("http://10.0.2.2:5166", _userData.profilePictureSource) : "";
            }
        }

        public ICommand GoToProfileCommand { get; }

        public FeedViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Shell.Current.NavigatedTo += NavigatedTo;

            GoToProfileCommand = new Command(GoToProfileAsync);
        }

        public async void GoToProfileAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true);
        }

        private async void NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            _userData = await _authenticationService.AuthenticateUser();
            if (_userData == null)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
            }
        }

        public async void Current_Loaded(object sender, EventArgs e)
        {
            _userData = await _authenticationService.AuthenticateUser();
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(PFPSource));
            if (_userData == null)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
            }
        }
    }
}
