using Microsoft.Extensions.Logging;
using CanadianVisaChatbot.Mobile.Services;
using CanadianVisaChatbot.Mobile.ViewModels;
using CanadianVisaChatbot.Mobile.Views;
using CommunityToolkit.Maui;

namespace CanadianVisaChatbot.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
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

        // Register HttpClient with base URL for API services
        builder.Services.AddHttpClient<IVisaApiService, VisaApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient<IVisaApplicationApiService, VisaApplicationApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromMinutes(5); // Longer timeout for file uploads
        });

        // Register ViewModels
        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<VisaApplicationViewModel>();

        // Register Views
        builder.Services.AddTransient<ChatPage>();
        builder.Services.AddTransient<VisaApplicationPage>();
        builder.Services.AddTransient<VisaApplicationDetailPage>();
        builder.Services.AddTransient<DocumentUploadPage>();
        builder.Services.AddTransient<NewApplicationPage>();

        // Configure Logging
#if DEBUG
        builder.Services.AddLogging(logging =>
            logging.AddDebug()
                   .SetMinimumLevel(LogLevel.Debug));
#endif

        return builder.Build();
    }
}
