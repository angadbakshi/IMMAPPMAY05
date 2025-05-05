using System.Net.Http.Headers;
using System.Text;
using CanadianVisaChatbot.Shared.AI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace CanadianVisaChatbot.Shared.AI.Services;

public class DeepSeekClient : IDeepSeekClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DeepSeekClient> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private const string MODEL_NAME = "deepseek-chat";

    public DeepSeekClient(HttpClient httpClient, ILogger<DeepSeekClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        // Configure retry policy
        _retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, attemptNumber, _) =>
                {
                    _logger.LogWarning(
                        "Error making API call. Retrying in {RetryInterval} seconds. Attempt {AttemptNumber}. Error: {Error}",
                        timeSpan.TotalSeconds,
                        attemptNumber,
                        exception.Exception?.Message ?? "Unknown error"
                    );
                }
            );
    }

    public async Task<string> GetResponseAsync(string userInput, CancellationToken cancellationToken = default)
        => await SendRequestAsync(userInput, cancellationToken);

    public async Task<string> GetReasoningResponseAsync(string userInput, CancellationToken cancellationToken = default)
        => await SendRequestAsync(userInput, cancellationToken);

    public async Task<string> GenerateDocumentAsync(string prompt, CancellationToken cancellationToken = default)
        => await SendRequestAsync(prompt, cancellationToken);

    private async Task<string> SendRequestAsync(string userInput, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Preparing API request. Input length: {Length}", userInput.Length);

            // Prepare the request payload for DeepSeek API
            var requestBody = new 
            { 
                messages = new[]
                {
                    new { role = "system", content = "You are a Canadian visa assistant, providing accurate and helpful information about visa applications." },
                    new { role = "user", content = userInput }
                },
                model = MODEL_NAME,
                temperature = 0.7,
                max_tokens = 2000,
                top_p = 0.95,
                stream = false
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            _logger.LogDebug("Request payload: {Payload}", jsonRequest);

            using var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Log request details
            _logger.LogDebug("Request URL: {Url}", _httpClient.BaseAddress);
            foreach (var header in _httpClient.DefaultRequestHeaders)
            {
                _logger.LogDebug("Request Header {Key}: {Value}", header.Key, string.Join(", ", header.Value));
            }

            // Send request and handle response
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                var httpResponse = await _httpClient.PostAsync("", content, cancellationToken);
                
                // Log response details
                _logger.LogDebug("Response Status: {Status}", httpResponse.StatusCode);
                var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogDebug("Raw response: {Content}", responseContent);
                
                httpResponse.EnsureSuccessStatusCode();
                return httpResponse;
            });

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Response content: {Response}", jsonResponse);

            try
            {
                // Parse the DeepSeek API response
                var apiResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new 
                { 
                    choices = new[] 
                    { 
                        new { message = new { content = "" } } 
                    }
                });

                var result = apiResponse?.choices?.FirstOrDefault()?.message?.content;

                if (string.IsNullOrEmpty(result))
                {
                    throw new InvalidOperationException("No valid response content in API response");
                }

                _logger.LogInformation("Successfully generated response of length: {Length}", result.Length);
                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing API response: {Response}", jsonResponse);
                throw new InvalidOperationException("Failed to parse API response", ex);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request: {Message}", ex.Message);
            throw new Exception($"Failed to process request: {ex.Message}", ex);
        }
    }
}