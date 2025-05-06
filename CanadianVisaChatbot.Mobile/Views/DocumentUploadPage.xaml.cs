using CanadianVisaChatbot.Mobile.ViewModels;

namespace CanadianVisaChatbot.Mobile.Views;

public partial class DocumentUploadPage : ContentPage, IQueryAttributable
{
    private readonly VisaApplicationViewModel _viewModel;

    public DocumentUploadPage(VisaApplicationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("applicationId", out var applicationId))
        {
            _viewModel.SetupDocumentUpload(applicationId.ToString());
        }
    }
}