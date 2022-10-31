using SocialMediaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialMediaApp.ViewModels
{
    public class ProfileEditViewModel : AuthenticatedPageViewModel
    {
        private FileResult _newPhoto;
        private string pfpsource;
        public ImageSource PFPSource
        {
            //get { return "http://10.0.2.2:5166/Images/Default/pfp.png"; }
            //set { pfpsource = value; }
            get
            {
                //return ImageSource.FromUri(new Uri(Path.Combine("http://10.0.2.2:5166", UserData.profilePictureSource)));
                if (UserData == null)
                {
                    return ImageSource.FromUri(new Uri("http://10.0.2.2:5166/Images/Default/pfp.png"));
                }
                else if (_newPhoto == null)
                {
                    return ImageSource.FromUri(new Uri(Path.Combine("http://10.0.2.2:5166", UserData.profilePictureSource)));
                }
                else
                {
                    return ImageSource.FromStream(() => _newPhoto.OpenReadAsync().Result);
                }
            }
        }

        public string Username
        {
            get
            {
                return UserData != null ? UserData.username : "Loading";
            }
            set
            {
                if (UserData != null)
                {
                    UserData.username = value;
                }
                OnPropertyChanged();
            }
        }

        public ICommand TakePhotoCommand { get; }
        public ICommand PickPhotoCommand { get; }
        public ICommand SaveCommand { get; }

        public ICommand GoBackCommand { get; }

        public ProfileEditViewModel(AuthenticationService authenticationService) : base(authenticationService)
        {
            TakePhotoCommand = new Command(TakeProfilePhoto);
            PickPhotoCommand = new Command(PickProfilePhoto);
            SaveCommand = new Command(Save);

            GoBackCommand = new Command(GoBack);
        }

        private async void TakeProfilePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    _newPhoto = photo;
                    OnPropertyChanged(nameof(PFPSource));
                }
            }
        }

        private async void PickProfilePhoto()
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                _newPhoto = photo;
                OnPropertyChanged(nameof(PFPSource));
            }
        }

        private async void Save()
        {
            if (_newPhoto != null)
            {
                using (var formContent = new MultipartFormDataContent())
                {
                    formContent.Headers.ContentType.MediaType = "multipart/form-data";
                    Stream fileStream = await _newPhoto.OpenReadAsync();
                    formContent.Add(new StreamContent(fileStream), "File", _newPhoto.FileName);
                    formContent.Add(new StringContent(_authenticationService.Token), "LoginToken");
                    formContent.Add(new StringContent(DeviceInfo.Current.Platform.ToString()), "DevicePlatform");
                    formContent.Add(new StringContent(DeviceInfo.Current.Idiom.ToString()), "DeviceIdiom");
                    formContent.Add(new StringContent(DeviceInfo.Current.DeviceType.ToString()), "DeviceType");
                    formContent.Add(new StringContent(DeviceInfo.Current.Model), "DeviceModel");
                    formContent.Add(new StringContent(DeviceInfo.Current.Manufacturer), "DeviceManufacturer");

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                        var response = await client.PostAsync("http://10.0.2.2:5166/api/File", formContent);
                        var result = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        private async void GoBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        protected override void AfterLoad()
        {
            OnPropertyChanged(nameof(PFPSource));
            OnPropertyChanged(nameof(Username));
        }
    }
}
