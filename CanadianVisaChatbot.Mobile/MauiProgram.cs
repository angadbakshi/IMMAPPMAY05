using Microsoft.Extensions.Logging;
using CanadianVisaChatbot.Mobile.Services;
using CanadianVisaChatbot.Mobile.ViewModels;
using CanadianVisaChatbot.Mobile.Views;

namespace CanadianVisaChatbot.Mobile;

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
            });

        // Get the device platform
        var platform = DeviceInfo.Current.Platform;

        // Configure API base URL based on platform
        var apiBaseUrl = platform == DevicePlatform.iOS ? 
            "http://localhost:7002" :  // Use http for iOS simulator
            "https://localhost:7001";   // Use https for other platforms

        // Register HttpClient with base URL
        builder.Services.AddHttpClient<IVisaApiService, VisaApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            
            // Configure timeout
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            // Allow self-signed certificates in development
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        // Register ViewModels
        builder.Services.AddTransient<ChatViewModel>();

        // Register Views
        builder.Services.AddTransient<ChatPage>();

        // Configure Logging
#if DEBUG
        builder.Services.AddLogging(logging =>
            logging.AddDebug()
                   .SetMinimumLevel(LogLevel.Debug));
#endif

        return builder.Build();
    }
}
