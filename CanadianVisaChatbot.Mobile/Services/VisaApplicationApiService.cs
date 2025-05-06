using System.Net.Http.Json;
using CanadianVisaChatbot.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CanadianVisaChatbot.Mobile.Services;

public class VisaApplicationApiService : IVisaApplicationApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VisaApplicationApiService> _logger;

    public VisaApplicationApiService(HttpClient httpClient, ILogger<VisaApplicationApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<VisaApplication> CreateApplicationAsync(VisaType type)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/VisaApplication", type);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VisaApplication>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating visa application");
            throw new ApplicationException("Failed to create visa application. Please try again.", ex);
        }
    }

    public async Task<VisaApplication> GetApplicationAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/VisaApplication/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VisaApplication>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving visa application");
            throw new ApplicationException("Failed to retrieve visa application. Please try again.", ex);
        }
    }

    public async Task<List<VisaApplication>> GetUserApplicationsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/VisaApplication/user");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<VisaApplication>>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user applications");
            throw new ApplicationException("Failed to retrieve applications. Please try again.", ex);
        }
    }

    public async Task<DocumentInfo> UploadDocumentAsync(string applicationId, string name, DocumentType type, string notes, Stream fileStream)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", name);
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(type.ToString()), "type");
            content.Add(new StringContent(notes ?? ""), "notes");

            var response = await _httpClient.PostAsync($"api/VisaApplication/{applicationId}/documents", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DocumentInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document");
            throw new ApplicationException("Failed to upload document. Please try again.", ex);
        }
    }

    public async Task<Stream> DownloadDocumentAsync(string applicationId, string documentId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/VisaApplication/{applicationId}/documents/{documentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading document");
            throw new ApplicationException("Failed to download document. Please try again.", ex);
        }
    }

    public async Task<ApplicationProgress> GetProgressAsync(string applicationId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/VisaApplication/{applicationId}/progress");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApplicationProgress>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application progress");
            throw new ApplicationException("Failed to retrieve progress. Please try again.", ex);
        }
    }
}