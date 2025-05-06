using System.Net.Http.Headers;
using CanadianVisaChatbot.Shared.AI.Services;
using Microsoft.Extensions.DependencyInjection;

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
            
            // Configure timeouts
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        // Register visa processing service
        services.AddScoped<IVisaProcessingService, VisaProcessingService>();

        return services;
    }
}