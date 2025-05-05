namespace CanadianVisaChatbot.Shared.AI.Services;

public interface IDeepSeekClient
{
    Task<string> GetResponseAsync(string userInput, CancellationToken cancellationToken = default);
    Task<string> GetReasoningResponseAsync(string userInput, CancellationToken cancellationToken = default);
    Task<string> GenerateDocumentAsync(string prompt, CancellationToken cancellationToken = default);
}