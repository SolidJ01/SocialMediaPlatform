using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}