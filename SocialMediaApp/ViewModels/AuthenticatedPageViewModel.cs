using SocialMediaApp.Models.API.Responses;
using SocialMediaApp.Services;
using SocialMediaApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.ViewModels
{
    public abstract class AuthenticatedPageViewModel : BaseViewModel
    {
        protected AuthenticationService _authenticationService;
        private AuthenticatedUserResponse? _userData;
        public AuthenticatedUserResponse? UserData
        {
            get
            {
                return _userData;
            }
            set
            {
                _userData = value;
                OnPropertyChanged();
            }
        }

        public AuthenticatedPageViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async void OnLoad(object sender, EventArgs e)
        {
            UserData = await _authenticationService.AuthenticateUser();
            if (_userData == null)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                AfterLoad();
            }
        }

        protected abstract void AfterLoad();
    }
}
