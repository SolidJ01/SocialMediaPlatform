using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Views;

public partial class AuthenticatedPage : ContentPage
{
	public AuthenticatedPage(AuthenticatedPageViewModel viewModel)
	{
		InitializeComponent();
		NavigatedTo += viewModel.OnLoad;
	}
}