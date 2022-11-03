namespace SocialMediaApp.Controls;

public partial class UserProfile : ContentView
{
    public static readonly BindableProperty PFPSourceProperty = BindableProperty.Create(nameof(PFPSource), typeof(string), typeof(UserProfile), string.Empty);
    public static readonly BindableProperty UsernameProperty = BindableProperty.Create(nameof(Username), typeof(string), typeof(UserProfile), string.Empty);
    public static readonly BindableProperty FollowersProperty = BindableProperty.Create(nameof(Followers), typeof(string), typeof(UserProfile), string.Empty);

    public string PFPSource
    {
        get => (string)GetValue(PFPSourceProperty);
        set => SetValue(PFPSourceProperty, value);
    }

    public string Username
    {
        get => (string)GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    public string Followers
    {
        get => (string)GetValue(FollowersProperty);
        set => SetValue(FollowersProperty, value);
    }

    public UserProfile()
	{
		InitializeComponent();
	}
}