using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Views;

public partial class ProfilePage : AuthenticatedPage
{
	public ProfilePage(ProfileViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}