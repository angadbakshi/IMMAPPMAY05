using Syncfusion.Maui.Chat;

namespace CanadianVisaChatbot.Mobile.Models;

public class ChatMessage : IAuthor, IChatMessage
{
    public string Author { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public object? Data { get; set; }

    // IAuthor implementation
    public string? Avatar { get; set; }
    public string? Name => Author;

    public MessageType MessageType { get; set; }
    public MessagePosition Position { get; set; }
}

public class VisaApplicationData
{
    public string VisaType { get; set; } = string.Empty;
    public Dictionary<string, string> UserData { get; set; } = new();
}