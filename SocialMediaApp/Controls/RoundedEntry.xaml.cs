namespace SocialMediaApp.Controls;

public partial class RoundedEntry : ContentView
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RoundedEntry), string.Empty, BindingMode.TwoWay);
	public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(RoundedEntry), false);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public bool IsPassword
	{
		get => (bool)GetValue(IsPasswordProperty);
		set => SetValue(IsPasswordProperty, value);
	}

	public RoundedEntry()
	{
		InitializeComponent();
	}
}