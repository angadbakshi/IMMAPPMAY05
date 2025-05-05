using CanadianVisaChatbot.Mobile.ViewModels;

namespace CanadianVisaChatbot.Mobile.Views;

public partial class ChatPage : ContentPage
{
    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}