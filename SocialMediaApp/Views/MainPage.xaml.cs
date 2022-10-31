using SocialMediaApp.ViewModels;

namespace SocialMediaApp;

public partial class MainPage : ContentPage
{
	public MainPage(FeedViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
		this.NavigatedTo += viewModel.Current_Loaded;
	}
}

