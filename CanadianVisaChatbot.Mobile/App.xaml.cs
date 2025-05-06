using CanadianVisaChatbot.Mobile.Views;

namespace CanadianVisaChatbot.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

        // Register routes
        Routing.RegisterRoute("applications/details", typeof(VisaApplicationDetailPage));
        Routing.RegisterRoute("applications/upload", typeof(DocumentUploadPage));
        Routing.RegisterRoute("applications/new", typeof(NewApplicationPage));
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        return window;
    }
}