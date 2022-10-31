using SocialMediaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialMediaApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private AuthenticationService _authenticationService;

        private string inputUsername;
        public string InputUsername
        {
            get { return inputUsername; }
            set
            {
                inputUsername = value;
                OnPropertyChanged();
            }
        }
        private string inputPassword;
        public string InputPassword
        {
            get { return inputPassword; }
            set
            {
                inputPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            LoginCommand = new Command(LoginAsync);
        }

        private async void LoginAsync()
        {
            bool success = await _authenticationService.LoginUser(inputUsername, inputPassword);
            if (success)
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                InputUsername = "";
                InputPassword = "";
            }
        }
    }
}
