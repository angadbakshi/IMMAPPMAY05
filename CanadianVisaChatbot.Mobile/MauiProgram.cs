using Microsoft.Extensions.Logging;
using CanadianVisaChatbot.Mobile.Services;
using CanadianVisaChatbot.Mobile.ViewModels;
using CanadianVisaChatbot.Mobile.Views;
using Syncfusion.Maui.Core.Hosting;

namespace CanadianVisaChatbot.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register HttpClient with base URL
        builder.Services.AddHttpClient<IVisaApiService, VisaApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7001/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
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
