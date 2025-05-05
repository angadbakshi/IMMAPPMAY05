namespace CanadianVisaChatbot.Shared.AI.Models;

public class DeepSeekMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class DeepSeekRequest
{
    public string Model { get; set; } = "deepseek-chat";
    public IEnumerable<DeepSeekMessage> Messages { get; set; } = Array.Empty<DeepSeekMessage>();
}

public class DeepSeekResponse
{
    public IEnumerable<DeepSeekChoice> Choices { get; set; } = Array.Empty<DeepSeekChoice>();
}

public class DeepSeekChoice
{
    public DeepSeekMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}