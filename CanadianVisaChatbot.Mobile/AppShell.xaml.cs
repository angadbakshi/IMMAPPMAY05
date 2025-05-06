using CanadianVisaChatbot.Mobile.Views;

namespace CanadianVisaChatbot.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        // Register detail pages
        Routing.RegisterRoute("chat", typeof(ChatPage));
        Routing.RegisterRoute("applications", typeof(VisaApplicationPage));
        Routing.RegisterRoute("applications/details", typeof(VisaApplicationDetailPage));
        Routing.RegisterRoute("applications/upload", typeof(DocumentUploadPage));
        Routing.RegisterRoute("applications/new", typeof(NewApplicationPage));
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        if (args.Target.Location.ToString().Contains("details") ||
            args.Target.Location.ToString().Contains("upload") ||
            args.Target.Location.ToString().Contains("new"))
        {
            // These routes should be handled as modal pages
            args.Cancel();
            NavigateModalAsync(args.Target.Location.ToString());
        }
    }

    private async void NavigateModalAsync(string route)
    {
        // Extract the page type from the route
        var pageType = route switch
        {
            var r when r.Contains("details") => typeof(VisaApplicationDetailPage),
            var r when r.Contains("upload") => typeof(DocumentUploadPage),
            var r when r.Contains("new") => typeof(NewApplicationPage),
            _ => null
        };

        if (pageType == null) return;

        // Get the page from the dependency injection container
        var page = Handler.MauiContext.Services.GetService(pageType) as Page;
        if (page != null)
        {
            await Navigation.PushModalAsync(new NavigationPage(page));
        }
    }
}
