using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CanadianVisaChatbot.Mobile.Models;
using CanadianVisaChatbot.Mobile.Services;
using Syncfusion.Maui.Chat;

namespace CanadianVisaChatbot.Mobile.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly IVisaApiService _visaApiService;
    private readonly ILogger<ChatViewModel> _logger;
    private VisaApplicationData _currentApplication = new();

    public ChatViewModel(IVisaApiService visaApiService, ILogger<ChatViewModel> logger)
    {
        _visaApiService = visaApiService;
        _logger = logger;
        Messages = new ObservableCollection<ChatMessage>();

        // Add initial welcome message
        AddBotMessage("Welcome to the Canadian Visa Chatbot! I can help you with:\n" +
                     "1. Study Visa\n" +
                     "2. Work Visa\n" +
                     "3. Spousal Sponsorship Visa\n\n" +
                     "Please select the type of visa you're interested in.");
    }

    public ObservableCollection<ChatMessage> Messages { get; }

    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    private async Task SendMessageAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            IsBusy = true;
            AddUserMessage(message);

            // Process the message and get bot response
            var response = await ProcessUserMessageAsync(message);
            AddBotMessage(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            AddBotMessage("I apologize, but I encountered an error. Please try again.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<string> ProcessUserMessageAsync(string message)
    {
        if (_currentApplication.VisaType == string.Empty)
        {
            // Initial visa type selection
            return await ProcessVisaTypeSelectionAsync(message);
        }

        // Process based on visa type and current state
        return _currentApplication.VisaType switch
        {
            "study" => await ProcessStudyVisaMessageAsync(message),
            "work" => await ProcessWorkVisaMessageAsync(message),
            "spousal" => await ProcessSpousalVisaMessageAsync(message),
            _ => "I apologize, but I don't understand. Please start over by selecting a visa type."
        };
    }

    private async Task<string> ProcessVisaTypeSelectionAsync(string message)
    {
        var normalizedMessage = message.ToLower();
        if (normalizedMessage.Contains("study") || message.Contains("1"))
        {
            _currentApplication.VisaType = "study";
            return "Let's work on your study visa application. Please provide the following information:\n" +
                   "- Your age\n" +
                   "- Your nationality\n" +
                   "- Your highest level of education";
        }
        else if (normalizedMessage.Contains("work") || message.Contains("2"))
        {
            _currentApplication.VisaType = "work";
            return "Let's work on your work visa application. Please provide:\n" +
                   "- Job title\n" +
                   "- Industry\n" +
                   "- Location in Canada\n" +
                   "- Expected salary";
        }
        else if (normalizedMessage.Contains("spous") || message.Contains("3"))
        {
            _currentApplication.VisaType = "spousal";
            return "Let's work on your spousal sponsorship application. Please provide:\n" +
                   "- Type of relationship (married/common-law)\n" +
                   "- Duration of relationship\n" +
                   "- Current living situation\n" +
                   "- How you maintain communication";
        }

        return "Please select a valid visa type:\n" +
               "1. Study Visa\n" +
               "2. Work Visa\n" +
               "3. Spousal Sponsorship Visa";
    }

    private async Task<string> ProcessStudyVisaMessageAsync(string message)
    {
        try
        {
            // Extract information from message and update application data
            UpdateApplicationData(message);

            // If we have all required information, assess eligibility
            if (HasRequiredStudyVisaInfo())
            {
                var result = await _visaApiService.AssessStudyVisaEligibilityAsync(_currentApplication.UserData);
                return result;
            }

            return "Please provide all required information for your study visa application.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing study visa message");
            return "I encountered an error processing your study visa information. Please try again.";
        }
    }

    private async Task<string> ProcessWorkVisaMessageAsync(string message)
    {
        try
        {
            UpdateApplicationData(message);

            if (HasRequiredWorkVisaInfo())
            {
                var result = await _visaApiService.AssessWorkVisaLMIAAsync(_currentApplication.UserData);
                return result;
            }

            return "Please provide all required information for your work visa application.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing work visa message");
            return "I encountered an error processing your work visa information. Please try again.";
        }
    }

    private async Task<string> ProcessSpousalVisaMessageAsync(string message)
    {
        try
        {
            UpdateApplicationData(message);

            if (HasRequiredSpousalVisaInfo())
            {
                var result = await _visaApiService.AssessSpousalRelationshipAsync(_currentApplication.UserData);
                return result;
            }

            return "Please provide all required information for your spousal sponsorship application.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing spousal visa message");
            return "I encountered an error processing your spousal visa information. Please try again.";
        }
    }

    private void UpdateApplicationData(string message)
    {
        // Simple logic to extract key-value pairs from message
        // In a real application, this would use more sophisticated NLP
        var parts = message.Split(',').Select(p => p.Trim());
        foreach (var part in parts)
        {
            var kvp = part.Split(':').Select(p => p.Trim()).ToArray();
            if (kvp.Length == 2)
            {
                _currentApplication.UserData[kvp[0].ToLower()] = kvp[1];
            }
        }
    }

    private bool HasRequiredStudyVisaInfo()
    {
        return _currentApplication.UserData.ContainsKey("age") &&
               _currentApplication.UserData.ContainsKey("nationality") &&
               _currentApplication.UserData.ContainsKey("education");
    }

    private bool HasRequiredWorkVisaInfo()
    {
        return _currentApplication.UserData.ContainsKey("jobtitle") &&
               _currentApplication.UserData.ContainsKey("industry") &&
               _currentApplication.UserData.ContainsKey("location") &&
               _currentApplication.UserData.ContainsKey("salary");
    }

    private bool HasRequiredSpousalVisaInfo()
    {
        return _currentApplication.UserData.ContainsKey("type") &&
               _currentApplication.UserData.ContainsKey("duration") &&
               _currentApplication.UserData.ContainsKey("livingsituation") &&
               _currentApplication.UserData.ContainsKey("communication");
    }

    private void AddUserMessage(string message)
    {
        Messages.Add(new ChatMessage
        {
            Author = "User",
            Text = message,
            DateTime = DateTime.Now,
            MessageType = MessageType.Text,
            Position = MessagePosition.Right
        });
    }

    private void AddBotMessage(string message)
    {
        Messages.Add(new ChatMessage
        {
            Author = "CanadianVisaBot",
            Text = message,
            DateTime = DateTime.Now,
            MessageType = MessageType.Text,
            Position = MessagePosition.Left
        });
    }
}