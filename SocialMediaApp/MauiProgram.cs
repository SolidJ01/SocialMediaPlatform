using SocialMediaApp.Services;
using SocialMediaApp.ViewModels;
using SocialMediaApp.Views;

namespace SocialMediaApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("FontAwesome6-Free-Solid-900.otf", "FontAwesomeSolid");
			});

		builder.Services.AddSingleton<HttpClient>();

		builder.Services.AddSingleton<AuthenticationService>();

		builder.Services.AddSingleton<FeedViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<LoginPage>();

        //builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
		
		builder.Services.AddTransient<ProfilePage>();

		//builder.Services.AddSingleton<ProfileEditViewModel>();
		builder.Services.AddTransient<ProfileEditViewModel>();

		builder.Services.AddTransient<ProfileEditPage>();

		return builder.Build();
	}
}
