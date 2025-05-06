namespace CanadianVisaChatbot.Mobile.Models;

public class ChatMessage
{
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string? Avatar { get; set; }
    public bool IsFromUser { get; set; }
}

public class VisaApplicationData
{
    public string VisaType { get; set; } = string.Empty;
    public Dictionary<string, string> UserData { get; set; } = new();
}