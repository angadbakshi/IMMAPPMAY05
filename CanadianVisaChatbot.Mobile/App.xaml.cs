namespace CanadianVisaChatbot.Mobile;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var chatPage = serviceProvider.GetRequiredService<Views.ChatPage>();
        MainPage = new NavigationPage(chatPage)
        {
            BarTextColor = Colors.White,
            BarBackgroundColor = Color.FromArgb("#0078D4")
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        if (window != null)
        {
            window.Title = "Canadian Visa Assistant";
            
            // Set default window size for desktop platforms
            if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst ||
                DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                window.Width = 800;
                window.Height = 600;
                window.MinimumWidth = 400;
                window.MinimumHeight = 400;
            }
        }

        return window;
    }
}