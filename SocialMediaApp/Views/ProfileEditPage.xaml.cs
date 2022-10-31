using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Views;

public partial class ProfileEditPage : AuthenticatedPage
{
	public ProfileEditPage(ProfileEditViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}