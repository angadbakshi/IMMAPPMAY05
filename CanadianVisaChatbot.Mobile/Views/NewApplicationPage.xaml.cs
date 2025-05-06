using CanadianVisaChatbot.Mobile.ViewModels;
using CanadianVisaChatbot.Shared.Models;

namespace CanadianVisaChatbot.Mobile.Views;

public partial class NewApplicationPage : ContentPage
{
    private readonly VisaApplicationViewModel _viewModel;

    public NewApplicationPage(VisaApplicationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.SelectedVisaType = VisaType.Study; // Set default visa type
        _viewModel.ErrorMessage = null;
    }
}