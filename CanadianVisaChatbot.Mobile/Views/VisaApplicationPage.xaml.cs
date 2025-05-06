using CanadianVisaChatbot.Mobile.ViewModels;

namespace CanadianVisaChatbot.Mobile.Views;

public partial class VisaApplicationPage : ContentPage
{
    private readonly VisaApplicationViewModel _viewModel;

    public VisaApplicationPage(VisaApplicationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadApplicationsCommand.ExecuteAsync(null);
    }
}