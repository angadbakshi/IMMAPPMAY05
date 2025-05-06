using CanadianVisaChatbot.Mobile.ViewModels;

namespace CanadianVisaChatbot.Mobile.Views;

public partial class VisaApplicationDetailPage : ContentPage, IQueryAttributable
{
    private readonly VisaApplicationViewModel _viewModel;

    public VisaApplicationDetailPage(VisaApplicationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("applicationId", out var applicationId))
        {
            _viewModel.LoadApplicationAsync(applicationId.ToString());
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadProgressCommand.Execute(null);
    }

    protected override bool OnBackButtonPressed()
    {
        // Ensure we refresh the application list when going back
        _viewModel.LoadApplicationsCommand.Execute(null);
        return base.OnBackButtonPressed();
    }
}