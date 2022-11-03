namespace SocialMediaApp.Controls;

public partial class RoundedEntry : ContentView
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RoundedEntry), string.Empty, BindingMode.TwoWay);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public RoundedEntry()
	{
		InitializeComponent();
	}
}