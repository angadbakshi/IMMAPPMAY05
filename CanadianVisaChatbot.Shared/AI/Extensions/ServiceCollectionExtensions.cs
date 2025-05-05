çusing CanadianVisaChatbot.Shared.AI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CanadianVisaChatbot.Shared.AI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVisaServices(
        this IServiceCollection services,
        string apiKey,
        string baseUrl)
    {
        // Configure HttpClient for DeepSeek API
        services.AddHttpClient<IDeepSeekClient, DeepSeekClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Set authorization header with DeepSeek API key format
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            
            // Add recommended headers
            client.DefaultRequestHeaders.Add("User-Agent", "CanadianVisaChatbot/1.0");
            
            // Configure timeouts for long-running requests
            client.Timeout = TimeSpan.FromSeconds(60);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            // Enable SSL/TLS
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        // Register visa processing service
        services.AddScoped<IVisaProcessingService, VisaProcessingService>();

        return services;
    }
}