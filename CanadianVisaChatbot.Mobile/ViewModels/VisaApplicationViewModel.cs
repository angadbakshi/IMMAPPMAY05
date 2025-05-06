using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CanadianVisaChatbot.Shared.Models;
using CanadianVisaChatbot.Mobile.Services;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using FileResult = Microsoft.Maui.Storage.FileResult;

namespace CanadianVisaChatbot.Mobile.ViewModels;

public partial class VisaApplicationViewModel : ObservableObject
{
    private readonly IVisaApplicationApiService _apiService;
    private readonly ILogger<VisaApplicationViewModel> _logger;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private VisaApplication _currentApplication;

    [ObservableProperty]
    private ObservableCollection<VisaApplication> _applications;

    [ObservableProperty]
    private ApplicationProgress _progress;

    [ObservableProperty]
    private bool _isNewApplication;

    [ObservableProperty]
    private VisaType _selectedVisaType;

    // Document Upload Properties
    [ObservableProperty]
    private DocumentType _selectedDocumentType;

    [ObservableProperty]
    private string _documentNotes;

    [ObservableProperty]
    private string _selectedFileName;

    [ObservableProperty]
    private FileResult _selectedFile;

    public bool CanUpload => SelectedFile != null;

    public VisaApplicationViewModel(IVisaApplicationApiService apiService, ILogger<VisaApplicationViewModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
        Applications = new ObservableCollection<VisaApplication>();
    }

    [RelayCommand]
    private async Task LoadApplications()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var applications = await _apiService.GetUserApplicationsAsync();
            Applications.Clear();
            foreach (var app in applications.OrderByDescending(a => a.LastModified))
            {
                Applications.Add(app);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading applications");
            ErrorMessage = "Unable to load applications. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task LoadProgress()
    {
        if (CurrentApplication == null) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Progress = await _apiService.GetProgressAsync(CurrentApplication.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading progress");
            ErrorMessage = "Unable to load progress. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task LoadApplicationAsync(string applicationId)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            CurrentApplication = await _apiService.GetApplicationAsync(applicationId);
            await LoadProgress();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading application");
            ErrorMessage = "Unable to load application. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateApplication()
    {
        if (SelectedVisaType == default) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var application = await _apiService.CreateApplicationAsync(SelectedVisaType);
            Applications.Insert(0, application);
            CurrentApplication = application;

            // Navigate to the application details
            var parameters = new Dictionary<string, object>
            {
                { "applicationId", application.Id }
            };
            await Shell.Current.GoToAsync("../applications/details", parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application");
            ErrorMessage = "Unable to create application. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelNewApplication()
    {
        await Shell.Current.GoToAsync("..");
    }

    public void SetupDocumentUpload(string applicationId)
    {
        CurrentApplication = new VisaApplication { Id = applicationId };
        SelectedDocumentType = DocumentType.Passport;
        DocumentNotes = string.Empty;
        SelectedFileName = string.Empty;
        SelectedFile = null;
    }

    [RelayCommand]
    private async Task UploadDocument()
    {
        if (SelectedFile == null || CurrentApplication == null) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            using var stream = await SelectedFile.OpenReadAsync();
            await _apiService.UploadDocumentAsync(
                CurrentApplication.Id,
                SelectedFile.FileName,
                SelectedDocumentType,
                DocumentNotes,
                stream);

            // Refresh progress after upload
            await LoadProgress();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document");
            ErrorMessage = "Unable to upload document. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SelectDocument()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync();
            if (result != null)
            {
                SelectedFile = result;
                SelectedFileName = result.FileName;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error selecting document");
            ErrorMessage = "Unable to select document. Please try again.";
        }
    }

    [RelayCommand]
    private void StartNewApplication()
    {
        IsNewApplication = true;
        SelectedVisaType = default;
    }

    [RelayCommand]
    private async Task NavigateToUpload()
    {
        if (CurrentApplication == null) return;
        
        var parameters = new Dictionary<string, object>
        {
            { "applicationId", CurrentApplication.Id }
        };
        await Shell.Current.GoToAsync("upload", parameters);
    }
}